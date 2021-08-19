using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartCountdown : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Text that displays the countdown")]
    private Text countdownText;

    [SerializeField]
    [Tooltip("Raycast Blocker")]
    private GameObject rayBlocker;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.instance.Spawner.Spawn = false;
        GameManager.instance.ConsumeFuel = false;
        StartCoroutine(StartCountdownRoutine());
    }

    private IEnumerator StartCountdownRoutine()
    {
        int countdown = 3;

        while(countdown > 0)
        {
            if (PauseMenu.isPaused)
            {
                yield return null;
                continue;
            }


            countdownText.text = "" + countdown--;
            yield return new WaitForSecondsRealtime(1.0f);
        }

        countdownText.enabled = false;
        rayBlocker.SetActive(false);
        GameManager.instance.Spawner.Spawn = true;
        GameManager.instance.ConsumeFuel = true;
    }
}
