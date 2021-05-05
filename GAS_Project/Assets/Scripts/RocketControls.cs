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
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);//Input.mousePosition

        if (!(touchPos.x <= xRightBound && touchPos.x >= xLeftBound)) return;
        if (!(touchPos.y <= yUpBound && touchPos.y >= yDownBound)) return;
        transform.position = touchPos;
    }
}
