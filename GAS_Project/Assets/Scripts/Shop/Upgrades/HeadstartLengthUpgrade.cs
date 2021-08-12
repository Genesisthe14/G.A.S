using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadstartLengthUpgrade : Upgrade
{
    private static int[] lengthUpgrades = { 500, 600, 700, 800, 900 };

    public override void UpgradeFeature()
    {
        PlayerData.instance.PermanentUpgradeIDsPlayerOwns.Add(UpgradeID, UpgradeType.ToString() + UpgradeNum);
        GameManager.HeadstartLength = lengthUpgrades[UpgradeNum - 1];
    }
}
