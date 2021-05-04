using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public Rigidbody2D[] windObjects;
    public int launchmultiplier;


    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {

        
            
        
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {   
    
        Debug.Log("hit");
        if (collision.tag =="Destructible")
        {

        
        foreach (var item in windObjects)
        {
            item.gameObject.transform.parent = null;
            item.AddForce(new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f)) * launchmultiplier);
            
        }
        this.gameObject.SetActive(false);
        }
    }
    
    
        
    
}
