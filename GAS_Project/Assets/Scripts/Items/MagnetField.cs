using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that is responsible for implementing the functionality
//of the magnet which pulls all fuelshards toward it self
//if they are in its range

public class MagnetField : BuffItem
{
    //Radius of the magnets effective range
    private static int fieldSize = 13;
    public static int FieldSize
    {
        get { return fieldSize; }
        set { fieldSize = value; }
    }
    
    //the speed at which the shards fly towards the rocket
    public int pullSpeed;

    [SerializeField]
    [Tooltip("Collider representing the magnets range")]
    private CircleCollider2D field;

    private void Awake()
    {
        field.radius = fieldSize;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //as long as a shard stays in the magnets range pull it towards the rocket
        if (collision.gameObject.CompareTag("collect"))
        {
            Vector2 veloc = gameObject.transform.position - collision.gameObject.transform.position;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = veloc * pullSpeed;
        }
    }
}
