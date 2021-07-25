using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that is responsible for moving the spawned coins to their target position

public class MoveCoins : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Position to move the coins to")]
    private Vector3 targetPos;

    //money to add
    private int moneyToAdd = 0;
    public int MoneyToAdd
    {
        set { moneyToAdd = value; }
    }
    
    //how long to wait before moving to target location
    private float startWaitTime = 0;
    public float StartWaitTime
    {
        set { startWaitTime = value; }
    }

    private void Start()
    {
        StartCoroutine(MoveCoin());
    }

    //Start moving this coin prefab to the target position after a certain
    //amount of time has passed
    private IEnumerator MoveCoin()
    {
        yield return new WaitForSecondsRealtime(startWaitTime);

        //while the coin is not yet at the target position
        while(transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 1.0f);
            yield return new WaitForSecondsRealtime(0.1f);
        }

        //if target position reached add money and change displaying text
        PlayerData.instance.CurrentMoney += moneyToAdd;
        GameManager.instance.MoneyText.text = PlayerData.instance.CurrentMoney + "";

        Destroy(gameObject);
    }
}
