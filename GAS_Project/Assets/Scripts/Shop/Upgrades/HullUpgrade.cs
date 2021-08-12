using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullUpgrade : Upgrade
{
    private static float[] hullUpgrades = { 90.0f, 80.0f, 70.0f, 60.0f, 50.0f };

    public override void UpgradeFeature()
    {
        PlayerData.instance.PermanentUpgradeIDsPlayerOwns.Add(UpgradeID, UpgradeType.ToString() + UpgradeNum);
        RocketBehaviour.LeakingFuel = hullUpgrades[UpgradeNum - 1];
    }
}
