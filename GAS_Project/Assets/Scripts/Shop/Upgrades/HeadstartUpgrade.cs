using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadstartUpgrade : Upgrade
{
    public override void UpgradeFeature()
    {
        if (!PlayerData.instance.TemporaryItemsOwned.ContainsKey(UpgradeTypes.HEADSTART))
        {
            PlayerData.instance.TemporaryItemsOwned.Add(UpgradeTypes.HEADSTART, 1);
        }
        else
        {
            int tempNum = PlayerData.instance.TemporaryItemsOwned[UpgradeTypes.HEADSTART] + 1;
            PlayerData.instance.TemporaryItemsOwned.ReplaceValue(UpgradeTypes.HEADSTART, tempNum);
        }

        Debug.Log("Headstarts: " + PlayerData.instance.TemporaryItemsOwned[UpgradeTypes.HEADSTART]);
    }
}
