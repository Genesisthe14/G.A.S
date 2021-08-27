using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Homescreen : MonoBehaviour
{
    //whether the Workshop should be activated when this scene loads
    private static bool openWorkshop = false;
    public static bool OpenWorkshop
    {
        get { return openWorkshop; }
        set { openWorkshop = value; }
    }

    [SerializeField]
    [Tooltip("Reference to the shop object")]
    private GameObject shop;

    [SerializeField]
    [Tooltip("Reference to the main screen object")]
    private GameObject main;

    [SerializeField]
    [Tooltip("Button that should be disabled if something is loaded")]
    private Button[] buttonsToDisable;

    [SerializeField]
    [Tooltip("Loading screen")]
    private GameObject loadingScreen;

    [SerializeField]
    [Tooltip("Loading bar")]
    private Slider loadingBar;

    [SerializeField]
    [Tooltip("First Tutorial screen")]
    private GameObject firstTutorialScreen;

    private void Awake()
    {
        if (openWorkshop)
        {
            openWorkshop = false;

            shop.SetActive(true);
            main.SetActive(false);
        }
    }

    private void Start()
    {
        if (firstTutorialScreen == null) return;

        if (PlayerData.instance.FirstPLayed)
        {
            PlayerData.instance.FirstPLayed = false;
            firstTutorialScreen.SetActive(true);
        }
        else
        {
            firstTutorialScreen.SetActive(false);
        }
    }

    public static IEnumerator LoadScreenCoroutine(string scene, Slider bar)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        float progress = 0.0f;
        
        Time.timeScale = 1;
        
        //update the loading screen text while loading/unloading process is going on
        while (!operation.isDone)
        {
            progress += Mathf.Clamp01(operation.progress / 0.9f);
            progress = Mathf.Clamp01(progress);

            bar.value = progress;

            //loadingText.text = "Loading... " + (int)(progress * 100.0f) + "%";

            //wait a frame before continuing
            yield return null;
        }
    }

    public void QuitGame () 
    {
        Debug.Log("Quet");
        Application.Quit();
    }

    public void StartGame()
    {
        loadingScreen.SetActive(true);
        DisableButtons();
        StartCoroutine(LoadScreenCoroutine("RocketScene", loadingBar));
    }

    public void DeleteSaveFile()
    {
        SaveLoadService.DeleteSaveFile();
    }

    //Loads the booster selection screen
    public void LoadBoosterSelectionScreen()
    {
        loadingScreen.SetActive(true);
        DisableButtons();
        StartCoroutine(LoadScreenCoroutine("BoosterSelection", loadingBar));
    }

    private void DisableButtons()
    {
        foreach(Button b in buttonsToDisable)
        {
            b.interactable = false;
        }
    }
}
