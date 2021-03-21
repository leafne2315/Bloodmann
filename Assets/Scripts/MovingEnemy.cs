using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    public int hp;
    public int maxHp = 5;
    public bool getHit;
    private tempGetHit tempGetHit;
    private Vector3 MovingDir;
    private Rigidbody rb;
    public GameObject Player;
    public float movingSpeed;
    public float patrolSpeed;
    public Transform wallCheck;
    public Transform groundCheck;
    public bool isgroundChecked;
    public bool iswallChecked;
    public LayerMask WhatIsGround;
    public float groundCheckLength;
    public bool isFacingRight = true;
    public enum EnemyState{Patrol, PreMoving,Moving, Idle,Repel, Dead}
    public bool PlayerDetect;
    public LayerMask WhatIsPlayer;
    public Vector3 DetectPlayerlength;
    public EnemyState currentState;
    private bool notDie = true;
    private float dieTimer = 0;
    public float dyingTime;
    public Vector3 RepelDir;
    public float RepelForce;
    public float RepelTime;
    private float RepelTimer;
    public float PreMovingTimer;
    public float PreMovingTime;
    public float MovingTime;
    public float MovingTimer;
    public bool isMoving;
    public bool isIdle;
    // Start is called before the first frame update
    void Start()
    {
        tempGetHit = GetComponent<tempGetHit>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        WallCheck();
        DieCheck();
        getHitCheck();
        PatrolDir();
        FacingCheck();

        switch(currentState)
        {
            case EnemyState.Patrol:

                rb.velocity = MovingDir*patrolSpeed;
                DetectingPlayer();
                
                if(PlayerDetect)
                {
                    currentState = EnemyState.PreMoving;
                    rb.velocity = Vector3.zero;
                }
                
            break;
            
            case EnemyState.PreMoving:

                GetMoveDir();
                FacingCheck();
                if(PreMovingTimer<PreMovingTime)
                {
                    PreMovingTimer+=Time.deltaTime;
                }
                else
                {
                    PreMovingTimer = 0;
                    currentState = EnemyState.Moving;
                    isMoving = true;
                }

            break;
            
            case EnemyState.Moving:
            
                rb.velocity = MovingDir*movingSpeed;

                DetectingPlayer();
                if(PlayerDetect == true && MovingTimer == 0)
                {
                    GetMoveDir();
                    FacingCheck();
                }
                
                if(MovingTimer<MovingTime)
                {
                    MovingTimer+=Time.deltaTime;
                }
                else
                {
                    //rb.velocity = Vector3.zero;
                    MovingTimer = 0;
                    //currentState = EnemyState.PreMoving;
                }
                
                if(!PlayerDetect && MovingTimer == 0)
                {
                    isMoving = false;
                    currentState = EnemyState.Patrol;
                    rb.velocity = Vector3.zero;
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
                    currentState = EnemyState.Moving;
                }

            break;

            case EnemyState.Idle:
                //FacingCheck();
                DetectingPlayer();
                
                // if(isMoving&&PlayerDetect==false)
                // {
                //     isIdle = false;
                //     //MovingDir.x*= -1;
                //     //FacingCheck();
                //     currentState = EnemyState.PreMoving;
                // }

                if(PlayerDetect==false)
                {
                    currentState = EnemyState.Patrol;
                    isMoving = false;
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
                    transform.GetChild(0).GetComponent<MovingEnemyHealthBar>().DestroyUI();
                    Destroy(gameObject);
                }

            break;
        }
    }

    void GroundCheck()
    {
        RaycastHit hitGround;
        if(Physics.Raycast(groundCheck.position, -transform.up, out hitGround,groundCheckLength,WhatIsGround))
        {
            Debug.DrawRay(groundCheck.position, -transform.up *groundCheckLength,Color.red);
            isgroundChecked = true;  
        }
        else
        {
            if(!isMoving &&!isIdle)
            {
                isgroundChecked = false;
                FacingCheck();
                MovingDir.x*= -1;
            }
            
            if(isMoving&&PlayerDetect==false)
            {
                Flip();
                isMoving = false;
                rb.velocity = Vector3.zero;
                isIdle = false;
                isgroundChecked = false;
                //GetMoveDir();
                //FacingCheck();
                MovingTimer = 0;
                MovingDir.x*= -1;
                //currentState = EnemyState.PreMoving;
            }

            if(isMoving&&PlayerDetect==true)
            {
                currentState = EnemyState.Idle;
                isIdle = true;         
                rb.velocity = Vector3.zero;
                isgroundChecked = false;
                //isMoving = false;
                //ovingTimer = 0;
                GetMoveDir();
                FacingCheck();
                MovingDir.x*= -1;
            }
        }

    }

    void WallCheck()
    {
        iswallChecked = Physics.CheckSphere(wallCheck.position,0.05f,WhatIsGround);
        if(iswallChecked)
        {
            if(!isMoving)
            {
                FacingCheck();
                MovingDir.x*= -1;
            }
            if(isMoving)
            {
                MovingTimer = 0;
                isMoving = false;
                rb.velocity = Vector3.zero;
                FacingCheck();
                MovingDir.x*= -1;
                currentState = EnemyState.PreMoving;
            }
            

        }
        // if(iswallChecked)
        // {
               
        // }
        // else
        // {
        //     Debug.DrawRay(wallCheck.position, transform.right * 1000, Color.white);
        //     iswallChecked = false;
        // }
    }
    void getHitCheck()
    {
        if(GetComponent<tempGetHit>().isHit)
        {
            hp--;
            get_RepelDir();
            // if(!isAttacking)
            // {
            currentState = EnemyState.Repel;
            //     Timer = 0;
            //     RushBugAni.SetTrigger("Repel");
            // }
        }
        if(tempGetHit.isHit)
        {
            getHit = true;
        }
        else
        {
            getHit = false;
        }
    }
    void get_RepelDir()
    {
        Vector3 dir = GetComponent<tempGetHit>().KnockDir;
        RepelDir = dir;
    }
    void GetMoveDir()
    {
        if(transform.position.x>Player.transform.position.x&& isgroundChecked ==true)
        {
            MovingDir = -Vector3.right;
        }
        else
        {
            MovingDir = Vector3.right;
        }
    }


    void PatrolDir()
    {
        if(MovingDir.x >= 0)
        {
            MovingDir = Vector3.right;
        }
        if(MovingDir.x <=0)
        {
            MovingDir = -Vector3.right;
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

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            Player.GetComponent<PlayerCtroller>().gettingHit();
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
                
                currentState = EnemyState.Dead;
                
            }   
        }
    }
  
}
