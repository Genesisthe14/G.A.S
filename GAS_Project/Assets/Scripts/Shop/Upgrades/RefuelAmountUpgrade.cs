using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuelAmountUpgrade : Upgrade
{
    private static int[] amountUpgrades = { 20, 30, 40, 50, 60 };

    public override void UpgradeFeature()
    {
        PlayerData.instance.PermanentUpgradeIDsPlayerOwns.Add(UpgradeID, UpgradeType.ToString() + UpgradeNum);
        GameManager.RefuelPercent = amountUpgrades[UpgradeNum - 1];
    }
}
