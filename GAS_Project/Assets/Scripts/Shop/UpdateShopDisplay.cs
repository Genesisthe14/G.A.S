using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateShopDisplay : MonoBehaviour
{
    [SerializeField]
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

    public void AddMoney()
    {
        PlayerData.instance.CurrentMoney += 10000;
    }
}
