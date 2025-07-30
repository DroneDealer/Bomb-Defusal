using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SimonSays : MonoBehaviour
{
    [SerializeField] private List<SimonSaysButton> PlayerButtons = new();
    [SerializeField] private SimonSaysDisplayButon displayButton;
    private List<int> randomSequence = new();
    private int currentStep = 0;
    private float DelayBetweenFlashes = 1f;
    private bool PlayerMove = false;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Button playButton;
    private int currentLevel = 1;
    public int maxLevels = 10;
    private bool HasWon = false;
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip success;
    public AudioClip fail;
    void Start()
    {
        for (int i = 0; i < PlayerButtons.Count; i++)
        {
            PlayerButtons[i].Setup(i, OnButtonPressed);
        }
    }
    public void StartGame()
    {
        if (HasWon)
        {
            return;
        }
        playButton.gameObject.SetActive(false);
        StartCoroutine(RestartGame());
    }
    void OnButtonPressed(int id)
    {
        Debug.Log("OnButtonPressed called: ID = " + id + ", PlayerMove = " + PlayerMove + ", currentStep = " + currentStep + ", randomSequence.Count = " + randomSequence.Count);
        Debug.Log($"Button {id} pressed");
        if (currentStep >= randomSequence.Count)
        {
            Debug.LogError("INVALID");
        }
        if (id == randomSequence[currentStep])
        {
            Debug.Log("Valid input. Button ID " + id + " == expected " + randomSequence[currentStep]);
            currentStep++;
            if (currentStep >= randomSequence.Count)
            {
                Debug.Log("Sequence complete! Moving to level " + (currentLevel + 1) + " / 10");
                audioSource.PlayOneShot(success);
                feedbackText.text = "DEFENSE BREACHED. INCREASING DIFFICULTY.";
                SetAllButtonsInteractable(false);
                StartCoroutine(NextRound());
            }
        }
        else
        {
            Debug.LogError("WRONG INPUT! Expected " + randomSequence[currentStep] + ", but got " + id);
            feedbackText.text = "ERROR: INCORRECT SEQUENCE ENTERED.";
            audioSource.PlayOneShot(fail);
            SetAllButtonsInteractable(false);
            bool gameOver = LivesManager.Instance.LoseLife();
            if (gameOver)
            {
                feedbackText.text = "ERROR: OUT OF ATTEMPTS. BOMB DETONATING...";
                return;
            }
            else
            {
                ShowPlayButton();
                return;
            }
        }
    }
    void GenerateRandomSequennce(int length)
    {
        Debug.Log("Generating sequence of length: " + length);
        randomSequence.Clear();
        for (int i = 0; i < length; i++)
        {
            int randomIndex = Random.Range(0, PlayerButtons.Count);
            randomSequence.Add(randomIndex);
        }
        Debug.Log("New sequence: " + string.Join(",", randomSequence));
    }
    IEnumerator PlaySequence()
    {
        Debug.Log("Starting sequence playback...");
        PlayerMove = false;
        SetAllButtonsInteractable(false);
        currentStep = 0;
        yield return new WaitForSeconds(0.5f);
        foreach (int index in randomSequence)
        {
            Color color = PlayerButtons[index].GetColor();
            Debug.Log("Flashing color for index: " + index + " (Color: " + color + ")");
            displayButton.Flash(color);
            yield return new WaitForSeconds(DelayBetweenFlashes);
        }
        yield return new WaitForSeconds(0.2f);
        feedbackText.text = "YOUR TURN: REPEAT THE SEQUENCE";
        PlayerMove = true;
        SetAllButtonsInteractable(true);
        Debug.Log("Sequence playback complete. Waiting for player input.");
    }
    IEnumerator NextRound()
    {
        PlayerMove = false;
        if (currentLevel >= maxLevels)
        {
            feedbackText.text = "ALL MEMORY SEQUENCES COMPLETED";
            SetAllButtonsInteractable(false);
            HasWon = true;
            yield break;
        }
        currentLevel++;
        UpdateLevelText();
        yield return new WaitForSeconds(1f);
        GenerateRandomSequennce(currentLevel);
        yield return PlaySequence();
    }
    IEnumerator RestartGame()
    {
        PlayerMove = false;
        currentLevel = 1;
        UpdateLevelText();
        yield return new WaitForSeconds(1f);
        GenerateRandomSequennce(1);
        yield return PlaySequence();
    }
    private void UpdateLevelText()
    {
        levelText.text = $"Level: {currentLevel}";
    }
    void SetAllButtonsInteractable(bool interactable)
    {
        foreach (var button in PlayerButtons)
        {
            button.GetComponent<UnityEngine.UI.Button>().interactable = interactable;
        }
    }
    public void ShowPlayButton()
    {
        playButton.gameObject.SetActive(true);
        feedbackText.text = "Press PLAY to begin another sequence";
    }
}