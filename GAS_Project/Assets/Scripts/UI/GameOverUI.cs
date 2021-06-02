using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{

    public Text pointsText;
    public void GameOver (float distance) 
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        pointsText.text = distance.ToString() + " KM";
    }

    public void Restart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("UIGaS");
    }
}
