using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Item that can be bought when this shopitem is clicked")]
    private Upgrade item;
    public Upgrade Item
    {
        get { return item; }
    }

    [SerializeField]
    [Tooltip("Text that displays the item name")]
    private Text itemNameText;

    [SerializeField]
    [Tooltip("Text that displays the item price")]
    private Text priceText;

    [SerializeField]
    [Tooltip("Button to buy this item")]
    private Button buyButton;
    public Button BuyButton
    {
        get { return buyButton; }
    }

    [SerializeField]
    [Tooltip("Item which should only be shown when this item was bought")]
    private GameObject activateOnBuy = null;
    public GameObject ActivateOnBuy
    {
        get { return activateOnBuy; }
    }

    //Action that is Invoked when an Item is bought
    private static Action ItemBought = null;

    //Limit for how many boosters the player can have of each type
    private int BuyLimitBoosters = 3;

    private void Awake()
    {
        if(activateOnBuy != null) activateOnBuy.SetActive(false);
        
        priceText.text = item.Price + " ";

        itemNameText.text = item.UpgradeName;

        ItemBought += UpdateActiveStateItem;
    }

    private void OnDestroy()
    {
        ItemBought -= UpdateActiveStateItem;
    }

    public void BuyItem()
    {
        if (item.Price > PlayerData.instance.CurrentMoney) return;

        if (!item.IsPermanentUpgrade)
        {
            if(PlayerData.instance.TemporaryItemsOwned[item.UpgradeType] >= 3)
            {
                PlayerData.instance.CurrentMoney -= item.Price;
                SelectionScreen.instance.SetBoosterTaken(item.UpgradeType);

                return;
            }

            item.UpgradeFeature();

            Debug.Log("Item bought: " + item.UpgradeName);

            PlayerData.instance.CurrentMoney -= item.Price;

            if (activateOnBuy != null)
            {        
                SelectionScreen.instance.SetBoosterTaken(item.UpgradeType);

                if (PlayerData.instance.TemporaryItemsOwned[item.UpgradeType] < 3) return;
            
                activateOnBuy.SetActive(true);
                gameObject.SetActive(false);
            }

        }

        item.UpgradeFeature();

        Debug.Log("Item bought: " + item.UpgradeName);

        PlayerData.instance.CurrentMoney -= item.Price;

        ItemBought.Invoke();
    }

    private void OnEnable()
    {
        UpdateActiveStateItem();
    }

    private void UpdateActiveStateItem()
    {
        //if permanent update and 
        if (item.IsPermanentUpgrade)
        {
            //check if not already obtained
            if (PlayerData.instance.PermanentUpgradeIDsPlayerOwns.ContainsKey(item.UpgradeID))
            {
                buyButton.interactable = false;

                if (activateOnBuy != null)
                {
                    activateOnBuy.SetActive(true);
                    gameObject.SetActive(false);
                }

                return;
            }

            if (item.UpgradeNum <= 1)
            {
                //check if player has enough money to buy otherwise set inactive
                if (item.Price > PlayerData.instance.CurrentMoney)
                {
                    buyButton.interactable = false;
                }
                else
                {
                    buyButton.interactable = true;
                }

                return;
            }

            //if previous update was obtained if is not first update
            string formerUpgrade = item.UpgradeType.ToString() + (item.UpgradeNum - 1);
            if (!DictContainsValue(formerUpgrade, PlayerData.instance.PermanentUpgradeIDsPlayerOwns))
            {
                //itemDisplay.interactable = false;
                gameObject.SetActive(false);
                return;
            }
        }

        //check if player has enough money to buy otherwise set inactive
        if (item.Price > PlayerData.instance.CurrentMoney)
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }


    private bool DictContainsValue(string value, SerializableDictionary<string, string> dict)
    {
        foreach(string dict_value in dict.Values)
        {
            if (dict_value == value) return true;
        }

        return false;
    }
}
