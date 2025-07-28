using UnityEngine;
using TMPro;
public class Countdown : MonoBehaviour
{
    public float startTime = 600f;
    public float currentTime;
    public TMP_Text countdownText;
    private bool timerRunning = true;
    private void Start()
    {
        currentTime = startTime;
        UpdateCountdownText();
    }
    private void Update()
    {
        if (timerRunning && currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Max(currentTime, 0f);
            UpdateCountdownText();
            if (timerRunning && currentTime <= 0)
            {
                timerRunning = false;
                TimerEnded();
            }
        }
    }
    private void UpdateCountdownText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    private void TimerEnded()
    {
        Debug.Log("Out of time");
        // add more logic later
    }
}
