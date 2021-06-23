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

    //IDs of the permanent upgrades the player owns
    [SerializeField]
    private SerializableDictionary<int, string> permanentUpgradeIDsPlayerOwns;
    public SerializableDictionary<int, string> PermanentUpgradeIDsPlayerOwns
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
        permanentUpgradeIDsPlayerOwns = new SerializableDictionary<int, string>();
        temporaryItemsOwned = new SerializableDictionary<Upgrade.UpgradeTypes, int>();

        intSaveData.Add("shield_baseHitpoints", Shield.baseHitpoints);
        intSaveData.Add("currentMoney", PlayerData.instance.CurrentMoney);

        permanentUpgradeIDsPlayerOwns = PlayerData.instance.PermanentUpgradeIDsPlayerOwns;
        temporaryItemsOwned = PlayerData.instance.TemporaryItemsOwned;
    }

    //Initializes the values that were previously saved with the attributes from
    //the calling SaveData
    public void SetData()
    {
        PlayerData.instance.CurrentMoney = intSaveData["currentMoney"];
        PlayerData.instance.PermanentUpgradeIDsPlayerOwns = permanentUpgradeIDsPlayerOwns;
        PlayerData.instance.TemporaryItemsOwned = temporaryItemsOwned;

        Shield.baseHitpoints = intSaveData["shield_baseHitpoints"];
    }
}