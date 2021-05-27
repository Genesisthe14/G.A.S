using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetField : MonoBehaviour
{
    public int pullSpeed;
    public float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, lifetime * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("collect"))
        {
            collision.transform.position += Vector3.MoveTowards(collision.transform.position, this.transform.position,pullSpeed*Time.deltaTime);
        }
    }
    
        
         
    
}
