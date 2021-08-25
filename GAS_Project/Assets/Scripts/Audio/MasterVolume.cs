using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterVolume : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the AudioMixer")]
    public AudioMixer mixer;

    //Set the master volume to the given value
    public void SetVolumeMaster (float sliderValue)
    {
        mixer.SetFloat ("Master", Mathf.Log10 (sliderValue) * 20);
    }

    //Set the volume of the music to the given value
    public void SetVolumeMusic (float sliderValue)
    {
        mixer.SetFloat ("Music", Mathf.Log10 (sliderValue) * 20);
    }

    //Set the volume of effects to the given value
    public void SetVolumeEffects (float sliderValue)
    {
        mixer.SetFloat ("Effects", Mathf.Log10 (sliderValue) * 20);
    }

}
