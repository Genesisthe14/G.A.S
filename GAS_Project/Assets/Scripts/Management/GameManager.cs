using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Spawning & Speed")]
    [SerializeField]
    [Tooltip("The amount of fuel substracted per repeatTime")]
    private float lowerRate = 0.5f;

    [SerializeField]
    [Tooltip("Interval between the substractions from the fuel")]
    private float fuelConsuptionTime = 1.0f;

    [SerializeField]
    [Tooltip("Lowest limit for the fuel consumption time to sink to")]
    private float limitFuelConsumptionTime = 0.5f;

    [SerializeField]
    [Tooltip("lowest interval between spawning meteors")]
    private float lowestSpawnInterval = 0.5f;

    [SerializeField]
    [Tooltip("Length of distance needed to be traveled to raise the speed")]
    private int raiseSpeedDistance = 10;

    [SerializeField]
    [Tooltip("Percentage to raise values by")]
    private int raisePercentage = 5;

    [SerializeField]
    [Tooltip("Buffs the player can activate")]
    private BuffItem[] possibleBuffs;
    public BuffItem[] PossibleBuffs
    {
        get { return possibleBuffs; }
    }

    [SerializeField]
    //distance traveled per second
    private float distancePerSecond = 5.0f;

    //Threshold determining on which multiples the speed is raised
    private float raiseIndex = 1.0f;

    //How much the threshold for multiples is raised everytime
    private float increaseIndexAmount = 2.0f;

    //Dictionary with initial values of the properties that are raised when the speed of the game should increase
    private Dictionary<string, float> initialSpeedValues = new Dictionary<string, float>();


    [Header("Texts & Management")]
    [SerializeField]
    [Tooltip("Text that displays the amount of fuel left")]
    private Text fuelText;

    [SerializeField]
    [Tooltip("Text that displays the distance the player has covered")]
    private Text distanceText;

    [SerializeField]
    [Tooltip("BG Manager of the game")]
    private BGManager[] bgManagers;

    [SerializeField]
    [Tooltip("Spawner of the game")]
    private Spawner spawner;
    public Spawner Spawner
    {
        get { return spawner; }
    }

    [SerializeField]
    [Tooltip("GameOver UI Reference")]
    private GameOverUI gameOverObject;

    [SerializeField]
    [Tooltip("Fuelbar reference")]
    private FuelBar fuelBar;

    [SerializeField]
    [Tooltip("Fill reference")]
    private Image fillColor;
    public ParticleSystem particleBar;

    [SerializeField]
    private int[] inputMasks;

    
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

            if(fuel <= 0.0f)
            {
                gameOverObject.GameOver((int)distance);
                fuel = 0.0f;
            }
            if(fuel <= 30f)
            {
                if((fuel * 100f) % 3 == 0) 
                {
                    //Debug.Log("White");
                    fillColor.GetComponent<Image>().color = new Color32(255,255,255,255);
                } else {
                    //Debug.Log("Purple");
                    fillColor.GetComponent<Image>().color = new Color32(159,0,158,255);
                }
            }
            fuelBar.SetFuel((int)fuel);
            //fuelText.text = "Fuel: " + (int)fuel;
        }
    }

    //Whether the fuel consumption should be stopped
    private bool consumeFuel = true;
    public bool ConsumeFuel
    {
        get { return consumeFuel; }
        set { consumeFuel = value; }
    }

    //Distance the player has covered so far
    private float distance = 0;


    private void Awake()
    {
        //Give the camera an event mask which tells the camera which objects can react to e.g. OnMouseDown, etc.
        List<string> eventMask = new List<string>();

        foreach(int mask in inputMasks)
        {
            eventMask.Add(LayerMask.LayerToName(mask));
        }

        Camera.main.eventMask = LayerMask.GetMask(eventMask.ToArray());
    }

    private void Start()
    {
        //set the static singleton instance to this object
        _instance = this;

        fuelText.text = "Fuel: " + fuel;
        distanceText.text = distance + " KM";

        //store initial values so that percentages stay constant

        for(int i = 0; i < bgManagers.Length; i++)
        {
            initialSpeedValues.Add("speedBG" + i, bgManagers[i].Speed);
        }

        initialSpeedValues.Add("fuelConsuptionTime", fuelConsuptionTime);
        initialSpeedValues.Add("spawnRateLowerBound", spawner.InvokeTimeRange[0]);
        initialSpeedValues.Add("spawnRateUpperBound", spawner.InvokeTimeRange[1]);
        initialSpeedValues.Add("distancePerSecond", distancePerSecond);
        initialSpeedValues.Add("lowerVelocityRange", spawner.VelocityRange[0]);
        initialSpeedValues.Add("upperVelocityRange", spawner.VelocityRange[1]);

        StartCoroutine(UseFuel());
        InvokeRepeating(nameof(RaiseDistance), 1.0f, 1.0f);
    }

    private void PlayParticle() 
    {
        particleBar.Play();
    }

    //Uses substracts fuel from the tank in height of lowerRate
    private IEnumerator UseFuel()
    {
        while (true)
        {
            if (consumeFuel)
            {
                LowerFuel(lowerRate);            
                yield return new WaitForSecondsRealtime(fuelConsuptionTime);
            }
            else yield return null;
        }
    }

    //Raises the distance traveled by the player
    private void RaiseDistance()
    {
        if(distance >= 100 & !particleBar.isPlaying)
        {
            distanceText.color = Color.yellow;
            PlayParticle();
        }

        distance += distancePerSecond;
        distanceText.text = (int)distance + " KM";

        RaiseSpeed();
    }

    //Raises the speed of the game by changing the bg speed, lowering the time between 
    //fuel consuption iterations, raising the distance traveled per second and raising
    //the spawn rate of the meteors
    private void RaiseSpeed()
    {
        //if the distance exceeds the next multiple of raiseDistance then raise speed
        if (distance >= raiseSpeedDistance * raiseIndex)
        {
            raiseIndex += increaseIndexAmount;

            //raise speed bg
            for (int i = 0; i < bgManagers.Length; i++)
            {
                bgManagers[i].Speed += initialSpeedValues["speedBG"+i] * raisePercentage / 35.0f;
            }

            //raise lowering rate of fuel
            if(fuelConsuptionTime >= limitFuelConsumptionTime)
            {
                fuelConsuptionTime -= initialSpeedValues["fuelConsuptionTime"] * raisePercentage / 100.0f;
                if (fuelConsuptionTime < 0) fuelConsuptionTime = 0.0f;
            }

            //raise the distance traveled per second
            distancePerSecond += initialSpeedValues["distancePerSecond"] * raisePercentage / 100.0f;


            //raise velocity of spawned objects
            spawner.VelocityRange[0] += initialSpeedValues["lowerVelocityRange"] * raisePercentage / 50.0f;
            spawner.VelocityRange[1] += initialSpeedValues["upperVelocityRange"] * raisePercentage / 50.0f;

            //raise object spawn rate

            //if the lower spawn rate bound is 0 lower the upper bound until it is 0
            if (spawner.InvokeTimeRange[0] == 0.0f)
            {
                if (spawner.InvokeTimeRange[1] > lowestSpawnInterval)
                {
                    spawner.InvokeTimeRange[1] -= initialSpeedValues["spawnRateUpperBound"] * raisePercentage / 100.0f;

                    if (spawner.InvokeTimeRange[1] < lowestSpawnInterval) spawner.InvokeTimeRange[1] = lowestSpawnInterval;
                }

            }
            //if the lower bound spawn rate is above 0 lower it
            else
            {
                spawner.InvokeTimeRange[0] -= initialSpeedValues["spawnRateLowerBound"] * raisePercentage / 100.0f;

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
