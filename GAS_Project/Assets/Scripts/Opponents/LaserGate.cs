using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LaserGate : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How long the rocket is frozen in place")]
    private float freezeTime = 1.5f;

    [SerializeField]
    [Tooltip("How much time it takes for the laser to reach the bottom")]
    private float timeTillBottom = 10.0f;

    [SerializeField]
    [Tooltip("Movement y limit for Lasers")]
    private float movementLimit = -6.2f;

    //Whether the laser gate can freeze the rocket
    private bool ableToFreeze = true;

    //Tween that saves the movement
    private Tween moveTween;

    private void Start()
    {
        moveTween = GetComponent<Rigidbody2D>().DOMoveY(movementLimit, timeTillBottom)
            .SetEase(Ease.Linear)
            .OnComplete(() => DestroyLaser());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("boop");
        if (collision.gameObject.CompareTag("rocket"))
        {
            OnRocketTouching(collision.gameObject.GetComponent<RocketControls>());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("rocket"))
        {
            OnRocketTouching(collision.gameObject.GetComponent<RocketControls>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("rocket"))
        {
            ableToFreeze = true;
        }
    }

    private void OnRocketTouching(RocketControls controls)
    {
        if (!controls.CanControl || !ableToFreeze) return;

        controls.ShockDisplay.SetActive(true);
        controls.CanControl = false;
        ableToFreeze = false;

        StartCoroutine(controls.FreezeCountDown(freezeTime));
    }

    private void DestroyLaser()
    {
        GameManager.instance.Spawner.CurrentAmountOpponents--;
        if (moveTween != null && moveTween.target != null) moveTween.Kill();

        Destroy(gameObject);
    }
}
