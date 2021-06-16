using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUpgrade : Upgrade
{
    private static int[] shieldUpgradeStrengths = { 5, 8};

    public override void UpgradeFeature()
    {
        PlayerData.instance.PermanentUpgradeIDsPlayerOwns.Add(UpgradeID, UpgradeType.ToString() + UpgradeNum);
        Shield.baseHitpoints = shieldUpgradeStrengths[UpgradeNum - 1];
    }
}
