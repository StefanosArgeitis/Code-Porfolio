using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Climbing : MonoBehaviour
{
    [Header("Misc")]
    public Transform orientation;           // Reference to the player's orientation (typically, the forward direction).
    public Rigidbody rb;                    // Reference to the player's Rigidbody for physics-based movement.
    public LayerMask whatIsGround;          // Layer mask used to identify climbable surfaces (ground/walls).
    public Player_Movement pm;              // Reference to the player movement script.

    [Header("W_Climbing")]
    public float climbSpeed;                // Speed at which the player climbs the wall.
    public float climbMaxTime;              // Maximum time the player can climb continuously.
    private float climbTimer;               // Timer to track the remaining climb time.

    private bool climbing;                  // Flag to indicate if the player is currently climbing.

    [Header("W_Detection")]
    public float detectLength;              // Distance to detect walls for climbing.
    public float sphereCastRadius;          // Radius of the sphere cast used to detect walls.
    public float maxWallAngle;              // Maximum angle of the wall that can be climbed.
    private float wallAngle;                // Angle of the detected wall relative to the player.

    [Header("W_Exit")]
    public bool exitWall;                   // Flag to indicate if the player is exiting the wall climb.
    public float exitWallTime;              // Time for which the player can't climb after exiting.
    private float exitWallTimer;            // Timer to track the exit duration.

    private RaycastHit frontWallHit;        // Information about the wall detected in front of the player.
    private bool frontWall;                 // Flag to indicate if there is a wall in front of the player.
    public KeyCode jumpKey = KeyCode.Space; // Key used to jump while climbing.
    public int jumps;                       // Number of jumps the player can make while climbing.
    public int jumpsLeft;                   // Number of remaining jumps while climbing.
    private Transform lastWall;             // Reference to the last wall the player climbed.
    private Vector3 lastWallNormal;         // Normal vector of the last wall climbed.
    public float wallNormalAngleChange;     // Minimum angle change in wall normal to reset climb timer.

    [Header("Jump")]
    public float jumpUpForce;               // Upward force applied during a wall climb jump.
    public float jumpBackForce;             // Backward force applied during a wall climb jump.

    // Called once per frame to handle updates.
    private void Update() {
        WallCheck();                       // Check for walls in front of the player.
        StateHandler();                    // Manage the climbing state based on input and conditions.

        // If the player is climbing and not exiting the wall, handle climbing movement.
        if (climbing && !exitWall){
            W_ClimbingMovement();
        }
    }

    // Manages the state transitions for climbing.
    private void StateHandler(){
        // If the player is pressing the 'W' key, there's a wall in front, and it's climbable:
        if ((frontWall && Input.GetKey(KeyCode.W)) && wallAngle < maxWallAngle && !exitWall){
            // Start climbing if not already climbing and the climb timer hasn't run out.
            if (!climbing && climbTimer > 0){
                Start_W_Climbing();
            }

            // Decrease the climb timer.
            if (climbTimer > 0){
                climbTimer -= Time.deltaTime;
            }

            // Stop climbing if the timer runs out.
            if (climbTimer < 0){
                Stop_W_Climbing();
            }

        } else if(exitWall){  // Handle exiting the climb.
            // Stop climbing if the player is still climbing.
            if (climbing){
                Stop_W_Climbing();
            }

            // Decrease the exit wall timer.
            if (exitWallTimer > 0){
                exitWallTimer -= Time.deltaTime;
            }

            // Allow climbing again once the exit timer ends.
            if (exitWallTimer < 0){
                exitWall = false;
            }

        } else {  // If none of the above conditions are met:
            // Stop climbing if the player is still climbing.
            if (climbing){
                Stop_W_Climbing();
            }
        }

        // Handle jump input while climbing.
        if(frontWall && Input.GetKeyDown(jumpKey) && jumpsLeft > 0){
            W_ClimbJump();
        }
    }

    // Checks for walls in front of the player to detect climbable surfaces.
    private void WallCheck(){
        // Use a sphere cast to detect walls in front of the player.
        frontWall = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectLength, whatIsGround);

        // Calculate the angle of the detected wall relative to the player's orientation.
        wallAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);
        
        // Check if the player is detecting a new wall or has changed enough in normal to reset the climb timer.
        bool newWall = frontWallHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > wallNormalAngleChange;

        // Reset the climb timer and jumps if the player detects a new wall or is grounded.
        if ((frontWall && newWall) || pm.Grounded){
            climbTimer = climbMaxTime;
            jumpsLeft = jumps;
        }
    }

    // Handles the player's movement while climbing.
    private void W_ClimbingMovement(){
        // Set the vertical velocity to the climb speed to simulate climbing up.
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
    }

    // Handles the player's jump while climbing.
    private void W_ClimbJump(){
        // Calculate the force to apply for the jump based on the jump forces and wall normal.
        Vector3 forceApplied = transform.up * jumpUpForce + frontWallHit.normal * jumpBackForce;

        // Reset the vertical velocity and apply the calculated jump force.
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceApplied, ForceMode.Impulse);

        // Set the exit state and reset the exit timer.
        exitWall = true;
        exitWallTimer = exitWallTime;

        // Decrease the number of remaining jumps.
        jumpsLeft--;
    }

    // Starts the wall climbing state.
    private void Start_W_Climbing(){
        // Set the climbing state to true.
        climbing = true;
        pm.w_climbing = true;  // Update the player movement state.

        // Store the reference to the current wall and its normal.
        lastWall = frontWallHit.transform;
        lastWallNormal = frontWallHit.normal;
    }

    // Stops the wall climbing state.
    private void Stop_W_Climbing(){
        // Set the climbing state to false.
        climbing = false;
        pm.w_climbing = false; // Update the player movement state.
    }
}
