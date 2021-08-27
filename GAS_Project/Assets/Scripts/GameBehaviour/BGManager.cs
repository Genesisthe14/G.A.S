using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that manages the instantiation of bg sprites
//and moves them to create the illusion of upward movement for the rocket

public class BGManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Container object acting as parent to spawned bgObjects")]
    private Transform containerObject;
    
    [SerializeField]
    [Tooltip("Y offset where the bg sprites spawn")]
    private float ySpawnOffset = 13.9f;

    [SerializeField]
    [Tooltip("Prefab posing as frame for the bgs")]
    private GameObject bgObject;

    [SerializeField]
    [Tooltip("All bg sprites that should be iterated through")]
    private List<Sprite> bgSprites;

    [SerializeField]
    [Tooltip("Speed at which the bgObjects should move")]
    private float speed = 0.1f;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField]
    [Tooltip("The number of the order the bgs of this manager should take")]
    private int orderInLayer = 0;

    //Index of the next sprite to use
    private int bgSpriteIndex = 0;

    //List of all instantiated bgObjects
    private GameObject[] bgObjects = new GameObject[2];

    private void Awake()
    {
        //initialize starting bg
        GameObject firstBG = Instantiate(bgObject);

        //parent to container object
        firstBG.transform.parent = containerObject;

        //set sprite
        firstBG.GetComponent<SpriteRenderer>().sprite = bgSprites[bgSpriteIndex++];
        firstBG.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;
        bgObjects[0] = firstBG;



        //initialize second bg object to fade to
        GameObject secondBG = Instantiate(bgObject);

        //parent to container object
        secondBG.transform.parent = containerObject;

        //set sprite
        secondBG.GetComponent<SpriteRenderer>().sprite = bgSprites[bgSpriteIndex++];
        secondBG.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;

        //Set the second Sprite above the first
        Vector3 secPos = secondBG.transform.position;
        secPos.y = ySpawnOffset;
        secondBG.transform.position = secPos;

        bgObjects[1] = secondBG;
    }

    private void Update()
    {
        foreach(GameObject bgObj in bgObjects)
        {
            bgObj.transform.position += Vector3.down * Time.deltaTime * (RocketBehaviour.IsWarpActive ? (speed + RocketBehaviour.CurrentWarpSpeedFactor) : speed);
        }

        CheckDespawn();
    }

    //swapp position when a bg is out of frame and destroyed so that the bg now closest to despawning
    //is at the 0 position in the list of bgObjects
    private void CheckDespawn()
    {
        if(bgObjects[0].transform.position.y < -ySpawnOffset)
        {
            Destroy(bgObjects[0]);
            bgObjects[0] = bgObjects[1];

            if (bgSpriteIndex >= bgSprites.Count) bgSpriteIndex = 0;

            GameObject nextBG = Instantiate(bgObject);
            nextBG.transform.parent = containerObject;
            nextBG.GetComponent<SpriteRenderer>().sprite = bgSprites[bgSpriteIndex++];
            nextBG.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;

            Vector3 nextBGPos = nextBG.transform.position;
            nextBGPos.y = bgObjects[0].transform.position.y + ySpawnOffset;
            nextBG.transform.position = nextBGPos;

            bgObjects[1] = nextBG;
        }
    }
}
