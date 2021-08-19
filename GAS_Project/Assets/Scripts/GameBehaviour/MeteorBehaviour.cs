using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//Class the determines the behaviour of the meteors in the scene

public class MeteorBehaviour : MonoBehaviour
{
    [Tooltip("PlayableDircetor that plays the explosion sound")]
    public PlayableDirector explosionSound;

    [SerializeField]
    [Tooltip("Does the meteor shatter when hit or is it destroyed immediately")]
    private bool shatters = false;
    public bool Shatters
    {
        get { return shatters; }
        set { shatters = value; }
    }

    [SerializeField]
    [Tooltip("Whether the parent should be destroyed if all other children have been destroyed")]
    private bool destroyParent = false;

    [SerializeField]
    [Range(1,20)]
    [Tooltip("Number of hits the meteor needs to be destroyed")]
    private int numOfHitsToDestroy = 1;

    [SerializeField]
    [Tooltip("Force applied to the shattered parts of the meteor when it is hit")]
    private int force = 1;

    [SerializeField]
    [Tooltip("Minimum Velocity the meteor needs to be hit with to shatter")]
    private float minVelocity = 18.0f;
    
    [SerializeField]
    [Tooltip("Shattered version of this GameObject as prefab")]
    private GameObject shatteredVersion = null;

    [SerializeField]
    [Tooltip("Prefab of the coin to spawn")]
    private GameObject coinPrefab;

    [SerializeField]
    [Tooltip("How many coins to spawn")]
    private int numCoinsToSpawn = 5;

    [SerializeField]
    [Tooltip("ParticleSystem with the effect that should be played when the meteor doesn't shatter but is simply destroyed")]
    private GameObject particle = null;

    public static float staticMinVelocity = 0.0f;

    //Number of times this meteor has been hit
    private int currentNumHits = 0;

    //Whether this object spawned a shattered version
    private bool spawnedShatteredVersion = false;

    private void Awake()
    {
        staticMinVelocity = minVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnMeteorCollision(collision.collider);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        OnMeteorCollision(collision.collider);
    }

    private void OnDestroy()
    {
        if (!destroyParent && !spawnedShatteredVersion) GameManager.instance.Spawner.CurrentAmountNormalObjects--;

        //for meteor parts 
        if (destroyParent && gameObject.transform.parent.childCount == 1)
        {
            //if (substractFromTotalObjects) 
            GameManager.instance.Spawner.CurrentAmountNormalObjects--;
            Destroy(gameObject.transform.parent.gameObject);
            return;
        }
    }

    //Start playing the particle effect and disable 
    //physics simulation and sprite
    public void StartParticle()
    {
        if(particle != null)
        {
            GameObject particleOb = Instantiate(particle, transform.position, transform.rotation);
            particleOb.GetComponent<ParticleSystem>().Play();
        }

        explosionSound.Play();
        
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<PolygonCollider2D>().enabled = false;

        Destroy(gameObject);

        //particleStartedPlaying = true;
    }

    //Returns a list of all children of the given gameobject
    public static List<GameObject> GetChildrenList(GameObject parent)
    {
        if (parent.transform.childCount <= 0) return null;

        List<GameObject> listChildren = new List<GameObject>();

        for(int i = 0; i < parent.transform.childCount; i++)
        {
            listChildren.Add(parent.transform.GetChild(i).gameObject);
        }

        return listChildren;
    }

    public void OnMeteorCollision(Collider2D collision)
    {
        //if the meteor hit the rocket or shield destroy it without spawning part stones
        if (!RocketBehaviour.IsWarpActive && (collision.gameObject.CompareTag("rocket") || collision.gameObject.CompareTag("shield")))
        {
            StartParticle();
            return;
        }

        //if the meteor collides with anything else but the weight then don't do anything
        if (!RocketBehaviour.IsWarpActive && !collision.gameObject.CompareTag("weight")) return;

        //if the velocity of the destructor is lower the the minimum velocity then let it pass through the meteor
        if (!RocketBehaviour.IsWarpActive && collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < minVelocity)
        {
            GetComponent<Collider2D>().isTrigger = true;
            return;
        }

        //raise number of hits on this meteor
        currentNumHits++;

        //if the current number of hits on this meteor is equal to or exceeds 
        //the number of hits necessary to destroy the meteor
        if (currentNumHits >= numOfHitsToDestroy)
        {
            if (GetComponent<GiveMoney>() != null)
            {
                GameManager.instance.CoinM.AddCoins(transform.position, (int)Mathf.Ceil(GetComponent<GiveMoney>().Money / 2.0f));
            }

            //if this metor doesn't shatter then just play the particle effect
            if (!shatters)
            {
                StartParticle();
                return;
            }

            //instatniate the shattered version of this meteor
            GameObject shatteredV = Instantiate(shatteredVersion, gameObject.transform.position, gameObject.transform.rotation);
            if(!CompareTag("satellite")) GameManager.instance.Spawner.CurrentAmountNormalObjects++;
            spawnedShatteredVersion = true;

            //list of the meteor pieces
            List<GameObject> meteorParts = GetChildrenList(shatteredV);

            //directions the meteor pieces are supposed to fly in
            Vector2[] directions = { new Vector2(1.0f, 1.0f), new Vector2(1.0f, -1.0f), new Vector2(-1.0f, 1.0f), new Vector2(-1.0f, -1.0f) };

            foreach (GameObject part in meteorParts)
            {
                //if(part.GetComponent<MeteorBehaviour>() != null) part.GetComponent<MeteorBehaviour>().SubstractFromTotalObjects = false;
                Vector2 dir = directions[Random.Range(0, 4)];

                //add impulse to the pieces of the shattered version
                Vector2 forceVec = dir * force;
                part.GetComponent<Rigidbody2D>().AddForce(forceVec, ForceMode2D.Impulse);
            }

            StartParticle();
        }

    }
}
