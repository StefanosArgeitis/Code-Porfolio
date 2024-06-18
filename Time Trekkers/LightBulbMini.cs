using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LightBulbMini : MonoBehaviour
{
    [SerializeField] private Transform top_Pivot;  // Top pivot point of the mini game
    [SerializeField] private Transform bottom_Pivot;  // Bottom pivot point of the mini game
    [SerializeField] private Transform mini_objective;  // Mini game objective transform

    public float mini_obj_pos;  // Current position of the mini game objective (0 to 1)
    public float mini_obj_des;  // Desired position of the mini game objective (0 to 1)
    public Narrator nar;

    public float mini_obj_timer;  // Timer for changing the desired position of the mini game objective
    [SerializeField] private float timer_multiplier = 3f;  // Multiplier for the mini game timer
    public float mini_obj_speed;  // Speed at which the mini game moves
    [SerializeField] private float smooth_motion = 1f;  // Smoothness factor for the motion of the mini game objective

    [SerializeField] private Transform contr;  // Controller transform
    public float contr_pos;  // Current position of the controller (0 to 1)
    public float contr_pos_start = 0.0575f;  // Starting position of the controller
    [SerializeField] private float contr_size = 0.1f;  // Size of the controller
    [SerializeField] private float contr_power = 0.5f;  // Power of the controller
    public float contr_progress;  
    public float contr_pull_vel; 
    [SerializeField] private float contr_pull_power = 0.01f;  // Power of the controller movement
    [SerializeField] private float contr_gravity = 0.005f;  // Gravity affecting the controller
    [SerializeField] private float contr_progress_degrade = 0.1f;  // Rate at which the controller progress degrades
    [SerializeField] private float mini_off_time = 3f;  // Time before the mini game turns off

    [SerializeField] Transform progress_bar_container;  // Progress bar container transform
    [SerializeField] private GameObject failed_txt;  
    [SerializeField] private GameObject success_txt;  
    [SerializeField] private GameObject invHuD; 

    public static event HandleLightBulbCollected OnLightBulbCollected;  // Event triggered when a light bulb is collected
    public delegate void HandleLightBulbCollected(ItemData itemData);
    public ItemData itemData;  // Data of the light bulb item

    public bool pause = false;  // Flag to pause the mini game
    public bool pause_char = false;  // Flag to pause the character
    public LightBulb bulb;  
    public SwitchCamera switchCamera;  
    public GameObject smoke; 
    public GameObject lightbulb;  
    public GameObject lightbulb_comps;  
    [SerializeField] float fail_time = 10f;  // Time before the mini game fails
    public bool starting = true; 
    //public bool can_fail = false;

   // Update is called once per frame
void Update()
{
    if (pause){
        return; // If the game is paused, exit the function
    }

    if (starting){
        miniStart(); 
        starting = false;
    }

    invHuD.SetActive(false); // Disable the inventory HUD
    Mini_Controller(); // Call the Mini_Controller() function to control the mini game objective
    Player_Control_Mini(); // Call the Player_Control_Mini() function to control the player's input
    ProgressCheck(); // Check the progress of the mini game
}

    public void miniStart(){
        pause_char = true; 
        smoke.SetActive(true); 
    }

    void Mini_Controller(){

        mini_obj_timer -= Time.deltaTime;

        if (mini_obj_timer < 0f){
            mini_obj_timer = UnityEngine.Random.value * timer_multiplier; // Set a random value for the mini game timer

            mini_obj_des = UnityEngine.Random.value; // Set a random value for the desired position of the mini game objective

        }

        // Ensure that the desired position of the mini game objective is within bounds
        if (mini_obj_des < contr_pos_start){
            mini_obj_des = contr_pos_start;
        }

        if (mini_obj_des > 1f - contr_pos_start){
            mini_obj_des = 1f - contr_pos_start;
        }

        mini_obj_pos = Mathf.SmoothDamp(mini_obj_pos, mini_obj_des, ref mini_obj_speed, smooth_motion); // Smoothly move the mini game objective
        mini_objective.position = Vector3.Lerp(bottom_Pivot.position, top_Pivot.position, mini_obj_pos); // Set the position of the mini game objective based on the current position
    }

    void Player_Control_Mini(){

        if (Input.GetMouseButton(0)){
            contr_pull_vel += contr_pull_power * Time.deltaTime; // Increase the velocity of the controller pull when the primary button is pressed
            //Debug.Log("Pressed primary button.");
        }

        contr_pull_vel -= contr_gravity * Time.deltaTime; // Apply gravity to the controller pull velocity
        contr_pos += contr_pull_vel; // Update the position of the controller based on the pull velocity

        // Ensure that the controller position stays within bounds
        if (contr_pos <= contr_pos_start && contr_pull_vel < 0f){
            contr_pull_vel = 0f;
        }

        if (contr_pos >= 1f - contr_pos_start && contr_pull_vel > 0f){
            contr_pull_vel = 0f;
        }

        contr_pos = Mathf.Clamp(contr_pos, contr_size / 2, 1 - contr_size / 2); // Clamp the controller position within the valid range
        contr.position = Vector3.Lerp(bottom_Pivot.position, top_Pivot.position, contr_pos); // Set the position of the controller based on the current position
    }

    void ProgressCheck(){

        Vector3 v3 = progress_bar_container.localScale;
        v3.y = contr_progress;
        progress_bar_container.localScale = v3; // Update the scale of the progress bar based on the controller progress

        float min = contr_pos - contr_size / 2;
        float max = contr_pos + contr_size / 2;
        fail_time -= Time.deltaTime; // Decrease the fail time

        // Check if the mini game objective is within the controller range
        if (min < mini_obj_pos && mini_obj_pos < max){
            contr_progress += contr_power * Time.deltaTime; // Increase the controller progress

        } else{
            contr_progress -= contr_progress_degrade * Time.deltaTime; // Decrease the controller progress

            // Check if the controller progress is zero and fail time has run out
            if (contr_progress <= 0f && fail_time < 0f){
                Lose(); // Call the Lose() function
            }

        }

        // Check if the controller progress is maximum
        if (contr_progress >= 1f){
            Win(); // Call the Win() function
        }

        contr_progress = Mathf.Clamp(contr_progress, 0f, 1f); // Clamp the controller progress within the valid range
        //Debug.Log(fail_time);
    }

    private void Lose(){
        pause = true; // Set pause to true
        failed_txt.SetActive(true); 
        switchCamera.ChangeCam(); // Change the camera view
        smoke.SetActive(false); 
        StartCoroutine("MinigameOff"); // Start the coroutine for turning off the mini game
    }

    private void Win(){

        pause = true; // Set pause to true
        lightbulb_comps.SetActive(false); 
        smoke.SetActive(false); 
        lightbulb.SetActive(true); 
        success_txt.SetActive(true); 
        switchCamera.ChangeCam(); // Change the camera view
        StartCoroutine("MinigameOffWin"); // Start the coroutine for turning off the mini game after winning

        // Play appropriate narrator line
        if (!bulb.firstBulb){
            Debug.Log("second Bulb");
            bulb.secondBulb = true;
            nar.PlayElectroMag(); 
        }

        if (bulb.firstBulb){
            Debug.Log("first Bulb");
            bulb.firstBulb = false;
            nar.PlayLightComps(); 
        }

    }

    public IEnumerator MinigameOff(){

        yield return new WaitForSeconds(mini_off_time); // Wait for a specified time before turning off the mini game
        ResetMinigame(); // Call the ResetMinigame() function
        smoke.SetActive(false); 
    }

    public IEnumerator MinigameOffWin(){

        yield return new WaitForSeconds(mini_off_time); // Wait for a specified time before turning off the mini game after winning
        ResetMinigame(); // Call the ResetMinigame() function
        OnLightBulbCollected?.Invoke(itemData); // Invoke the event for collecting the light bulb
        lightbulb.SetActive(false); // Deactivate the light bulb
    }

    private void ResetMinigame()
    {
        // Reset all the variables and game objects to their initial state
        mini_obj_pos = 0.5f;
        mini_obj_des = 0f;
        mini_obj_timer = 0f;
        mini_obj_speed = 0f;
        contr_pos = contr_pos_start;
        contr_progress = 0f;
        contr_pull_vel = 0f;
        fail_time = 10f;
        starting = true;
        pause = false;
        pause_char = false;
        failed_txt.SetActive(false);
        success_txt.SetActive(false);
        invHuD.SetActive(true);
        gameObject.SetActive(false);
    }


}
