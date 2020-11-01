using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonController : MonoBehaviour
{
     void Start()
     {
   
     }
     void Update()
     {
   
     }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
