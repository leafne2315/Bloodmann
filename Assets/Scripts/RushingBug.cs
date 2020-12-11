using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushingBug : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    public GameObject Player;
    private Vector3 MovingDir;
    private bool FacingRight = true;

    [Header("Basic Element")]
    public float health;
    public float movingSpeed;
    public float gravityScale;

    [Header("Detect Settings")]
    public float DetectPlayerRadius;
    public LayerMask WhatIsPlayer;
    public bool PlayerDetect;

    [Header("Attack Settings")]
    public bool PlayerInRange;
    public bool canAttack;
    public float AttackLength;
    public float DashForce;
    public float AttackCD;
    public float damage;

    [Header("Statement")]
    public EnemyState currentState;
    public enum EnemyState{Idle,Patrol,Combat,Attack,Die}

    void Start()
    {
        MovingDir = transform.forward;
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        GravityInput();

        switch(currentState)
        {
            case EnemyState.Patrol:

            break;
            case EnemyState.Combat:
                
            break;
            case EnemyState.Attack:

            break;
            case EnemyState.Die:

            break;
            case EnemyState.Idle:
                
            break;
            default:
            break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        DetectingPlayer();
        FacingCheck();

        switch(currentState)
        {
            case EnemyState.Patrol:

            break;
            case EnemyState.Combat:
                
                FacingPlayer();
                MoveTowardPlayer();

                AttackRangeDetect();
                if(PlayerInRange&&canAttack)
                {
                    currentState = EnemyState.Attack;
                }

                if(!PlayerDetect)
                {
                    currentState = EnemyState.Idle;
                    rb.velocity = Vector3.zero;
                }

            break;
            case EnemyState.Attack:

            break;
            case EnemyState.Die:

            break;
            case EnemyState.Idle:


                if(PlayerDetect)
                {
                    currentState = EnemyState.Combat;
                }

            break;
            default:
            break;
        }
    }
    void DetectingPlayer()
    {
        if(Physics.CheckSphere(transform.position,DetectPlayerRadius,WhatIsPlayer))
        {
            PlayerDetect = true;
        }
        else
        {
            PlayerDetect = false;
        }
    }
    void AttackRangeDetect()
    {
        if(Physics.Raycast(transform.position,transform.right,AttackLength,WhatIsPlayer))
        {
            PlayerInRange = true;
            Debug.DrawLine(transform.position,transform.position + MovingDir*AttackLength,Color.red);
        }
        else
        {
            PlayerInRange = false;
            Debug.DrawLine(transform.position,transform.position + MovingDir*AttackLength,Color.green);
        }
    }
    void FacingPlayer()
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
    void MoveTowardPlayer()
    {
        rb.velocity = MovingDir*movingSpeed;
    }
    void FacingCheck()
    {
        if(FacingRight==false&&MovingDir.x>0)
        {
            Flip();
        }
        else if(FacingRight == true&&MovingDir.x<0)
        {
            Flip();
        }
    }
    void Flip()
    {
        FacingRight = !FacingRight;
        Vector3 Scaler = transform.localScale;
		Scaler.x*=-1;
		transform.localScale = Scaler;
    }
    void GravityInput()
    {
        rb.AddForce(Physics.gravity*gravityScale,ForceMode.Acceleration);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,DetectPlayerRadius);
    }
    
}
