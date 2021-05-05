using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleState : MonoBehaviour
{
    //if the particlesystem started playing
    private bool startedPlaying = false;

    //particlesystem that produces the effects
    private ParticleSystem particle;

    // Start is called before the first frame update
    void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        startedPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        //check if the particlesystem started playing and has finished this loop
        //if so destory this object
        if(particle.isStopped && startedPlaying)
        {
            startedPlaying = false;
            Destroy(gameObject);
        }
    }
}
