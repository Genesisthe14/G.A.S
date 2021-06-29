using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    //instance of player data
    private static PlayerData _instance = null;
    public static PlayerData instance
    {
        get { return _instance; }
    }

    //save
    //Amount of money the player currently has
    private int currentMoney = 0;
    public int CurrentMoney
    {
        get { return currentMoney; }
        set 
        { 
            currentMoney = value;
            if (currentMoney < 0) currentMoney = 0;

            if(UpdateShopDisplay.instance.MoneyText != null) UpdateShopDisplay.instance.MoneyText.text = "Money: " + currentMoney;
        }
    }

    private bool firstLoaded = true;

    //save
    //IDs of the permanent upgrades the player owns
    private SerializableDictionary<int, string> permanentUpgradeIDsPlayerOwns = new SerializableDictionary<int, string>();
    public SerializableDictionary<int, string> PermanentUpgradeIDsPlayerOwns
    {
        get { return permanentUpgradeIDsPlayerOwns; }
        set { permanentUpgradeIDsPlayerOwns = value; }
    }

    //save
    //dictionary containing the temporary upgrades the player has bought and how many of them
    private SerializableDictionary<Upgrade.UpgradeTypes, int> temporaryItemsOwned = new SerializableDictionary<Upgrade.UpgradeTypes, int>();
    public SerializableDictionary<Upgrade.UpgradeTypes, int> TemporaryItemsOwned
    {
        get { return temporaryItemsOwned; }
        set { temporaryItemsOwned = value; }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        //load the last saved game
        if (firstLoaded)
        {
            SaveLoadService.LoadGame();
            firstLoaded = false;
        }

        SceneManager.sceneLoaded += OnSceneLoaded; 
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(currentMoney);
        //UpdateShopDisplay.instance.MoneyText.text = "Money: " + currentMoney;
    }
}
