using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despwan : MonoBehaviour
{
    private static float[] xLimits = { -3.4f, 3.4f};
    private static float yLimit = -6.0f;

    private void Update()
    {
        if (gameObject.CompareTag("shatterParent") && transform.childCount == 0)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 currentPos = transform.position;

        if(currentPos.x < xLimits[0] || currentPos.x > xLimits[1] || currentPos.y < yLimit)
        {
            Destroy(gameObject);
        }
    }
}
