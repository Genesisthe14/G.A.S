using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUpgrade : Upgrade
{
    private static int[] shieldUpgradeStrengths = { 2, 3, 4, 5, 6};

    public override void UpgradeFeature()
    {
        PlayerData.instance.PermanentUpgradeIDsPlayerOwns.Add(UpgradeID, UpgradeType.ToString() + UpgradeNum);
        Shield.baseHitpoints = shieldUpgradeStrengths[UpgradeNum - 1];
    }
}
