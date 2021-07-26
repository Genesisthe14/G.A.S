using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelCapacityUpgrade : Upgrade
{
    private static int[] capacityUpgrades = { 150, 200, 250 };

    public override void UpgradeFeature()
    {
        PlayerData.instance.PermanentUpgradeIDsPlayerOwns.Add(UpgradeID, UpgradeType.ToString() + UpgradeNum);
        GameManager.StartFuel = capacityUpgrades[UpgradeNum - 1];
        FuelBar.CurrentNumFuelUpgrades++;
    }
}
