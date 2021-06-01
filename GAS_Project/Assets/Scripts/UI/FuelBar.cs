using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    public Slider fuelSlider;

    public void SetHealth(int health)
    {
        fuelSlider.value = health;
    }
}
