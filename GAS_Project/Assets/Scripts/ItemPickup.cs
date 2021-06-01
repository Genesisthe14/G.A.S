using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour

{
    public GameObject[] possibleBuffs;
    public int chosenBuff;
    private bool allBuffsDeactivated = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("rocket"))
        {

        
        foreach (var item in possibleBuffs)
        {
            if (item.activeSelf)
            {
                allBuffsDeactivated = false;
            }
        }
        if (allBuffsDeactivated) {
            possibleBuffs[chosenBuff].SetActive(true);
        }
        Destroy(this.gameObject);

        }
    }
}
