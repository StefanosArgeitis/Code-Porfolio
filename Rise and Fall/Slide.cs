using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    // Header attributes for organization in the Unity Inspector
    [Header("Reference")]
    public Transform orientation;   // Reference to the player's orientation (forward direction).
    public Transform playerObj;     // Reference to the player's transform (for scaling and positioning).
    private Rigidbody rb;           // Rigidbody component for physics-based movement.
    private Player_Movement pm;     // Reference to the Player_Movement script.

    [Header("Slide")]
    public float slideMaxTime;      // Maximum duration for a slide.
    public float slideForce;        // Force applied during a slide.
    private float slideTimer;       // Timer to keep track of slide duration.

    [Header("Inputs")]
    public KeyCode slideKey = KeyCode.LeftControl; // Key to initiate slide.
    private float horizontalInput;  // Horizontal input from the player (left/right).
    private float verticalInput;    // Vertical input from the player (forward/backward).

    public float slideYScale;       // Y scale for the player during the slide (to simulate crouching).
    private float startYScale;      // Initial Y scale of the player (before sliding).
    private RaycastHit roofHit;     // Raycast hit for checking obstacles above the player.
    public float playerHeight;      // Height of the player (used for raycasting).

    // Start is called before the first frame update
    private void Start() {
        // Initialize the Rigidbody and Player_Movement references.
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<Player_Movement>();

        // Store the initial Y scale of the player object.
        startYScale = playerObj.localScale.y;
    }

    // Update is called once per frame
    private void Update() {
        // Capture horizontal and vertical input.
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Start sliding if the slide key is pressed and there's movement input.
        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0)){
            StartSlide();
        }

        // Stop sliding if the slide key is released and the player is currently sliding.
        if (Input.GetKeyUp(slideKey) && pm.sliding){
            StopSlide();
        }
    }

    // FixedUpdate is called every fixed framerate frame (used for physics calculations)
    private void FixedUpdate() {
        // Continue sliding movement if the player is in sliding state.
        if (pm.sliding){
            SlideMovement();
        }
    }

    // Handles the sliding movement and mechanics.
    private void SlideMovement() {
        // Determine the direction of the slide based on player input and orientation.
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Sliding mechanics on different terrains.
        if (pm.OnSlope() || rb.velocity.y > -0.1f) {
            // Slide on ground or up a slope.
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            // Decrease the slide timer.
            slideTimer -= Time.deltaTime;
        } else {
            // Slide down a slope.
            rb.AddForce(pm.getSlopeMoveDir(inputDirection) * slideForce, ForceMode.Force);
        }

        // Stop sliding when the slide timer runs out.
        if (slideTimer <= 0){
            StopSlide();
        }
    }

    // Initiates the slide.
    private void StartSlide() {
        // Set sliding state to true.
        pm.sliding = true;

        // Change the player's Y scale to simulate a crouching posture.
        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        // Apply a downward force to quickly lower the player's height.
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        // Reset the slide timer to the maximum slide duration.
        slideTimer = slideMaxTime;
    }

    // Stops the slide.
    private void StopSlide() {
        // Set sliding state to false.
        pm.sliding = false;
        // Reset the player's Y scale to the original value.
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }
}
