using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempGetHit : MonoBehaviour
{
    // Start is called before the first frame update
    public float KnockTimer;
    public bool isHit;
    Rigidbody rb;
    public Vector3 KnockDir;
    private Color OriginColor;
    void Start()
    {
        OriginColor = GetComponent<Renderer>().material.color;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if(isHit)
        // {
        //     KnockBack(20.0f,new Vector2(Mathf.Cos(30*Mathf.Deg2Rad),Mathf.Sin(30*Mathf.Deg2Rad)));
        // }
        // else
        // {

        // }
    }
    void Update()
    {  
        
    }
    public void KnockBack(float KnockPwr,Vector2 KnockDir)
	{
        
        rb.AddForce(KnockDir*KnockPwr,ForceMode.VelocityChange);
		isHit = false;
	}
    public IEnumerator HitTrigger(Vector3 HitDir)
    {
        isHit = true;
        KnockDir = HitDir;
        GetComponent<Renderer>().material.color = Color.red;
		yield return 0;
        isHit = false;
        GetComponent<Renderer>().material.color = OriginColor;
    }
    
}
