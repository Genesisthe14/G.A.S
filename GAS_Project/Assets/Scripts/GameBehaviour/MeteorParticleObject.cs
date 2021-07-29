using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that is used for objects whose sole purpose is to play a particle effect once
//to then be destroyed
//Particle System plays on Awake or has to be started from outside when spawning
//a partcile object

public class MeteorParticleObject : MonoBehaviour
{
    private void Update()
    {
        if (!GetComponent<ParticleSystem>().IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
