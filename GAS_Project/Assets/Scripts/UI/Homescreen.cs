using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Homescreen : MonoBehaviour
{
    public void StartGame () 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame () 
    {
        Debug.Log("I quited le Game, only available in actual builded Game");
        Application.Quit();
    }
}
