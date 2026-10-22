using UnityEngine;

// WASD / arrow keys: W/S = forward/backward, A/D = strafe left/right, Space = jump.
[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Charavter Settings")]
    [SerializeField] private float turnSpeed = 720f;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float sprintSpeedMultiplier = 2f;
    [SerializeField] private float defaultSpeed = 7f;

    [Header("Jump Settings")]
    [SerializeField] private int numOfDoubleJumpTotal = 1;
    [SerializeField] private float minJumpForce = 3f;
    [SerializeField] private float maxJumpForce = 9f;
    [SerializeField] private float maxChargeTime = 3f;
    [SerializeField] private float doubleJumpForce = 5f;
    

    [Header("Ground Check Settings")]
    [SerializeField] private float groundDistance = 0.5f;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private Vector3 lookDirection;
    private bool isGrounded;
    private bool jumpRequested = false;

    private bool isCharging = false;
    private float chargeTime = 0f;
    private float queuedJumpForce;

    private float doubleJumpTime = 0f;
    private float doubleJumpEndTime = 0f;
    public bool CanDoubleJump => Time.time < doubleJumpEndTime;

    private bool isSprinting = false;
    private float speedBoostMultiplier = 1f;
    private float speedBoostEndTime = 0f;
    

    private int numOfDoubleJumpRemaining;
    private bool isDoubleJumping = false;
    
    public bool IsGrounded => isGrounded;
    public bool IsDoubleJumping => isDoubleJumping;

    private void Start()
    {
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
        Vector3 camForward = Camera.main.transform.forward;
        
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        lookDirection = camForward;

        moveDirection = (camRight * moveX + camForward * moveZ).normalized;

        if (moveDirection.magnitude > 0.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                targetRotation, 
                turnSpeed * Time.deltaTime);
        }

        // 3. Jump Input
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded && !isDoubleJumping)
            {
                // Start charging
                isCharging = true;
                chargeTime = 0f;
            }
            else if (!isGrounded && CanDoubleJump && numOfDoubleJumpRemaining > 0)
            {
                queuedJumpForce = doubleJumpForce;
                jumpRequested = true;
                isDoubleJumping = true;
                numOfDoubleJumpRemaining--;
                doubleJumpTime = 1.5f;
            }
        }

        if (isCharging)
        {
            if (!isGrounded)
            {
                isCharging = false;
            }
            else
            {
                chargeTime += Time.deltaTime;

                if (chargeTime > maxChargeTime)
                {
                    chargeTime = maxChargeTime;
                }

                // Jump on release, or automatically at max charge time
                if (Input.GetButtonUp("Jump") || chargeTime >= maxChargeTime)
                {
                    float percent = chargeTime / maxChargeTime;
                    queuedJumpForce = minJumpForce + (maxJumpForce - minJumpForce) * percent;
                    jumpRequested = true;
                    isCharging = false;
                }
            }
        }

        if (isDoubleJumping && doubleJumpTime > 0)
        {
            doubleJumpTime -= Time.deltaTime;
        }
        else
        {
            isDoubleJumping = false;
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
            Jump(queuedJumpForce);
            jumpRequested = false;
        }
    }

    private void MovePlayer()
    {
        float boost = Time.time < speedBoostEndTime ? speedBoostMultiplier : 1f;
        moveSpeed = (isSprinting ? defaultSpeed * sprintSpeedMultiplier : defaultSpeed) * boost;
        Vector3 targetVelocity = moveDirection * moveSpeed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }

    private void Jump(float force)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * force, ForceMode.VelocityChange);
    }

    public void EnableDoubleJump(float duration)
    {
        doubleJumpEndTime = Time.time + duration;
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        speedBoostMultiplier = multiplier;
        speedBoostEndTime = Time.time + duration;
    }
}