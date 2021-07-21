using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketBehaviour : MonoBehaviour
{
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
            if (value) isWarpActive = value;
            
            //start the counting down of current warp speed factor and if that at 0 then set false
            
            OnWarpActiveEvent.Invoke(value);
        }
    }

    //factor by which the game should be sped up if headstart is active
    private static float currentWarpSpeedFactor = 0.0f;
    public static float CurrentWarpSpeedFactor
    {
        get { return currentWarpSpeedFactor; }
        set { currentWarpSpeedFactor = value; }
    }

    //The warp speed factor that should be reached
    private static float targetWarpSpeedFactor = 100.0f;
    public static float TargetWarpSpeedFactor
    {
        get { return targetWarpSpeedFactor; }
    }

    //how long to wait between raising/lowering the current warp speed factor
    private static float waitTime = 0.05f;

    //Factor by which the currentWarpSpeedFactor should be raised/lowered when eased
    private float easeValue = 2.0f;

    //Action that is Invoked when the Warp changes its active state
    private static Action<bool> OnWarpActiveEvent = null;
    
    //amount of fuel that is leaked when the rocket hits a meteor
    private static float leakingFuel = 10.0f;
    public static float LeakingFuel
    {
        get { return leakingFuel; }
        set { leakingFuel = value; }
    }

    [SerializeField]
    [Tooltip("Reference to the shield buff")]
    private GameObject shield;

    [SerializeField]
    [Tooltip("Aura shown when Headstart is active")]
    private GameObject WarpAura;

    [SerializeField]
    private GameObject damageScreen;

    private void Awake()
    {
        OnWarpActiveEvent += OnWarpActive;
    }

    private void OnDestroy()
    {
        OnWarpActiveEvent -= OnWarpActive;
    }

    //Function that activates when the active state of the warp is supposed to change
    private void OnWarpActive(bool active)
    {          
        if(active) CheckIfWarpAvailable();
        else
        {
            WarpAura.SetActive(false);
            StartCoroutine(EaseWarp());
        }
    }

    //Checks whether the player has at least one warp in his possesion
    private void CheckIfWarpAvailable()
    {
        if (PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.HEADSTART] <= 0)
        {
            Debug.Log("No Headstart available");
            IsWarpActive = false;
            return;
        }

        WarpAura.SetActive(true);

        int tempNum = PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.HEADSTART] - 1;
        PlayerData.instance.TemporaryItemsOwned.Remove(Upgrade.UpgradeTypes.HEADSTART);

        PlayerData.instance.TemporaryItemsOwned.Add(Upgrade.UpgradeTypes.HEADSTART, tempNum);

        StartCoroutine(EaseWarp());

        Debug.Log("Headstart activated");
    }

    //Makes the Warp ease in and out of the starting/anding sequence
    private IEnumerator EaseWarp()
    {
        //slow Warp down
        if(currentWarpSpeedFactor > 0.0f)
        {
            Debug.Log(currentWarpSpeedFactor);
            while (currentWarpSpeedFactor > 0.0f)
            {
                currentWarpSpeedFactor -= easeValue * 2.0f;
                yield return new WaitForSecondsRealtime(waitTime);
            }

            currentWarpSpeedFactor = 0.0f;
            isWarpActive = false;
        }
        //speed Warp up
        else if(currentWarpSpeedFactor <= 0.0f)
        {
            while (currentWarpSpeedFactor < targetWarpSpeedFactor)
            {
                currentWarpSpeedFactor += easeValue;
                yield return new WaitForSecondsRealtime(waitTime);
            }

            currentWarpSpeedFactor = targetWarpSpeedFactor;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the rocket hits a meteor then substract fuel
        if (collision.gameObject.CompareTag("meteor"))
        {
            if (shield.activeInHierarchy || IsWarpActive) return;

            Debug.Log(LeakingFuel);

            GameManager.instance.LowerFuel(leakingFuel);
            Damage();
        }
    }

    private void Damage() 
    {
        var color = damageScreen.GetComponent<Image>().color;
        color.a = 0.8f;

        damageScreen.GetComponent<Image>().color = color;
    }

    private void Update()
    {
        if (damageScreen.GetComponent<Image>().color.a > 0)
        {
            var color = damageScreen.GetComponent<Image>().color;
            color.a -= 0.01f;

            damageScreen.GetComponent<Image>().color = color;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.CompareTag("meteor") || collision.CompareTag("satellite"))) return;

        collision.GetComponent<MeteorBehaviour>().OnMeteorCollision(collision);
    }
}
