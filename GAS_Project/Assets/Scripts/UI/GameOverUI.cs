using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;
    
    [SerializeField]
    private GameObject pause;
    
    public Text pointsText;
    
    public void GameOver() 
    {
        GameManager.instance.ConsumeFuel = false;
        GameManager.instance.Spawner.Spawn = false;


        gameObject.SetActive(true);
        pause.SetActive(false);

        source.Play();
        Time.timeScale = 0;

        int distance = (int)GameManager.instance.Distance;
        pointsText.text = distance + " KM";
    }

    public void Restart() 
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("UIGaS");
    }
}
