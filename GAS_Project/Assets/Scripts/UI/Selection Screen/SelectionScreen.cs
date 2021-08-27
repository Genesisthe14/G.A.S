using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Class that implements the functionality of the selection screen
//where the player buys the boosters he wants to use on this run

public class SelectionScreen : MonoBehaviour
{
    //Booster Items the player takes with him
    private static Upgrade.UpgradeTypes[] boostersTaken = { Upgrade.UpgradeTypes.NONE, Upgrade.UpgradeTypes.NONE };
    public static Upgrade.UpgradeTypes[] BoostersTaken
    {
        get { return boostersTaken; }
        set { boostersTaken = value; }
    }

    [SerializeField]
    [Tooltip("All boosters the Player can buy")]
    private Upgrade[] boosters;

    [SerializeField]
    [Tooltip("All Shopitem instances for boosters")]
    private ShopItem[] allBoosterShopItems;

    [SerializeField]
    [Tooltip("Dictionary for booster amounts in first booster slot")]
    private SerializableDictionary<int, GameObject> firstSlotLevels;

    [SerializeField]
    [Tooltip("Dictionary for booster amounts in second booster slot")]
    private SerializableDictionary<int, GameObject> secondSlotLevels;

    [SerializeField]
    [Tooltip("Total Coins Text")]
    private Text totalCoins;

    [SerializeField]
    [Tooltip("Dictionaries containing the icons for the first slot")]
    private SerializableDictionary<Upgrade.UpgradeTypes, GameObject> firstSlotIcons = null;

    [SerializeField]
    [Tooltip("Dictionaries containing the icons for the first slot")]
    private SerializableDictionary<Upgrade.UpgradeTypes, GameObject> secondSlotIcons = null;

    [SerializeField]
    [Tooltip("Button that should be disabled if something is loaded")]
    private Button[] buttonsToDisable;

    [SerializeField]
    [Tooltip("Loading screen")]
    private GameObject loadingScreen;

    [SerializeField]
    [Tooltip("Loading bar")]
    private Slider loadingBar;

    //List of all dictionaries containing the icons for the correct slots
    //index of lists is number slot and upgrade type is key for correct GameObject to activate
    private List<SerializableDictionary<Upgrade.UpgradeTypes, GameObject>> slotIcons = null;
    
    //The amount the player had of each booster before he entered the selection screen
    private Dictionary<Upgrade.UpgradeTypes, int> amountBeforeBuy = new Dictionary<Upgrade.UpgradeTypes, int>();

    //index of the current booster slot
    private static int currentBoosterSlot = 0;

    //instance of the selection screen
    private static SelectionScreen _instance;
    public static SelectionScreen instance
    {
        get { return _instance; }
    }

    //List of booster slot level dictionaries
    private List<SerializableDictionary<int, GameObject>> slotLevelList = new List<SerializableDictionary<int, GameObject>>();

    private bool enabledOnce = false;

    private void Awake()
    {
        _instance = this;

        //initialize the amountBeforeBuy list with all booster upgrade types
        foreach (Upgrade up in boosters)
        {
            amountBeforeBuy.Add(up.UpgradeType, 0);
        }

        slotIcons = new List<SerializableDictionary<Upgrade.UpgradeTypes, GameObject>>{ firstSlotIcons, secondSlotIcons };

        slotIcons[0][boostersTaken[0]].SetActive(true);
        slotIcons[1][boostersTaken[1]].SetActive(true);

        slotLevelList.Add(firstSlotLevels);
        slotLevelList.Add(secondSlotLevels);
    }

    private void OnEnable()
    {
        if (enabledOnce) return;

        enabledOnce = true;

        //if the selection screen is entered by the player 
        //save the amount of boosters the player had for each booster type in boosters
        foreach (Upgrade up in boosters)
        {
            if(amountBeforeBuy.ContainsKey(up.UpgradeType)) amountBeforeBuy[up.UpgradeType] = PlayerData.instance.TemporaryItemsOwned[up.UpgradeType];
            else amountBeforeBuy.Add(up.UpgradeType, PlayerData.instance.TemporaryItemsOwned[up.UpgradeType]);
        }

        ResetBoostersTaken(true);

        //Update the selection screen
        UpdateSelectScreen();
    }

    //If the player buys a booster it is set as a booster the player has taken
    //if both slots are full already the player can't buy another booster
    public void SetBoosterTaken(Upgrade.UpgradeTypes boost)
    {
        //if the player already has bought this kind of booster or the player
        //already has bought two kinds of boosters then just update the screen but 
        //don't set a new booster
        if (BoostersContain(boost) || currentBoosterSlot >= boostersTaken.Length)
        {
            UpdateSelectScreen();
            return;
        }

        //set the booster the player just bought in the current booster slot
        boostersTaken[currentBoosterSlot] = boost;

        //List of dictionaries to then activate the correct icons for slots
        slotIcons[currentBoosterSlot][Upgrade.UpgradeTypes.NONE].SetActive(false);
        slotIcons[currentBoosterSlot][boost].SetActive(true);

        Debug.Log(boostersTaken[currentBoosterSlot]);
        currentBoosterSlot++;

        //Update Selection screen
        UpdateSelectScreen();
    }

