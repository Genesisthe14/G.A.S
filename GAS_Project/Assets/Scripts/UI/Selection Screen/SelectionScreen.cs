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
    }

    public void SetBoosterTaken(Upgrade.UpgradeTypes boost)
    {
        if (boostersTaken[boosterIndex] == boost) return;
        
        boostersTaken[boosterIndex] = boost;

        Debug.Log(boostersTaken[boosterIndex]);
        boosterIndex++;

        UpdateSelectScreen();

        //if both slots occupied then disable buy option for last one with Action
        //make new class for booster buy options that has attribute with what should be enabled 
        //if slots full
    }

    private void UpdateSelectScreen()
    {        
        foreach(ShopItem shop in allBoosterShopItems)
        {
            if(shop.Item.Price > PlayerData.instance.CurrentMoney)
            {
                shop.BuyButton.interactable = false;
            }
            
            //if two slots are already set
            if (boosterIndex >= boostersTaken.Length)
            {
                foreach(Upgrade.UpgradeTypes type in boostersTaken)
                {
                    if(type != shop.Item.UpgradeType)
                    {
                        shop.BuyButton.interactable = false;
                    }
                }
            }
        }
    }

    public void ResetBoostersTaken()
    {
        for(int i = 0; i < boostersTaken.Length; i++)
        {
            PlayerData.instance.CurrentMoney += SearchForBooster(boostersTaken[i]);

            boostersTaken[i] = Upgrade.UpgradeTypes.NONE;
        }

        foreach(ShopItem item in allBoosterShopItems)
        {
            item.ActivateOnBuy.SetActive(false);
            item.gameObject.SetActive(true);
        }

        boosterIndex = 0;

        //Update boosters displayed with Action
    }

    private int SearchForBooster(Upgrade.UpgradeTypes type)
    {
        foreach(Upgrade up in boosters)
        {
            if (up.UpgradeType == type) return up.Price;
        }

        return 0;
    }
}
