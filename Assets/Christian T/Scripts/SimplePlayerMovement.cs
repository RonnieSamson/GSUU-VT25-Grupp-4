using UnityEngine;

// BARA FÖR TEST
public class SimplePlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform cameraTransform;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = (transform.right * h + transform.forward * v).normalized;
        transform.position += direction * speed * Time.deltaTime;


        float mouseX = Input.GetAxis("Mouse X") * 2f;
        transform.Rotate(Vector3.up * mouseX);
    }
}
