using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RocketControls : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private float xRightBound;

    [SerializeField]
    private float xLeftBound;

    [SerializeField]
    private float yUpBound;

    [SerializeField]
    private float yDownBound;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//Input.GetTouch(0).position

        if (!(touchPos.x <= xRightBound && touchPos.x >= xLeftBound)) return;
        if (!(touchPos.y <= yUpBound && touchPos.y >= yDownBound)) return;

        transform.position = touchPos;
    }
}
