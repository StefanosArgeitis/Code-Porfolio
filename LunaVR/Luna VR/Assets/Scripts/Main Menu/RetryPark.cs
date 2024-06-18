using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryPark : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Park");
    }

}