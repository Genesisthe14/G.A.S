using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCountdown : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Text that displays the countdown")]
    private Text countdownText;

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

        countdownText.text = "" + countdown;

        while(countdown > 0)
        {
            yield return new WaitForSecondsRealtime(1.0f);
            countdownText.text = "" + countdown--;
        }

        countdownText.enabled = false;
        GameManager.instance.Spawner.Spawn = true;
        GameManager.instance.ConsumeFuel = true;
    }
}
