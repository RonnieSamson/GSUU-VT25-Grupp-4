using UnityEngine;

public class BlinkWhenPickupAvailable : MonoBehaviour
{
    public PlayerHealth playerHealth; // tilldela i Inspector
    public float blinkSpeed = 2f;

    private Renderer rend;
    private Color originalColor;
    private bool canBlink = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    void Update()
    {
        if (playerHealth != null && playerHealth.currentHealth < playerHealth.maxHealth)
        {
            canBlink = true;
        }
        else
        {
            canBlink = false;
            rend.material.color = originalColor;
        }

        if (canBlink)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            Color blinkColor = originalColor;
            blinkColor.a = alpha;
            rend.material.color = blinkColor;
        }
    }
}
