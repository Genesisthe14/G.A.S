using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadstartLengthUpgrade : Upgrade
{
    private static int[] lengthUpgrades = { 500, 650, 850 };

    public override void UpgradeFeature()
    {
        PlayerData.instance.PermanentUpgradeIDsPlayerOwns.Add(UpgradeID, UpgradeType.ToString() + UpgradeNum);
        GameManager.HeadstartLength = lengthUpgrades[UpgradeNum - 1];
    }
}
