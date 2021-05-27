using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{ public int hitpoints;
    // Schild wird nach gegebener Menge an hits zerstört
    // Problem: Meteoriten Stoppen 
    // Mögliche Lösung: Bei Impact zerstören oder ablenken
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hitpoints <= 0)
        {
            Destroy(this.gameObject);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("meteor"))
        {
            hitpoints -= 1;
        }
    }
   
        
   

}
