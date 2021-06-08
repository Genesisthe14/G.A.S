using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructorBehaviour : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Shade to darken the sprite to if the destructor is too slow to destroy the meteor")]
    private Color darkerShade;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!(collision.CompareTag("meteor") || collision.CompareTag("satellite"))) return;

        GetComponent<SpriteRenderer>().color = darkerShade;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!(collision.CompareTag("meteor") || collision.CompareTag("satellite"))) return;

        Debug.Log("Meteor left");

        GetComponent<SpriteRenderer>().color = Color.white;
        collision.isTrigger = false;
    }
}
