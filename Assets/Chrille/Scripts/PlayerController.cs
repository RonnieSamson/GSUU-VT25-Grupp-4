using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PLayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 8f;
    public float jumpForce = 5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool isGrounded;
    public bool isAlive = true;

    void Start()
    {
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        if (rb == null)
        {
            Debug.LogError("PlayerMovement: Rigidbody-komponent saknas på detta GameObject!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
       if (isAlive)
        {
            PlayerInput();
            GroundCheck();

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                Jump();
            }
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void PlayerInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    void HandleMovement()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            // Svagare styrning i luften (valfritt)
            rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Nollställ y-hastighet
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}