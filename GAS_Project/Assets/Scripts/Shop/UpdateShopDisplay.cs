using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class that is responsible for updating the displayed money in
//the shop

public class UpdateShopDisplay : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Text which displays the amount of money the player owns")]
    private Text moneyText;
    public Text MoneyText
    {
        get { return moneyText; }
    }

    //instance of player data
    private static UpdateShopDisplay _instance = null;
    public static UpdateShopDisplay instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;

        moneyText.text = "" + PlayerData.instance.CurrentMoney;
    }

    //For test purposes
    //adds 100.000 credits to the players money
    public void AddMoney()
    {
        PlayerData.instance.CurrentMoney += 100000;
        ShopItem.UpdateShopDisplayEvent.Invoke();
    }
}
