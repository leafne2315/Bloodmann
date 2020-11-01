using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveForm : MonoBehaviour
{
    // Start is called before the first frame update
    float heightScale = 10.0f;
    float xScale = 1.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 pos = transform.position;
        pos.y = heightScale * Mathf.PerlinNoise(Time.time * xScale, 0.0f);
        transform.position = pos;
    }

}
