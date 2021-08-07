using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelCapacityUpgrade : Upgrade
{
    private static int[] capacityUpgrades = { 120, 140, 160, 180, 200 };

    public override void UpgradeFeature()
    {
        PlayerData.instance.PermanentUpgradeIDsPlayerOwns.Add(UpgradeID, UpgradeType.ToString() + UpgradeNum);
        GameManager.StartFuel = capacityUpgrades[UpgradeNum - 1];
        FuelBar.CurrentNumFuelUpgrades++;
    }
}
