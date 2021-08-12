using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetUpgrade : Upgrade
{
    private static int[] magnetUpgrades = { 20, 40, 60, 80, 100 };

    public override void UpgradeFeature()
    {
        PlayerData.instance.PermanentUpgradeIDsPlayerOwns.Add(UpgradeID, UpgradeType.ToString() + UpgradeNum);
        MagnetField.FieldSize = magnetUpgrades[UpgradeNum - 1];
    }
}
