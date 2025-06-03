using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject winPanel;

    public void TryAgain()
    {
        Debug.Log("TryAgain click");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
{
    Time.timeScale = 1f; // Återställ tidsskalan
    SceneManager.LoadScene("MainMenu");
}

    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
        winPanel.SetActive(false);
    }

    public void ShowWinScreen()
    {
        winPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    public void HideAll()
    {
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
    }
}

