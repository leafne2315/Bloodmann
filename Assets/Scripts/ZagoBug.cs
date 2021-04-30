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
    public bool isZagoSFXPlaying;
    public AudioSource zagoBugSFX;

    [Header("Material")]
    public Material zagoDieMaterial;
    public GameObject zagoModel;
    public float dissolveSpeed;
    float dissolveOverTime;
    void Awake()
    {
        tempGetHit = GetComponent<tempGetHit>();
        rb = GetComponent<Rigidbody>();
        
    }
    void Start()
    {
        zagoDieMaterial.SetFloat("_Fade",-1);
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
        GetStabbedCheck();
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

                if(!isZagoSFXPlaying&&notDie)
                {
                    isZagoSFXPlaying = true;
                    zagoBugSFX.Play();
                }
                else if(!notDie)
                {
                    isZagoSFXPlaying = false;
                    zagoBugSFX.Stop();
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
                    zagoModel.GetComponent<SkinnedMeshRenderer>().material = zagoDieMaterial;
                    dissolveOverTime += Time.deltaTime *dissolveSpeed;
                    zagoDieMaterial.SetFloat("_Fade",dissolveOverTime);
                    transform.GetChild(0).GetComponent<MovingEnemyHealthBar>().DestroyUI();
                    Destroy(gameObject,1.5f);
                }
                
                isZagoSFXPlaying = false;
                zagoBugSFX.Stop();

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
            isZagoSFXPlaying = false;
            zagoBugSFX.Stop();
            get_RepelDir();
            hp--;
            LastState = currentState;
            currentState = EnemyState.Repel;
            ZagoAni.SetTrigger("getHit");
        }
    }
    void GetStabbedCheck()
    {
        if(GetComponent<tempGetHit>().isStabbed)
        {
            hp--;
            if(transform.position.x>Player.transform.position.x)
            {
                RepelDir = Vector3.right;
            }
            else
            {
                RepelDir = Vector3.left;
            }

            currentState = EnemyState.Repel;
            rb.velocity = Vector3.zero;

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
                ZagoAni.SetTrigger("Dead");
                

            }   
        }
    }
    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            if(notDie)
            {
                PlayerCtroller p = Player.GetComponent<PlayerCtroller>();

                if(transform.position.x>Player.transform.position.x)
                {
                    p.getHitByRight = true;
                }
                else
                {
                    p.getHitByRight = false;
                }
                p.gettingHit();
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}
