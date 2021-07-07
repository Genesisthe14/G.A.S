using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuelUpgrade : Upgrade
{
    public override void UpgradeFeature()
    {
        if (!PlayerData.instance.TemporaryItemsOwned.ContainsKey(UpgradeTypes.REFUEL))
        {
            PlayerData.instance.TemporaryItemsOwned.Add(UpgradeTypes.REFUEL, 1);
        }
        else
        {
            int tempNum = PlayerData.instance.TemporaryItemsOwned[UpgradeTypes.REFUEL] + 1;
            PlayerData.instance.TemporaryItemsOwned.Remove(UpgradeTypes.REFUEL);

            PlayerData.instance.TemporaryItemsOwned.Add(UpgradeTypes.REFUEL, tempNum);
        }

        Debug.Log("Refuel: "+PlayerData.instance.TemporaryItemsOwned[UpgradeTypes.REFUEL]);
    }
}
