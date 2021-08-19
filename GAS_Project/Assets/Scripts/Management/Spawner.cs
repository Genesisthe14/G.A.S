using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that is responsible for spawning the obstacles
//in random time intervals

public class Spawner : MonoBehaviour
{
    [Header("Object pool")]
    [SerializeField]
    [Tooltip("Parent to put the objects under")]
    private GameObject poolParent;

    [SerializeField]
    [Tooltip("Amount each object type shouls spawn")]
    private int amountEachType = 50;

    private Queue<GameObject> meteorQueue = new Queue<GameObject>();
    private Queue<GameObject> specialMeteorQueue = new Queue<GameObject>();
    private Queue<GameObject> satelliteQueue = new Queue<GameObject>();
    private Queue<GameObject> ufoQueue = new Queue<GameObject>();
    private Queue<GameObject> jellyQueue = new Queue<GameObject>();
    private Queue<GameObject> laserQueue = new Queue<GameObject>();


    [Space]
    [Header("Spawn Parameters")]

    [SerializeField]
    [Tooltip("List of absolute propabilities of each spawn category spawning")]
    private SerializableDictionary<float, SpawnCategories> categoryPropabilities;
    
    [SerializeField]
    [Tooltip("Timerange for the intervals in which the spawn objects can be spawned")]
    private float[] invokeTimeRange = new float[2];
    public float[] InvokeTimeRange
    {
        get { return invokeTimeRange; }
    }

    [SerializeField]
    [Tooltip("y boundaries in between which the spawn objects can spawn")]
    private float[] ySpawn = new float[2];
    public float[] YSpawn
    {
        get { return ySpawn; }
    }

    [SerializeField]
    [Tooltip("x boundaries in between which the spawn objects can spawn")]
    private float[] xSpawn = new float[2];
    public float[] XSpawn
    {
        get { return xSpawn; }
    }

    [SerializeField]
    [Tooltip("Range of the possible velocity of the spawn objects")]
    private float[] velocityRange = new float[2];
    public float[] VelocityRange
    {
        get { return velocityRange; }
    }

    //Sum of all category propabilities
    private float sumCatPropabilities = 0.0f;

    private enum SpawnCategories { NORMAL, OPPONENT, NONE};


    [Header("Normal objects")]

    [SerializeField]
    [Tooltip("List of all normal objects that can be spawned")]
    private List<SpawnObject> normObjects;

    [SerializeField]
    [Tooltip("Total amount of spawn objects allowed in the scene")]
    private int totalAmountNormalObjects = 5;
    
    //Sum of all normal propabilities
    private float sumNormalPropabilities = 0.0f;



    [Header("Opponents")]

    [SerializeField]
    [Tooltip("Total amount of spawn objects allowed in the scene")]
    private int totalAmountOpponents = 1;

    [SerializeField]
    [Tooltip("List of all opponents that can be spawned")]
    private List<SpawnObject> opponents;

    [SerializeField]
    [Tooltip("Points that are used for random movement of the UFOs")]
    private List<Transform> wayPointsUFO = null;

    [SerializeField]
    [Tooltip("Points where the ufos can end at off screen")]
    private List<Transform> endPointsUFO = null;

    //Sum of all category propabilities
    private float sumOpponentPropabilities = 0.0f;


    [SerializeField]
    //Current amount of spawn objects in the scene
    private int currentAmountNormalObjects = 0;
    public int CurrentAmountNormalObjects
    {
        get { return currentAmountNormalObjects; }
        set 
        { 
            currentAmountNormalObjects = value;
            if (currentAmountNormalObjects < 0) currentAmountNormalObjects = 0;
            if (currentAmountNormalObjects > totalAmountNormalObjects) currentAmountNormalObjects = totalAmountNormalObjects;
        }
    }

    [SerializeField]
    //Current amount of spawn objects in the scene
    private int currentAmountOpponents = 0;
    public int CurrentAmountOpponents
    {
        get { return currentAmountOpponents; }
        set
        {
            currentAmountOpponents = value;
            if (currentAmountOpponents < 0) currentAmountOpponents = 0;
            if (currentAmountOpponents > totalAmountOpponents) currentAmountOpponents = totalAmountOpponents;
        }
    }

