using UnityEngine;

public class TestEnemyScript : MonoBehaviour
{
    public float damageAmount = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(0.5f);
                Debug.Log("Player damaged!");
            }
        }
    }
}
