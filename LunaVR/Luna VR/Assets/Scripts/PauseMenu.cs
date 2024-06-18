using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
 
    public static bool GameIsPaused = false; //0 or 1 if game is paused
    
    public GameObject PauseMenuUI; //triggers UI

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //checks ESC for being pressed
        {
            if (GameIsPaused) 
            {
                Resume(); //calls method
            }
            else
            {
                Pause(); //above
            }


        }    


    }

    public void Resume () //public so return button can get the code
    {
    PauseMenuUI.SetActive(false); //if UI is shown
    Time.timeScale = 1f; //speed set in hexadecimal
    GameIsPaused = false; //checking condition 
    }


    void Pause () 
    {
    PauseMenuUI.SetActive(true); 
    Time.timeScale = 0f;
    GameIsPaused = true;

    }


    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu"); //load main menu on button click
    }

    public void QuitGame()
    {
        Debug.Log("Working IG");
        Application.Quit(); //commits taskkill aka self death - Stef
    }

}
