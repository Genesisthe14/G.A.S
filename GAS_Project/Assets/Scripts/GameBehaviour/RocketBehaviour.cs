using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketBehaviour : MonoBehaviour
{
    //Number of headstarts the player has
    private static int numOfHeadstarts = 0;
    public static int NumOfHeadstarts
    {
        get { return numOfHeadstarts; }
        set { numOfHeadstarts = value; }
    }

    //Whether a headstart is active or not
    private static bool isHeadstartActive = false;
    public static bool IsHeadstartActive
    {
        get { return isHeadstartActive; }
        set 
        { 
            isHeadstartActive = value;
            OnHeadstartActiveEvent.Invoke(isHeadstartActive);
        }
    }

    //factor by which the game should be sped up if headstart is active
    private static float headstartSpeedFactor = 10.0f;
    public static float HeadstartSpeedFactor
    {
        get { return headstartSpeedFactor; }
    }

    private static Action<bool> OnHeadstartActiveEvent = null;
    
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
    private GameObject headstartAura;

    [SerializeField]
    private GameObject damageScreen;

    private void Awake()
    {
        OnHeadstartActiveEvent += OnHeadstartActive;
        CheckIfHeadstartAvailable();
    }

    private void OnDestroy()
    {
        OnHeadstartActiveEvent -= OnHeadstartActive;
    }

    private void CheckIfHeadstartAvailable()
    {
        if (PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.HEADSTART] <= 0)
        {
            Debug.Log("No Headstart available");
            return;
        }

        int tempNum = PlayerData.instance.TemporaryItemsOwned[Upgrade.UpgradeTypes.HEADSTART] - 1;
        PlayerData.instance.TemporaryItemsOwned.Remove(Upgrade.UpgradeTypes.HEADSTART);

        PlayerData.instance.TemporaryItemsOwned.Add(Upgrade.UpgradeTypes.HEADSTART, tempNum);

        IsHeadstartActive = true;
        Debug.Log("Headstart activated");
    }

    private void OnHeadstartActive(bool isActive)
    {
        headstartAura.SetActive(isActive);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the rocket hits a meteor then substract fuel
        if (collision.gameObject.CompareTag("meteor"))
        {
            if (shield.activeInHierarchy || IsHeadstartActive) return;

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
