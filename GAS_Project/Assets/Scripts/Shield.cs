using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{ 
    public int hitpoints;
    
    // Schild wird nach gegebener Menge an hits zerst�rt
    // Problem: Meteoriten Stoppen 
    // M�gliche L�sung: Bei Impact zerst�ren oder ablenken

    // Update is called once per frame
    void Update()
    {
        if (hitpoints <= 0)
        {
            Destroy(gameObject);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("meteor"))
        {
            hitpoints--;
        }
    }
}
