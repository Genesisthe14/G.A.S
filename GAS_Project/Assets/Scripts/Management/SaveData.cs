using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    //Contains all sata which needs to be saved and is of type integer
    //string describes what this int is used for
    [SerializeField]
    private SerializableDictionary<string, int> intSaveData;
    public SerializableDictionary<string, int> IntSaveData
    {
        get { return intSaveData; }
    }

    //Contains all sata which needs to be saved and is of type float
    //string describes what this float is used for
    [SerializeField]
    private SerializableDictionary<string, float> floatSaveData;
    public SerializableDictionary<string, float> FloatSaveData
    {
        get { return floatSaveData; }
    }

    //IDs of the permanent upgrades the player owns
    [SerializeField]
    private SerializableDictionary<string, string> permanentUpgradeIDsPlayerOwns;
    public SerializableDictionary<string, string> PermanentUpgradeIDsPlayerOwns
    {
        get { return permanentUpgradeIDsPlayerOwns; }
    }

    //dictionary containing the temporary upgrades the player has bought and how many of them
    [SerializeField]
    private SerializableDictionary<Upgrade.UpgradeTypes, int> temporaryItemsOwned;
    public SerializableDictionary<Upgrade.UpgradeTypes, int> TemporaryItemsOwned
    {
        get { return temporaryItemsOwned; }
    }

    //Constructor that initializes the attributes of SaveData with values 
    //that are supposed to be saved
    public SaveData()
    {
        intSaveData = new SerializableDictionary<string, int>();
        floatSaveData = new SerializableDictionary<string, float>();
        permanentUpgradeIDsPlayerOwns = new SerializableDictionary<string, string>();
        temporaryItemsOwned = new SerializableDictionary<Upgrade.UpgradeTypes, int>();


        intSaveData.Add("shield_baseHitpoints", Shield.baseHitpoints);
        intSaveData.Add("currentMoney", PlayerData.instance.CurrentMoney);
        intSaveData.Add("magnetSize", MagnetField.FieldSize);
        intSaveData.Add("numFuelUpgrades", FuelBar.CurrentNumFuelUpgrades);
        intSaveData.Add("numOfHeadstarts", RocketBehaviour.NumOfHeadstarts);

        floatSaveData.Add("leakingFuel", RocketBehaviour.LeakingFuel);
        floatSaveData.Add("fuelLevel", GameManager.StartFuel);

        permanentUpgradeIDsPlayerOwns = PlayerData.instance.PermanentUpgradeIDsPlayerOwns;
        temporaryItemsOwned = PlayerData.instance.TemporaryItemsOwned;
    }

    //Initializes the values that were previously saved with the attributes from
    //the calling SaveData
    public void SetData()
    {
        PlayerData.instance.PermanentUpgradeIDsPlayerOwns = permanentUpgradeIDsPlayerOwns;
        PlayerData.instance.TemporaryItemsOwned = temporaryItemsOwned;

        MagnetField.FieldSize = intSaveData["magnetSize"];
        Shield.baseHitpoints = intSaveData["shield_baseHitpoints"];        
        PlayerData.instance.CurrentMoney = intSaveData["currentMoney"];
        FuelBar.CurrentNumFuelUpgrades = intSaveData["numFuelUpgrades"];
        RocketBehaviour.NumOfHeadstarts = intSaveData["numOfHeadstarts"];

        RocketBehaviour.LeakingFuel = floatSaveData["leakingFuel"];
        GameManager.StartFuel = floatSaveData["fuelLevel"];
    }
}
