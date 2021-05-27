using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Zieht Sämtliche Fuel Meteoriten zu sich 
//Nicht zusammen mit Schild verwenden
//Zerstört sich nach gegebener Zeit selbst
public class MagnetField : MonoBehaviour
{
    public int pullSpeed;
    public float lifetime;

    private void OnEnable()
    {
        StartCoroutine(DestorySequence());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("collect"))
        {
            Vector2 veloc = gameObject.transform.position - collision.gameObject.transform.position;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = veloc * pullSpeed;
        }
    }
        
    private IEnumerator DestorySequence()
    {
        yield return new WaitForSecondsRealtime(lifetime);

        Destroy(gameObject);
    }  
    
}
