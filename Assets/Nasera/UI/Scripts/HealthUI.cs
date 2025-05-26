using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public TextMeshProUGUI healthText;

    void Update()
    {
        if (playerHealth != null && healthText != null)
        {
            healthText.text = string.Join(" ", new string('❤', (int)playerHealth.currentHealth).ToCharArray());

        }
    }

}
