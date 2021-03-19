using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEnemyCtrller : MonoBehaviour
{
    public int hp;
    public int maxHp = 5;
    public bool getHit;
    private tempGetHit tempGetHit;
    public Vector3 isfallCheck;
    private Vector3 MovingDir;
    public bool isfall;
    private Rigidbody rb;
    public RaycastHit hit;
    public GameObject Player;
    public EnemyState currentState;
    public float movingSpeed;
    public bool isGrounded;
    public bool isFacingRight;
    public bool addForce;
    public float checkRadius;
    public Transform GroundCheck;
    public LayerMask WhatIsGround;
    public LayerMask WhatIsPlayer;
    private bool notDie = true;
    private float dieTimer = 0;
    public float dyingTime;
    public enum EnemyState{OnWall, Moving, Dead}
    // Start is called before the first frame update
    void Start()
    {
        tempGetHit = GetComponent<tempGetHit>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        DieCheck();
        getHitCheck();
        
        isGrounded = Physics.CheckSphere(GroundCheck.position,checkRadius,WhatIsGround);
        
        switch(currentState)
        {
            case EnemyState.OnWall:

            if(Physics.Raycast(transform.position, -transform.up, out hit, 10,WhatIsPlayer))
            {
                Debug.DrawLine(transform.position, hit.point, Color.yellow);
                isfall = true;
                rb.useGravity = true;
                addForce = true;
            }
            else
            {
                Debug.DrawRay(transform.position, -transform.up * 10, Color.white);
            }

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

    void FixedUpdate() 
    {   
        if(addForce)
        rb.AddForce(Physics.gravity*5.0f,ForceMode.Acceleration);
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

    public void PullTrigger(Collider c)
    {
        if(c.CompareTag("Player"))
        {
            Player.GetComponent<PlayerCtroller>().gettingHit();
        }
    }
   
}
