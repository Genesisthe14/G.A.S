using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnObject;

    [SerializeField]
    private float invokeTime = 0.5f;

    [SerializeField]
    private float[] invokeTimeRange = new float[2];

    [SerializeField]
    private float[] ySpawn = new float[2];

    [SerializeField]
    private float[] xSpawn = new float[2];

    [SerializeField]
    private float[] velocityRange = new float[2];

    // Start is called before the first frame update
    void Start()
    {
        //Invoke("InstantiateObjects", Random.Range(invokeTimeRange[0], invokeTimeRange[1]));
        StartCoroutine(InstantiateObjects());
    }

    private IEnumerator InstantiateObjects()
    {
        while (true)
        {
            Vector2 spawnPos = new Vector2(Random.Range(xSpawn[0], xSpawn[1]), Random.Range(ySpawn[0], ySpawn[1]));
            GameObject temp = Instantiate(spawnObject, spawnPos, spawnObject.transform.rotation);

            temp.GetComponent<Rigidbody2D>().velocity = Vector2.down * Random.Range(velocityRange[0], velocityRange[1]);

            yield return new WaitForSecondsRealtime(Random.Range(invokeTimeRange[0], invokeTimeRange[1]));
        }
    }
}
