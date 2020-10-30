using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveForm : MonoBehaviour
{
    // Start is called before the first frame update
    float heightScale = 10.0f;
    float xScale = 20.0f;
    float originHeight;
    void Start()
    {
        originHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 pos = transform.position;
        pos.y = originHeight * Mathf.Sin(Time.time);
        transform.position = pos;
    }
}
