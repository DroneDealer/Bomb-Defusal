using UnityEngine;
using TMPro;

public class LivesManager : MonoBehaviour
{
    public static LivesManager Instance;
    public int MaxLives = 15;
    public int CurrentLives;
    public TMP_Text livesText;
    public AudioSource audioSource;
    public AudioClip explosion;
    public GameObject gameOverCanvas;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CurrentLives = MaxLives;
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
            audioSource.PlayOneShot(explosion);
            Debug.Log("Game Over! No more lives.");
            if (gameOverCanvas != null)
            {
                gameOverCanvas.SetActive(true);
            }
            Time.timeScale = 0f;
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
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
    
}