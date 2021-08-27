using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that implements the functionality of the shield item

public class Shield : BuffItem
{
    [SerializeField]
    [Tooltip("Tags of objects to collide with")]
    private List<string> collideTags;

    [SerializeField]
    [Tooltip("Shield hit sound")]
    private AudioSource shieldHitSound;

    [SerializeField]
    [Tooltip("Shield active sound")]
    private AudioSource shieldActiveSound;
    public AudioSource ShieldActiveSound
    {
        get { return shieldActiveSound; }
    }

    //Basis hit points the shield can take
    public static int baseHitpoints = 1;
    
    //current amount of hits the shield can still receive after activation
    private int currentHitPoints;
    public int CurrentHitPoints
    {
        get { return currentHitPoints; }
        set 
        { 
            currentHitPoints = value;
            //if the shield doesn't have any hits left then 
            //deactivate it
            if (currentHitPoints <= 0)
            {
                gameObject.SetActive(false);
                shieldActiveSound.Stop();
                currentHitPoints = baseHitpoints;
            }
        }
    }

    private void Awake()
    {
        GameManager.instance.GameOverEvent += StopSounds;
        GameManager.instance.PauseAllAudioEvent += PauseSounds;
    }

    private void OnDestroy()
    {
        GameManager.instance.GameOverEvent -= StopSounds;
        GameManager.instance.PauseAllAudioEvent -= PauseSounds;
    }

    private void Start()
    {
        currentHitPoints = baseHitpoints;
    }

    private void StopSounds()
    {
        shieldHitSound.Stop();
        shieldActiveSound.Stop();
    }

    private void PauseSounds(bool pause)
    {
        if (pause)
        {
            shieldHitSound.Pause();
            shieldActiveSound.Pause();
        }
        else
        {
            shieldHitSound.UnPause();
            shieldActiveSound.UnPause();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If a meteor hits the shield substract a hit point
        if (collideTags.Contains(collision.gameObject.tag))
        {
            CurrentHitPoints--;
            shieldHitSound.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.CompareTag("meteor") || collision.CompareTag("satellite"))) return;

        //if a meteor/satellite in trigger mode hits the shield tell the meteor to destroy itself
        collision.GetComponent<MeteorBehaviour>().OnMeteorCollision(collision);
        Debug.Log("Shield Collide");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!(collision.CompareTag("meteor") || collision.CompareTag("satellite"))) return;

        //if a meteor/satellite in trigger mode hits the shield tell the meteor to destroy itself
        collision.GetComponent<MeteorBehaviour>().OnMeteorCollision(collision);
        Debug.Log("Shield Collide");
    }

    //Restarts the shield so it has full hitpoints again
    public override void RestartItem()
    {
        currentHitPoints = baseHitpoints;
    }
}
