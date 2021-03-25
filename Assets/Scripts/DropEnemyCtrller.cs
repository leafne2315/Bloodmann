using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEnemyCtrller : MonoBehaviour
{
    public int hp;
    public int maxHp = 5;
    public bool getHit;
    private tempGetHit tempGetHit;
    private Vector3 MovingDir;
    private Rigidbody rb;
    public RaycastHit hit;
    public GameObject Player;
    public Transform DetectPos;
    public Vector3 DetectRange;
    public EnemyState currentState;
    public float movingSpeed;
    public bool isGrounded;
    public bool isFacingRight;
    public bool addForce;
    public float checkRadius;
    public Transform[] GroundCheckPos;
    public LayerMask WhatIsGround;
    public LayerMask WhatIsPlayer;
    private bool notDie = true;
    private float dieTimer = 0;
    public float dyingTime;
    public enum EnemyState{OnWall,Fall, Moving, Dead}
    // Start is called before the first frame update
    void Start()
    {
        tempGetHit = GetComponent<tempGetHit>();
        rb = GetComponent<Rigidbody>();
        //transform.eulerAngles = new Vector3(0,0,180);
    }

    void FixedUpdate() 
    {   
        if(currentState!=EnemyState.OnWall)
        {
            rb.AddForce(Physics.gravity*5.0f,ForceMode.Acceleration);
        }
    }
    // Update is called once per frame
    void Update()
    {
        DieCheck();
        getHitCheck();
        GroundCheck();
        
        switch(currentState)
        {
            case EnemyState.OnWall:

                if(Physics.CheckBox(DetectPos.position,DetectRange,Quaternion.identity,WhatIsPlayer))
                {
                    currentState = EnemyState.Fall;
                    transform.eulerAngles = Vector3.zero;

                    Debug.DrawLine(transform.position, hit.point, Color.yellow);
                }
                else
                {
                    Debug.DrawRay(transform.position, Vector3.down * 10, Color.white);
                }

            break; 

            case EnemyState.Fall:

                rb.velocity = new Vector3(0.0f,rb.velocity.y,0.0f);

                if(isGrounded)
                {
                    currentState = EnemyState.Moving;
                }

            break;

            case EnemyState.Moving:

                rb.velocity = MovingDir*movingSpeed;
            
                if(Player.transform.position.x-transform.position.x>2 ||Player.transform.position.x-transform.position.x<-2)
                {
                    GetMoveDir();
                    FacingCheck();
                }

                if(!isGrounded)
                {
                    currentState = EnemyState.Fall;
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
                    transform.GetChild(2).GetComponent<DropEnemyHealthBar>().DestroyUI();
                    Destroy(gameObject);
                }

            break;
        }        
    }
    void GroundCheck()
    {
        for(int i = 0;i<GroundCheckPos.Length;i++)
        {
            RaycastHit hit;
            if(Physics.Raycast(GroundCheckPos[i].position,Vector3.down,out hit,0.2f,WhatIsGround))
            {
                isGrounded = true;
                break;
            }
            else
            {
                isGrounded = false;
            }
            Debug.DrawRay(GroundCheckPos[i].position, Vector3.down *0.2f,Color.red);
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

        if(GetComponent<tempGetHit>().isHit)
        {
            hp--;
            // if(!isAttacking)
            // {
            //     currentState = EnemyState.Repel;
            //     Timer = 0;
            //     RushBugAni.SetTrigger("Repel");
            // }
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
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerCtroller>().gettingHit();
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(DetectPos.position,2*DetectRange);
    }

    // public void PullTrigger(Collider c)
    // {
    //     if(c.CompareTag("Player"))
    //     {
    //         Player.GetComponent<PlayerCtroller>().gettingHit();
    //     }
    // }
   
}
