using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionScreen : MonoBehaviour
{
    //Booster Items the player takes with him
    private static Upgrade.UpgradeTypes[] boostersTaken = new Upgrade.UpgradeTypes[2];
    public static Upgrade.UpgradeTypes[] BoostersTaken
    {
        get { return boostersTaken; }
        set { boostersTaken = value; }
    }

    [SerializeField]
    [Tooltip("Booster Slots that tell what items the player has currently selected")]
    private BoosterSlot[] boosterSlots;

    [SerializeField]
    [Tooltip("Start game button")]
    private Button startGameButton;

    //Whether the selected Boosters are valid
    private static bool boostsValid = false;
    public static bool BoostsValid
    {
        get { return boostsValid; }
    }

    public static Action OnBoosterAvailability = null;

    private void Awake()
    {
        OnBoosterAvailability += CheckAvailabilityBoosters;
    }

    private void OnDestroy()
    {
        OnBoosterAvailability -= CheckAvailabilityBoosters;
    }

    private void CheckAvailabilityBoosters()
    {
        foreach(BoosterSlot slot in boosterSlots)
        {
            if (!slot.IsValid)
            {
                startGameButton.interactable = false;
                boostsValid = false;
                return;
            }
        }

        startGameButton.interactable = true;
        boostsValid = true;
    }

    public static void SwitchBoosterTaken(int index, Upgrade.UpgradeTypes boost)
    {
        boostersTaken[index] = boost;
    }
}
