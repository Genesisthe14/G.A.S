using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;   
    public GameObject PauseMenuUI;

    [SerializeField]
    [Tooltip("Countdown text object")]
    private GameObject countText;

    [SerializeField]
    [Tooltip("Button that should be disabled if something is loaded")]
    private Button[] buttonsToDisable;

    [SerializeField]
    [Tooltip("Loading screen")]
    private GameObject loadingScreen;

    [SerializeField]
    [Tooltip("Loading bar")]
    private Slider loadingBar;

    //whether the counter was still counting down to the start of the game
    private bool counterActive = false;

    public void pauseGame() 
    {
        if (isPaused) 
        {
            if (!counterActive)
            {
                GameManager.instance.ConsumeFuel = true;
                GameManager.instance.Spawner.Spawn = true;
            }
                
            counterActive = false;
            Time.timeScale = 1;
            PauseMenuUI.SetActive(false);
            countText.SetActive(true);
            isPaused = false;
        } 
        else {

            if (!GameManager.instance.ConsumeFuel && !GameManager.instance.Spawner.Spawn) counterActive = true;

            GameManager.instance.ConsumeFuel = false;
            GameManager.instance.Spawner.Spawn = false;
            Time.timeScale = 0;
            PauseMenuUI.SetActive(true);
            countText.SetActive(false);
            isPaused = true;
        }
    }

    public void Crash()
    {
        pauseGame();
        GameManager.instance.CurrentFuel = 0.0f;
    }

    public void LoadMenu()
    {
        //Time.timeScale = 1f;
        isPaused = false;

        SelectionScreen.instance.EmptyBoosters();
        SelectionScreen.instance.ResetBoostersTaken(true);
        
        RocketBehaviour.isWarpActive = false;
        RocketBehaviour.CurrentWarpSpeedFactor = 0.0f;

        GameManager.InRun = false;

        loadingScreen.SetActive(true);
        DisableButtons();
        StartCoroutine(Homescreen.LoadScreenCoroutine("Startscreen", loadingBar));
    }

    public void Quit()
    {
        Debug.Log("Quit game");
        SaveLoadService.SaveGame();
        Application.Quit();
    }

    public void RestartLevel()
    {
        //Time.timeScale = 1f;
        isPaused = false;

        PlayerData.instance.CurrentMoney = GameManager.instance.BeforeRun["currentMoney"];
        PlayerData.instance.TemporaryItemsOwned.ReplaceValue(Upgrade.UpgradeTypes.NUMSHIELDS, GameManager.instance.BeforeRun["numShields"]);
        PlayerData.instance.TemporaryItemsOwned.ReplaceValue(Upgrade.UpgradeTypes.REFUEL, GameManager.instance.BeforeRun["refuels"]);
        PlayerData.instance.TemporaryItemsOwned.ReplaceValue(Upgrade.UpgradeTypes.HEADSTART, GameManager.instance.BeforeRun["headstarts"]);

        RocketBehaviour.isWarpActive = false;
        RocketBehaviour.CurrentWarpSpeedFactor = 0.0f;

        loadingScreen.SetActive(true);
        DisableButtons();
        StartCoroutine(Homescreen.LoadScreenCoroutine("RocketScene", loadingBar));
    }

    private void DisableButtons()
    {
        foreach (Button b in buttonsToDisable)
        {
            b.interactable = false;
        }
    }
}
