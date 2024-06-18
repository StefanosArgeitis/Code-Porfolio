using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string Moon;
    
    
    public void LoadMoonScene()
    {
        SceneManager.LoadScene(Moon);
    }
    
    public void ExitGame()
    {
        Application.Quit(); 
    }
}
