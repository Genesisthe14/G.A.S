using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyRange : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Space jelly of this collider proxy")]
    private SpaceJelly jelly;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("rocket"))
        {
            jelly.FollowObject = collision.gameObject;
            jelly.InRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("rocket"))
        {
            jelly.FollowObject = null;
            jelly.InRange = false;
        }
    }
}
