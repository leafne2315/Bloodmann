using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEnemyCtrller : MonoBehaviour
{
    public int hp;
    public int maxHp = 5;
    public bool getHit;
    private float Timer;
    private tempGetHit tempGetHit;
    private Vector3 MovingDir;
    private Rigidbody rb;
    public RaycastHit hit;
    public GameObject Player;
    public Vector3 DetectPos;
    public Vector3 DetectBox;
    public EnemyState currentState;
    public float movingSpeed;
    public bool isGrounded;
    public bool isFacingRight;
    public float checkRadius;
    public Transform[] GroundCheckPos;
    public LayerMask WhatIsGround;
    public LayerMask WhatIsPlayer;
    private bool notDie = true;
    private float dieTimer = 0;
    public float dyingTime;
    public bool isSlimeSFXPlaying;
    public AudioSource slimeSFX;
    [Header("Material")]
    public Material dropEnemyDieMaterial;
    public GameObject dropEnemyModel;
    public float dissolveSpeed;
    float dissolveOverTime;
    public enum EnemyState{OnWall,Fall, Moving,GetStabbed, Dead}
    // Start is called before the first frame update
    void Start()
    {
        tempGetHit = GetComponent<tempGetHit>();
        rb = GetComponent<Rigidbody>();
        dropEnemyDieMaterial.SetFloat("_Fade",-1);
        
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
        GetStabbedCheck();
        GroundCheck();
        
        switch(currentState)
        {
            case EnemyState.OnWall:

                RaycastHit hit;              
                if(Physics.Raycast(transform.position, Vector3.down, out hit, WhatIsGround))
                {
                    Debug.DrawLine(transform.position,transform.position+Vector3.down*hit.distance,Color.red);
                }

                float Detectlength = hit.distance;
                DetectBox.y = Detectlength/2;
                DetectPos = transform.position + Vector3.down * Detectlength/2;

                if(Physics.CheckBox(DetectPos,DetectBox,Quaternion.identity,WhatIsPlayer))
                {
                    currentState = EnemyState.Fall;

                    GameObject sfx = Instantiate(Resources.Load("SoundPrefab/SlimeDrop") as GameObject, transform.position, Quaternion.identity);
                    Vector3 Scaler = transform.localScale;
                    Scaler.y*=-1;
                    transform.localScale = Scaler;
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

                if(!isSlimeSFXPlaying&&notDie)
                {
                    isSlimeSFXPlaying = true;
                    slimeSFX.Play();
                }
                else if(!notDie)
                {
                    isSlimeSFXPlaying = false;
                    slimeSFX.Stop();
                }

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
                    dropEnemyModel.GetComponent<SkinnedMeshRenderer>().material = dropEnemyDieMaterial;
                    dissolveOverTime += Time.deltaTime *dissolveSpeed;
                    dropEnemyDieMaterial.SetFloat("_Fade",dissolveOverTime);
                    transform.GetChild(2).GetComponent<DropEnemyHealthBar>().DestroyUI();
                    Destroy(gameObject,1.5f);
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
            hp--;
        }
        
    }
    void GetStabbedCheck()
    {
        if(tempGetHit.isStabbed)
        {
            hp--;
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
                isSlimeSFXPlaying = false;
                slimeSFX.Stop();
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
        if(currentState == EnemyState.OnWall)
            Gizmos.DrawWireCube(DetectPos,2*DetectBox);
    }

    // public void PullTrigger(Collider c)
    // {
    //     if(c.CompareTag("Player"))
    //     {
    //         Player.GetComponent<PlayerCtroller>().gettingHit();
    //     }
    // }
   
}
