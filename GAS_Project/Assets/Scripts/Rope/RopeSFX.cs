using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RopeSFX : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Sound effect for swinging the detructor")]
    private AudioSource soundeffect;

    [SerializeField]
    [Tooltip("Minimum velocity to play the sound at")]
    private float minVelocity = 0.7f;

    private void Awake()
    {
        GameManager.instance.GameOverEvent += StopSounds;
    }

    private void OnDestroy()
    {
        GameManager.instance.GameOverEvent -= StopSounds;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity.magnitude >= minVelocity && !soundeffect.isPlaying)
        {
            soundeffect.Play();            
        }
        else if((GetComponent<Rigidbody2D>().velocity.magnitude < minVelocity && soundeffect.isPlaying) || GameManager.instance.IsGameOver)
        {
            soundeffect.Stop();
        }
    }

    private void StopSounds()
    {
        soundeffect.Stop();
    }
}
