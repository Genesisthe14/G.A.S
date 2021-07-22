using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Homescreen : MonoBehaviour
{
    public void StartGame () 
    {
        if(SelectionScreen.BoostsValid)SceneManager.LoadScene("RocketScene");
    }

    public void QuitGame () 
    {
        SaveLoadService.SaveGame();
        Application.Quit();
    }

    public void DeleteSaveFile()
    {
        SaveLoadService.DeleteSaveFile();
    }
}
