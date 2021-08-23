using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("All Scenes this audio manager can exist in")]
    private List<string> scenesExistIn;

    private void Awake()
    {
        SceneManager.sceneLoaded += DoOnLoad;
    }

    private void DoOnLoad(Scene scene, LoadSceneMode mode)
    {
        if (!scenesExistIn.Contains(SceneManager.GetActiveScene().name)) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
