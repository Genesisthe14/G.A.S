using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private SerializableDictionary<string, GameObject> spawnObjects;

    [SerializeField]
    private float[] invokeTimeRange = new float[2];
    public float[] InvokeTimeRange
    {
        get { return invokeTimeRange; }
    }

    [SerializeField]
    private float[] ySpawn = new float[2];

    [SerializeField]
    private float[] xSpawn = new float[2];

    [SerializeField]
    private float[] velocityRange = new float[2];
    public float[] VelocityRange
    {
        get { return velocityRange; }
    }

    [SerializeField]
    private bool spawn = true;
    public bool Spawn
    {
        get { return spawn; }
        set { spawn = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InstantiateObjects());
    }

    private IEnumerator InstantiateObjects()
    {
        while (true)
        {
            if (!spawn)
            {
                yield return null;
                continue;
            }
            
            /*
             * item meteor = 10%
             * normal meteor = 55%
             * satellite = 35%
             */
            
            GameObject spawnObject = null;

            int randomNum = Random.Range(0, 100);

            if (randomNum < 10)
            {
                spawnObject = spawnObjects["item_meteor"];
            }

            else if (randomNum >= 5 && randomNum < 45) spawnObject = spawnObjects["satellite"];
            else if (randomNum >= 45 && randomNum < 100) spawnObject = spawnObjects["normal_meteor"];

            Vector2 spawnPos = new Vector2(Random.Range(xSpawn[0], xSpawn[1]), Random.Range(ySpawn[0], ySpawn[1]));
            GameObject temp = Instantiate(spawnObject, spawnPos, spawnObject.transform.rotation);

            float velocityMultiplier = Random.Range(velocityRange[0], velocityRange[1]);

            velocityMultiplier += RocketBehaviour.IsHeadstartActive ? RocketBehaviour.HeadstartSpeedFactor : 0.0f;

            temp.GetComponent<Rigidbody2D>().velocity = Vector2.down * velocityMultiplier;


            float invokeTime = Random.Range(invokeTimeRange[0], invokeTimeRange[1]);

            invokeTime -= RocketBehaviour.IsHeadstartActive ? 1.0f : 0.0f;

            yield return new WaitForSecondsRealtime(invokeTime < 0.0f ? 0.0f : invokeTime);
        }
    }
}
