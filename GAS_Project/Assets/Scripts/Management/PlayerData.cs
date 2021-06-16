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
    private Dictionary<int, string> permanentUpgradeIDsPlayerOwns = new Dictionary<int, string>();
    public Dictionary<int, string> PermanentUpgradeIDsPlayerOwns
    {
        get { return permanentUpgradeIDsPlayerOwns; }
    }

    //save
    //dictionary containing the temporary upgrades the player has bought and how many of them
    private Dictionary<Upgrade.UpgradeTypes, int> temporaryItemsOwned = new Dictionary<Upgrade.UpgradeTypes, int>();
    public Dictionary<Upgrade.UpgradeTypes, int> TemporaryItemsOwned
    {
        get { return temporaryItemsOwned; }
    }

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(this);
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        moneyText.text = "Money: " + currentMoney;
    }
}
