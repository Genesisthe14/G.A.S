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

    //AudioSource playing the laser move sound
    private AudioSource laserMoveSound;

    //base value of timeTillBottom for the percantage
    private static float startTimeTillBottom = -1;

    //Working value for timeTillBottom to raise speed
    private static float lowerTimeTillBottom = -1;

    private static float minimumTime = 2.0f;

    private static bool first = true;

    private void Awake()
    {
        laserMoveSound = GetComponent<AudioSource>();

        if (first)
        {
            lowerTimeTillBottom = timeTillBottom;
            first = false;
        }

        if (startTimeTillBottom < 0) startTimeTillBottom = timeTillBottom;
        if (lowerTimeTillBottom > 0) timeTillBottom = lowerTimeTillBottom;

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
        laserMoveSound.Play();
        moveTween = GetComponent<Rigidbody2D>().DOMoveY(movementLimit, timeTillBottom)
            .SetEase(Ease.Linear)
            .OnComplete(() => DestroyLaser());
    }

    private void StopSounds()
    {
        laserMoveSound.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("boop");
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

    //What happens when the rocket is touching the laser gate
    private void OnRocketTouching(RocketControls controls)
    {
        if (!controls.CanControl || !ableToFreeze) return;

        controls.ShockDisplay.SetActive(true);
        controls.CanControl = false;
        ableToFreeze = false;

        controls.StartFreezeCountdown(freezeTime);
    }

    //Destroys the laser
    private void DestroyLaser()
    {
        GameManager.instance.Spawner.CurrentAmountOpponents--;
        if (moveTween != null && moveTween.target != null) moveTween.Kill();

        laserMoveSound.Stop();

        Destroy(gameObject);
    }
}
