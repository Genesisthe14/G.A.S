using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterOption : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Which Booster type this Option is")]
    private Upgrade.UpgradeTypes boosterType;
    public Upgrade.UpgradeTypes BoosterType
    {
        get { return boosterType; }
    }
}
