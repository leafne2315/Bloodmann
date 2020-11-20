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
    public bool isNull;
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
            isNull = false;

            if(!PlayerCtroller.isFlying&&!isNull)
            { 
                print("a");
                OtherForce = new Vector2(obj.GetComponent<Rigidbody>().velocity.x,0);
            }
            else
            {
                OtherForce = Vector2.zero;
                isNull = true;
            }
        }

        
    }
}
