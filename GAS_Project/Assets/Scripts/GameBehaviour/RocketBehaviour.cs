using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//The class that is responsible for the behaviour of the rocket in the scene

//Note: Headstart = Warp = Hyperjump, It's all the same but was renamed several times
//and renaming the attributes would break the references in the scene and scripts have to be deleted 
//and recreated to rename them
//
//Set on backburner for now due to lack of time

public class RocketBehaviour : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Change if headstart should be faster/slower (the higher the faster)")]
    private float serializedTargetSpeedFactor = 100.0f;

    //The warp speed factor that should be reached
    //Change if headstart should be faster/slower (the higher the faster)
    private static float targetWarpSpeedFactor = 100.0f;
    public static float TargetWarpSpeedFactor
    {
        get { return targetWarpSpeedFactor; }
    }



    [SerializeField]
    [Tooltip("Reference to the shield buff")]
    private GameObject shield;

    [SerializeField]
    [Tooltip("Aura shown when Headstart is active")]
    private GameObject WarpAura;

    [SerializeField]
    [Tooltip("Screen that is displayed for a short time when the rocket takes damage")]
    private GameObject damageScreen;

    [SerializeField]
    [Tooltip("Dictionary of damage dealt for each tag")]
    private SerializableDictionary<string, float> damageDict;

    [Header("Audio")]
    [SerializeField]
    [Tooltip("AudioSource for rocket is hit sound")]
    private AudioSource rocketIsHit;

    [SerializeField]
    [Tooltip("Audiosource for the laser hit sound")]
    private AudioSource laserHitSound;
    public AudioSource LaserHitSound
    {
        get { return laserHitSound; }
    }

    [SerializeField]
    [Tooltip("Audiosource for the hyperjump")]
    private AudioSource hyperjumpSound;

    [SerializeField]
    [Tooltip("Audiosource for the shards when they are collected")]
    private AudioSource shardSound;
    public AudioSource ShardSound
    {
        get { return shardSound; }
    }

    //Number of headstarts the player has
    private static int numOfWarps = 0;
    public static int NumOfWarps
    {
        get { return numOfWarps; }
        set { numOfWarps = value; }
    }

    //Whether a headstart is active or not
    public static bool isWarpActive = false;
    public static bool IsWarpActive
    {
        get { return isWarpActive; }
        set 
        { 
            //if (value) isWarpActive = value;
            
            //start the counting down of current warp speed factor and if that at 0 then set false
            
            OnWarpActiveEvent.Invoke(value);
        }
    }

    //factor by which the game should be sped up if headstart is active
    public static float currentWarpSpeedFactor = 0.0f;
    public static float CurrentWarpSpeedFactor
    {
        get { return currentWarpSpeedFactor; }
        set { currentWarpSpeedFactor = value; }
    }

    //how long to wait between raising/lowering the current warp speed factor
    private static float waitTime = 0.05f;

    //Factor by which the currentWarpSpeedFactor should be raised/lowered when eased
    //if acceleration/deccelaration should be more rapid raise value
    private float easeValue = 2.0f;

    //Action that is Invoked when the Warp changes its active state
    private static Action<bool> OnWarpActiveEvent = null;
    
    //amount of fuel that is leaked when the rocket hits a meteor
    private static float leakingFuel = 100.0f;
    public static float LeakingFuel
    {
        get { return leakingFuel; }
        set { leakingFuel = value; }
    }

    //Attribute that stores the ease warp coroutine
    private Coroutine easeWarp = null;

    private void Awake()
    {
        targetWarpSpeedFactor = serializedTargetSpeedFactor;

        //subscribe to the OnWarpActive Event so that OnWarpActive is executed 
        //when the warp is activated
        OnWarpActiveEvent += OnWarpActive;

        damageDict.Add("UFO", GameManager.StartFuel / 100.0f * 10.0f);

        GameManager.instance.GameOverEvent += StopSounds;
        GameManager.instance.PauseAllAudioEvent += PauseSounds;
    }

    private void StopSounds()
    {
        rocketIsHit.Stop();
        laserHitSound.Stop();
        hyperjumpSound.Stop();
        shardSound.Stop();
    }

    private void PauseSounds(bool pause)
    {
        if (pause)
        {
            rocketIsHit.Pause();
            laserHitSound.Pause();
            hyperjumpSound.Pause();
            shardSound.Pause();
        }
        else
        {
            rocketIsHit.UnPause();
            laserHitSound.UnPause();
            hyperjumpSound.UnPause();
            shardSound.UnPause();
        }
    }

    private void OnDestroy()
    {
        //unsubscribe from the OnWarpActive Event
        OnWarpActiveEvent -= OnWarpActive;
        GameManager.instance.GameOverEvent -= StopSounds;
        GameManager.instance.PauseAllAudioEvent -= PauseSounds;
    }

    //Function that activates when the active state of the warp is supposed to change
    private void OnWarpActive(bool active)
    {          
        //if the player just tried to activate the warp check if it can be activated
        if(active) CheckIfWarpAvailable();
        //the warp isn't active anymore so deactivate the protective aura and slow down the rocket
        else
        {
            if (WarpAura.activeInHierarchy)
            {
                WarpAura.SetActive(false);

                if (easeWarp != null)
                    StopCoroutine(easeWarp);
                
                easeWarp = StartCoroutine(EaseWarp());
            }
            else
            {
                //isWarpActive = false;
                Debug.Log("hey");
            }
        }
    }

    //Checks whether the player has at least one warp in his possesion
    private void CheckIfWarpAvailable()
    {
        //if the player has no more warps or the warp is active but currently slowing down the don't activate the warp
        if (PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.HEADSTART] <= 0 || isWarpActive)
        {
            Debug.Log("No Hyperjump available");
            //if(!isWarpActive) IsWarpActive = false;
            return;
        }

        isWarpActive = true;

        //Activate the protective aura of the warp
        WarpAura.SetActive(true);

        //Remove one warp from the number of warps the player owns
        int tempNum = PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.HEADSTART] - 1;
        PlayerData.instance.TemporaryItemsOwned.ReplaceValue(Upgrade.UpgradeTypes.HEADSTART, tempNum);

        BoosterButtons.BoostersOwnedChangedEvent.Invoke(Upgrade.UpgradeTypes.HEADSTART);

        //Start to accelerate the rocket to simulate warp
        if(easeWarp != null)
            StopCoroutine(easeWarp);

        hyperjumpSound.Play();

        easeWarp = StartCoroutine(EaseWarp());

        if (GameManager.instance.BoosterUsedEvent != null) GameManager.instance.BoosterUsedEvent.Invoke(false);

        Debug.Log("Headstart activated");
    }

    //Makes the Warp ease in and out of the starting/anding sequence
    private IEnumerator EaseWarp()
    {
        //slow Warp down
        if(currentWarpSpeedFactor > 0.0f)
        {
            int toggleAura = 10;

            while (currentWarpSpeedFactor > 0.0f)
            {
                //deccelerates twice as fast as it accelerates
                currentWarpSpeedFactor -= easeValue * 1.5f;


                if(currentWarpSpeedFactor % toggleAura == 0)
                {
                    WarpAura.SetActive(!WarpAura.activeInHierarchy);

                    if (currentWarpSpeedFactor < 30) toggleAura = 3;
                    else if (currentWarpSpeedFactor < 60) toggleAura = 7;
                }


                yield return new WaitForSecondsRealtime(waitTime);
            }

            currentWarpSpeedFactor = 0.0f;
            isWarpActive = false;
            WarpAura.SetActive(false);
        }
        //speed Warp up
        else if(currentWarpSpeedFactor <= 0.0f)
        {
            while (currentWarpSpeedFactor < targetWarpSpeedFactor)
            {
                //Increase the speed of warp by easeValue
                currentWarpSpeedFactor += easeValue;
                yield return new WaitForSecondsRealtime(waitTime);
            }

            currentWarpSpeedFactor = targetWarpSpeedFactor;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Collision(collision);
    }

    //What happens on collision
    private void Collision(Collision2D collision)
    {
        //if the rocket hits a meteor then substract fuel
        if (damageDict.Keys.Contains(collision.gameObject.tag))
        {
            //if the shield or the warp is active then don't take damage
            if (shield.activeInHierarchy || isWarpActive) return;

            GameManager.instance.LowerFuel(damageDict[collision.gameObject.tag] / 100.0f * leakingFuel);
            rocketIsHit.Play();
            Damage();
        }
    }

    //Function that changes the alpha of the damage screen
    private void Damage() 
    {
        var color = damageScreen.GetComponent<Image>().color;
        color.a = 0.8f;

        damageScreen.GetComponent<Image>().color = color;
    }

    private void Update()
    {
        //if the alpha of the damage screen is higher than zero then slowly lower it
        if (damageScreen.GetComponent<Image>().color.a > 0)
        {
            var color = damageScreen.GetComponent<Image>().color;
            color.a -= 0.01f;

            damageScreen.GetComponent<Image>().color = color;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Laser") && !IsWarpActive) laserHitSound.Play();
        InTrigger(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        InTrigger(collision);
    }

    private void InTrigger(Collider2D collision)
    {
        if (!(collision.CompareTag("meteor") || collision.CompareTag("satellite"))) return;

        //if the shield or the warp is active then don't take damage
        if (shield.activeInHierarchy || isWarpActive)
        {
            collision.GetComponent<MeteorBehaviour>().OnMeteorCollision(null);
            return;
        }

        GameManager.instance.LowerFuel(damageDict[collision.gameObject.tag] / 100.0f * leakingFuel);
        Damage();

        //rocket collided with meteor or satellite while they were in trigger mode
        //therefore activate meteor collision function for satellite/meteor
        collision.GetComponent<MeteorBehaviour>().OnMeteorCollision(null);
    }
}
