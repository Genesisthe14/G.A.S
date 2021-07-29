using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that determines the behaviour of the destructor

public class DestructorBehaviour : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Shade to darken the sprite to if the destructor is too slow to destroy the meteor")]
    private Color darkerShade;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.CompareTag("meteor") || collision.CompareTag("satellite"))) return;

        //if the destructor collides with a meteor that has a trigger then make it's shade darker
        //to simulate that it passed behind the meteor
        GetComponent<SpriteRenderer>().color = darkerShade;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!(collision.CompareTag("meteor") || collision.CompareTag("satellite"))) return;

        //if a meteor/satellite left the destructor collider then reset to normal color
        //and set the collider of the meteor/satellite no trigger
        GetComponent<SpriteRenderer>().color = Color.white;
        collision.isTrigger = false;
    }
}
