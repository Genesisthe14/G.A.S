using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private GameObject pause;
    
    public Text pointsText;

    [SerializeField]
    [Tooltip("Button that should be disabled if something is loaded")]
    private Button[] buttonsToDisable;

    [SerializeField]
    [Tooltip("Hud elements that should be disabled when game over")]
    private GameObject hudElement;

    [SerializeField]
    [Tooltip("Loading screen")]
    private GameObject loadingScreen;

    [SerializeField]
    [Tooltip("Loading bar")]
    private Slider loadingBar;

    public void GameOver() 
    {
        GameManager.instance.ConsumeFuel = false;
        GameManager.instance.Spawner.Spawn = false;

        GameManager.InRun = false;

        gameObject.SetActive(true);
        pause.SetActive(false);

        hudElement.SetActive(false);

        Time.timeScale = 0;

        AudioManager.StaticMusicInstance.GetComponent<AudioSource>().Pause();

        int distance = (int)GameManager.instance.Distance;
        pointsText.text = distance + " KM";
        PlayerData.instance.TotalDistance += distance;

        if (distance > PlayerData.instance.HighestDistance) PlayerData.instance.HighestDistance = distance;

        PlayerData.instance.Deaths++;
    }

    public void Restart() 
    {
        ResetValues();
        loadingScreen.SetActive(true);
        DisableButtons();
        StartCoroutine(Homescreen.LoadScreenCoroutine("BoosterSelection", loadingBar));
    }

    public void MainMenu()
    {
        ResetValues();
        loadingScreen.SetActive(true);
        DisableButtons();
        StartCoroutine(Homescreen.LoadScreenCoroutine("Startscreen", loadingBar));
    }

    public void ReturnToWorkshop()
    {
        ResetValues();
        Homescreen.OpenWorkshop = true;

        loadingScreen.SetActive(true);
        DisableButtons();
        StartCoroutine(Homescreen.LoadScreenCoroutine("Startscreen", loadingBar));
    }

    private void ResetValues()
    {
        AudioManager.StaticMusicInstance.GetComponent<AudioSource>().Play();
        SelectionScreen.instance.EmptyBoosters();
        SelectionScreen.instance.ResetBoostersTaken(true);
        GameManager.InRun = false;
    }

    private void DisableButtons()
    {
        foreach (Button b in buttonsToDisable)
        {
            b.interactable = false;
        }
    }
}
