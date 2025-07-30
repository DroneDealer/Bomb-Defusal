using UnityEngine;
using TMPro;

public class LivesManager : MonoBehaviour
{
    public static LivesManager Instance;
    public int MaxLives = 3;
    public int CurrentLives;
    public TMP_Text livesText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CurrentLives = MaxLives;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        UpdateLivesUI();
    }
    public bool LoseLife()
    {
        if (CurrentLives <= 0)
        {
            return true;
        }

        CurrentLives--;
        UpdateLivesUI();

        if (CurrentLives <= 0)
        {
            Debug.Log("Game Over! No more lives.");
            return true;
        }
        return false;
    }
    public void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Attempts remaining: " + CurrentLives;
        }
    }
    public void ResetLives()
    {
        CurrentLives = MaxLives;
        UpdateLivesUI();
    }
}