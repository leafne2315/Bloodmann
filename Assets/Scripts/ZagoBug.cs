using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZagoBug : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    public int hp;
    public int maxHp;
    public bool isFacingRight = true;
    private tempGetHit tempGetHit;
    public GameObject Player;
    public enum EnemyState{Move,Repel,Dead,Idle,Fall}
    public EnemyState currentState;
    private EnemyState LastState;
    public Vector3 MovingDir;
    public float MovimgSpeed;
    private float timer;
    public bool notDie = true;

    [Header("Detect Settings")]
    public bool isGroundCheck;
    public bool isWallCheck;
    public Transform CheckPoint;
    public float groundCheckLength;
    public LayerMask GroundLayer;

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
    [Header("Animation Setting")]
    public Animator ZagoAni;
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
            case EnemyState.Move:
                
                rb.velocity = MovingDir*MovimgSpeed;
                
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
                    transform.GetChild(0).GetComponent<MovingEnemyHealthBar>().DestroyUI();
                    Destroy(gameObject);
                }

            break;

            case EnemyState.Fall:

                rb.velocity = new Vector3(0.0f,rb.velocity.y,0);

                if(isLand)
                {
                    currentState = EnemyState.Move;
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
            ZagoAni.SetTrigger("getHit");
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
                ZagoAni.SetBool("Dead",true);
                //Die animation

            }   
        }
    }
    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            Player.GetComponent<PlayerCtroller>().gettingHit();
        }
    }
    void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        
    }
}