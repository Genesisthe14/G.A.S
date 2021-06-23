using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private int currentMoney = 1000;
    public int CurrentMoney
    {
        get { return currentMoney; }
        set 
        { 
            currentMoney = value;
            if (currentMoney < 0) currentMoney = 0;

            if(moneyText.IsActive()) moneyText.text = "Money: " + currentMoney;
        }
    }

    [SerializeField]
    private Text moneyText;

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
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        //load the last saved game
        SaveLoadService.LoadGame();

        moneyText.text = "Money: " + currentMoney;
    }
}
