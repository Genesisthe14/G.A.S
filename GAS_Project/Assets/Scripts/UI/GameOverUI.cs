using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        GameManager.InRun = false;

        gameObject.SetActive(true);
        pause.SetActive(false);

        source.Play();
        Time.timeScale = 0;

        int distance = (int)GameManager.instance.Distance;
        pointsText.text = distance + " KM";
    }

    public void Restart() 
    {
        ResetValues();
        SceneManager.LoadScene("BoosterSelection");
    }

    public void MainMenu()
    {
        ResetValues();
        SceneManager.LoadScene("Startscreen");
    }

    public void ReturnToWorkshop()
    {
        ResetValues();
        Homescreen.OpenWorkshop = true;
        SceneManager.LoadScene("Startscreen");
    }

    private void ResetValues()
    {
        Time.timeScale = 1;
        SelectionScreen.instance.EmptyBoosters();
        SelectionScreen.instance.ResetBoostersTaken(true);
        GameManager.InRun = false;
    }
}
