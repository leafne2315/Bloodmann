using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBlackObjCtrller : MonoBehaviour
{
    private float Duration = 5;
    public Animator animator;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            animator.SetTrigger("Fading");
        }

        Destroy(gameObject,Duration);
    }
}

