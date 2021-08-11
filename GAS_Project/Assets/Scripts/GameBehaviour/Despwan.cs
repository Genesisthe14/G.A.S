using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that is responsible for despawning meteors if they are off screen
//or not supposed to exist anymore

public class Despwan : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Whether this object should substract from the total number of objects")]
    private bool substract = true;
    
    //x boundaries that mark how wide the screen is
    private static float[] xLimits = { -3.4f, 3.4f};

    //y boundaries that mark how high the screen is
    private static float yLimit = -6.0f;
    public static float YLimit
    {
        get { return yLimit; }
    }

    private void Update()
    {
        //if this object is a functional shatterParent used to hold together the shatter variant prefab
        //and it has no children anymore then destroy it
        if (gameObject.CompareTag("shatterParent") && transform.childCount == 0)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 currentPos = transform.position;

        //if this object goes beyond the screen boundaries then destory it
        if(currentPos.x < xLimits[0] || currentPos.x > xLimits[1] || currentPos.y < yLimit)
        {
            if(substract) GameManager.instance.Spawner.CurrentAmountNormalObjects--;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (gameObject.CompareTag("shatterParent")) GameManager.instance.Spawner.CurrentAmountNormalObjects--;
    }
}
