using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class responsible for making the player able to collect fuel

public class Collect : MonoBehaviour
{
    //how much fuel is collected when this object is collected
    private static float collectedFuel = 2.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObj = collision.gameObject;

        //if the rocket collides with this object
        if (collisionObj.CompareTag("rocket"))
        {
            GameObject parent = gameObject.transform.parent.gameObject;                      

            //if the difference between the full fuel tank and the current fuel is
            //smaller than the fuel collected than set current fuel to full
            if(GameManager.StartFuel - GameManager.instance.CurrentFuel < collectedFuel)
            {
                GameManager.instance.CurrentFuel = GameManager.StartFuel;
            }
            //otherwise add fuel this object gives
            else
            {
                GameManager.instance.AddFuel(collectedFuel);
            }
            
            //if the parent only has one child left then destroy parent
            if(parent.transform.childCount == 1)
            {
                Destroy(parent);
            }

            Destroy(gameObject);
        }
    }

}
