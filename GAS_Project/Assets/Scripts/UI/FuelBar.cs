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

    [SerializeField]
    [Tooltip("Color for the fuelbars when the jelly is on the rocket")]
    private Color jellyColor;

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

        GameManager.instance.JellyOnRocketEvent += ChangeFuelbarColor;
    }

    private void ChangeFuelbarColor(bool jellyOnRocket)
    {
        if (jellyOnRocket)
        {
            foreach (GameObject slider in fuelSliders)
            {
                slider.GetComponent<Slider>().fillRect.gameObject.GetComponent<Image>().color = jellyColor;
            }
        }
        else
        {
            foreach (GameObject slider in fuelSliders)
            {
                slider.GetComponent<Slider>().fillRect.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
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
