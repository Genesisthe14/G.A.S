using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region attributes
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
    [Tooltip("Length of a section in meters which after the speed of the game is raised")]
    private int sectionLength = 10;

    [SerializeField]
    [Tooltip("Percentage to raise values by")]
    private int raisePercentage = 5;

    [SerializeField]
    [Tooltip("distance traveled per second")]
    private float mPerSec = 5.0f;

    //Threshold determining on which multiples the speed is raised
    private float raiseIndex = 1.0f;

    //How much the threshold for multiples is raised everytime
    private float increaseIndexAmount = 1.0f;

    //Dictionary with initial values of the properties that are raised when the speed of the game should increase
    private Dictionary<string, float> initialSpeedValues = new Dictionary<string, float>();


    [Header("Buffs")]
    [SerializeField]
    [Tooltip("Buffs the player can activate")]
    private BuffItem[] possibleBuffs;
    public BuffItem[] PossibleBuffs
    {
        get { return possibleBuffs; }
    }


    [Header("Texts, References & Management")]
    [SerializeField]
    [Tooltip("Text that displays the amount of money player has")]
    private Text moneyText;
    public Text MoneyText
    {
        get { return moneyText; }
    }

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
    [Tooltip("Indices of the layers that can trigger events like touching the screen")]
    private int[] inputMasks;

    [SerializeField]
    [Tooltip("Object representing the Shield")]
    private GameObject shieldObj;

    
    //instance of the GameManager for the singleton
    private static GameManager _instance;
    public static GameManager instance
    {
        get { return _instance; }
    }

    //Amount of fuel the player starts with
    private static float startFuel = 100;
    public static float StartFuel
    {
        get { return startFuel; }
        set { startFuel = value; }
    }

    //Total amount of fuel
    private float currentFuel;
    public float CurrentFuel
    {
        get { return currentFuel; }
        set 
        { 
            currentFuel = value;

            if(currentFuel <= 0.0f)
            {
                gameOverObject.GameOver((int)distance);
                currentFuel = 0.0f;
            }
            
            if(currentFuel <= 30f)
            {
                if((currentFuel * 100f) % 3 == 0) 
                {
                    //Debug.Log("White");
                    fillColor.GetComponent<Image>().color = new Color32(255,255,255,255);
                } else {
                    //Debug.Log("Purple");
                    fillColor.GetComponent<Image>().color = new Color32(159,0,158,255);
                }
            }
            else
            {
                fillColor.GetComponent<Image>().color = new Color32(159, 0, 158, 255);
            }

            fuelBar.SetFuel((int)currentFuel);
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
    public float Distance
    {
        get { return distance; }
        set 
        { 
            distance = value;
            if (RocketBehaviour.IsHeadstartActive)
            {
                //if the speed has increased by about 40% (which is reached after raising the speed 
                //by 10% 4-times) then stop the headstart
                if(distance >= sectionLength * (increaseIndexAmount * 4))
                {
                    RocketBehaviour.IsHeadstartActive = false;
                }
            }
        }
    }
    
    [SerializeField]
    private Dictionary<string, int> beforeRun = new Dictionary<string, int>();
    public Dictionary<string, int> BeforeRun
    {
        get { return beforeRun; }
    }

    //time between raising the distance the player traveled
    private float[] timeIntervalDistance = { 1.0f, 0.1f };
    #endregion

    private void Awake()
    {
        //Give the camera an event mask which tells the camera which objects can react to e.g. OnMouseDown, etc.
        List<string> eventMask = new List<string>();

        foreach(int mask in inputMasks)
        {
            eventMask.Add(LayerMask.LayerToName(mask));
        }

        Camera.main.eventMask = LayerMask.GetMask(eventMask.ToArray());

        //store the initial values of certain attributes to restore them if the player exits the
        //game via the pause menu
        beforeRun.Add("currentMoney", PlayerData.instance.CurrentMoney);
        beforeRun.Add("numShields", PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.NUMSHIELDS]);
        beforeRun.Add("refuels", PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.REFUEL]);
        beforeRun.Add("headstarts", PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.HEADSTART]);
    }

    private void Start()
    {
        _instance = this;

        currentFuel = startFuel;

        moneyText.text = PlayerData.instance.CurrentMoney + "";
        distanceText.text = distance + " KM";

        //store initial values so that percentages stay constant

        for (int i = 0; i < bgManagers.Length; i++)
        {
            initialSpeedValues.Add("speedBG" + i, bgManagers[i].Speed);
        }

        initialSpeedValues.Add("fuelConsuptionTime", fuelConsuptionTime);
        initialSpeedValues.Add("spawnRateLowerBound", spawner.InvokeTimeRange[0]);
        initialSpeedValues.Add("spawnRateUpperBound", spawner.InvokeTimeRange[1]);
        initialSpeedValues.Add("distancePerSecond", mPerSec);
        initialSpeedValues.Add("lowerVelocityRange", spawner.VelocityRange[0]);
        initialSpeedValues.Add("upperVelocityRange", spawner.VelocityRange[1]);

        StartCoroutine(UseFuel());
        StartCoroutine(RaiseDistance());
        //InvokeRepeating(nameof(RaiseDistance), 1.0f, 1.0f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2)) Refuel();
        if (Input.GetMouseButtonDown(1)) RocketBehaviour.IsHeadstartActive = true; //ActivateShield();
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
            if (consumeFuel && !RocketBehaviour.IsHeadstartActive)
            {
                LowerFuel(lowerRate);            
                yield return new WaitForSecondsRealtime(fuelConsuptionTime);
            }
            else yield return null;
        }
    }

    //Raises the distance traveled by the player
    private IEnumerator RaiseDistance()
    {
        while (true)
        {
            if (PauseMenu.GamePaused)
            {
                yield return null;
                continue;
            }

            yield return new WaitForSecondsRealtime(RocketBehaviour.IsHeadstartActive ? timeIntervalDistance[1] : timeIntervalDistance[0]);

            if(distance >= 100 & !particleBar.isPlaying)
            {
                distanceText.color = Color.yellow;
                PlayParticle();
            }

            Distance += mPerSec;
            distanceText.text = (int)distance + " KM";

            RaiseSpeed();
        }
    }

    //Raises the speed of the game by changing the bg speed, lowering the time between 
    //fuel consuption iterations, raising the distance traveled per second and raising
    //the spawn rate of the meteors
    private void RaiseSpeed()
    {
        //if the distance exceeds the next multiple of raiseDistance then raise speed
        if (distance >= sectionLength * raiseIndex)
        {
            raiseIndex += increaseIndexAmount;

            //raise speed bg
            for (int i = 0; i < bgManagers.Length; i++)
            {
                bgManagers[i].Speed += initialSpeedValues["speedBG"+i] * raisePercentage / 10.0f;
            }

            //raise lowering rate of fuel
            if(fuelConsuptionTime >= limitFuelConsumptionTime)
            {
                fuelConsuptionTime -= initialSpeedValues["fuelConsuptionTime"] * raisePercentage / 100.0f;
                if (fuelConsuptionTime < 0) fuelConsuptionTime = 0.0f;
            }

            //raise the distance traveled per second
            mPerSec += initialSpeedValues["distancePerSecond"] * raisePercentage / 100.0f;


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
        CurrentFuel -= lower;
    }

    //Adds the specified amount of fuel to the current fuel left
    public void AddFuel(float addedFuel)
    {
        CurrentFuel += addedFuel;
    }

    public void Refuel()
    {
        if (PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.REFUEL] <= 0)
        {
            Debug.Log("Can't Refuel");
            return;
        }

        int tempNum = PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.REFUEL] - 1;
        PlayerData.instance.TemporaryItemsOwned.Remove(Upgrade.UpgradeTypes.REFUEL);

        PlayerData.instance.TemporaryItemsOwned.Add(Upgrade.UpgradeTypes.REFUEL, tempNum);

        CurrentFuel = startFuel;
        Debug.Log("Refueled");
    }

    public void ActivateShield()
    {
        if (PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.NUMSHIELDS] <= 0)
        {
            Debug.Log("No shields available");
            return;
        }

        int tempNum = PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.NUMSHIELDS] - 1;
        PlayerData.instance.TemporaryItemsOwned.Remove(Upgrade.UpgradeTypes.NUMSHIELDS);

        PlayerData.instance.TemporaryItemsOwned.Add(Upgrade.UpgradeTypes.NUMSHIELDS, tempNum);

        shieldObj.SetActive(true);
    }
}
