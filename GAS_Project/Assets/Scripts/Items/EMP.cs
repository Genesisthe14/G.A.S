using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : MonoBehaviour
{   float maxScale = 20;
    Vector3 size;
    // Start is called before the first frame update
    void Start()
    {
        size = this.gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {

        size.x += 0.1f;
        size.y += 0.1f;
        size.z += 0.1f;
        if (size.magnitude >= maxScale)
        {
            Destroy(this.gameObject);
        } 
    }

    
}
