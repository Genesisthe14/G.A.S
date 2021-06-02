using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioSource audioSource;

    [SerializeField]
    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider.value = audioSource.volume;
    }

    public void SetVolume (float volume) 
    {
        audioSource.volume = volume;
        Debug.Log(volume);
    }

}
