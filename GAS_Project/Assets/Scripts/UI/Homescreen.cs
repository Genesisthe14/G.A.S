using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Homescreen : MonoBehaviour
{
    public void StartGame () 
    {
        SceneManager.LoadScene("RocketScene");
    }

    public void QuitGame () 
    {
        Debug.Log("I quited le Game, only available in actual builded Game");
        SaveLoadService.SaveGame();
        Application.Quit();
    }
}
