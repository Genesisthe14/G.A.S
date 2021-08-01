using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberShieldsUpgrade : Upgrade
{
    public override void UpgradeFeature()
    {
        if (!PlayerData.instance.TemporaryItemsOwned.ContainsKey(UpgradeTypes.NUMSHIELDS))
        {
            PlayerData.instance.TemporaryItemsOwned.Add(UpgradeTypes.NUMSHIELDS, 1);
        }
        else
        {
            int tempNum = PlayerData.instance.TemporaryItemsOwned[UpgradeTypes.NUMSHIELDS] + 1;
            PlayerData.instance.TemporaryItemsOwned.ReplaceValue(UpgradeTypes.NUMSHIELDS, tempNum);
        }

        Debug.Log("Number Shields: "+ PlayerData.instance.TemporaryItemsOwned[UpgradeTypes.NUMSHIELDS]);
    }
}
