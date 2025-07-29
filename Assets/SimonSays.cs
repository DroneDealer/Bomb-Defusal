using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSays : MonoBehaviour
{
    [SerializeField] private List<SimonSaysButton> PlayerButtons = new();
    [SerializeField] private SimonSaysDisplayButon displayButton;
    private List<int> randomSequence = new();
    private int currentStep = 0;
    private float DelayBetweenFlashes = 1f;
    private bool PlayerMove = false;
    void Start()
    {
        StartCoroutine(RestartGame());
        for (int i = 0; i < PlayerButtons.Count; i++)
        {
            PlayerButtons[i].Setup(i, OnButtonPressed);
        }
    }
    void OnButtonPressed(int id)
    {
        if (!PlayerMove)
        {
            return;
        }
        Debug.Log($"Button {id} pressed");
        if (id == randomSequence[currentStep])
        {
            currentStep++;
            if (currentStep >= randomSequence.Count)
            {
                Debug.Log("DEFENSE BREACHED. INCREASING DIFFICULTY.");
                PlayerMove = false;
                StartCoroutine(NextRound());
            }
        }
        else
        {
            Debug.Log("ERROR: INCORRECT SEQUENCE ENTERED.");
            StartCoroutine(RestartGame());
        }
    }
    void GenerateRandomSequennce(int length)
    {
        randomSequence.Clear();
        for (int i = 0; i < length; i++)
        {
            int randomIndex = Random.Range(0, PlayerButtons.Count);
            randomSequence.Add(randomIndex);
        }
    }
    IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (int index in randomSequence)
        {
            Color color = PlayerButtons[index].GetColor();
            displayButton.Flash(color);
            yield return new WaitForSeconds(DelayBetweenFlashes);
        }
        currentStep = 0;
        PlayerMove = true; 
    }
    IEnumerator NextRound()
    {
        yield return new WaitForSeconds(1f);
        GenerateRandomSequennce(randomSequence.Count + 1);
        yield return PlaySequence();
    }
    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1f);
        GenerateRandomSequennce(1);
        // I COULD just be nice about it and make the player restart their current round but this is funnier (I will regret this while I test)
        yield return PlaySequence();
    }
}