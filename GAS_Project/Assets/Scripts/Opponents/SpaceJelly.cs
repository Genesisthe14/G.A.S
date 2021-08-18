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

            if (!inRange)
            {
                StopCoroutine(followRoutine);

                if (rigBod == null) return;

                moveTween = rigBod.DOMoveY(Despwan.YLimit, timeToBottom / totalDistanceToCover * Mathf.Abs(transform.position.y - Despwan.YLimit))
                    .SetEase(Ease.Linear)
                    .OnComplete(() => DestroyJelly());
            }
            else
            {
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

    //Whether the jelly is on the Rocket or not
    private bool isOnRocket = false;

    private void Awake()
    {
        rigBod = GetComponent<Rigidbody2D>();
        totalDistanceToCover = Mathf.Abs(transform.position.y - Despwan.YLimit);
    }

    private void Start()
    {
        moveTween = rigBod.DOMoveY(Despwan.YLimit, timeToBottom)
            .SetEase(Ease.Linear)
            .OnComplete(() => DestroyJelly());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("rocket"))
        {
            isOnRocket = true;

            transform.parent = collision.gameObject.transform;
        }
        else if (collision.gameObject.CompareTag("weight"))
        {
            if(collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < minVelocity)
            {
                GetComponent<Collider2D>().isTrigger = true;
                return;
            }

            DestroyJelly();
        }
    }

    private void DestroyJelly()
    {
        Debug.Log("Sploosh");
        GameManager.instance.Spawner.CurrentAmountOpponents--;

        if (increaseFuelConsumption != null)
        {
            StopCoroutine(increaseFuelConsumption);
            if (GameManager.instance != null) GameManager.instance.LowerRate /= 2.0f;
        }

        if (moveTween != null && moveTween.target != null) moveTween.Kill();
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
