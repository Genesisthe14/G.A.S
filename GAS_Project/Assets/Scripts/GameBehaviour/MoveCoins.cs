using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private IEnumerator MoveCoin()
    {
        yield return new WaitForSecondsRealtime(startWaitTime);

        while(transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 1.0f);
            yield return new WaitForSecondsRealtime(0.1f);
        }

        PlayerData.instance.CurrentMoney += moneyToAdd;
        GameManager.instance.MoneyText.text = PlayerData.instance.CurrentMoney + "";

        Destroy(gameObject);
    }
}
