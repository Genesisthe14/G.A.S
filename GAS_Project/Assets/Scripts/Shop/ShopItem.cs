using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class that depicts the items that can be bought in the shop
//and on the selection screen

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
    private int buyLimitBoosters = 3;

    private void Awake()
    {
        if(activateOnBuy != null) activateOnBuy.SetActive(false);
        
        //Set the texts that are supposed to show the important info of this shop item
        priceText.text = item.Price + "";
        itemNameText.text = item.UpgradeName;

        if(item.IsPermanentUpgrade)
            ItemBought += UpdateActiveStateItem;
    }

    private void OnDestroy()
    {
        if (item.IsPermanentUpgrade)
            ItemBought -= UpdateActiveStateItem;
    }

    //Buy the item depicted with this shop item
    public void BuyItem()
    {
        //if the player doesn't have enough money than don't do anything and return
        if (item.Price > PlayerData.instance.CurrentMoney) return;

        //if this shop item is a booster
        if (!item.IsPermanentUpgrade)
        {
            //if the player has already reached the limit of boosters then don't add a new booster
            //just equip it
            if(PlayerData.instance.TemporaryItemsOwned[item.UpgradeType] >= buyLimitBoosters)
            {
                PlayerData.instance.CurrentMoney -= item.Price;
                SelectionScreen.instance.SetBoosterTaken(item.UpgradeType);

                activateOnBuy.SetActive(true);
                gameObject.SetActive(false);
                return;
            }

            //Add the booster 
            item.UpgradeFeature();

            Debug.Log("Item bought: " + item.UpgradeName);

            //substract the money
            PlayerData.instance.CurrentMoney -= item.Price;

            if (activateOnBuy != null)
            {        
                SelectionScreen.instance.SetBoosterTaken(item.UpgradeType);

                //if the number of the boost of this shop item is still below the buy limit
                //then don't activate the object that is displayed when the buy limit is reached
                if (PlayerData.instance.TemporaryItemsOwned[item.UpgradeType] < buyLimitBoosters) return;
            
                activateOnBuy.SetActive(true);
                gameObject.SetActive(false);
            }

            return;
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

    //Update the active state of this shop item
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

    //Check if the given serializable dictionary contains the given string
    private bool DictContainsValue(string value, SerializableDictionary<string, string> dict)
    {
        foreach(string dict_value in dict.Values)
        {
            if (dict_value == value) return true;
        }

        return false;
    }
}
