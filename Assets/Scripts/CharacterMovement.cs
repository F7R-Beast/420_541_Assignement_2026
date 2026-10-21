using UnityEngine;

// WASD / arrow keys: W/S = forward/backward, A/D = strafe left/right, Space = jump.
[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float sprintSpeedMultiplier = 2f;
    [SerializeField] private float defaultSpeed = 7f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Jump Settings")]
    [SerializeField] private int numOfDoubleJumpTotal = 1;

    [Header("Ground Check Settings")]
    [SerializeField] private float groundDistance = 0.5f;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool isGrounded;
    private bool jumpRequested = false;
    private bool isDoubleJumping = false;

    private bool isSprinting = false;
    

    private int numOfDoubleJumpRemaining;

    public bool IsGrounded => isGrounded;
    public bool IsDoubleJumping => isDoubleJumping;

    private void Start()
    {
        if (cameraTransform == null && Camera.main != null){
            cameraTransform = Camera.main.transform;
        }
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        numOfDoubleJumpRemaining = numOfDoubleJumpTotal;
    }

    private void Update()
    {
        // 1. Ground Check
        isGrounded = Physics.Raycast(transform.position + transform.up * groundDistance / 2, -transform.up, groundDistance, groundMask);

        // 2. Read Inputs
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical"); 

        // Using the camera to decide where forward is
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        moveDirection = (camRight * moveX + camForward * moveZ).normalized;

        // 3. Jump Input
        if (Input.GetButtonDown("Jump") && isGrounded && !isDoubleJumping)
        {
            jumpRequested = true;
        }else if(Input.GetButtonDown("Jump") && !isGrounded && numOfDoubleJumpRemaining > 0){
            jumpRequested = true;
            isDoubleJumping = true;
            numOfDoubleJumpRemaining--;
        }
        
        // 4. Sprint input
        isSprinting = Input.GetButton("Sprint");
        
        if(isGrounded){
            numOfDoubleJumpRemaining = numOfDoubleJumpTotal;
            isDoubleJumping = false;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (jumpRequested)
        {
            Jump();
            jumpRequested = false;
        }
    }

    private void MovePlayer()
    {
        moveSpeed = isSprinting? defaultSpeed * sprintSpeedMultiplier: defaultSpeed;
        Vector3 targetVelocity = moveDirection * moveSpeed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }
}