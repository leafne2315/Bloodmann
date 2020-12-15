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
    public Vector3 DetectPlayerlength;
    public LayerMask WhatIsPlayer;
    public bool PlayerDetect;
    public Transform GroundCheck;
    public float GroundCheckRadius = 0.1f;
    public LayerMask WhatIsGround;

    [Header("Attack Settings")]
    public bool PlayerInRange;
    public bool canAttack;
    public bool isAttacking;
    public float AttackLength;
    public float AttForce;
    public float AttackCD;
    public float damage;
    public float AttackAngle;
    public float PreAttackTime;
    public float AfterAttackWaitingTime;
    [Header("Repel Setting")]
    public float RepelForce;
    public float RepelTime;
    public bool isRepeling;
    [Header("Statement")]
    public EnemyState currentState;
    public enum EnemyState{Idle,Patrol,Combat,Attack,Die,WaitForTransfer,Repel}
    [Header("IEnumerator Settings")]
    private IEnumerator StartAttackCoroutine;
    private IEnumerator AfterAttackCoroutine;
    private IEnumerator AttackCDCoroutine;
    void Awake()
    {
        
    }
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
            case EnemyState.WaitForTransfer:
            break;
            case EnemyState.Repel:
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
        getHitCheck();


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
                    StartAttackCoroutine = StartAttacking();
                    StartCoroutine(StartAttackCoroutine);
                    currentState = EnemyState.WaitForTransfer;
                }

                if(!PlayerDetect)
                {
                    currentState = EnemyState.Idle;
                    rb.velocity = Vector3.zero;
                }

            break;
            case EnemyState.Attack:

                
                if(isAttacking)
                {
                    bool isGrounded = Physics.CheckSphere(GroundCheck.position,GroundCheckRadius,WhatIsGround);
                    if(isGrounded)
                    {
                        rb.velocity = Vector3.zero;
                        isAttacking = false;
                    }
                }
                else
                {
                    AfterAttackCoroutine = AfterAttack();
                    StartCoroutine(AfterAttackCoroutine);
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

            case EnemyState.WaitForTransfer:
                //do nothing 
            break; 
            
            case EnemyState.Repel:
                
                print(Timer);
                if(Timer<RepelTime*0.3f)
                {
                    Timer += Time.deltaTime;

                    rb.velocity = RepelDir()*RepelForce;
                }
                else if(Timer<RepelTime)
                {
                    Timer += Time.deltaTime;
                    rb.velocity = Vector3.zero;
                }
                else
                {
                    Timer = 0;
                    currentState = EnemyState.Combat;
                }

            break;

            default:
            break;
        }
        print(currentState);
    }

    Vector3 RepelDir()
    {
        Vector3 dir = GetComponent<tempGetHit>().KnockDir;
        return dir;
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

        if(Physics.CheckBox(transform.position,DetectPlayerlength,Quaternion.identity,WhatIsPlayer))
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
        if(Vector3.Distance(transform.position,Player.transform.position)<0.5f)
        {
            rb.velocity = new Vector3(0,rb.velocity.y,0);
        }
        else
        {
            rb.velocity = MovingDir*movingSpeed;
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
    void GravityInput()
    {
        rb.AddForce(Physics.gravity*gravityScale,ForceMode.Acceleration);
    }
    void JumpAttack()
    {
        rb.velocity = AttackDir()*AttForce;
        canAttack = false;
        AttackCDCoroutine = AttackCD_Count();
        StartCoroutine(AttackCDCoroutine);
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
        isAttacking = true;
        for(float i =0 ; i<=PreAttackTime ; i+=Time.deltaTime)
		{
            print("start att");
			yield return 0;
		}
		JumpAttack();
        currentState = EnemyState.Attack;
    }
    IEnumerator AfterAttack()
    {
        
        rb.velocity = Vector3.zero;
        currentState = EnemyState.WaitForTransfer;
        for(float i =0 ; i<=AfterAttackWaitingTime ; i+=Time.deltaTime)
		{
            print("counting after Attack");
			yield return 0;
		}
        currentState = EnemyState.Combat;
        
    }
    void getHitCheck()
    {
        if(GetComponent<tempGetHit>().isHit)
        {
            if(!isAttacking)
            {
                StopCoroutinesExceptTiming();
                currentState = EnemyState.Repel;
                print("REPEL!!");
            }
            else
            {
                print("GetHit BUT Enemy ATTACKING");
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position,2*DetectPlayerlength);
    }
    void StopCoroutinesExceptTiming()
    {
        if(AfterAttackCoroutine!=null)
            StopCoroutine(AfterAttackCoroutine);
        if(StartAttackCoroutine!=null)
            StopCoroutine(StartAttackCoroutine);
    }
}
