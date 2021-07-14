using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    //Number of Fuelcontainer upgrades the player has bought
    private static int currentNumFuelUpgrades = 0;
    public static int CurrentNumFuelUpgrades
    {
        get { return currentNumFuelUpgrades; }
        set { currentNumFuelUpgrades = value; }
    }

    [SerializeField]
    [Tooltip("Sliders that represent the complete fuel bar")]
    private GameObject[] fuelSliders;

    private void Awake()
    {
        foreach(GameObject slider in fuelSliders)
        {
            slider.SetActive(false);
        }

        for(int i = 0; i <= currentNumFuelUpgrades; i++)
        {
            fuelSliders[i].SetActive(true);
        }
    }

    public void SetFuel(float fuel)
    {
        for(int i = 0; i <= currentNumFuelUpgrades; i++)
        {
            if(fuel > fuelSliders[i].GetComponent<Slider>().maxValue)
            {
                fuelSliders[i].GetComponent<Slider>().value = fuelSliders[i].GetComponent<Slider>().maxValue;
            }
            else
            {
                fuelSliders[i].GetComponent<Slider>().value = fuel;
            }
        }
    }
}
