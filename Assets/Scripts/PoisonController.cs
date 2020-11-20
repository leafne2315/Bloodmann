using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonController : MonoBehaviour
{
    public float stayTime = 2.0f;
    private float Timer = 0;
    public bool isfall;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }
    void Update()
    {
        if(Timer<stayTime)
        {
            Timer += Time.deltaTime;
        }
        else
        {
            isfall = true;
            rb.useGravity = true;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Water"))
        {
            Destroy(gameObject);
        }
    }
}