using UnityEngine;

public class HealthPowerup : MonoBehaviour
{
    public int healthAmount = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null && health.currentHealth < health.maxHealth)
            {
                health.IncreaseHealth(healthAmount);
                Debug.Log("Health increased!");
                Destroy(gameObject); // ✅ Endast förstör om liv gavs
            }
            else
            {
                Debug.Log("Already at max health – can't pick up health yet.");
            }
        }
    }
}


