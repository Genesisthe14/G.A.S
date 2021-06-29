using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveMoney : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How much money this meteor gives")]
    private int money = 0;

    public int AddMoney()
    {
        return money;
    }
}