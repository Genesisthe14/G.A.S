using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    [Tooltip("Prefab to represent the money")]
    private GameObject coinPrefab;

    [SerializeField]
    [Tooltip("Target the coins are moving to")]
    private RectTransform target;

    [SerializeField]
    [Tooltip("Coin collect sound prefab")]
    private GameObject collectSoundPrefab;

    [SerializeField]
    [Tooltip("Parent for the audio prefabs")]
    private GameObject audioParent;

    [Header("Object pooling")]
    [SerializeField]
    [Tooltip("Maximum amount of coins that can occur")]
    private int maxNumCoins;

    private Queue<GameObject> coinsQueue = new Queue<GameObject>();

    [Header("Animation settings")]
    [SerializeField]
    [Tooltip("Minimum amount of time a coin animation takes")]
    [Range(0.5f, 0.9f)]
    private float minAnimationDuration;

    [SerializeField]
    [Tooltip("Maximum amount of time a coin animation takes")]
    [Range(0.9f, 2.0f)]
    private float maxAnimationDuration;

    [SerializeField]
    [Tooltip("Minimum Curve y offset")]
    private float minYCurve;

    [SerializeField]
    [Tooltip("Maximum Curve y offset")]
    private float maxYCurve;

    [SerializeField]
    [Tooltip("Ease Setting for the coin movement")]
    private Ease easeType;

    private Vector3 targetPosition;

    private List<AudioSource> coinCollectSoundSources = new List<AudioSource>();

    private void Awake()
    {
        targetPosition = ReturnMiddleRect(target);

        PrepareCoins();
    }

    private Vector3 ReturnMiddleRect(RectTransform rect)
    {
        Vector3 temp = Vector3.zero;
        Vector3[] corners = new Vector3[4]; 
        rect.GetWorldCorners(corners);

        foreach (var item in corners)
        {
            temp += item;
        }

        temp.z = 0;

        return temp /= corners.Length;
    }

    private void PrepareCoins()
    {
        GameObject coin;
        for(int i = 0; i < maxNumCoins; i++)
        {
            coin = Instantiate(coinPrefab);
            coin.SetActive(false);
            coin.transform.parent = transform;
            coinsQueue.Enqueue(coin);

            GameObject temp = Instantiate(collectSoundPrefab);
            temp.transform.position = targetPosition;
            temp.transform.parent = audioParent.transform;
            coinCollectSoundSources.Add(temp.GetComponent<AudioSource>());
        }
    }

    private void AnimateCoins(Vector3 collectPos, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if(coinsQueue.Count == 0)
            {
                GameObject newCoin = Instantiate(coinPrefab);
                newCoin.SetActive(false);
                newCoin.transform.parent = transform;
                coinsQueue.Enqueue(newCoin);
            }
            
            GameObject coin = coinsQueue.Dequeue();

            coin.SetActive(true);
            coin.transform.position = collectPos;

            float duration = Random.Range(minAnimationDuration, maxAnimationDuration);
            Vector3 betweenPoint = (collectPos + targetPosition) / 2;
            betweenPoint.y += Random.Range(minYCurve, maxYCurve);

            coin.transform.DOPath(new Vector3[] { betweenPoint, targetPosition }, duration, PathType.CatmullRom)
                .SetEase(easeType)
                .OnComplete(() =>
                {
                    coin.SetActive(false);
                    coinsQueue.Enqueue(coin);

                    coinCollectSoundSources[i].Play();

                    PlayerData.instance.CurrentMoney++;
                    GameManager.instance.MoneyText.text = "" + PlayerData.instance.CurrentMoney;
                });
        }
    }

    public void AddCoins(Vector3 collectPos, int amount)
    {
        AnimateCoins(collectPos, amount);
    }
}
