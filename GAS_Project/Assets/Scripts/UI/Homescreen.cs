using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Awake()
    {
        if (openWorkshop)
        {
            openWorkshop = false;

            shop.SetActive(true);
            main.SetActive(false);
        }
    }


    public void StartGame () 
    {
        SceneManager.LoadScene("RocketScene");
    }

    public void QuitGame () 
    {
        Debug.Log("Quet");
        Application.Quit();
    }

    public void DeleteSaveFile()
    {
        SaveLoadService.DeleteSaveFile();
    }

    //Loads the booster selection screen
    public void LoadBoosterSelectionScreen()
    {
        SceneManager.LoadScene("BoosterSelection");
    }
}
