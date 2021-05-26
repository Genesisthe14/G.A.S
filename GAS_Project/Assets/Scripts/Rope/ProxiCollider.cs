using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxiCollider : MonoBehaviour
{
    private Collider2D proxiColl;
    public Collider2D ProxiColl
    {
        get { return proxiColl; }
    }

    private int colliderID;
    public int ColliderID
    {
        get { return colliderID; }
        set { colliderID = value; }
    }

    private bool collided = false;
    public bool Collided
    {
        get { return collided; }
        set { collided = value; }
    }

    private void Awake()
    {
        proxiColl = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collided = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collided = false; 
    }
}
