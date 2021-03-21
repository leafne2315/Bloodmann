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
    public enum EnemyState{Patrol,PreMoving,Stop,Attacking,Repel,Dead,Idle}
    public EnemyState currentState;
    private EnemyState LastState;
    public Vector3 MovingDir;
    private float timer;
    public bool goAttack;
    public bool notDie = true;

    [Header("Detect Settings")]
    public LayerMask PlayerLayer;
    public float DetectPlayerRadius;
    public Vector3 DetectPlayerlength;
    public bool PlayerDetect;
    public bool isGroundCheck;
    public Transform groundCheck;
    public float groundCheckLength;
    public LayerMask GroundLayer;

    [Header("Patrol Settings")]
    public float patrolSpeed;
    [Header("Attacking Settings")]
    public float AttackingSpeed;
    public float AttackTime;
    
    [Header("Stop Settings")]
    public float StopTime;

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

    // Update is called once per frame
    void Update()
    {
        getHitCheck();
        DieCheck();

        switch(currentState)
        {
            case EnemyState.Patrol:
                
                rb.velocity = MovingDir*patrolSpeed;
                
                GroundCheck();
                if(!isGroundCheck)
                {
                    MovingDir.x*=-1;
                }

                DetectPlayer();
                if(PlayerDetect)
                {
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
                    timer += Time.deltaTime;
                }
                else
                {
                    timer = 0;

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

    void DetectPlayer()
    {
        if(Physics.CheckBox(transform.position,DetectPlayerlength,Quaternion.identity,PlayerLayer))
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
        if(Physics.Raycast(groundCheck.position, Vector3.down, out hitGround,groundCheckLength,GroundLayer))
        {
            Debug.DrawRay(groundCheck.position, Vector3.down *groundCheckLength,Color.red);
            isGroundCheck = true;  
        }
        else
        {
            isGroundCheck = false;
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
}
