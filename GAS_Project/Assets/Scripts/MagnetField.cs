using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Zieht Sämtliche Fuel Meteoriten zu sich 
//Nicht zusammen mit Schild verwenden
//Zerstört sich nach gegebener Zeit selbst
//funzt noch nicht
public class MagnetField : MonoBehaviour
{
    public int pullSpeed;
    public float lifetime;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   //funzt net zerstört zu schnell
        //Destroy(this.gameObject, lifetime * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("collect"))
        {
            collision.GetComponent<Rigidbody2D>().AddForce(Vector2.MoveTowards(collision.transform.position, this.transform.position, pullSpeed * Time.deltaTime)) ;
        }
    }
    
        
         
    
}
