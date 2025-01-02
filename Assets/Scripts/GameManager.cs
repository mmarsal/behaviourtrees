using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject gameOverUI; // Lose UI
    public GameObject winnerUI;   // Winner UI

    [Header("Timer Settings")]
    public float timeRemaining = 180f; // Zeitlimit in Sekunden
    public TextMeshProUGUI timerText;
    private bool timerRunning = true;

    private void Update()
    {
        // Timer-Logik
        if (timerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                timeRemaining = 0;
                timerRunning = false;
                TimerEnded();
            }
        }

        // Überprüfe, ob eines der Endgame-UI aktiviert ist
        if (winnerUI.activeSelf || gameOverUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return)) // Überprüfe, ob Enter gedrückt wurde
            {
                RestartGame();
            }
        }
    }

    // Aktualisiere die Timeranzeige im UI
    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}"; // Format als MM:SS
    }

    // Wird aufgerufen, wenn der Timer endet
    void TimerEnded()
    {
        Debug.Log("Timer has ended! You Win!");
        ShowWinnerUI();
    }

    // Wird aufgerufen, wenn der Spieler stirbt
    public void GameOver()
    {
        Debug.Log("Game Over! You Lose!");
        ShowLoseUI();
    }

    // Zeige den WinnerUI an und pausiere das Spiel
    void ShowWinnerUI()
    {
        if (winnerUI != null)
        {
            winnerUI.SetActive(true);
            Time.timeScale = 0; // Spiel anhalten
        }
        else
        {
            Debug.LogWarning("WinnerUI ist nicht zugewiesen.");
        }
    }

    // Zeige den LoseUI an und pausiere das Spiel
    void ShowLoseUI()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0; // Spiel anhalten
        }
        else
        {
            Debug.LogWarning("gameOverUI ist nicht zugewiesen.");
        }
    }

    // Neustarten des Spiels
    public void RestartGame()
    {
        Time.timeScale = 1; // Zeit zurücksetzen
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Aktuelle Szene neu laden
    }
}
