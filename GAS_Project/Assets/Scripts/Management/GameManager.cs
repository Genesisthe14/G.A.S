using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class that manages the majority of the games flow and functionality

public class GameManager : MonoBehaviour
{
    #region attributes
    #region spawning
    [Header("Spawning & Speed")]
    [SerializeField]
    [Tooltip("The amount of fuel substracted per repeatTime")]
    private float lowerRate = 0.5f;
    public float LowerRate
    {
        get { return lowerRate; }
        set { lowerRate = value; }
    }

    [SerializeField]
    [Tooltip("Interval between the substractions from the fuel")]
    private float fuelConsuptionTime = 1.0f;

    [SerializeField]
    [Tooltip("Lowest limit for the fuel consumption time to sink to")]
    private float limitFuelConsumptionTime = 0.5f;

    [SerializeField]
    [Tooltip("lowest interval between spawning meteors")]
    private float lowestSpawnInterval = 0.3f;

    [SerializeField]
    [Tooltip("Length of a section in meters which after the speed of the game is raised")]
    private int sectionLength = 10;

    [SerializeField]
    [Tooltip("Percentage to raise values by")]
    //raise when the speed between milestones should be raised more
    private int raisePercentage = 5; 

    [SerializeField]
    [Tooltip("distance traveled per second")]
    private float mPerSec = 5.0f;

    //Tells how much the target multiple is raised on every raise speed
    [SerializeField]
    [Tooltip("Raise if the speed should be raised on different multiples. E.g. with raiseIndex 1 and sectionLength 10 it would be every 10 meters. With raise Index 3 it would be every 30 meters")]
    private float increaseIndexAmount = 1.0f;

    //Index telling on what multiple of sectionLength the speed is currently raised
    private float raiseIndex = 1.0f;

