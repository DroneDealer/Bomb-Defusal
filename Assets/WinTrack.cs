using UnityEngine;

public class WinTrack : MonoBehaviour
{
    public static WinTrack Instance;
    private int completedMinigames = 0;
    public GameObject winCanvas;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Optional: DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void minigameCompleted()
    {
        completedMinigames++;
        if (completedMinigames >= 3)
        {
            ShowWinCanvas();
        }
    }
    private void ShowWinCanvas()
    {
        if (winCanvas != null)
        {
            winCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Debug.LogWarning("WinCanvas is not assigned!");
        }
    }
}