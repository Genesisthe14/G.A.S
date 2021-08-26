using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterUseButtons : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Icons for the boosters in the first slot")]
    private SerializableDictionary<Upgrade.UpgradeTypes, GameObject> iconsFirstSlot;

    [SerializeField]
    [Tooltip("Icons for the boosters in the second slot")]
    private SerializableDictionary<Upgrade.UpgradeTypes, GameObject> iconsSecondSlot;

    [SerializeField]
    [Tooltip("Levels of the first Slot")]
    private List<GameObject> levelsFirstSlot;

    [SerializeField]
    [Tooltip("Levels of the secondSlot")]
    private List<GameObject> levelsSecondSlot;

    private void Awake()
    {
        GameManager.instance.BoosterUsedEvent += UpdateButtonDisplay;
    }

    private void OnDestroy()
    {
        GameManager.instance.BoosterUsedEvent -= UpdateButtonDisplay;
    }

    private void Start()
    {
        //initialize booster buttons
        UpdateButtonDisplay(true);
    }

    //Update the buttons to use the boosters
    private void UpdateButtonDisplay(bool initialize = false)
    {
        Upgrade.UpgradeTypes typeFirstSlot = SelectionScreen.BoostersTaken[0];
        Upgrade.UpgradeTypes typeSecondSlot = SelectionScreen.BoostersTaken[1];

        if (initialize)
        {
            //activate the icons of the boosters the player took
            iconsFirstSlot[Upgrade.UpgradeTypes.NONE].SetActive(false);
            iconsFirstSlot[typeFirstSlot].SetActive(true);

            iconsSecondSlot[Upgrade.UpgradeTypes.NONE].SetActive(false);
            iconsSecondSlot[typeSecondSlot].SetActive(true);
        }

        //set how many boosters of a type the player has
        ActivateLevels(levelsFirstSlot, typeFirstSlot);
        ActivateLevels(levelsSecondSlot, typeSecondSlot);
    }

    //Activate the same number of levels as the player has boosters of that kind
    private void ActivateLevels(List<GameObject> levelList, Upgrade.UpgradeTypes type)
    {
        if (type == Upgrade.UpgradeTypes.NONE) return;

        foreach (GameObject item in levelList)
        {
            item.SetActive(false);
        }

        for (int i = 0; i < PlayerData.instance.TemporaryItemsOwned[type]; i++)
        {
            levelList[i].SetActive(true);
        }
    }
}
