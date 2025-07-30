using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject gameOverCanvas;
    public void PauseGame()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
