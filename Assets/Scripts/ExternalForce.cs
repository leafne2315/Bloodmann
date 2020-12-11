using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalForce : MonoBehaviour
{
    private PlayerCtroller PlayerCtroller;
    private Rigidbody playerRb;
    //private Collider2D MovingObj;
    public LayerMask MovingGround;
    public Transform GroundCheck;
    public float checkRadius;
    public bool onMovingObj;
    public Vector2 OtherForce;

    
    void Awake()
    {
        PlayerCtroller = GetComponent<PlayerCtroller>();
        playerRb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] MovingObj = Physics.OverlapSphere(GroundCheck.position,checkRadius,MovingGround);
        foreach(Collider obj in MovingObj)
        {
            onMovingObj = true;

            if(!PlayerCtroller.isFlying&&onMovingObj)
            { 
                OtherForce = new Vector3(obj.GetComponent<Rigidbody>().velocity.x,0,0) ;
            }
            else
            {
                onMovingObj = false;
                OtherForce = Vector2.zero;
            }
        }
              
        if(MovingObj.Length ==0)
        {
            onMovingObj = false;
            OtherForce = Vector2.zero;
        }

        
    }

    void OnDrawGizmos() 
    {    
        
    }
}
