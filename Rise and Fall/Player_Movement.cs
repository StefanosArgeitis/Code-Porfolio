using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;           // Current movement speed of the player.
    public float walkSpeed;           // Speed when the player is walking.
    public float sprintSpeed;         // Speed when the player is sprinting.
    public float groundDrag;          // Drag applied to the player when grounded.
    public float slideSpeed;          // Speed when the player is sliding.
    public float wallrunSpeed;        // Speed when the player is wall running.
    public float climbSpeed;          // Speed when the player is climbing.

    private float desMoveSpeed;       // Desired movement speed based on current action.
    private float finalDesMoveSpeed;  // Final desired movement speed for smooth transition.
    public float speedIncreaseMult;   // Multiplier for speed increase during transitions.
    public float slopeIncreaseMult;   // Multiplier for speed increase on slopes.

    [Header("Jumping")]
    public float jumpForce;           // Force applied when the player jumps.
    public float JumpCooldown;        // Cooldown time between jumps.
    public float airMultiplier;       // Movement speed multiplier when in the air.
    bool ready_to_jump = true;        // Indicates if the player is ready to jump.

    [Header("Crouching")]
    public float crouchSpeed;         // Speed when the player is crouching.
    public float crouchYScale;        // Y scale of the player when crouching.
    private float startYScale;        // Original Y scale of the player.
    bool crouching = false;           // Indicates if the player is currently crouching.
    private RaycastHit roofHit;       // Raycast hit information for detecting overhead obstacles.

    [Header("Keybinds")]
    public KeyCode jumpkey = KeyCode.Space;      // Key to jump.
    public KeyCode runkey = KeyCode.LeftShift;   // Key to sprint.
    public KeyCode crouchkey = KeyCode.C;        // Key to crouch.

    [Header("CheckForGround")]
    public float playerHeight;        // Height of the player for ground detection.
    public LayerMask is_ground;       // Layer mask used to identify ground.
    public bool Grounded;             // Indicates if the player is currently grounded.

    [Header("Slope Handler")]
    public float maxSlopeAngle;       // Maximum angle considered as a slope.
    private RaycastHit slopeHit;      // Raycast hit information for slope detection.
    public float slopeSpeedMod;       // Speed modifier when moving on slopes.
    private bool exitSlope;           // Indicates if the player is exiting a slope.

    /*
    [Header ("Step Climb")]
    public GameObject stepUpper;
    public GameObject stepLower;
    public float stepHeight;
    public float stepSmoothness;
    */

    public Transform orientation;     // Transform representing the player's orientation.

    float horizontalInput;            // Horizontal input from the player.
    float verticalInput;              // Vertical input from the player.

    public bool w_climbing;           // Indicates if the player is wall climbing.
    public bool sliding;              // Indicates if the player is sliding.
    public bool wallrunning;          // Indicates if the player is wall running.
    public bool wallrunningAllowed;   // Indicates if wall running is currently allowed.

    Vector3 moveDirection;            // Direction in which the player will move.

    Rigidbody rb;                     // Rigidbody component of the player.

    public Wall_Climbing wc;          // Reference to the wall climbing component.

    public MovementState state;       // Current movement state of the player.

    // Enumeration to represent different movement states.
    public enum MovementState
    {
        walking,
        sprinting,
        w_climbing,
        wallrunning,
        crouching,
        sliding,
        air
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Get the Rigidbody component attached to this GameObject.
        rb = GetComponent<Rigidbody>();
        // Freeze rotation to prevent unwanted rotation from physics interactions.
        rb.freezeRotation = true;

        // Set the initial state for jumping.
        ready_to_jump = true;

        // Store the initial Y scale of the player.
        startYScale = transform.localScale.y;

        // Optionally set the step upper position (currently commented out).
        // stepUpper.transform.position = new Vector3(stepUpper.transform.position.x, stepHeight, stepUpper.transform.position.z);
    }

    // FixedUpdate is called every fixed framerate frame
    private void FixedUpdate()
    {
        // Handle player movement in a physics update loop.
        MovePlayer();
        // Step climb handling (currently commented out).
        // stepClimb();

        // Calculate the player's speed in the horizontal plane.
        Vector3 speed = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        Debug.Log("Speed: " + (speed.magnitude).ToString("F2"));
        // Debugging slope detection (currently commented out).
        // Debug.Log(OnSlope());
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the player is grounded using a raycast downwards.
        Grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, is_ground);

        // Handle player input for movement.
        P_Inputs();
        // Control player speed based on the current movement state.
        Speed_Control();
        // Determine and set the player's movement state.
        StateHandle();

        // Set drag based on whether the player is grounded.
        if (Grounded)
        {
            rb.drag = groundDrag;
            wallrunningAllowed = true;
        }
        else
        {
            rb.drag = 0;
        }
    }

    // Handles player input for movement and actions.
    private void P_Inputs()
    {
        // Get horizontal and vertical input.
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Jumping logic.
        if (Input.GetKey(jumpkey) && ready_to_jump && Grounded)
        {
            ready_to_jump = false;  // Prevents multiple jumps until reset.
            Jump();
            // Reset the jump state after the cooldown period.
            Invoke(nameof(JumpReset), JumpCooldown);
        }

        // Crouching logic.
        if (Input.GetKeyDown(crouchkey))
        {
            // Toggle crouching only if there is enough space above the player.
            if (crouching && !Physics.Raycast(transform.position, Vector3.up, out roofHit, playerHeight * 0.75f + 0.3f))
            {
                // Stand up if crouching.
                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
                crouching = false;
            }
            else
            {
                // Crouch if not crouching.
                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                rb.AddForce(Vector3.down * 7f, ForceMode.Impulse); // Add downward force to simulate quick crouch.
                crouching = true;
            }
        }
    }

    // Handles the player's movement state and speed adjustments.
    private void StateHandle()
    {
        if (w_climbing)
        {
            // Wall climbing state.
            state = MovementState.w_climbing;
            desMoveSpeed = climbSpeed;
            wallrunningAllowed = false;
        }
        else if (sliding)
        {
            // Sliding state.
            state = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f)
            {
                // Sliding on a slope.
                desMoveSpeed = slideSpeed;
            }
            else
            {
                // Sliding on flat ground.
                desMoveSpeed = sprintSpeed;
            }
        }
        else if (crouching)
        {
            // Crouching state.
            state = MovementState.crouching;
            desMoveSpeed = crouchSpeed;
        }
        else if (Grounded && Input.GetKey(runkey))
        {
            // Sprinting state.
            state = MovementState.sprinting;
            desMoveSpeed = sprintSpeed;
        }
        else if (Grounded)
        {
            // Walking state.
            state = MovementState.walking;
            desMoveSpeed = walkSpeed;
        }
        else
        {
            // Airborne state.
            state = MovementState.air;
        }

        if (wallrunning)
        {
            // Wall running state.
            state = MovementState.wallrunning;
            desMoveSpeed = wallrunSpeed;
        }

        // Smoothly interpolate to the desired movement speed.
        if (Mathf.Abs(desMoveSpeed - finalDesMoveSpeed) > 6f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desMoveSpeed;
        }

        // Set the final desired speed for future reference.
        finalDesMoveSpeed = desMoveSpeed;
    }

    // Coroutine to smoothly interpolate the player's movement speed.
    private IEnumerator SmoothLerpMoveSpeed()
    {
        float time = 0;
        float dif = Mathf.Abs(desMoveSpeed - moveSpeed);
        float startVal = moveSpeed;

        while (time < dif)
        {
            moveSpeed = Mathf.Lerp(startVal, desMoveSpeed, time / dif);

            if (OnSlope())
            {
                // Adjust speed increase based on slope angle.
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float angleIncrease = 1 + (angle / 90f);

                time += Time.deltaTime * speedIncreaseMult * slopeIncreaseMult * angleIncrease;
            }
            else
            {
                // Regular speed increase.
                time += Time.deltaTime * speedIncreaseMult;
            }

            yield return null;
        }

        // Set the movement speed to the desired speed after interpolation.
        moveSpeed = desMoveSpeed;
    }

    // Method to handle player movement based on current inputs and states.
    private void MovePlayer()
    {
        // Prevent movement if exiting wall climbing.
        if (wc.exitWall)
        {
            return;
        }

        // Determine the move direction based on orientation and input.
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitSlope)
        {
            // Apply movement force while on a slope.
            rb.AddForce(getSlopeMoveDir(moveDirection) * moveSpeed * slopeSpeedMod, ForceMode.Force);

            if (rb.velocity.y != 0)
            {
                // Apply downward force to keep the player grounded on slopes.
                rb.AddForce(Vector3.down * 60f, ForceMode.Force);
            }
        }

        if (Grounded)
        {
            // Apply movement force while grounded.
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!Grounded)
        {
            // Apply movement force while in the air.
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
        }

        if (!wallrunning)
        {
            // Use gravity if not wall running.
            rb.useGravity = !OnSlope();
        }
    }

    // Controls the player's speed to prevent exceeding the maximum allowed speed.
    private void Speed_Control()
    {
        if (OnSlope() && !exitSlope)
        {
            // Limit speed while on a slope.
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            // Limit speed on ground and in the air.
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitVelocity.x, rb.velocity.y, limitVelocity.z);
            }
        }
    }

    // Method to handle jumping logic.
    private void Jump()
    {
        Grounded = false;  // Player is no longer grounded.
        exitSlope = true;  // Player is exiting the slope.

        // Reset vertical velocity for consistent jump height.
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Apply upward force to make the player jump.
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // Resets the jump state after a cooldown period.
    private void JumpReset()
    {
        ready_to_jump = true;
        exitSlope = false;
    }

    // Determines if the player is on a slope and returns true if the angle is within the acceptable range.
    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    // Returns the direction for movement adjusted for the slope's angle.
    public Vector3 getSlopeMoveDir(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    ////// TODO:
    /*
    private void stepClimb()
    {
        if (moveDirection != Vector3.zero && !OnSlope())
        {
            // Add step climbing logic here.
        }

        // RaycastHit hitLower;
        // if (Physics.Raycast(stepLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.2f))
    }
    */
}
