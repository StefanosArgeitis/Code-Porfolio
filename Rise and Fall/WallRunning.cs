using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("WallRun")]
    public LayerMask is_wall;               // Layer mask to identify walls for wall running.
    public LayerMask is_ground;             // Layer mask to identify ground.
    public float wallRunForce;              // Force applied while running on the wall.
    public float wallJumpUpForce;           // Upward force applied during a wall jump.
    public float wallJumpSideForce;         // Side force applied during a wall jump.
    public float wallClimbSpeed;            // Speed for climbing up or down the wall.
    public float maxWallRunTime;            // Maximum duration for wall running.
    private float wallRunTimer;             // Timer to keep track of remaining wall run time.

    [Header("Inputs")]
    public KeyCode jumpKey = KeyCode.Space; // Key to initiate wall jump.
    public KeyCode upwardsRunKey = KeyCode.LeftShift; // Key to run upwards on the wall.
    public KeyCode downwardsRunKey = KeyCode.LeftControl; // Key to run downwards on the wall.
    private bool upwardsRunning;            // Flag for running upwards.
    private bool downwardsRunning;          // Flag for running downwards.
    private float horizontalInput;          // Horizontal input from the player (left/right).
    private float verticalInput;            // Vertical input from the player (forward/backward).

    [Header("Detect")]
    public float wallCheckDistance;         // Distance for checking walls.
    public float minJumpHeight;             // Minimum height required to start a wall run.
    private RaycastHit leftWallhit;         // Information about the wall hit on the left side.
    private RaycastHit rightWallhit;        // Information about the wall hit on the right side.
    private bool wallLeft;                  // Flag to indicate if there's a wall on the left.
    private bool wallRight;                 // Flag to indicate if there's a wall on the right.

    [Header("Exiting")]
    private bool exitingWall;               // Flag for exiting the wall run.
    public float exitWallTime;              // Duration to prevent re-entry into wall run after exiting.
    private float exitWallTimer;            // Timer to keep track of exit duration.

    [Header("Gravity")]
    public bool useGravity;                 // Flag to determine if gravity should be used during wall run.
    public float gravityCounterForce;       // Force to counteract gravity during wall run.

    [Header("References")]
    public Transform orientation;           // Reference to the player's orientation (forward direction).
    public FP_Camera cam;                   // Reference to the first-person camera script.
    private Player_Movement pm;             // Reference to the player movement script.
    private Rigidbody rb;                   // Reference to the Rigidbody component for physics-based movement.

    // Start is called before the first frame update
    void Start(){
        // Initialize the Rigidbody and Player_Movement references.
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<Player_Movement>();
    }

    // Update is called once per frame
    void Update(){
        // Check for walls around the player.
        CheckForWall();
        // Manage the wall running state.
        StateMachine();
    }

    // FixedUpdate is called every fixed framerate frame (used for physics calculations)
    private void FixedUpdate()
    {
        // Handle movement during wall running if the player is wall running.
        if (pm.wallrunning){
            WallRunningMovement();
        }
    }

    // Checks for walls on the left and right sides of the player.
    private void CheckForWall(){
        // Cast rays to detect walls on both sides.
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, is_wall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, is_wall);
    }

    // Checks if the player is above the ground by a minimum height.
    private bool AboveGround(){
        // Returns true if there's no ground detected within the minimum jump height distance.
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, is_ground);
    }

    // Manages the state transitions for wall running.
    private void StateMachine(){
        // Capture player input.
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Check if the player is trying to run upwards or downwards.
        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        // Wall running conditions:
        if((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall){
            // Start wall running if conditions are met and the player isn't already wall running.
            if (!pm.wallrunning && pm.wallrunningAllowed){
                StartWallRun();
            }

            // Decrease the wall run timer.
            if (wallRunTimer > 0){
                wallRunTimer -= Time.deltaTime;
            }

            // Start exiting wall run if the timer runs out.
            if(wallRunTimer <= 0 && pm.wallrunning){
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            // Handle wall jump input.
            if (Input.GetKeyDown(jumpKey)){
                WallJump();
            }

        } else if (exitingWall){  // Exiting the wall run state.
            // Stop wall running.
            if (pm.wallrunning){
                StopWallRun();
            }

            // Decrease the exit timer.
            if (exitWallTimer > 0){
                exitWallTimer -= Time.deltaTime;
            }

            // Allow re-entry to wall run after the exit timer ends.
            if (exitWallTimer <= 0){
                exitingWall = false;
            }

        } else {
            // Stop wall running if conditions are not met.
            if (pm.wallrunning){
                StopWallRun();
            }
        }
    }

    // Starts the wall running state.
    private void StartWallRun(){
        // Set the wall running state to true.
        pm.wallrunning = true;

        // Reset the wall run timer to the maximum duration.
        wallRunTimer = maxWallRunTime;

        // Zero out the vertical velocity to maintain constant height during wall run.
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Apply camera tilt effects depending on the wall side.
        if (wallLeft) cam.DoTilt(-5f);
        if (wallRight) cam.DoTilt(5f);
    }

    // Manages movement while wall running.
    private void WallRunningMovement(){
        // Enable or disable gravity based on the configuration.
        rb.useGravity = useGravity;

        // Determine the normal of the wall surface.
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        // Calculate the forward direction for wall running.
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        // Ensure the player runs forward along the wall.
        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude){
            wallForward = -wallForward;
        }

        // Apply forward force to move along the wall.
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // Apply vertical movement forces based on input.
        if (upwardsRunning){
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        }

        if (downwardsRunning){
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);
        }

        // Push the player towards the wall to keep them attached.
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0)){
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }

        // Apply counter-force to weaken gravity if required.
        if (useGravity){
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
        }
    }

    // Stops the wall running state.
    private void StopWallRun(){
        // Set the wall running state to false.
        pm.wallrunning = false;

        // Reset the camera tilt to normal.
        cam.DoTilt(0f);
    }

    // Handles wall jumping mechanics.
    private void WallJump(){
        // Set exiting state to prevent re-entry into wall running.
        exitingWall = true;
        exitWallTimer = exitWallTime;

        // Determine the normal of the wall surface.
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        // Calculate the force to apply for wall jump.
        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        // Reset the vertical velocity and apply the jump force.
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
