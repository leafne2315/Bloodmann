using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushingBug : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    public GameObject Player;
    private Vector3 MovingDir;
    private bool isFacingRight = true;
    private float Timer;

    [Header("Basic Element")]
    public float health;
    public float movingSpeed;
    public float gravityScale;

    [Header("Detect Settings")]
    public float DetectPlayerRadius;
    public LayerMask WhatIsPlayer;
    public bool PlayerDetect;
    public Transform GroundCheck;
    public float GroundCheckRadius = 0.1f;
    public LayerMask WhatIsGround;

    [Header("Attack Settings")]
    public bool PlayerInRange;
    public bool canAttack;
    public float AttackLength;
    public float AttForce;
    public float AttackCD;
    public float damage;
    public float AttackAngle;
    private bool AttackOver;
    public float PreAttackTime;
    public float AfterAttackWaitingTime;

    [Header("Statement")]
    public EnemyState currentState;
    public enum EnemyState{Idle,Patrol,Combat,Attack,Die,WaitForTrasfer}

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
            case EnemyState.WaitForTrasfer:
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
                    StartCoroutine(StartAttacking());
                    currentState = EnemyState.WaitForTrasfer;
                }

                if(!PlayerDetect)
                {
                    currentState = EnemyState.Idle;
                    rb.velocity = Vector3.zero;
                }

            break;
            case EnemyState.Attack:

                
                if(!AttackOver)
                {
                    bool isGrounded = Physics.CheckSphere(GroundCheck.position,GroundCheckRadius,WhatIsGround);
                    if(isGrounded)
                    {
                        rb.velocity = Vector3.zero;
                        AttackOver = true;
                    }
                }
                else
                {
                    if(Timer<AfterAttackWaitingTime)
                    {
                        Timer+=Time.deltaTime;
                    }
                    else
                    {
                        Timer = 0.0f;

                        currentState = EnemyState.Idle;
                        AttackOver = false;
                    }
                }

            break;
            case EnemyState.Die:

            break;
            case EnemyState.Idle:


                if(PlayerDetect)
                {
                    currentState = EnemyState.Combat;
                }

            break;

            case EnemyState.WaitForTrasfer:
                //do nothing 
            break; 
            
            default:
            break;
        }
    }
    IEnumerator AttackCD_Count()
	{
		for(float i =0 ; i<=AttackCD ; i+=Time.deltaTime)
		{
			yield return 0;
		}
		canAttack = true;
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
        RaycastHit hit;

        if(Physics.Raycast(transform.position,MovingDir,out hit,AttackLength,WhatIsPlayer))
        {
            PlayerInRange = true;
            Debug.DrawLine(transform.position,hit.point,Color.red);
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
    void GravityInput()
    {
        rb.AddForce(Physics.gravity*gravityScale,ForceMode.Acceleration);
    }
    void JumpAttack()
    {
        rb.velocity = AttackDir()*AttForce;
        canAttack = false;
        StartCoroutine(AttackCD_Count());
    }

    Vector3 AttackDir()
    {
        if(isFacingRight)
        {
            Vector3 dir = new Vector3(Mathf.Cos(AttackAngle*Mathf.Deg2Rad),Mathf.Sin(AttackAngle*Mathf.Deg2Rad),0.0f);
            return dir;
        }
        else
        {
            Vector3 dir = new Vector3(-Mathf.Cos(AttackAngle*Mathf.Deg2Rad),Mathf.Sin(AttackAngle*Mathf.Deg2Rad),0.0f);
            return dir;
        }    
    }
    
    IEnumerator StartAttacking()
    {
        rb.velocity = Vector3.zero;
        for(float i =0 ; i<=PreAttackTime ; i+=Time.deltaTime)
		{
			yield return 0;
		}
		JumpAttack();
        currentState = EnemyState.Attack;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,DetectPlayerRadius);
    }
    
}
