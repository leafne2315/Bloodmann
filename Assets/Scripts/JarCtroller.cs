using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JarCtroller : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isClimbing;
    public float speed;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }   

    // Update is called once per frame
    void Update()
    {
        isClimbing = GetComponent<OnAttached>().isAttached;

        if(isClimbing)
        {
            rb.velocity = Vector3.down*speed;
        }
        else
        {
            rb.velocity = Vector3.up*speed;
        }
    }
}
