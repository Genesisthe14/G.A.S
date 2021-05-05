using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //instance of the GameManager for the singleton
    public static GameManager instance;

    [SerializeField]
    [Tooltip("Interval between the substractions from the fuel")]
    private float reapeatTime = 1.0f;

    [SerializeField]
    [Tooltip("The amount of fuel substracted per repeatTime")]
    private float lowerRate = 0.5f;

    //Total amount of fuel
    private float fuel = 100.0f;
    public float Fuel
    {
        get { return fuel; }
        set 
        { 
            fuel = value;
            fuelText.text = "Fuel: " + GameManager.instance.Fuel;
        }
    }

    [SerializeField]
    [Tooltip("Text that displays the amount of fuel left")]
    private Text fuelText;

    private void Start()
    {
        //set the static singleton instance to this object
        instance = this;

        InvokeRepeating("LowerFuel", reapeatTime, reapeatTime);
        fuelText.text = "Fuel: " + GameManager.instance.Fuel;
    }

    //Lowers the fuel by lowerRate
    private void LowerFuel()
    {
        GameManager.instance.Fuel -= lowerRate;
    }

    //Adds the specified amount of fuel to the current fuel left
    public void AddFuel(float addedFuel)
    {
        GameManager.instance.Fuel += addedFuel;
    }
}
