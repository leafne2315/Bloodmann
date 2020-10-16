using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempGetHit : MonoBehaviour
{
    // Start is called before the first frame update
    public float KnockTimer;
    public bool isHit;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isHit)
        {
            KnockBack(5.0f,new Vector2(Mathf.Cos(30*Mathf.Deg2Rad),Mathf.Sin(30*Mathf.Deg2Rad)));
        }    
    }
    void Update()
    {  
        
        
    }
    public void KnockBack(float KnockPwr,Vector2 KnockDir)
	{
        rb.AddForce(KnockDir*KnockPwr,ForceMode2D.Impulse);
		isHit = false;
	}
}
