using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Does the meteor shatter when hit or is it destroyed immediately")]
    private bool shatters = false;
    public bool Shatters
    {
        get { return shatters; }
        set { shatters = value; }
    }

    [SerializeField]
    [Range(1,20)]
    [Tooltip("Number of hits the meteor needs to be destroyed")]
    private int numOfHitsToDestroy = 1;

    //Number of times this meteor has been hit
    private int currentNumHits = 0;

    [SerializeField]
    [Tooltip("Shattered version of this GameObject as prefab")]
    private GameObject shatteredVersion = null;

    [SerializeField]
    [Tooltip("Force applied to the shattered parts of the meteor when it is hit")]
    private int force = 1;

    [SerializeField]
    [Tooltip("ParticleSystem with the effect that should be played when the meteor doesn't shatter but is simply destroyed")]
    private GameObject particle;

    private float minVelocity = 18.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the meteor collides with anything else but the weight then don't do anything
        if (!collision.gameObject.CompareTag("weight")) return;

        if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < minVelocity) return;

        currentNumHits++;

        //if the current number of hits on this meteor is equal to or exceeds 
        //the number of hits necessary to destroy the meteor
        if (currentNumHits >= numOfHitsToDestroy)
        {

            //if the meteor shatters then instantiate the shattered version of this object
            if (shatters)
            {
                GameObject shatteredV = Instantiate(shatteredVersion, gameObject.transform.position, gameObject.transform.rotation);

                //list of the meteor pieces
                List<GameObject> meteorParts = GetChildrenList(shatteredV);
                
                //directions the meteor pieces are supposed to fly in
                Vector2[] directions = { new Vector2(1.0f, 1.0f), new Vector2(1.0f, -1.0f), new Vector2(-1.0f, 1.0f), new Vector2(-1.0f, -1.0f) };
                int directionIndex = 0;

                foreach(GameObject part in meteorParts)
                {
                    //add impulse to the pieces of the shattered version
                    Vector2 forceVec = directions[directionIndex++] * force;
                    part.GetComponent<Rigidbody2D>().AddForce(forceVec, ForceMode2D.Impulse);
                }
            }
            //if the meteor doesn't shatter instantiate the particlesystem that produces the destroying effect
            else
            {
                Instantiate(particle, gameObject.transform.position, gameObject.transform.rotation);
            }

            //destroy this meteor object
            Destroy(gameObject);
        }
    }

    //returns all children of a gameobject
    private List<GameObject> GetChildrenList(GameObject parent)
    {
        if (parent.transform.childCount <= 0) return null;

        List<GameObject> listChildren = new List<GameObject>();

        for(int i = 0; i < parent.transform.childCount; i++)
        {
            listChildren.Add(parent.transform.GetChild(i).gameObject);
        }

        return listChildren;
    }
}
