﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Externalforce : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerCtroller PlayerCtroller;
    private Rigidbody2D playerRb;
    private Collider2D MovingObj;
    public LayerMask MovingGround;
    public Transform GroundCheck;
    public float checkRadius;

    public Vector2 OtherForce;

    
    void Awake()
    {
        PlayerCtroller = GetComponent<PlayerCtroller>();
        playerRb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovingObj = Physics2D.OverlapCircle(GroundCheck.position,checkRadius,MovingGround);

        if(!PlayerCtroller.isFlying&&MovingObj)
        { 
            OtherForce = new Vector2(MovingObj.GetComponent<Rigidbody2D>().velocity.x,0);
        }
        else
        {
            OtherForce = Vector2.zero;
        }
    }
}