    //Whether objects should spawn or not
    private bool spawn = true;
    public bool Spawn
    {
        get { return spawn; }
        set { spawn = value; }
    }

    private void Awake()
    {
        foreach(float prop in categoryPropabilities.Keys)
        {
            sumCatPropabilities += prop;
        }

        foreach(SpawnObject sp in normObjects)
        {
            sumNormalPropabilities += sp.SpawnWeight;
        }

        foreach (SpawnObject sp in opponents)
        {
            sumOpponentPropabilities += sp.SpawnWeight;
        }

        GameObject meteorOb = GetObjectWithSpawnTag("meteor", normObjects).SpawnOb;
        GameObject specialMeteorOb = GetObjectWithSpawnTag("specialMeteor", normObjects).SpawnOb;
        GameObject satelliteOb = GetObjectWithSpawnTag("satellite", normObjects).SpawnOb;
        GameObject ufoOb = GetObjectWithSpawnTag("ufo", opponents).SpawnOb;
        GameObject jellyOb = GetObjectWithSpawnTag("jelly", opponents).SpawnOb;
        GameObject laserOb = GetObjectWithSpawnTag("laser", opponents).SpawnOb;

        for (int i = 0; i < amountEachType; i++)
        {
            MakePoolObject(meteorOb, meteorQueue);
            MakePoolObject(specialMeteorOb, specialMeteorQueue);
            MakePoolObject(satelliteOb, satelliteQueue);
            MakePoolObject(ufoOb, ufoQueue);
            MakePoolObject(jellyOb, jellyQueue);
            MakePoolObject(laserOb, laserQueue);
        }
    }

    #region Object pooling
    private SpawnObject GetObjectWithSpawnTag(string tag, List<SpawnObject> list)
    {
        foreach(SpawnObject ob in list)
        {
            if (ob.SpawnTag == tag) return ob;
        }

        return new SpawnObject();
    }

