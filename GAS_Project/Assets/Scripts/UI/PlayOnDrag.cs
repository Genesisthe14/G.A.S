using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayOnDrag : MonoBehaviour, IDragHandler
{
    [SerializeField]
    [Tooltip("Event to trigger on swipe")]
    private UnityEvent onSwipe;

    public void OnDrag(PointerEventData eventData)
    {
        onSwipe.Invoke();
    }
}
