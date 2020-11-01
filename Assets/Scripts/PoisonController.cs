using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonController : MonoBehaviour
{
    public float stayTime = 2.0f;
    private float Timer = 0;
    public bool isfall;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
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
            rb.gravityScale = 1.0f;
        }
    }

}
