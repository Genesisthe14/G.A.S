using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterButtons : MonoBehaviour
{
    [SerializeField]
    [Tooltip("List of Buttons for the booster in the first slot")]
    private SerializableDictionary<Upgrade.UpgradeTypes, GameObject> buttonsFirstSlot;

    [SerializeField]
    [Tooltip("List of Buttons for the booster in the second slot")]
    private SerializableDictionary<Upgrade.UpgradeTypes, GameObject> buttonsSecondSlot;

    //Action that is invoked if the number of boosters the player has changes
    private static Action<Upgrade.UpgradeTypes> boostersOwnedChangedEvent;
    public static Action<Upgrade.UpgradeTypes> BoostersOwnedChangedEvent
    {
        get { return boostersOwnedChangedEvent; }
    }

    private void Awake()
    {
        boostersOwnedChangedEvent += OnBoostersChanged;
    }

    private void OnDestroy()
    {
        boostersOwnedChangedEvent -= OnBoostersChanged;
    }

    private void Start()
    {
        foreach(GameObject b in buttonsFirstSlot.Values)
        {
            b.SetActive(false);
        }

        foreach (GameObject b in buttonsSecondSlot.Values)
        {
            b.SetActive(false);
        }

        buttonsFirstSlot[SelectionScreen.BoostersTaken[0]].SetActive(true);
        buttonsSecondSlot[SelectionScreen.BoostersTaken[1]].SetActive(true);
    }

    private void OnBoostersChanged(Upgrade.UpgradeTypes booster)
    {
        if(PlayerData.instance.TemporaryItemsOwned[booster] <= 0)
        {
            int slotOfBooster = BoosterSlot(booster);

            switch (slotOfBooster)
            {
                case 0:
                    buttonsFirstSlot[booster].GetComponent<Button>().interactable = false;
                    break;
                case 1:
                    buttonsSecondSlot[booster].GetComponent<Button>().interactable = false;
                    break;
                case -1:
                    Debug.LogError("The booster is not in the boostersTaken array");
                    return;
                default:
                    Debug.LogError("The number is higher than the slots available");
                    return;
            }

        }
    }

    private int BoosterSlot(Upgrade.UpgradeTypes booster)
    {
        for(int i = 0; i < SelectionScreen.BoostersTaken.Length; i++)
        {
            if(SelectionScreen.BoostersTaken[i] == booster)
            {
                return i;
            }
        }

        return -1;
    }
}
