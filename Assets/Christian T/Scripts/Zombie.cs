using UnityEngine;
// BARA FÖR TEST
public class Zombie : MonoBehaviour, IDamageable
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(amount);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Zombie died!");
        Destroy(gameObject);
    }
}