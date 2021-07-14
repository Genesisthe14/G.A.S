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

    [SerializeField]
    [Tooltip("Text that displays the item name")]
    private Text itemNameText;

    [SerializeField]
    [Tooltip("Text that displays the item price")]
    private Text priceText;

    [SerializeField]
    [Tooltip("Price of this shop item")]
    private int price;

    [SerializeField]
    [Tooltip("Button depicting this item")]
    private Button itemDisplay;

    [SerializeField]
    [Tooltip("Item which should only be shown when this item was bought")]
    private GameObject activateOnBuy = null;

    private void Awake()
    {
        if(activateOnBuy != null) activateOnBuy.SetActive(false);
        
        priceText.text = price + " ";

        string itemName = item.UpgradeType.ToString().ToLower();

        itemName = char.ToUpper(itemName[0]) + itemName.Substring(1);

        itemNameText.text = itemName + " Upgrade " + item.UpgradeNum;
    }

    public void BuyItem()
    {
        if (price > PlayerData.instance.CurrentMoney) return;
        
        item.UpgradeFeature();

        Debug.Log("Item bought: " + item.UpgradeType.ToString() + " "+item.UpgradeNum);

        PlayerData.instance.CurrentMoney -= price;

        UpdateActiveStateItem();
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
                itemDisplay.interactable = false;

                if (activateOnBuy != null)
                {
                    activateOnBuy.SetActive(true);
                    gameObject.SetActive(false);
                }

                return;
            }

            //if previous update was obtained if is not first update
            if (item.UpgradeNum <= 1) return;

            string formerUpgrade = item.UpgradeType.ToString() + (item.UpgradeNum - 1);
            if (!DictContainsValue(formerUpgrade, PlayerData.instance.PermanentUpgradeIDsPlayerOwns))
            {
                //itemDisplay.interactable = false;
                gameObject.SetActive(false);
                return;
            }
        }

        //check if player has enough money to buy otherwise set inactive
        if(price > PlayerData.instance.CurrentMoney)
        {
            itemDisplay.interactable = false;
            return;
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
