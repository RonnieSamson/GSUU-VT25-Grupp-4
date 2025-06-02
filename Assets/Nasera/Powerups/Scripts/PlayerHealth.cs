using UnityEngine;


public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float currentHealth = 3f;
    public float maxHealth = 3f;


    public HeartsUIManager heartsUIManager;
    public GameObject gameOverPanel;
    public PLayerController playerController;
    void Start()
    {
        currentHealth = maxHealth;

        // Visa hjärtan direkt när spelet startar
        if (heartsUIManager != null)
            heartsUIManager.UpdateHearts();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void IncreaseHealth(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Debug.Log("Health increased! Current Health: " + currentHealth);
        heartsUIManager.UpdateHearts();
    }


    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
            currentHealth = 0;

        Debug.Log("Health decreased! Current Health: " + currentHealth);
        heartsUIManager.UpdateHearts();

        if (currentHealth == 0)
        {
            Debug.Log("Player died!");
            playerController.isAlive = false;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
        }

    }
}

