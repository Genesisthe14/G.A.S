using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MasterVolume : MonoBehaviour
{

    public AudioMixer mixer;

    public void SetVolumeMaster (float sliderValue)
    {
        mixer.SetFloat ("Master", Mathf.Log10 (sliderValue) * 20);
    }

    public void SetVolumeMusic (float sliderValue)
    {
        mixer.SetFloat ("Music", Mathf.Log10 (sliderValue) * 20);
    }

    public void SetVolumeEffects (float sliderValue)
    {
        mixer.SetFloat ("Effects", Mathf.Log10 (sliderValue) * 20);
    }

}
