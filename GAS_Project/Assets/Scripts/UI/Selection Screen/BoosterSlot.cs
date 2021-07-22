using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterSlot : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Index of the slot")]
    private int slotIndex;

    [SerializeField]
    [Tooltip("Possible items array")]
    private GameObject[] boosterOptions;

    //index of the booster currently shown in this slot
    private int currentOptionIndex = 0;

    //Whether the Option of this Slot is currently valid
    private bool isValid = false;
    public bool IsValid
    {
        get { return isValid; }
    }

    private void Awake()
    {
        foreach(GameObject ob in boosterOptions)
        {
            ob.SetActive(false);
        }

        boosterOptions[currentOptionIndex].SetActive(true);
        CheckIfCurrentOptionValid();
    }

    //shows the next item of this slot
    public void NextOption()
    {
        boosterOptions[currentOptionIndex++].SetActive(false);

        if (currentOptionIndex >= boosterOptions.Length) currentOptionIndex = 0;

        boosterOptions[currentOptionIndex].SetActive(true);

        CheckIfCurrentOptionValid();
    }

    //shows the previous item of this slot
    public void PrevOption()
    {
        boosterOptions[currentOptionIndex--].SetActive(false);

        if (currentOptionIndex < 0) currentOptionIndex = boosterOptions.Length - 1;

        boosterOptions[currentOptionIndex].SetActive(true);

        CheckIfCurrentOptionValid();
    }

    //Check whether the current Option is valid
    private void CheckIfCurrentOptionValid()
    {
        Upgrade.UpgradeTypes boost = boosterOptions[currentOptionIndex].GetComponent<BoosterOption>().BoosterType;

        bool alreadySelected = false;
        foreach(Upgrade.UpgradeTypes b in SelectionScreen.BoostersTaken)
        {
            if (b == boost && boost != Upgrade.UpgradeTypes.NONE)
            {
                alreadySelected = true;
                break;
            }
        }

        if ((boost == Upgrade.UpgradeTypes.NONE || PlayerData.instance.TemporaryItemsOwned[boost] >= 1) && !alreadySelected)
        {
            isValid = true;
            SelectionScreen.SwitchBoosterTaken(slotIndex, boost);
            boosterOptions[currentOptionIndex].GetComponent<Image>().color = Color.white;
        }
        else
        {
            isValid = false;
            SelectionScreen.SwitchBoosterTaken(slotIndex, Upgrade.UpgradeTypes.NONE);
            boosterOptions[currentOptionIndex].GetComponent<Image>().color = Color.grey;
        }

        SelectionScreen.OnBoosterAvailability.Invoke();
    }
}
