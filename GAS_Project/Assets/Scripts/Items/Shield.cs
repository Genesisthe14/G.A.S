using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : BuffItem
{ 
    public int hitpoints;
    
    // Schild wird nach gegebener Menge an hits zerstört
    // Problem: Meteoriten Stoppen 
    // Mögliche Lösung: Bei Impact zerstören oder ablenken

    // Update is called once per frame
    void Update()
    {
        if (hitpoints <= 0)
        {
            gameObject.SetActive(false);
            hitpoints = 3;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("meteor"))
        {
            hitpoints--;
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
        hitpoints = 3;
    }
}
