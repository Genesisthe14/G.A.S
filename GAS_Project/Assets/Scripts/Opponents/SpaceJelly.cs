using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class SpaceJelly : MonoBehaviour
{
    [SerializeField]
    [Tooltip("time it takes to move bottom of screen")]
    private float timeToBottom;

    [SerializeField]
    [Tooltip("How long the jelly is on the rocket")]
    private float attachTime = 3.0f;

    [SerializeField]
    [Tooltip("Minimum velocity the jelly has to be hit with")]
    private float minVelocity = 10.0f;

    [SerializeField]
    [Tooltip("Positions to put the jelly on on the rocket")]
    private Vector3[] rocketPositions;

    [SerializeField]
    [Tooltip("Particle object to spawn")]
    private GameObject particleObject;

    [SerializeField]
    [Tooltip("Sprite to change to when rocket is in range of jelly")]
    private Sprite lockedOnJelly;

    //Object this jelly should follow
    private GameObject followObject;
    public GameObject FollowObject
    {
        get { return followObject; }
        set { followObject = value; }
    }

    //Whether the rocket is in range of the jelly
    private bool inRange = false;
    public bool InRange
    {
        get { return inRange; }
        set 
        { 
            inRange = value;

            if (isOnRocket) return;

            //if the jelly is not in range of the rocket
            if (!inRange)
            {
                rend.sprite = normalJelly;
                
                StopCoroutine(followRoutine);

                if (rigBod == null) return;

                moveTween = rigBod.DOMoveY(Despwan.YLimit, timeToBottom / totalDistanceToCover * Mathf.Abs(transform.position.y - Despwan.YLimit))
                    .SetEase(Ease.Linear)
                    .OnComplete(() => DestroyJelly(false));
            }
            //jelly is in range of the rocket
            else
            {
                rend.sprite = lockedOnJelly;

                if(moveTween != null && moveTween.target != null) moveTween.Kill();

                followRoutine = StartCoroutine(Follow());
            }
        }
    }

    //the total Distance the jelly has to cover to Despawn
    private float totalDistanceToCover;

    private Rigidbody2D rigBod;

    private Tween moveTween;

    //Coroutine that makes the jelly follow the rocket
    private Coroutine followRoutine;

    //Coroutine that slowly lowers the fuel of the player
    private Coroutine increaseFuelConsumption;

    //AudioSource that plays the destroy sound
    private AudioSource destroySound;
    public AudioSource DestroySound
    {
        get { return destroySound; }
        set { destroySound = value; }
    }

    //SpriteRenderer of the jelly
    private SpriteRenderer rend;

    //Sprite that is first used for jelly
    private Sprite normalJelly;

    //Whether the jelly is on the Rocket or not
    private bool isOnRocket = false;

    //base value of timeTillBottom for the percantage
    private static float startTimeTillBottom = -1;

    //Working value for timeTillBottom to raise speed
    private static float lowerTimeTillBottom = -1;

    private static float minimumTime = 1.0f;

    private static bool first = true;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();

        normalJelly = rend.sprite;
        rigBod = GetComponent<Rigidbody2D>();
        totalDistanceToCover = Mathf.Abs(transform.position.y - Despwan.YLimit);

        if (first)
        {
            lowerTimeTillBottom = timeToBottom;
            first = false;
        }

        if (startTimeTillBottom < 0) startTimeTillBottom = timeToBottom;
        if (lowerTimeTillBottom > 0) timeToBottom = lowerTimeTillBottom;

        GameManager.instance.GameOverEvent += StopSounds;
    }

    public static void RaiseTempo(float percentage)
    {
        if (lowerTimeTillBottom > minimumTime)
        {
            lowerTimeTillBottom -= startTimeTillBottom * percentage / 100.0f;
            if (lowerTimeTillBottom < minimumTime) lowerTimeTillBottom = minimumTime;
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.GameOverEvent -= StopSounds;
    }

    private void Start()
    {
        moveTween = rigBod.DOMoveY(Despwan.YLimit, timeToBottom)
            .SetEase(Ease.Linear)
            .OnComplete(() => DestroyJelly(false));
    }

    private void StopSounds()
    {
        destroySound.Stop();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("rocket"))
        {
            isOnRocket = true;

            int posIndex = 0;

            for (int i = 0; i < collision.gameObject.transform.childCount; i++)
            {
                posIndex++;

                if (posIndex > rocketPositions.Length - 1) posIndex = 0;
            }
            
            transform.parent = collision.gameObject.transform;
            transform.localPosition = rocketPositions[posIndex];
            //Vector2 localPos = collision.gameObject.transform.position + rocketPositions[posIndex];
            //rigBod.MovePosition(Vector3.zero);
        }
        else if (collision.gameObject.CompareTag("weight"))
        {
            if(collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < minVelocity)
            {
                GetComponent<Collider2D>().isTrigger = true;
                return;
            }

            PlayerData.instance.DestroyedObjects++;

            DestroyJelly();
        }
        else if (collision.gameObject.CompareTag("shield") || collision.gameObject.CompareTag("headstartAura"))
        {
            PlayerData.instance.DestroyedObjects++;
            DestroyJelly();
        }
    }

    private void DestroyJelly(bool playDestroySound = true)
    {
        Debug.Log("Sploosh");
        GameManager.instance.Spawner.CurrentAmountOpponents--;

        if (increaseFuelConsumption != null)
        {
            StopCoroutine(increaseFuelConsumption);
            if (GameManager.instance != null) GameManager.instance.LowerRate /= 2.0f;
        }

        if (moveTween != null && moveTween.target != null) moveTween.Kill();

        GameObject particle = Instantiate(particleObject);
        particle.transform.position = gameObject.transform.position;

        if(playDestroySound) destroySound.Play();
        Destroy(gameObject);
    }

    private IEnumerator Follow()
    {
        while (!isOnRocket)
        {
            rigBod.MovePosition(Vector2.Lerp(transform.position, followObject.transform.position, 0.03f));
            yield return new WaitForSeconds(0.015f);
        }

        //start jelly count down
        increaseFuelConsumption = StartCoroutine(ConsumeMoreFuel());
    }

    private IEnumerator ConsumeMoreFuel()
    {
        if(GameManager.instance != null) GameManager.instance.LowerRate *= 2.0f;

        yield return new WaitForSeconds(attachTime);

        if (GameManager.instance != null) GameManager.instance.LowerRate /= 2.0f;
        DestroyJelly();
    }
}
