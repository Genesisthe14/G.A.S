using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UFO : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Time ufo needs to reach bottom")]
    private float timeTillBottom = 15.0f;

    [SerializeField]
    [Tooltip("Shattered version of the UFO")]
    private GameObject shatteredUFO;

    [SerializeField]
    [Tooltip("Force applied to the shatter parts")]
    private float force = 3.0f;

    [SerializeField]
    [Tooltip("GameObject with a particleSystem which is played when the Ufo is destroyed")]
    private GameObject particle = null;

    //AudioSource that plays the destroy sound
    private AudioSource destroySound;
    public AudioSource DestroySound
    {
        get { return destroySound; }
        set { destroySound = value; }
    }

    //Points that are used for random movement
    private List<Transform> wayPoints = null;
    public List<Transform> WayPoints
    {
        set { wayPoints = value; }
    }

    //Points where the ufos can end at off screen
    private List<Transform> endPoints = null;
    public List<Transform> EndPoints
    {
        set { endPoints = value; }
    }

    private Tween moveTween;

    //audioSource playing the movement sound
    private AudioSource movementSound;

    //base value of timeTillBottom for the percantage
    private static float startTimeTillBottom = -1;

    //Working value for timeTillBottom to raise speed
    private static float lowerTimeTillBottom = -1;

    private static float minimumTime = 2.0f;

    private static bool first = true;

    private void Awake()
    {
        if (first)
        {
            lowerTimeTillBottom = timeTillBottom;
            first = false;
        }

        movementSound = GetComponent<AudioSource>();

        if(startTimeTillBottom < 0) startTimeTillBottom = timeTillBottom;
        if (lowerTimeTillBottom > 0) timeTillBottom = lowerTimeTillBottom;
        
        GameManager.instance.GameOverEvent += StopSounds;
        GameManager.instance.PauseAllAudioEvent += PauseSounds;
    }

    public static void RaiseTempo(float percentage)
    {
        if(lowerTimeTillBottom > minimumTime)
        {
            lowerTimeTillBottom -= startTimeTillBottom * percentage / 50.0f;
            if (lowerTimeTillBottom < minimumTime) lowerTimeTillBottom = minimumTime;
        }
    }

    private void Start()
    {
        movementSound.Play();
        moveTween = GetComponent<Rigidbody2D>().DOPath(ReturnPathPoints(), timeTillBottom, PathType.Linear, PathMode.Ignore)
            .SetAutoKill(false)
            .SetEase(Ease.Linear)
            .OnComplete( () => DestroyUFO(false) );
    }

    private void OnDestroy()
    {
        GameManager.instance.GameOverEvent -= StopSounds;
        GameManager.instance.PauseAllAudioEvent -= PauseSounds;
    }

    private void StopSounds()
    {
        destroySound.Stop();
        movementSound.Stop();
    }

    private void PauseSounds(bool pause)
    {
        if (pause)
        {
            destroySound.Pause();
            movementSound.Pause();
        }
        else
        {
            destroySound.UnPause();
            movementSound.UnPause();
        }
    }

    //Generates an array of way points the ufo should take
    private Vector2[] ReturnPathPoints()
    {
        List<Vector2> path = new List<Vector2>();

        for(int i = 0; i < wayPoints.Count; i++)
        {
            bool addPoint = Random.Range(0.0f, 1.0f) > 0.5;

            if(path.Count <= 0)
            {
                if (addPoint)
                {
                    path.Add(wayPoints[i].position);
                    continue;
                }
            }
            else
            {
                bool pointLowerThanLast = wayPoints[i].position.y <= path[path.Count - 1].y ? true : false;

                if(addPoint && pointLowerThanLast)
                {
                    path.Add(wayPoints[i].position);
                    continue;
                }
            }
        }

        int randomEndPoint = Random.Range(0, endPoints.Count - 1);
        path.Add(endPoints[randomEndPoint].position);

        return path.ToArray();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionHappening(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CollisionHappening(collision);
    }

    private void CollisionHappening(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("weight") || collision.gameObject.CompareTag("shield") || collision.gameObject.CompareTag("headstartAura"))
        {
            //if the velocity of the destructor is lower the the minimum velocity then let it pass through the meteor
            if (collision.gameObject.CompareTag("weight") && !RocketBehaviour.IsWarpActive
                && collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < MeteorBehaviour.staticMinVelocity)
            {
                GetComponent<Collider2D>().isTrigger = true;
                return;
            }

            PlayerData.instance.DestroyedObjects++;

            DestroyUFO(true);
        }
        else if (collision.gameObject.CompareTag("rocket"))
        {
            DestroyUFO(false);
        }
    }

    private void DestroyUFO(bool spawnShatter = true)
    {
        Debug.Log("Puff");
        GameManager.instance.Spawner.CurrentAmountOpponents--;

        if (particle != null)
        {
            GameObject particleOb = Instantiate(particle, transform.position, transform.rotation);
            particleOb.GetComponent<ParticleSystem>().Play();
        }

        if (spawnShatter)
        {
            //Instantiate ufo broken parts
            GameObject shatteredV = Instantiate(shatteredUFO, gameObject.transform.position, gameObject.transform.rotation);

            //list of the meteor pieces
            List<GameObject> meteorParts = MeteorBehaviour.GetChildrenList(shatteredV);

            //directions the meteor pieces are supposed to fly in
            Vector2[] directions = { new Vector2(1.0f, 1.0f), new Vector2(1.0f, -1.0f), new Vector2(-1.0f, 1.0f), new Vector2(-1.0f, -1.0f) };

            foreach (GameObject part in meteorParts)
            {
                Vector2 dir = directions[Random.Range(0, 4)];

                //add impulse to the pieces of the shattered version
                Vector2 forceVec = dir * force;
                part.GetComponent<Rigidbody2D>().AddForce(forceVec, ForceMode2D.Impulse);
            }
        }

        if(moveTween != null && moveTween.target != null) moveTween.Kill();

        destroySound.Play();
        movementSound.Stop();

        Destroy(gameObject);
    }
}
