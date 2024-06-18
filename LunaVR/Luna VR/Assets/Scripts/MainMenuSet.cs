using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSet : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Earth");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}