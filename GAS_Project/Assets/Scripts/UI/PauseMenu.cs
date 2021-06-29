using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;   
    public GameObject PauseMenuUI;

    public void pauseGame() 
    {
        if (GamePaused) 
        {
            GameManager.instance.ConsumeFuel = true;
            GameManager.instance.Spawner.Spawn = true;
            Time.timeScale = 1;
            PauseMenuUI.SetActive(false);
            GamePaused = false;
        } 
        else {
            GameManager.instance.ConsumeFuel = false;
            GameManager.instance.Spawner.Spawn = false;
            Time.timeScale = 0;
            PauseMenuUI.SetActive(true);
            GamePaused = true;
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;

        PlayerData.instance.CurrentMoney = GameManager.instance.BeforeRunMoney;
        SceneManager.LoadScene("UIGaS");
    }

    public void Quit()
    {
        Debug.Log("Quit game");
        SaveLoadService.SaveGame();
        Application.Quit();
    }
}
