using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that is used to save the game state between sessions

[Serializable]
public class SaveData
{
    //Contains all sata which needs to be saved and is of type integer
    //string describes what this int is used for
    [SerializeField]
    private SerializableDictionary<string, int> intSaveData;

    //Contains all sata which needs to be saved and is of type float
    //string describes what this float is used for
    [SerializeField]
    private SerializableDictionary<string, float> floatSaveData;

    //IDs of the permanent upgrades the player owns
    [SerializeField]
    private SerializableDictionary<string, string> permanentUpgradeIDsPlayerOwns;

    //Constructor that initializes the attributes of SaveData with values 
    //that are supposed to be saved
    public SaveData()
    {
        intSaveData = new SerializableDictionary<string, int>();
        floatSaveData = new SerializableDictionary<string, float>();
        permanentUpgradeIDsPlayerOwns = new SerializableDictionary<string, string>();
        
        //save money
        intSaveData.Add("currentMoney", PlayerData.instance.CurrentMoney);
       
        //Save permanent upgrades
        intSaveData.Add("shield_baseHitpoints", Shield.baseHitpoints);
        intSaveData.Add("magnetSize", MagnetField.FieldSize);
        intSaveData.Add("numFuelUpgrades", FuelBar.CurrentNumFuelUpgrades);

        floatSaveData.Add("leakingFuel", RocketBehaviour.LeakingFuel);
        floatSaveData.Add("fuelLevel", GameManager.StartFuel);
        floatSaveData.Add("refuelAmount", GameManager.RefuelPercent);
        floatSaveData.Add("headstartLength", GameManager.HeadstartLength);

        //save stats
        intSaveData.Add("totalDistance", PlayerData.instance.TotalDistance);
        intSaveData.Add("highestDistance", PlayerData.instance.HighestDistance);
        intSaveData.Add("deaths", PlayerData.instance.Deaths);
        intSaveData.Add("destroyedObjects", PlayerData.instance.DestroyedObjects);
        intSaveData.Add("totalGold", PlayerData.instance.TotalGold);
        //Peter was here
        intSaveData.Add("clever", PlayerData.instance.clever);

        //save audio levels
        PlayerData.instance.MasterMixer.GetFloat("Master", out float masterVolume);
        floatSaveData.Add("masterVolume", masterVolume);

        PlayerData.instance.MasterMixer.GetFloat("Effects", out float effectsVolume);
        floatSaveData.Add("effectsVolume", effectsVolume);

        PlayerData.instance.MasterMixer.GetFloat("Music", out float musicVolume);
        floatSaveData.Add("musicVolume", musicVolume);

        permanentUpgradeIDsPlayerOwns = PlayerData.instance.PermanentUpgradeIDsPlayerOwns;
    }

    //Initializes the values that were previously saved with the attributes from
    //the calling SaveData
    public void SetData()
    {
        PlayerData.instance.PermanentUpgradeIDsPlayerOwns = permanentUpgradeIDsPlayerOwns;

        MagnetField.FieldSize = intSaveData["magnetSize"];
        Shield.baseHitpoints = intSaveData["shield_baseHitpoints"];        
        PlayerData.instance.CurrentMoney = intSaveData["currentMoney"];
        FuelBar.CurrentNumFuelUpgrades = intSaveData["numFuelUpgrades"];
        RocketBehaviour.LeakingFuel = floatSaveData["leakingFuel"];
        GameManager.StartFuel = floatSaveData["fuelLevel"];
        GameManager.RefuelPercent = floatSaveData["refuelAmount"];
        GameManager.HeadstartLength = floatSaveData["headstartLength"];

        PlayerData.instance.TotalDistance = intSaveData["totalDistance"];
        PlayerData.instance.HighestDistance = intSaveData["highestDistance"];
        PlayerData.instance.Deaths = intSaveData["deaths"];
        PlayerData.instance.DestroyedObjects = intSaveData["destroyedObjects"];
        PlayerData.instance.TotalGold = intSaveData["totalGold"];
        //Peter was here
        PlayerData.instance.clever = intSaveData["clever"];

        if (PlayerData.instance.MasterSlider != null)
        {
            PlayerData.instance.MasterSlider.value = Mathf.Pow(10, floatSaveData["masterVolume"] / 20);
            PlayerData.instance.EffectsSlider.value = Mathf.Pow(10, floatSaveData["effectsVolume"] / 20);
            PlayerData.instance.MusicSlider.value = Mathf.Pow(10, floatSaveData["musicVolume"] / 20);
        }
    }
}
