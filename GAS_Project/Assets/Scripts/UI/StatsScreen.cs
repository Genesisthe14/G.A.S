using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsScreen : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Total distance text")]
    private Text totalDistanceText;
    
    [SerializeField]
    [Tooltip("Total distance text")]
    private Text highestDistanceText;

    [SerializeField]
    [Tooltip("Total distance text")]
    private Text deathsText;

    [SerializeField]
    [Tooltip("Total distance text")]
    private Text totalGoldText;

    [SerializeField]
    [Tooltip("Total distance text")]
    private Text objectsDestroyedText;

    private void OnEnable()
    {
        totalDistanceText.text = "" + PlayerData.instance.TotalDistance;
        highestDistanceText.text = "" + PlayerData.instance.HighestDistance;
        deathsText.text = "" + PlayerData.instance.Deaths;
        totalGoldText.text = "" + PlayerData.instance.TotalGold;
        objectsDestroyedText.text = "" + PlayerData.instance.DestroyedObjects;
    }
}
