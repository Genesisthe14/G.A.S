using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UpdateVolumeSliders : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the AudioMixer")]
    public AudioMixer mixer;

    [SerializeField]
    [Tooltip("Master slider")]
    private Slider masterSlider;

    [SerializeField]
    [Tooltip("Effects slider")]
    private Slider effectsSlider;

    [SerializeField]
    [Tooltip("Music slider")]
    private Slider musicSlider;

    private void OnEnable()
    {
        if (masterSlider != null)
        {
            mixer.GetFloat("Master", out float masterFloat);
            masterSlider.value = Mathf.Pow(10, masterFloat / 20);
        }

        if (effectsSlider != null)
        {
            mixer.GetFloat("Effects", out float effectsFloat);
            effectsSlider.value = Mathf.Pow(10, effectsFloat / 20);
        }

        if (musicSlider != null)
        {
            mixer.GetFloat("Music", out float musicFloat);
            musicSlider.value = Mathf.Pow(10, musicFloat / 20);
        }
    }
}
