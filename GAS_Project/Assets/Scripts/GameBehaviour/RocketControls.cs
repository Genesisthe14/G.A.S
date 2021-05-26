using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketControls : MonoBehaviour
{
    [SerializeField]
    private float xRightBound;

    [SerializeField]
    private float xLeftBound;

    [SerializeField]
    private float yUpBound;

    [SerializeField]
    private float yDownBound;


    private void OnMouseDrag()
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//Input.GetTouch(0).position

        if (!(touchPos.x <= xRightBound && touchPos.x >= xLeftBound)) return;
        if (!(touchPos.y <= yUpBound && touchPos.y >= yDownBound)) return;

        //lower the fuel by moveDiff * the lowerRate specified in GameManager
        /*float moveDiff = ((Vector2)transform.position - touchPos).magnitude;
        GameManager.instance.LowerFuel(moveDiff);*/

        transform.position = touchPos;
    }
}
