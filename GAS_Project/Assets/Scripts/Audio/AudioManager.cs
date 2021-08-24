using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Whether the music instance should be destroyed")]
    private bool destroyMusicInstance = false;

    [SerializeField]
    [Tooltip("Whether the music instance should be saved")]
    private bool saveMusicInstance = false;

    [SerializeField]
    [Tooltip("Music instance")]
    private GameObject musicInstance;

    private static GameObject staticMusicInstance;

    private void Awake()
    {
        DestroyBGMusic();
    }

    private void DestroyBGMusic()
    {
        bool destroyedMusicInstance = false;
        if (destroyMusicInstance && staticMusicInstance != null && musicInstance != null && staticMusicInstance != musicInstance)
        {
            destroyedMusicInstance = true;
            Destroy(musicInstance);
        }

        if(destroyMusicInstance && !saveMusicInstance) Destroy(staticMusicInstance);

        if (saveMusicInstance && !destroyedMusicInstance)
        {
            if(staticMusicInstance != null)
            {
                Destroy(staticMusicInstance);
            }

            musicInstance.transform.parent = null;
            staticMusicInstance = musicInstance;
            DontDestroyOnLoad(staticMusicInstance);
        }
    }
}
