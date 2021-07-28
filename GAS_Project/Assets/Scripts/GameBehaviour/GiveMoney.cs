using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that is responsible for assigning the 
// coinprefabs the value they give 

public class GiveMoney : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How much money this meteor gives")]
    private int money = 0;
    public int Money
    {
        get { return money; }
    }
}