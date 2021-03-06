using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Class that let's the player control the rocket

public class RocketControls : MonoBehaviour, IDragHandler
{
    [SerializeField]
    [Tooltip("x value of the screen boundary to the right")]
    private float xRightBound;

    [SerializeField]
    [Tooltip("x value of the screen boundary to the left")]
    private float xLeftBound;

    [SerializeField]
    [Tooltip("y value of the screen boundary on the top")]
    private float yUpBound;

    [SerializeField]
    [Tooltip("y value of the screen boundary on the bottom")]
    private float yDownBound;

    [SerializeField]
    [Tooltip("Object for displaying laser shock")]
    private GameObject shockDisplay;
    public GameObject ShockDisplay
    {
        get { return shockDisplay; }
    }

    //Whether the player can control the rocket or not
    private bool canControl = true;
    public bool CanControl
    {
        get { return canControl; }
        set{ canControl = value; }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //if no fuel is consumed a.k.a. the game is paused or over
        //then don't move on drag
        if (!GameManager.instance.ConsumeFuel || !canControl) return;
        
        //get the position of the finger
        Vector2 touchPos = Input.touchCount > 0 ? Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) : Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //if the player moves beyond the boundaries then don't set the position and return
        if (!(touchPos.x <= xRightBound && touchPos.x >= xLeftBound)) return;
        if (!(touchPos.y <= yUpBound && touchPos.y >= yDownBound)) return;

        //set position of rocket to finger position
        transform.position = touchPos;
    }

    //Start FreezeCounterCoroutine
    public void StartFreezeCountdown(float freezeTime)
    {
        StartCoroutine(FreezeCountdown(freezeTime, GetComponent<RocketBehaviour>().LaserHitSound));
    }

    //Starts the countdown for how long the rocket is frozen in place
    public IEnumerator FreezeCountdown(float freezeTime, AudioSource laserSound)
    {
        yield return new WaitForSecondsRealtime(freezeTime);

        if (laserSound != null) laserSound.Stop();

        shockDisplay.SetActive(false);
        canControl = true;
    }
}
