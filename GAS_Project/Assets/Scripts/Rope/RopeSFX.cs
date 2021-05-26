using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class RopeSFX : MonoBehaviour
{
    public PlayableDirector soundeffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<Rigidbody2D>().velocity.magnitude > 0.7)
        {
            soundeffect.Play();
            
        }
        else
        {
            soundeffect.Stop();
        }
    }
}
