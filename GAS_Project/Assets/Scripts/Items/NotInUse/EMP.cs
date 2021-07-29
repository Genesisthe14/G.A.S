using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Vergr��ert EMP Feld bis der Destruction Timer abla�ft, alle ins Feld fliegenden Satelliten und Meteoriten werden zerst�rt
 * Meteor Behaviour StartParticle wurde zu public Methode ge�ndert
 * TODO: ben�tige andere public Methode die gleiche Zerst�rung wie weight hervorruft.
 */
public class EMP : MonoBehaviour
{   
    float maxScale = 20.0f;
    
    Vector3 size;
    
    public float destructionTimer;

    void FixedUpdate()
    {
        StartCoroutine(BiggerEMP());
        
            Destroy(this.gameObject,destructionTimer); 
        
    }

    IEnumerator BiggerEMP()
    {
        this.gameObject.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
        size = this.gameObject.transform.localScale;
        yield return new WaitForSeconds(1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("meteor") || collision.gameObject.CompareTag("satellite"))
        {
            collision.gameObject.GetComponent<MeteorBehaviour>().StartParticle();
        }
    }


}
