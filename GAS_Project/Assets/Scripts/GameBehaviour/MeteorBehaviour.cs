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
    
    //Number of times this meteor has been hit
    private int currentNumHits = 0;
    
    //ParticleSystem with the effect that should be played when the meteor doesn't shatter but is simply destroyed
    private ParticleSystem particle;

    //Whether the particle effect started playing already
    private bool particleStartedPlaying = false;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        //if the particle effect is not playing but has been played at least once
        if (particle.isStopped && particleStartedPlaying)
        {
            particleStartedPlaying = false;

            //if this is the last child of the parent destory the parent 
            //and this object with it
            if (destroyParent && gameObject.transform.parent.childCount == 1)
            {
                Destroy(gameObject.transform.parent.gameObject);
                return;
            }

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnMeteorCollision(collision);
    }

    //Start playing the particle effect and disable 
    //physics simulation and sprite
    public void StartParticle()
    {
        particle.Play();
        explosionSound.Play();
        
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<PolygonCollider2D>().enabled = false;

        particleStartedPlaying = true;
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

    public void OnMeteorCollision(Collision2D collision)
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
        if (!RocketBehaviour.IsWarpActive && collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < minVelocity )
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
                //spawn the same amount of coin prefabs as numCoinsToSpawn
                for (int i = 0; i < numCoinsToSpawn; i++)
                {
                    GameObject coin = Instantiate(coinPrefab);

                    coin.transform.position = transform.position;
                    coin.GetComponent<MoveCoins>().MoneyToAdd = GetComponent<GiveMoney>().Money / numCoinsToSpawn;
                    
                    //wait a random amount of time to move the coins to their target position
                    coin.GetComponent<MoveCoins>().StartWaitTime = Random.Range(0.1f, 0.8f);
                }
            }

            //if this metor doesn't shatter then just play the particle effect
            if (!shatters)
            {
                StartParticle();
                return;
            }

            //instatniate the shattered version of this meteor
            GameObject shatteredV = Instantiate(shatteredVersion, gameObject.transform.position, gameObject.transform.rotation);

            //list of the meteor pieces
            List<GameObject> meteorParts = GetChildrenList(shatteredV);

            //directions the meteor pieces are supposed to fly in
            Vector2[] directions = { new Vector2(1.0f, 1.0f), new Vector2(1.0f, -1.0f), new Vector2(-1.0f, 1.0f), new Vector2(-1.0f, -1.0f) };

            foreach (GameObject part in meteorParts)
            {
                /*if (part.CompareTag("pickup"))
                {
                    part.GetComponent<ItemPickup>().ChosenBuff = Random.Range(0, GameManager.instance.PossibleBuffs.Length);
                    continue;
                }*/

                Vector2 dir = directions[Random.Range(0, 4)];

                //add impulse to the pieces of the shattered version
                Vector2 forceVec = dir * force;
                part.GetComponent<Rigidbody2D>().AddForce(forceVec, ForceMode2D.Impulse);
            }

            StartParticle();
        }
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

        //if the destructors velocity is smaller then the minimum velocity then pass through the meteor
        if (!RocketBehaviour.IsWarpActive && collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < minVelocity)
        {
            GetComponent<Collider2D>().isTrigger = true;
            return;
        }

        currentNumHits++;

        //if the current number of hits on this meteor is equal to or exceeds 
        //the number of hits necessary to destroy the meteor
        if (currentNumHits >= numOfHitsToDestroy)
        {
            //spawn the same amount of coins as numCoinsToSpawn
            if (GetComponent<GiveMoney>() != null)
            {
                for(int i = 0; i < numCoinsToSpawn; i++)
                {
                    GameObject coin = Instantiate(coinPrefab);

                    coin.transform.position = transform.position;
                    coin.GetComponent<MoveCoins>().MoneyToAdd = GetComponent<GiveMoney>().Money / numCoinsToSpawn;
                    coin.GetComponent<MoveCoins>().StartWaitTime = Random.Range(0.1f, 0.8f);
                }
            }

            //if the meteor doesn't shatter the just play the particle effect
            if (!shatters)
            {
                StartParticle();
                return;
            }

            //instantiate the shattered version of the meteor
            GameObject shatteredV = Instantiate(shatteredVersion, gameObject.transform.position, gameObject.transform.rotation);

            //list of the meteor pieces
            List<GameObject> meteorParts = GetChildrenList(shatteredV);

            //directions the meteor pieces are supposed to fly in
            Vector2[] directions = { new Vector2(1.0f, 1.0f), new Vector2(1.0f, -1.0f), new Vector2(-1.0f, 1.0f), new Vector2(-1.0f, -1.0f) };

            foreach (GameObject part in meteorParts)
            {
                /*if (part.CompareTag("pickup"))
                {
                    part.GetComponent<ItemPickup>().ChosenBuff = Random.Range(0, GameManager.instance.PossibleBuffs.Length);
                    continue;
                }*/

                Vector2 dir = directions[Random.Range(0, 4)];

                //add impulse to the pieces of the shattered version
                Vector2 forceVec = dir * force;
                part.GetComponent<Rigidbody2D>().AddForce(forceVec, ForceMode2D.Impulse);
            }

            StartParticle();
        }
        
    }
}