    private void MakePoolObject(GameObject prefab, Queue<GameObject> queue)
    {
        GameObject ob = Instantiate(prefab);
        ob.SetActive(false);
        ob.transform.parent = poolParent.transform;
        queue.Enqueue(ob);
    }
    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(InstantiateObjects());
    }

    private IEnumerator InstantiateObjects()
    {
        bool value = sumCatPropabilities > 0.0f;
        while (value)
        {
            //if spawn is false then don't spawn anything and continue to the next iteration
            if (!spawn || currentAmountNormalObjects >= totalAmountNormalObjects)
            {
                yield return null;
                continue;
            }

            float randomCat = Random.Range(0.0f, sumCatPropabilities - 1.0f);
            SpawnCategories cat = SpawnCategories.NONE;

            foreach(float catKey in categoryPropabilities.Keys)
            {
                if(randomCat < catKey)
                {
                    cat = categoryPropabilities[catKey];
                    break;
                }

                randomCat -= catKey;
            }

            switch (cat)
            {
                case SpawnCategories.NORMAL:
                    if(normObjects != null && normObjects.Count > 0)
                        yield return SpawnNormalObject();
                    break;

                case SpawnCategories.OPPONENT:
                    if (opponents == null && opponents.Count <= 0) break;
                    
                    if (currentAmountOpponents < totalAmountOpponents)
                    {
                        yield return SpawnOpponent();
                    }
                    else yield return null;
                    break;

                case SpawnCategories.NONE:
                    Debug.LogError("Spawner: No spawn category was chosen.");
                    yield return null;
                    break;
            }
        }
    }

    private IEnumerator SpawnNormalObject()
    {
        GameObject spawnObject = null;
        string spawnTag = "";
        currentAmountNormalObjects++;

        float randomNorm = Random.Range(0, sumNormalPropabilities - 1);
        foreach (SpawnObject norm in normObjects)
        {
            if (randomNorm < norm.SpawnWeight)
            {
                spawnObject = norm.SpawnOb;
                spawnTag = norm.SpawnTag;
                break;
            }

            randomNorm -= norm.SpawnWeight;
        }

        //Generate the random spawn position with the boundaries for x and y position
        Vector2 spawnPos = new Vector2(Random.Range(xSpawn[0], xSpawn[1]), Random.Range(ySpawn[0], ySpawn[1]));

        GameObject temp = Instantiate(spawnObject, spawnPos, spawnObject.transform.rotation);

        //generate the velocity of the spawn object based on the velocity range
        //and up the velocity if the warp is active
        float velocityMultiplier = Random.Range(velocityRange[0], velocityRange[1]);
        velocityMultiplier += RocketBehaviour.IsWarpActive ? RocketBehaviour.CurrentWarpSpeedFactor : 0.0f;

        temp.GetComponent<Rigidbody2D>().velocity = Vector2.down * velocityMultiplier;

        //wait certain amount of time generated by the invoke time range to spawn next spawn object
        float invokeTime = Random.Range(invokeTimeRange[0], invokeTimeRange[1]);

        //If warp is active then wait shorter amount of time
        invokeTime -= RocketBehaviour.IsWarpActive ? 1.0f : 0.0f;

        yield return new WaitForSecondsRealtime(invokeTime < 0.0f ? 0.0f : invokeTime);
    }

    private GameObject GetSpawnObject(string spawnTag)
    {
        switch (spawnTag)
        {
            case "meteor":
                return meteorQueue.Dequeue();

            case "specialMeteor":
                return specialMeteorQueue.Dequeue();

            case "satellite":
                return satelliteQueue.Dequeue();

            case "ufo":
                return ufoQueue.Dequeue();

            case "jelly":
                return jellyQueue.Dequeue();

            case "laser":
                return laserQueue.Dequeue();

            default:
                return null;
        }
    }

    private IEnumerator SpawnOpponent()
    {
        float randomOP = Random.Range(0.0f, sumOpponentPropabilities - 1.0f);
        GameObject opponent = null;

        foreach (SpawnObject sp in opponents)
        {
            if (randomOP < sp.SpawnWeight)
            {
                opponent = sp.SpawnOb;
                break;
            }

            randomOP -= sp.SpawnWeight;
        }

        currentAmountOpponents++;

        GameObject spawnedOb = Instantiate(opponent);

        if(spawnedOb.GetComponent<UFO>() != null)
        {
            UFO ufo = spawnedOb.GetComponent<UFO>();

            ufo.EndPoints = endPointsUFO;
            ufo.WayPoints = wayPointsUFO;
        }
        else if (spawnedOb.GetComponent<LaserGate>() != null)
        {
            spawnedOb.transform.position = new Vector2(Random.Range(xSpawn[0], xSpawn[1]), Random.Range(ySpawn[0], ySpawn[1]));
            
            float angle = Random.Range(-135.0f, 135.0f);
            spawnedOb.transform.Rotate(Vector3.forward, angle);
        }

        //wait certain amount of time generated by the invoke time range to spawn next spawn object
        float invokeTime = Random.Range(invokeTimeRange[0], invokeTimeRange[1]);

        //If warp is active then wait shorter amount of time
        invokeTime -= RocketBehaviour.IsWarpActive ? 1.0f : 0.0f;

        yield return new WaitForSecondsRealtime(invokeTime < 0.0f ? 0.0f : invokeTime);
    }

    [System.Serializable]
    public struct FloatList
    {
        public List<float> floatList;
    }

    [System.Serializable]
    private struct SpawnObject
    {
        [SerializeField]
        [Tooltip("Relative propability this spawnObject should spawn with")]
        private float spawnWeight;
        public float SpawnWeight
        {
            get { return spawnWeight; }
        }

        [SerializeField]
        [Tooltip("Gameobject representing this spawnObject")]
        private GameObject spawnOb;
        public GameObject SpawnOb
        {
            get { return spawnOb; }
        }

        [SerializeField]
        [Tooltip("Tag of the spawnObject")]
        private string spawnTag;
        public string SpawnTag
        {
            get { return spawnTag; }
        }
    }
}
