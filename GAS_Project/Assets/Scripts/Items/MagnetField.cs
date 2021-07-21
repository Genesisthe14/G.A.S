using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Zieht S�mtliche Fuel Meteoriten zu sich 
//Nicht zusammen mit Schild verwenden
//Zerst�rt sich nach gegebener Zeit selbst
//funzt noch nicht
public class MagnetField : BuffItem
{
    private static int fieldSize = 13;
    public static int FieldSize
    {
        get { return fieldSize; }
        set { fieldSize = value; }
    }
    
    public int pullSpeed;
    public float lifetime;

    [SerializeField]
    [Tooltip("Collider representing the magnetfield")]
    private CircleCollider2D field;

    private Coroutine destoryRoutine;

    private void Awake()
    {
        field.radius = fieldSize;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("collect"))
        {
            Vector2 veloc = gameObject.transform.position - collision.gameObject.transform.position;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = veloc * pullSpeed;
        }
    }
}
