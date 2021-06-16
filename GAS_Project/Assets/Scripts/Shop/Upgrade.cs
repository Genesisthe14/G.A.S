using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : ScriptableObject
{
    [SerializeField]
    [Tooltip("Unique ID of the upgrade")]
    private int upgradeID;
    public int UpgradeID
    {
        get { return upgradeID; }
    }
    
    [SerializeField]
    [Tooltip("What what number of this type of upgrade is that")]
    private int upgradeNum;
    public int UpgradeNum
    {
        get { return upgradeNum; }
    }

    [SerializeField]
    [Tooltip("What what number of this type of upgrade is that")]
    private bool isPermanentUpgrade;
    public bool IsPermanentUpgrade
    {
        get { return isPermanentUpgrade; }
    }

    [SerializeField]
    [Tooltip("Type of this upgrade")]
    private UpgradeTypes upgradeType;
    public UpgradeTypes UpgradeType
    {
        get { return upgradeType; }
    }

    public enum UpgradeTypes { SHIELD, FUELTANK, ROCKETHULL}

    public virtual void UpgradeFeature()
    {
        Debug.Log("Upgraded");
    }
}
