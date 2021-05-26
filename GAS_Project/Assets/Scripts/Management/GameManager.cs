using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The amount of fuel substracted per repeatTime")]
    private float lowerRate = 0.5f;

    [SerializeField]
    [Tooltip("Interval between the substractions from the fuel")]
    private float fuelConsuptionTime = 1.0f;

    [SerializeField]
    [Tooltip("lowest interval between spawning meteors")]
    private float lowestSpawnInterval = 0.5f;

    [SerializeField]
    [Tooltip("Length of distance needed to be traveled to raise the speed")]
    private int raiseDistance = 1200000;

    [SerializeField]
    [Tooltip("Percentage to raise values by")]
    private int raisePercentage = 5;

    [SerializeField]
    [Tooltip("Text that displays the amount of fuel left")]
    private Text fuelText;

    [SerializeField]
    [Tooltip("Text that displays the distance the player has covered")]
    private Text distanceText;

    [SerializeField]
    [Tooltip("BG Manager of the game")]
    private BGManager bgManager;

    [SerializeField]
    [Tooltip("Spawner of the game")]
    private Spawner spawner;

    //instance of the GameManager for the singleton
    private static GameManager _instance;
    public static GameManager instance
    {
        get { return _instance; }
    }

    //Total amount of fuel
    private float fuel = 100;
    public float Fuel
    {
        get { return fuel; }
        set 
        { 
            fuel = value;

            fuelText.text = "Fuel: " + (int)fuel;
        }
    }

    //Distance the player has covered so far
    private float distance = 0;

    //distance traveled per second
    private float distancePerSecond = 100.0f;

    //Threshold determining on which multiples the speed is raised
    private float raiseIndex = 1.0f;

    //How much the threshold for multiples is raised everytime
    private float increaseIndexAmount = 2.0f;

    //Dictionary with initial values of the properties that are raised when the speed of the game should increase
    private Dictionary<string, float> initialSpeedValues = new Dictionary<string, float>();

    private void Start()
    {
        //set the static singleton instance to this object
        _instance = this;

        fuelText.text = "Fuel: " + fuel;
        distanceText.text = "Distance: " + distance;

        //store initial values so that percentages stay constant
        initialSpeedValues.Add("speedBG", bgManager.Speed);
        initialSpeedValues.Add("fuelConsuptionTime", fuelConsuptionTime);
        initialSpeedValues.Add("spawnRateLowerBound", spawner.InvokeTimeRange[0]);
        initialSpeedValues.Add("spawnRateUpperBound", spawner.InvokeTimeRange[1]);
        initialSpeedValues.Add("distancePerSecond", distancePerSecond);

        StartCoroutine(UseFuel());
        InvokeRepeating(nameof(RaiseDistance), 1.0f, 1.0f);
    }

    //Uses substracts fuel from the tank in height of lowerRate
    private IEnumerator UseFuel()
    {
        while (true)
        {
            LowerFuel(lowerRate);
            yield return new WaitForSecondsRealtime(fuelConsuptionTime);
        }
    }

    //Raises the distance traveled by the player
    private void RaiseDistance()
    {
        distance += distancePerSecond;
        distanceText.text = "Distance: " + (int)distance;

        RaiseSpeed();
    }

    //Raises the speed of the game by changing the bg speed, lowering the time between 
    //fuel consuption iterations, raising the distance traveled per second and raising
    //the spawn rate of the meteors
    private void RaiseSpeed()
    {
        //if the distance exceeds the next multiple of raiseDistance then raise speed
        if (distance >= raiseDistance * raiseIndex)
        {
            raiseIndex += increaseIndexAmount;

            //raise speed bg
            bgManager.Speed += initialSpeedValues["speedBG"] * raisePercentage / 100.0f;

            //raise lowering rate of fuel
            if(fuelConsuptionTime > 0)
            {
                fuelConsuptionTime -= initialSpeedValues["fuelConsuptionTime"] * raisePercentage / 100.0f;
                if (fuelConsuptionTime < 0) fuelConsuptionTime = 0.0f;
            }

            //raise the distance traveled per second
            distancePerSecond += initialSpeedValues["distancePerSecond"] * raisePercentage / 100.0f;


            //raise object spawn rate

            //if the lower spawn rate bound is 0 lower the upper bound until it is 0
            if (spawner.InvokeTimeRange[0] == 0.0f)
            {
                if (spawner.InvokeTimeRange[1] > lowestSpawnInterval)
                {
                    spawner.InvokeTimeRange[1] -= initialSpeedValues["spawnRateUpperBound"] * raisePercentage * 1.5f / 100.0f;

                    if (spawner.InvokeTimeRange[1] < lowestSpawnInterval) spawner.InvokeTimeRange[1] = lowestSpawnInterval;
                }

            }
            //if the lower bound spawn rate is above 0 lower it
            else
            {
                spawner.InvokeTimeRange[0] -= initialSpeedValues["spawnRateLowerBound"] * raisePercentage * 1.5f / 100.0f;

                if (spawner.InvokeTimeRange[0] < 0.0f) spawner.InvokeTimeRange[0] = 0.0f;
            }

        }
    }

    //Lowers the fuel by lowerRate
    public void LowerFuel(float lower)
    {
        Fuel -= lower;
    }

    //Adds the specified amount of fuel to the current fuel left
    public void AddFuel(float addedFuel)
    {
        Fuel += addedFuel;
    }
}
