using UnityEngine;

public class TestPowerupScript : MonoBehaviour
{
    public float rotationSpeed = 50f;

    void Update()
    {
        // Snurra runt Y-axeln
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Powerup plockad!");
            Destroy(gameObject);
        }
    }
}
