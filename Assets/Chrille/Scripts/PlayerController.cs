using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PLayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 8f;
    public float jumpForce = 5f;

    [Header("Water Settings")]
    public float waterJumpMultiplier = 1.5f; // Justerbar i Inspector
    public Transform waterSurface; // Dra in WaterSurface-objektet i Inspector
    public Transform waterSurfaceCheck;
    public float surfaceCheckDistance = 0.2f;
    public LayerMask waterSurfaceMask;
    private bool isInWater = false; // Koll på om spelaren är i vatten
    private bool isAtWaterSurface = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool isGrounded;



    void Start()
    {
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
        PlayerInput();
        GroundCheck();
        CheckWater();

        if (Input.GetButtonDown("Jump") && (isGrounded || IsTouchingWaterSurface()))
        {
            Jump();
        }

        Debug.Log("In Water: " + isInWater + " | Grounded: " + isGrounded + " | TouchingSurface: " + IsTouchingWaterSurface());
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

    bool IsTouchingWaterSurface()
    {
        return Physics.CheckSphere(waterSurfaceCheck.position, surfaceCheckDistance, waterSurfaceMask);
    }

    void HandleMovement()
    {
        if (isInWater)
        {
            Vector3 move = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");

            float vertical = 0f;
            if (Input.GetKey(KeyCode.Space)) vertical = 1f;
            if (Input.GetKey(KeyCode.LeftControl)) vertical = -1f;

            move += transform.up * vertical;

            rb.linearVelocity = move.normalized * speed;
        }
        else
        {
            rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);
        }
    }

    void Jump()
    {
        float actualJumpForce = isInWater ? jumpForce * waterJumpMultiplier : jumpForce;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * actualJumpForce, ForceMode.Impulse);
    }

    void CheckWater()
    {
        float surfaceY = waterSurface.position.y;
        float playerY = transform.position.y;

        if (playerY < surfaceY)
        {
            isInWater = true;
            isAtWaterSurface = Mathf.Abs(playerY - surfaceY) < 0.5f; // Justera toleransen!
            

            if (isAtWaterSurface)
            {
                rb.useGravity = false;
                rb.linearDamping = 2f;
                
            }
            else
            {
                rb.useGravity = true;
                rb.linearDamping = 3f;
                rb.AddForce(Vector3.up * 10f); // Flytkraft
            }
        }
        else
        {
            isInWater = false;
            isAtWaterSurface = false;
            rb.useGravity = true;
            rb.linearDamping = 0f;
            
        }
    }

  



    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }

        if (waterSurfaceCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(waterSurfaceCheck.position, surfaceCheckDistance);
        }
    }


}
