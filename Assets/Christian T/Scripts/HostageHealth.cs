using UnityEngine;
using UnityEngine.AI;

public class HostageHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;
    public GameOverUIManager gameOverUIManager;


    private HostageController hostageController;

    void Start()
    {
        currentHealth = maxHealth;
        hostageController = GetComponent<HostageController>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (hostageController != null)
            hostageController.enabled = false;

        var navAgent = GetComponent<NavMeshAgent>();
        if (navAgent != null)
            navAgent.enabled = false;

        var collider = GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;

        Debug.Log("Hostagen dog.");

        // Visa Game Over-skärmen
        if (gameOverUIManager != null)
            gameOverUIManager.ShowGameOverScreen();

        Destroy(gameObject);

    }

}
