using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseLogic : MonoBehaviour
{

    public GameObject wristUI;
    public GameObject rayInteractor;
    public GameObject rightHandInteractor;

    public bool activeWristUI = true;

    void Start()
    {
        DisplayWristUI();
    }

    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DisplayWristUI();
        }
    }

    public void DisplayWristUI()
    {
        if (activeWristUI)
        {
            AudioListener.pause = false;
            rightHandInteractor.SetActive(true);
            rayInteractor.SetActive(false);
            wristUI.SetActive(false);
            activeWristUI = false;
            Time.timeScale = 1f;
        }
        else if (!activeWristUI)
        {
            AudioListener.pause = true;
            rightHandInteractor.SetActive(false);
            rayInteractor.SetActive(true);
            wristUI.SetActive(true);
            activeWristUI = true;
            Time.timeScale = 0f;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Main Menu");
    }
}