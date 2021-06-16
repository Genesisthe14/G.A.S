using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : BuffItem
{
    [HideInInspector]
    //save
    public static int baseHitpoints = 3;
    
    [SerializeField]
    private int currentHitPoints = 3;

    // Schild wird nach gegebener Menge an hits zerstört
    // Problem: Meteoriten Stoppen 
    // Mögliche Lösung: Bei Impact zerstören oder ablenken

    private void Start()
    {
        currentHitPoints = baseHitpoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHitPoints <= 0)
        {
            gameObject.SetActive(false);
            currentHitPoints = baseHitpoints;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("meteor"))
        {
            currentHitPoints--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.CompareTag("meteor") || collision.CompareTag("satellite"))) return;

        collision.GetComponent<MeteorBehaviour>().OnMeteorCollision(collision);
        Debug.Log("Shield Collide");
    }

    public override void RestartItem()
    {
        currentHitPoints = baseHitpoints;
    }
}
