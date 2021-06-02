using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Zieht S�mtliche Fuel Meteoriten zu sich 
//Nicht zusammen mit Schild verwenden
//Zerst�rt sich nach gegebener Zeit selbst
//funzt noch nicht
public class MagnetField : BuffItem
{
    public int pullSpeed;
    public float lifetime;

    private Coroutine destoryRoutine;

    private void OnEnable()
    {
        destoryRoutine = StartCoroutine(DestorySequence());
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

        gameObject.SetActive(false);
    }

    public override void RestartItem()
    {
        StopCoroutine(destoryRoutine);

        destoryRoutine = StartCoroutine(DestorySequence());
    }
}
