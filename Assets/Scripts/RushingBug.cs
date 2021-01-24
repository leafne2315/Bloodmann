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
    public float Timer;
    public bool isOverFrame;
    public Animator RushBugAni;
    public bool notDie = true;
    [Header("Basic Element")]
    public float hp;
    public float MaxHp;
    public float movingSpeed;
    public float gravityScale;

    [Header("Detect Settings")]
    public Vector3 DetectPlayerlength;
    public float DetectPlayerRadius;
    public LayerMask WhatIsPlayer;
    public bool PlayerDetect;
    public Transform GroundCheck;
    public float GroundCheckRadius = 0.1f;
    public LayerMask WhatIsGround;
    public bool GetHit;

    [Header("Attack Settings")]
    public Vector3 AttackDir;
    public bool canAttack;
    public bool isAttacking;
    public float AttackLength;
    public Vector3 AttackRange;
    public Transform AttackPos;
    public float AttForce;
    public float AttackCD;
    public float damage;
    public float AttackAngle;
    public float PreAttackTime;
    public float AfterAttackWaitingTime;
    [Header("Repel Setting")]
    public Vector3 RepelDir;
    public float RepelForce;
    public float RepelTime;
    private float RepelTimer;
    [Header("Die Settings")]
    private float dieTimer;
    public float dyingTime = 1;

    [Header("Statement")]
    public EnemyState currentState;
    public enum EnemyState{Idle,Patrol,InCombat,PreAttack,Attacking,AfterAttack,Die,WaitForTransfer,Repel}
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
            case EnemyState.InCombat:
                
            break;
            case EnemyState.PreAttack:

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
        FacingCheck();
        GetHitCheck();
        DieCheck();
        //print(currentState);

        switch(currentState)
        {
            case EnemyState.Idle:

                DetectingPlayer();

                if(PlayerDetect)
                {
                    currentState = EnemyState.InCombat;
                    RushBugAni.SetTrigger("ToMove");
                }

            break;
            case EnemyState.Patrol:

            break;
            case EnemyState.InCombat:
                
                GetMoveDir();
                DetectingPlayer();
                MoveTowardPlayer();

                if(PlayerInRange()&&canAttack)
                {

                    currentState = EnemyState.PreAttack;
                    RushBugAni.SetTrigger("Attack");

                    rb.velocity = Vector3.zero;
                    isAttacking = true;
                    get_AttackDir();
                }

                if(!PlayerDetect)
                {
                    currentState = EnemyState.Idle;
                    rb.velocity = Vector3.zero;
                    RushBugAni.SetTrigger("ToIdle");
                }
                
            break;
            case EnemyState.PreAttack:

                if(Timer<PreAttackTime)
                {
                    Timer+=Time.deltaTime;
                    //waiting
                }
                else
                {
                    Timer = 0;
                    JumpAttack(AttackDir);
                    currentState = EnemyState.Attacking;

                    AttackCDCoroutine = AttackCD_Count();
                    StartCoroutine(AttackCDCoroutine);
                    StartCoroutine(AfterFrame(2));
                }

            break;
            case EnemyState.Attacking:
                
                bool isGrounded = Physics.CheckSphere(GroundCheck.position,GroundCheckRadius,WhatIsGround);
                if(isGrounded&&isOverFrame)
                {
                    rb.velocity = Vector3.zero;
                    isAttacking = false;
                }
                
                if(!isAttacking)
                {
                    currentState = EnemyState.AfterAttack;
                    rb.velocity = Vector3.zero;
                }

            break;

            case EnemyState.AfterAttack:

                if(Timer<AfterAttackWaitingTime)
                {
                    Timer+=Time.deltaTime;
                }
                else
                {
                    Timer = 0;
                    currentState = EnemyState.InCombat;
                    RushBugAni.SetTrigger("ToMove");
                }
            break;
            
            case EnemyState.Repel:
                
                //print(RepelTimer);
                if(RepelTimer<RepelTime*0.3f)
                {
                    RepelTimer += Time.deltaTime;

                    rb.velocity = RepelDir*RepelForce;
                }
                else if(RepelTimer<RepelTime)
                {
                    RepelTimer += Time.deltaTime;
                    rb.velocity = Vector3.zero;
                }
                else
                {
                    RepelTimer = 0;
                    currentState = EnemyState.InCombat;
                    RushBugAni.SetTrigger("ToMove");
                }

            break;

            case EnemyState.Die:
                rb.velocity = Vector3.zero;

                if(dieTimer<dyingTime)
                {
                    dieTimer+=Time.deltaTime;
                }
                else
                {
                    transform.GetChild(2).GetComponent<RushEnemyUIController>().DestroyUI();
                    Destroy(gameObject);
                }

            break;

            default:
            break;
        }
    }
    
    IEnumerator AfterFrame(int frameNum)
    {
        isOverFrame = false;

        for(int i = 0;i<frameNum;i++)
		{
			yield return 0;
		}
        isOverFrame = true;
    }
    void get_RepelDir()
    {
        Vector3 dir = GetComponent<tempGetHit>().KnockDir;
        RepelDir = dir;
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
        if(Physics.CheckBox(transform.position,DetectPlayerlength,Quaternion.identity,WhatIsPlayer)||Physics.CheckSphere(transform.position,DetectPlayerRadius,WhatIsPlayer))
        {
            PlayerDetect = true;
            print("checked");
        }
        else
        {
            PlayerDetect = false;
            print("NotFound");
        }
    }
    bool PlayerInRange()
    {
        if(Physics.CheckBox(AttackPos.position,AttackRange,Quaternion.identity,WhatIsPlayer))
        {
            return true;
        }
        else
        {
            return false;
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
    void MoveTowardPlayer()
    {
        if(Mathf.Abs(transform.position.x-Player.transform.position.x)<0.5f)
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
    void JumpAttack(Vector3 dir)
    {
        rb.velocity = dir*AttForce;
        canAttack = false;
    }

    void get_AttackDir()
    {
        if(isFacingRight)
        {
            Vector3 dir = new Vector3(Mathf.Cos(AttackAngle*Mathf.Deg2Rad),Mathf.Sin(AttackAngle*Mathf.Deg2Rad),0.0f);
            AttackDir = dir;
        }
        else
        {
            Vector3 dir = new Vector3(-Mathf.Cos(AttackAngle*Mathf.Deg2Rad),Mathf.Sin(AttackAngle*Mathf.Deg2Rad),0.0f);
            AttackDir = dir;
        }    
    }
    void GetHitCheck()
    {
        if(GetComponent<tempGetHit>().isHit)
        {
            get_RepelDir();
            hp--;

            if(!isAttacking)
            {
                currentState = EnemyState.Repel;
                Timer = 0;
                RushBugAni.SetTrigger("Repel");
            }
        }
    }
    void DieCheck()
    {
        if(hp<=0)
        {
            if(notDie)
            {   
                notDie = false;

                gameObject.tag = "DeadObject";
                gameObject.layer = LayerMask.NameToLayer("DeadObject");
                
                currentState = EnemyState.Die;
                RushBugAni.SetTrigger("Die");
            }   
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position,2*DetectPlayerlength);
        Gizmos.DrawWireSphere(transform.position,DetectPlayerRadius);

        if(PlayerInRange())
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawWireCube(AttackPos.position,2*AttackRange);
    }
    void StopCoroutinesExceptTiming()
    {
        if(AfterAttackCoroutine!=null)
            StopCoroutine(AfterAttackCoroutine);
        if(StartAttackCoroutine!=null)
            StopCoroutine(StartAttackCoroutine);
    }

}
