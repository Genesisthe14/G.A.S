using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [Tooltip("Total Coins Text")]
    private Text totalCoins;

    [SerializeField]
    [Tooltip("List of all dictionaries containing the icons for the correct slots")]
    //index of lists is number slot and upgrade type is key for correct GameObject to activate
    private List<SerializableDictionary<Upgrade.UpgradeTypes, GameObject>> slotIcons;

    private Dictionary<Upgrade.UpgradeTypes, int> amountBeforeBuy = new Dictionary<Upgrade.UpgradeTypes, int>();

    //index of the current booster slot
    private static int boosterIndex = 0;

    private static SelectionScreen _instance;
    public static SelectionScreen instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;

        foreach (Upgrade up in boosters)
        {
            amountBeforeBuy.Add(up.UpgradeType, 0);
        }
    }

    private void OnEnable()
    {
        totalCoins.text = "" + PlayerData.instance.CurrentMoney;
        
        foreach (Upgrade up in boosters)
        {
            if(amountBeforeBuy.ContainsKey(up.UpgradeType)) amountBeforeBuy[up.UpgradeType] = PlayerData.instance.TemporaryItemsOwned[up.UpgradeType];
            else amountBeforeBuy.Add(up.UpgradeType, PlayerData.instance.TemporaryItemsOwned[up.UpgradeType]);
        }
    }

    public void SetBoosterTaken(Upgrade.UpgradeTypes boost)
    {
        if (BoostersContain(boost) || boosterIndex >= boostersTaken.Length)
        {
            UpdateSelectScreen();
            return;
        }

        
        boostersTaken[boosterIndex] = boost;

        //List of dictionaries to then activate the correct icons for slots


        Debug.Log(boostersTaken[boosterIndex]);
        boosterIndex++;

        UpdateSelectScreen();

        //if both slots occupied then disable buy option for last one with Action
        //make new class for booster buy options that has attribute with what should be enabled 
        //if slots full
    }

    private bool BoostersContain(Upgrade.UpgradeTypes boost)
    {
        foreach(Upgrade.UpgradeTypes up in boostersTaken)
        {
            if (up == boost) return true;
        }

        return false;
    }

    private void UpdateSelectScreen()
    {
        totalCoins.text = "" + PlayerData.instance.CurrentMoney;

        foreach (ShopItem shop in allBoosterShopItems)
        {
            if(shop.Item.Price > PlayerData.instance.CurrentMoney)
            {
                shop.BuyButton.interactable = false;
            }
            else shop.BuyButton.interactable = true;

            //if two slots are already set
            if (boosterIndex >= boostersTaken.Length)
            {
                bool isInBoostersTaken = false;

                foreach(Upgrade.UpgradeTypes type in boostersTaken)
                {
                    //set the booster item that wasnt taken to not interactable
                    if (type == shop.Item.UpgradeType)
                    {
                        isInBoostersTaken = true;
                        break;
                    }
                }

                shop.BuyButton.interactable = isInBoostersTaken;
            }
        }
    }

    public void ResetBoostersTaken(bool afterRun = false)
    {
        for(int i = 0; i < boostersTaken.Length; i++)
        {
            if (!afterRun && boostersTaken[i] != Upgrade.UpgradeTypes.NONE)
            {
                int differenceOwned = Math.Abs(PlayerData.instance.TemporaryItemsOwned[boostersTaken[i]] - amountBeforeBuy[boostersTaken[i]]);
                Debug.Log("Before: "+PlayerData.instance.TemporaryItemsOwned[boostersTaken[i]]);

                PlayerData.instance.CurrentMoney += SearchForBoosterPrice(boostersTaken[i]) * differenceOwned;
                PlayerData.instance.TemporaryItemsOwned.Remove(boostersTaken[i]);
                PlayerData.instance.TemporaryItemsOwned.Add(boostersTaken[i], amountBeforeBuy[boostersTaken[i]]);

                Debug.Log("After: "+PlayerData.instance.TemporaryItemsOwned[boostersTaken[i]]);
            }

            boostersTaken[i] = Upgrade.UpgradeTypes.NONE;
        }
        
        boosterIndex = 0;

        if (afterRun) return;

        foreach(ShopItem item in allBoosterShopItems)
        {
            item.ActivateOnBuy.SetActive(false);
            item.gameObject.SetActive(true);
        }



        //Update boosters displayed with Action
    }

    private int SearchForBoosterPrice(Upgrade.UpgradeTypes type)
    {
        foreach(Upgrade up in boosters)
        {
            if (up.UpgradeType == type) return up.Price;
        }

        return 0;
    }
}
