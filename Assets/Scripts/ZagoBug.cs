using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZagoBug : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    public int hp;
    public int MaxHp;
    public bool isFacingRight = true;
    private tempGetHit tempGetHit;
    public GameObject Player;
    public enum EnemyState{Patrol,PreMoving,Stop,Attacking,Repel,Dead,Idle,Fall}
    public EnemyState currentState;
    private EnemyState LastState;
    public Vector3 MovingDir;
    private float timer;
    public bool goAttack;
    public bool notDie = true;

    [Header("Detect Settings")]
    public LayerMask PlayerLayer;
    
    public bool PlayerDetect;
    public bool isGroundCheck;
    public bool isWallCheck;
    public Transform CheckPoint;
    public float groundCheckLength;
    public LayerMask GroundLayer;
    

    [Header("Patrol Settings")]
    public Vector3 PatrolDetectlength;
    public float patrolSpeed;
    public float patrolTime;
    public float patrolRest;

    [Header("Attacking Settings")]
    public float AttackingSpeed;
    public float AttackTime;
    public Transform AttackPos;
    public float AttackCD;
    public Vector3 AttackDetectlength;
    
    [Header("Stop Settings")]
    public float StopTime;

    [Header("Fall Settings")]
    public bool isLand;
    public Transform[] Button;

    [Header("Repel Setting")]
    public Vector3 RepelDir;
    public float RepelForce;
    public float RepelTime;
    private float RepelTimer;

    [Header("Die Settings")]
    private float dieTimer;
    public float dyingTime = 1;
    void Start()
    {
        tempGetHit = GetComponent<tempGetHit>();
        rb = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        rb.AddForce(Physics.gravity*4.5f,ForceMode.Acceleration);
    }
    // Update is called once per frame
    void Update()
    {
        FacingCheck();
        getHitCheck();
        ButtonCheck();
        DieCheck();

        switch(currentState)
        {
            case EnemyState.Patrol:
                
                if(timer<patrolTime)
                {
                    timer+=Time.deltaTime;

                    rb.velocity = MovingDir*patrolSpeed;
                }
                else
                {
                    timer = 0;
                    currentState = EnemyState.Stop;
                }
                
                GroundCheck();
                if(!isGroundCheck)
                {
                    MovingDir.x*=-1;
                }
                WallCheck();
                if(isWallCheck)
                {
                    MovingDir.x*=-1;
                }
                

                PatrolDetect();
                if(PlayerDetect)
                {
                    if(Player.transform.position.x>transform.position.x)
                    {
                        MovingDir.x = 1;
                    }
                    else
                    {
                        MovingDir.x = -1;
                    }

                    currentState = EnemyState.Stop;
                    goAttack = true;
                }
  

            break;

            case EnemyState.Attacking:

                if(timer<AttackTime)
                {
                    rb.velocity = MovingDir*AttackingSpeed;
                    timer += Time.deltaTime;
                }
                else
                {
                    timer = 0;
                    rb.velocity = Vector3.zero;

                    currentState = EnemyState.Stop;
                    goAttack = false;
                }

            break;

            case EnemyState.Stop:

                if(timer<StopTime)
                {
                    rb.velocity = Vector3.zero;
                    timer += Time.deltaTime;
                }
                else
                {
                    timer = 0;

                    AttackDetect();
                    if(PlayerDetect)
                    {
                        if(Player.transform.position.x>transform.position.x)
                        {
                            MovingDir.x = 1;
                        }
                        else
                        {
                            MovingDir.x = -1;
                        }
                        
                        currentState = EnemyState.Stop;
                        goAttack = true;
                        StopTime = AttackCD;
                    }
                    

                    if(goAttack)
                    {
                        currentState = EnemyState.Attacking;
                    }
                    else
                    {
                        currentState = EnemyState.Patrol;
                    }
                }

            break;

            case EnemyState.Repel:

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
                    currentState = LastState;
                }

            break;

            case EnemyState.Dead:

                rb.velocity = Vector3.zero;

                if(dieTimer<dyingTime)
                {
                    dieTimer+=Time.deltaTime;
                }
                else
                {
                    Destroy(gameObject);
                }

            break;

            case EnemyState.Fall:

                rb.velocity = new Vector3(0.0f,rb.velocity.y,0);

                if(isLand)
                {
                    currentState = EnemyState.Patrol;
                }

            break;

            case EnemyState.Idle:
            break;
        }
    }
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x*=-1;
        transform.localScale = Scaler;
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
    void PatrolDetect()
    {
        if(Physics.CheckBox(AttackPos.position,PatrolDetectlength,Quaternion.identity,PlayerLayer))
        {
            PlayerDetect = true;
        }
        else
        {
            PlayerDetect = false;
        }
    }
    void AttackDetect()
    {
        if(Physics.CheckBox(transform.position,AttackDetectlength,Quaternion.identity,PlayerLayer))
        {
            PlayerDetect = true;
        }
        else
        {
            PlayerDetect = false;
        }
    }
    void GroundCheck()
    {
        RaycastHit hitGround;
        if(Physics.Raycast(CheckPoint.position, Vector3.down, out hitGround,groundCheckLength,GroundLayer))
        {
            Debug.DrawRay(CheckPoint.position, Vector3.down *groundCheckLength,Color.red);
            isGroundCheck = true;  
        }
        else
        {
            isGroundCheck = false;
        }
    }
    void WallCheck()
    {
        RaycastHit hitWall;
        if(Physics.Raycast(CheckPoint.position,MovingDir,out hitWall,0.2f,GroundLayer))
        {
            isWallCheck = true;
        }
        else
        {
            isWallCheck = false;
        }
    }
    void getHitCheck()
    {
        if(GetComponent<tempGetHit>().isHit)
        {
            get_RepelDir();
            hp--;
            LastState = currentState;
            currentState = EnemyState.Repel;
        }
    }
    void ButtonCheck()
    {
        for(int i = 0;i<Button.Length;i++)
        {
            RaycastHit hit;
            if(Physics.Raycast(Button[i].position,Vector3.down,out hit,0.2f,GroundLayer))
            {
                isLand = true;
                break;
            }
            else
            {
                isLand = false;
            }
            Debug.DrawRay(Button[i].position, Vector3.down *0.2f,Color.red);
        }
        

        if(!isLand&&currentState!=EnemyState.Dead)
        {
            currentState = EnemyState.Fall;
        }
    }
    void get_RepelDir()
    {
        Vector3 dir = GetComponent<tempGetHit>().KnockDir;
        RepelDir = dir;
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
                
                currentState = EnemyState.Dead;
                //Die animation
            }   
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(AttackPos.position,2*PatrolDetectlength);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position,2*AttackDetectlength);
    }
}
