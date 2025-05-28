using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform cameraTransform; 
    public float mouseSensitivity = 2f;

    private float xRotation = 0f; 

    void Update()
    {
        // Rörelse
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = (transform.right * h + transform.forward * v).normalized;
        transform.position += direction * speed * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
