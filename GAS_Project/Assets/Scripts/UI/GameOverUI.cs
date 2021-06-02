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

    public Text pointsText;
    public void GameOver(int distance) 
    {
        GameManager.instance.ConsumeFuel = false;
        GameManager.instance.Spawner.Spawn = false;

        gameObject.SetActive(true);

        source.Play();
        Time.timeScale = 0;
        pointsText.text = distance.ToString() + " KM";
    }

    public void Restart() 
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("UIGaS");
    }
}
