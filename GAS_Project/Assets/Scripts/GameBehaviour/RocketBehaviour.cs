using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketBehaviour : MonoBehaviour
{
    //amount of fuel that is leaked when the rocket hits a meteor
    private static float leakingFuel = 10.0f;
    public static float LeakingFuel
    {
        get { return leakingFuel; }
        set { leakingFuel = value; }
    }

    [SerializeField]
    [Tooltip("Reference to the shield buff")]
    private GameObject shield;

    [SerializeField]
    private GameObject damageScreen;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the rocket hits a meteor then substract fuel
        if (collision.gameObject.CompareTag("meteor"))
        {
            if (shield.activeInHierarchy) return;

            Debug.Log(LeakingFuel);

            GameManager.instance.LowerFuel(leakingFuel);
            Damage();
        }
    }

    private void Damage() 
    {
        var color = damageScreen.GetComponent<Image>().color;
        color.a = 0.8f;

        damageScreen.GetComponent<Image>().color = color;
    }

    private void Update()
    {
        if (damageScreen.GetComponent<Image>().color.a > 0)
        {
            var color = damageScreen.GetComponent<Image>().color;
            color.a -= 0.01f;

            damageScreen.GetComponent<Image>().color = color;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.CompareTag("meteor") || collision.CompareTag("satellite"))) return;

        collision.GetComponent<MeteorBehaviour>().OnMeteorCollision(collision);
    }
}