    //Checks whether the array of the boosters the player has already taken 
    //contains the given boost/upgrade
    private bool BoostersContain(Upgrade.UpgradeTypes boost)
    {
        foreach(Upgrade.UpgradeTypes up in boostersTaken)
        {
            if (up == boost) return true;
        }

        return false;
    }

    //Updates the status of the selection screen when the player e.g. bought a booster
    private void UpdateSelectScreen()
    {
        totalCoins.text = "" + PlayerData.instance.CurrentMoney;

        //iterate over all shop items/ booster items to update their individual appearance
        foreach (ShopItem shop in allBoosterShopItems)
        {
            for (int i = 0; i < boostersTaken.Length; i++)
            {
                if (boostersTaken[i] == Upgrade.UpgradeTypes.NONE) continue;
                
                int numberOfBoosters = PlayerData.instance.TemporaryItemsOwned[boostersTaken[i]];
                slotLevelList[i][numberOfBoosters].SetActive(true);
            }

            //if the price of the item is bigger than the amount of money the player currently has
            //then disable the buy button for this item
            if(shop.Item.Price > PlayerData.instance.CurrentMoney)
            {
                if(shop.BuyButton.interactable) shop.BuyButton.interactable = false;
            }
            else if (!shop.BuyButton.interactable) shop.BuyButton.interactable = true;

            //if two slots are already set
            if (currentBoosterSlot >= boostersTaken.Length)
            {
                bool isInBoostersTaken = false;

                //Iterate over all booster types the player has taken
                //if the type of the shop item is one of the types the
                //player has already bought then set the buy button of
                //the shop item to active

                foreach(Upgrade.UpgradeTypes type in boostersTaken)
                {
                    if (type == shop.Item.UpgradeType)
                    {
                        isInBoostersTaken = true;
                        break;
                    }
                }

                //set the booster item that wasn't taken to not interactable
                shop.BuyButton.interactable = isInBoostersTaken;
            }
        }
    }

    //Reset the selection screen and the boosters the player has bought and
    //return the money
    //if this function is activated after a run, when the player returns to the main menu
    //then only the boosters are reset and the current booster slot
    public void ResetBoostersTaken(bool afterRun = false)
    {
        for(int i = 0; i < boostersTaken.Length; i++)
        {
            //if it's not after a run and this booster in boostersTaken is not NONE 
            //then return the money
            if (!afterRun && boostersTaken[i] != Upgrade.UpgradeTypes.NONE)
            {
                int differenceOwned = Math.Abs(PlayerData.instance.TemporaryItemsOwned[boostersTaken[i]] - amountBeforeBuy[boostersTaken[i]]);
                Debug.Log("Before: "+PlayerData.instance.TemporaryItemsOwned[boostersTaken[i]]);

                PlayerData.instance.CurrentMoney += SearchForBoosterPrice(boostersTaken[i]) * differenceOwned;

                PlayerData.instance.TemporaryItemsOwned.ReplaceValue(boostersTaken[i], amountBeforeBuy[boostersTaken[i]]);

                Debug.Log("After: "+PlayerData.instance.TemporaryItemsOwned[boostersTaken[i]]);
            }

            //Set boosters taken to NONE
            boostersTaken[i] = Upgrade.UpgradeTypes.NONE;
        }
        
        //reset the current Slot
        currentBoosterSlot = 0;

        if (afterRun) return;

        //deactivate the objects that that are activated when a shop item is bought
        //and activate the gameobject of the shop item again
        foreach(ShopItem item in allBoosterShopItems)
        {
            item.ActivateOnBuy.SetActive(false);
            item.gameObject.SetActive(true);
        }

        if (slotIcons == null) return;

        //reset the icons that are displayed in the slots
        foreach(SerializableDictionary<Upgrade.UpgradeTypes, GameObject> iconDict in slotIcons)
        {
            foreach(Upgrade.UpgradeTypes key in iconDict.Keys)
            {
                if(key != Upgrade.UpgradeTypes.NONE)
                {
                    iconDict[key].SetActive(false);
                }
                else iconDict[key].SetActive(true);
            }
        }
    }

    //Search for the shop item that gives the Upgrade/booster type given
    private int SearchForBoosterPrice(Upgrade.UpgradeTypes type)
    {
        foreach(Upgrade up in boosters)
        {
            if (up.UpgradeType == type) return up.Price;
        }

        return 0;
    }

    //set boosters player has to zero
    public void EmptyBoosters()
    {
        foreach(Upgrade.UpgradeTypes booster in PlayerData.instance.TemporaryItemsOwned.Keys)
        {
            PlayerData.instance.TemporaryItemsOwned.ReplaceValue(booster, 0);
        }
    }

    //Loads the start screen
    public void LoadStartScreen()
    {
        loadingScreen.SetActive(true);
        DisableButtons();
        StartCoroutine(Homescreen.LoadScreenCoroutine("Startscreen", loadingBar));
    }

    private void DisableButtons()
    {
        foreach (Button b in buttonsToDisable)
        {
            b.interactable = false;
        }
    }
}
