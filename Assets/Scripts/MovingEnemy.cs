using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    public bool getHit;
    private tempGetHit tempGetHit;
    private Vector3 MovingDir;
    private Rigidbody rb;
    public GameObject Player;
    public float movingSpeed;
    public bool isFacingRight;
    public enum EnemyState{Idle, Moving, Dead}
    public bool PlayerDetect;
    public LayerMask WhatIsPlayer;
    public Vector3 DetectPlayerlength;
    public EnemyState currentState;
    // Start is called before the first frame update
    void Start()
    {
        tempGetHit = GetComponent<tempGetHit>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        getHitCheck();
        switch(currentState)
        {
            case EnemyState.Idle:
            DetectingPlayer();
            if(PlayerDetect)
            {
                currentState = EnemyState.Moving;
            }
            break;
            case EnemyState.Moving:
            rb.velocity = MovingDir*movingSpeed;
            GetMoveDir();
            FacingCheck();
            DetectingPlayer();
            if(!PlayerDetect)
            {
                currentState = EnemyState.Idle;
                rb.velocity = Vector3.zero;
            }
            break;
            case EnemyState.Dead:
            break;
        }
    }

    void getHitCheck()
    {
        if(tempGetHit.isHit)
        {
            getHit = true;
        }
        else
        {
            getHit = false;
        }
    }
    void GetMoveDir()
    {
        if(transform.position.x>Player.transform.position.x)
        {
            MovingDir = -Vector3.right;
        }
        else
        {
            MovingDir = Vector3.right;
        }

    }

   
            
    void FacingCheck()
    {
        if(isFacingRight==false&&MovingDir.x>0)
        {
            Flip();
        }
        else if(isFacingRight == true&&MovingDir.x<0)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x*=-1;
        transform.localScale = Scaler;
    }
     void DetectingPlayer()
    {
        if(Physics.CheckBox(transform.position,DetectPlayerlength,Quaternion.identity,WhatIsPlayer))
        {
            PlayerDetect = true;
            //print("checked");
        }
        else
        {
            PlayerDetect = false;
            //print("NotFound");
        }
    }

     void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position,2*DetectPlayerlength);
    }
}
