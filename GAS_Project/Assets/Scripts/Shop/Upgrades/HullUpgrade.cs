using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullUpgrade : Upgrade
{
    private static float[] hullUpgrades = { 9.0f, 8.0f, 7.0f, 6.0f, 5.0f };

    public override void UpgradeFeature()
    {
        PlayerData.instance.PermanentUpgradeIDsPlayerOwns.Add(UpgradeID, UpgradeType.ToString() + UpgradeNum);
        RocketBehaviour.LeakingFuel = hullUpgrades[UpgradeNum - 1];
    }
}
