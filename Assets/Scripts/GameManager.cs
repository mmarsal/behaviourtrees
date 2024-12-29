using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;

    public float timeRemaining = 180f;
    public TextMeshProUGUI timerText;
    private bool timerRunning = true;

    void Update()
    {
        if (timerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                timerText.text = "00:00";
                timeRemaining = 0;
                timerRunning = false;
                TimerEnded();
            }
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}"; // Format as MM:SS
    }

    void TimerEnded()
    {
        Debug.Log("Timer has ended!");
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }
}