    //Dictionary with initial values of the properties that are raised when the speed of the game should increase
    private Dictionary<string, float> initialSpeedValues = new Dictionary<string, float>();
    #endregion

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
    [Tooltip("Coin Manager")]
    private CoinManager coinManager;
    public CoinManager CoinM
    {
        get { return coinManager; }
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

    [SerializeField]
    [Tooltip("Indices of the layers that can trigger events like touching the screen")]
    private int[] inputMasks;

    [SerializeField]
    [Tooltip("Object representing the Shield")]
    private GameObject shieldObj;

    [Header("Audio")]
    [SerializeField]
    [Tooltip("AudioSource Crash sound")]
    private AudioSource crashSound;

    [SerializeField]
    [Tooltip("AudioSource fuel low sound")]
    private AudioSource fuelLowSound;

    [SerializeField]
    [Tooltip("AudioSource use refuel sound")]
    private AudioSource useRefuelSound;

    //instance of the GameManager for the singleton
    private static GameManager _instance;
    public static GameManager instance
    {
        get { return _instance; }
    }

    //Amount of fuel the player starts with
    private static float startFuel = 100.0f;
    public static float StartFuel
    {
        get { return startFuel; }
        set { startFuel = value; }
    }

    //how many % the fuel is filled up if refuel is hit
    private static float refuelPercent = 10.0f;
    public static float RefuelPercent
    {
        get { return refuelPercent; }
        set { refuelPercent = value; }
    }

    //Normal color of the fuelbar
    private Color normalFuelColor = new Color32(255, 255, 255, 255);

    //Total amount of fuel
    private float currentFuel;
    public float CurrentFuel
    {
        get { return currentFuel; }
        set 
        { 
            currentFuel = value;

            if (currentFuel > startFuel) currentFuel = startFuel;

            if(currentFuel <= 0.0f)
            {
                IsGameOver = true;
                gameOverObject.GameOver();
                currentFuel = 0.0f;
            }
            
            if(currentFuel <= 30f)
            {
                if(Mathf.Approximately(Mathf.Round(currentFuel) % 3.0f, 0.0f)) 
                {
                    //Debug.Log("White");
                    fillColor.GetComponent<Image>().color = normalFuelColor;
                } else {
                    //Debug.Log("red);
                    fillColor.GetComponent<Image>().color = new Color32(222, 22, 22,255);
                }

                if (!fuelLowSound.isPlaying && !isGameOver) fuelLowSound.Play();
            }
            else
            {
                fillColor.GetComponent<Image>().color = normalFuelColor;
                if (fuelLowSound.isPlaying) fuelLowSound.Stop();
            }

            fuelBar.SetFuel((int)currentFuel);
        }
    }

    //Event for when a jelly is on the rocket
    public Action<bool> JellyOnRocketEvent { get; set; } = null;

    //Whether the fuel consumption should be stopped
    private bool consumeFuel = false;
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
            float temp = Mathf.Abs(distance - value);

            distance = value;

            if (RocketBehaviour.IsWarpActive)
            {
                headstartDistanceCounter += temp;

                //if the warp has covered headstarLength distance then stop the headstart
                if (headstartDistanceCounter >= headstartLength && RocketBehaviour.CurrentWarpSpeedFactor >= RocketBehaviour.TargetWarpSpeedFactor)
                {
                    RocketBehaviour.IsWarpActive = false;
                    headstartDistanceCounter = 0.0f;
                }
            }
        }
    }

    //Attribute that keeps track of how much distance the warp has already covered
    private float headstartDistanceCounter = 0.0f;

    //Distance for which the headstart is active after activation 
    private static float headstartLength = 400.0f;
    public static float HeadstartLength
    {
        get { return headstartLength; }
        set { headstartLength = value; }
    }
    
    //Values the player had before a run and that need to be restored if he
    //abandons the run
    private Dictionary<string, int> beforeRun = new Dictionary<string, int>();
    public Dictionary<string, int> BeforeRun
    {
        get { return beforeRun; }
    }

    //time between raising the distance the player traveled
    private float[] timeIntervalDistance = { 1.0f, 0.05f };

    //Whether the crash sound was already played one
    private bool crashPlayed = false;

    //Whether the player has lost or not
    private bool isGameOver = false;
    public bool IsGameOver
    {
        get { return isGameOver; }
        set 
        { 
            isGameOver = value;

            if (isGameOver)
            {
                if (!crashPlayed)
                {
                    crashSound.Play();
                    crashPlayed = true;
                }

                if (gameOverEvent != null) gameOverEvent.Invoke();
            }

        }
    }

    //Event that is triggered when isGameOver is set to true
    private Action gameOverEvent = null;
    public Action GameOverEvent
    {
        get { return gameOverEvent; }
        set { gameOverEvent = value; }
    }

    //Event that is triggered when a booster is used
    private Action<bool> boosterUsedEvent = null;
    public Action<bool> BoosterUsedEvent
    {
        get { return boosterUsedEvent; }
        set { boosterUsedEvent = value; }
    }

    //if the player is in a run
    private static bool inRun = false;
    public static bool InRun
    {
        get { return inRun; }
        set { inRun = value; }
    }

    //time till bottom event
    public Action<float> TimeTillBottomRaiseEvent { get; set; } = null;

    //Pause all audio event
    public Action<bool> PauseAllAudioEvent { get; set; } = null;

    #endregion

    private void Awake()
    {
        _instance = this;

        Time.timeScale = 1;

        JellyOnRocketEvent += JellyFuelColor;
        
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

        GameOverEvent += StopSounds;
        PauseAllAudioEvent += PauseSounds;

        TimeTillBottomRaiseEvent += UFO.RaiseTempo;
        TimeTillBottomRaiseEvent += LaserGate.RaiseTempo;
        TimeTillBottomRaiseEvent += SpaceJelly.RaiseTempo;
    }

    private void JellyFuelColor(bool onRocket)
    {
        if (onRocket)
        {
            normalFuelColor = new Color32(124, 2, 180, 255);
        }
        else
        {
            normalFuelColor = new Color32(255, 255, 255, 255);
        }
    }

    private void OnDestroy()
    {
        TimeTillBottomRaiseEvent -= UFO.RaiseTempo;
        TimeTillBottomRaiseEvent -= LaserGate.RaiseTempo;
        TimeTillBottomRaiseEvent -= SpaceJelly.RaiseTempo;

        GameOverEvent -= StopSounds;
        PauseAllAudioEvent -= PauseSounds;
    }

    private void Start()
    {
        inRun = true;

        currentFuel = startFuel;

        moneyText.text = PlayerData.instance.CurrentMoney + "";
        distanceText.text = distance + " KM";
        
        
        //Save all initial values of attributes that are supposed to be raised over time
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

        AudioManager.StaticMusicInstance.GetComponent<AudioSource>().Play();

        //Start using fuel and raising distance traveled
        StartCoroutine(UseFuel());
        StartCoroutine(RaiseDistance());
    }

    private void StopSounds()
    {
        fuelLowSound.Stop();
        useRefuelSound.Stop();
    }

    private void PauseSounds(bool pause)
    {
        if (pause)
        {
            fuelLowSound.Pause();
            useRefuelSound.Pause();
        }
        else
        {
            fuelLowSound.UnPause();
            useRefuelSound.UnPause();
        }
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
    private IEnumerator RaiseDistance()
    {
        while (true)
        {
            //If the game is paused or over don't raise the distance anymore
            if (PauseMenu.isPaused || isGameOver || !spawner.Spawn || !consumeFuel)
            {
                yield return null;
                continue;
            }

            //use different time intervals to wait to raise distance based on whether
            //the warp is active or not
            yield return new WaitForSecondsRealtime(RocketBehaviour.IsWarpActive ? timeIntervalDistance[1] : timeIntervalDistance[0]);

            //Raise distance and adjust distance text
            if(!isGameOver)Distance += mPerSec;
            distanceText.text = (int)distance + " KM";

            //raise the speed of the game
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

            if(TimeTillBottomRaiseEvent != null) TimeTillBottomRaiseEvent.Invoke(raisePercentage);

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

    //Function that allows the player to use a Refuel booster if he still owns at least one
    public void Refuel()
    {
        //if the player has not refuel booster left do nothing
        if (PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.REFUEL] <= 0)
        {
            Debug.Log("Can't Refuel");
            return;
        }

        //substract one refuel booster form the total the player has
        int tempNum = PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.REFUEL] - 1;
        PlayerData.instance.TemporaryItemsOwned.ReplaceValue(Upgrade.UpgradeTypes.REFUEL, tempNum);

        BoosterButtons.BoostersOwnedChangedEvent.Invoke(Upgrade.UpgradeTypes.REFUEL);

        CurrentFuel += startFuel / 100.0f * refuelPercent;
        useRefuelSound.Play();

        if (boosterUsedEvent != null) boosterUsedEvent.Invoke(false);

        Debug.Log("Refueled");
    }

    //Activates the shield of the player if he is in possesion of at least one
    public void ActivateShield()
    {
        //if the player has no shield left or it is already active don't do anything
        if (PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.NUMSHIELDS] <= 0 || shieldObj.activeInHierarchy)
        {
            Debug.Log("No shields available");
            return;
        }

        //substract one from the total amount of shields the player owns
        int tempNum = PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.NUMSHIELDS] - 1;
        PlayerData.instance.TemporaryItemsOwned.ReplaceValue(Upgrade.UpgradeTypes.NUMSHIELDS, tempNum);

        BoosterButtons.BoostersOwnedChangedEvent.Invoke(Upgrade.UpgradeTypes.NUMSHIELDS);

        shieldObj.SetActive(true);
        shieldObj.GetComponent<Shield>().ShieldActiveSound.Play();

        if (boosterUsedEvent != null) boosterUsedEvent.Invoke(false);

        Debug.Log("Shield used");
    }

    //Activates the warp if the player owns at least one
    public void ActivateWarp()
    {
        if (!RocketBehaviour.IsWarpActive) RocketBehaviour.IsWarpActive = true;
        else Debug.Log("Warp Already Active");
    }
}
