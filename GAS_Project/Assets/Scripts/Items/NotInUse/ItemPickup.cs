using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private int chosenBuff;
    public int ChosenBuff
    {
        get { return chosenBuff; }
        set
        {
            chosenBuff = value;
            buffContainer.sprite = spritesForBuff[chosenBuff];
        }
    }

    [SerializeField]
    [Tooltip("Sprite for the accorcing buff")]
    private SerializableDictionary<int, Sprite> spritesForBuff;

    [SerializeField]
    [Tooltip("Buff Sprite container object")]
    private SpriteRenderer buffContainer;

    private void Awake()
    {
        buffContainer.sprite = spritesForBuff[chosenBuff];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("rocket"))
        {        
            if (GameManager.instance.PossibleBuffs[chosenBuff].gameObject.activeInHierarchy)
            {
                GameManager.instance.PossibleBuffs[chosenBuff].RestartItem();
            }
            else
                GameManager.instance.PossibleBuffs[chosenBuff].gameObject.SetActive(true);

            Destroy(gameObject);
        }
    }
}
