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

    [Tooltip("ParticleSystem with the effect that should be played when the meteor doesn't shatter but is simply destroyed")]
    private ParticleSystem particle;

    private float minVelocity = 18.0f;

    private bool particleStartedPlaying = false;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (particle.isStopped && particleStartedPlaying)
        {
            particleStartedPlaying = false;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("rocket"))
        {
            StartParticle();
        }

        //if the meteor collides with anything else but the weight then don't do anything
        if (!collision.gameObject.CompareTag("weight")) return;

        if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < minVelocity) return;

        currentNumHits++;

        //if the current number of hits on this meteor is equal to or exceeds 
        //the number of hits necessary to destroy the meteor
        if (currentNumHits >= numOfHitsToDestroy)
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

            StartParticle();
        }
    }

    private void StartParticle()
    {
        particle.Play();

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<PolygonCollider2D>().enabled = false;

        particleStartedPlaying = true;
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
