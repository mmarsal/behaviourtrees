using System.Collections;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float maxYSpeed;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMuliplier;
    bool readyToJump;
    public PlayerCam playerCamScript;
    public GameManager gameManager;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    public float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround; // Ensure this is set to NavMesh walkable area
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;
    private bool canJump;
    private bool doubleJump;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    private AudioSource audioSource;
    public MovementState state;

    public enum MovementState
    {
        freeze,
        unlimited,
        walking,
        sprinting,
        crouching,
        air,
    }

    public bool freeze;
    public bool unlimited;
    public bool restricted;

    [Header("Hiding")]
    public LayerMask whatIsHidespot;
    public bool hiding;
    public bool exposed = false;
    private Hidespot hidespotScript;
    private GameObject alien;
    private BehaviorTree behaviorTree;
    public BehaviorTree doppelgangerTree;

    void Start()
    {
        alien = GameObject.Find("AIThirdPersonController");
        behaviorTree = alien.GetComponent<BehaviorTree>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        doubleJump = false;
        startYScale = transform.localScale.y;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 0.28f;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        hiding = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, playerHeight * 0.5f + 0.2f, whatIsHidespot);
        behaviorTree.SetVariableValue("playerIsHiding", hiding);
        doppelgangerTree.SetVariableValue("playerIsHiding", hiding);
        if (exposed)
        {
            hiding = false;
            behaviorTree.SetVariableValue("playerIsHiding", false);
        }
        if (hiding)
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != null)
            {
                hidespotScript = hitObject.GetComponent<Hidespot>();
                if (hidespotScript != null)
                {
                    hidespotScript.playerIsHiding = true;
                }
            }
            else
            {
                Debug.Log("The hit object does not have a parent.");
            }
        }
        else
        {
            if (hidespotScript != null)
            {
                hidespotScript.playerIsHiding = false;
            }
        }

        if (!freeze)
        {
            MyInput();
            SpeedControl();
            StateHandler();
        }
    }

    public void PlayHitSound(AudioClip hitSound)
    {
        audioSource.PlayOneShot(hitSound);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetKeyDown(jumpKey) && doubleJump && !grounded)
        {
            doubleJump = false;
            Jump();
        }

        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MovementState lastState;
    private bool keepMomentum;

    private void StateHandler()
    {
        if (freeze)
        {
            state = MovementState.freeze;
            rb.velocity = Vector3.zero;
            return;
        }

        if (unlimited)
        {
            state = MovementState.unlimited;
            desiredMoveSpeed = 999f;
            return;
        }

        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;

            if (desiredMoveSpeed < sprintSpeed)
                desiredMoveSpeed = walkSpeed;
            else
                desiredMoveSpeed = sprintSpeed;
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;

        if (desiredMoveSpeedHasChanged)
        {
            StopAllCoroutines();
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (state != MovementState.freeze)
        {
            if (grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

            if (OnSlope() && !exitingSlope)
            {
                rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

                if (rb.velocity.y > 0)
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
            else if (!grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMuliplier, ForceMode.Force);

            rb.useGravity = !OnSlope();
        }
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
    }

    public void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
        doubleJump = true;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public void GameOver()
    {
        // Disable player movement and mouse input
        freeze = true;
        playerCamScript.enabled = false;

        // Calculate the direction to the alien
        Vector3 directionToAlien = (alien.transform.position - transform.position).normalized;

        // Remove vertical component for horizontal rotation only
        directionToAlien.y = 0;

        // Check if the direction is valid (not zero)
        if (directionToAlien != Vector3.zero)
        {
            // Smoothly rotate the player to face the alien
            Quaternion targetRotation = Quaternion.LookRotation(directionToAlien);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        // Make the camera look a bit up
        Vector3 targetCameraRotation = new(10f, playerCamScript.transform.localRotation.eulerAngles.y, 0f);
        playerCamScript.transform.localRotation = Quaternion.Slerp(playerCamScript.transform.localRotation, Quaternion.Euler(targetCameraRotation), Time.deltaTime * 2f);

        gameManager.GameOver();
    }
}
