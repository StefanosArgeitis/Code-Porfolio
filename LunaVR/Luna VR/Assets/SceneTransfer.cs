using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransfer : MonoBehaviour
{
    public string Scene;

   public void Transfer(){

        SceneManager.LoadScene(Scene);

   }
}
