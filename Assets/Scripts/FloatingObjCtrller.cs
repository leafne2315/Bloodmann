using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObjCtrller : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    public float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(-speed,rb.velocity.y);
    }
    
    // void OnCollisionEnter2D(Collision2D other)
    // {
    //     if(other.collider.CompareTag("Ground"))
    //     {
    //         Destroy(gameObject,0.2f);
    //     }
    // }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BoxDestroy"))
        {
            Destroy(gameObject,0.2f);
        }
    }
}
