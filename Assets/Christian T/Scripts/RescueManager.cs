using UnityEngine;
using TMPro;

public class RescueManager : MonoBehaviour
{
    private int rescuedHostages = 0;
    private int totalHostages = 0;

    public TextMeshProUGUI uiText;
    public GameOverUIManager gameOverUIManager;

    void Start()
    {
        totalHostages = GameObject.FindGameObjectsWithTag("Hostage").Length;
        UpdateUIText();
    }

    public void HostageRescued()
    {
        rescuedHostages++;
        UpdateUIText();

        if (rescuedHostages >= totalHostages)
        {
            Debug.Log("Alla hostages är räddade!");

            // Pausa spelet och lås upp musen
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (gameOverUIManager != null)
            {
                Debug.Log("Visar WinScreen");
                gameOverUIManager.ShowWinScreen();
            }
            else
            {
                Debug.LogWarning("GameOverUIManager är null i RescueManager!");
            }
        }
    }

    private void UpdateUIText()
    {
        uiText.text = $"Hostages Rescued: {rescuedHostages} / {totalHostages}";
    }
}
