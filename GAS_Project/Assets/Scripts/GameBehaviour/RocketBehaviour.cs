using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    //amount of fuel that is leaked when the rocket hits a meteor
    private static float leakingFuel = 5.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the rocket hits a meteor then substract fuel
        if (collision.gameObject.CompareTag("meteor"))
        {
            GameManager.instance.LowerFuel(leakingFuel);
        }
    }
}
