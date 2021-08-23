using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class RopeSFX : MonoBehaviour
{
    public PlayableDirector soundeffect;

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity.magnitude > 0.7)
        {
            soundeffect.Play();            
        }
        else
        {
            soundeffect.Stop();
        }
    }
}
