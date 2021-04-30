using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AirEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    private tempGetHit tempGetHit;
    public GameObject AttackPf;
    private float Timer;
    public int health;
    public int maxhealth = 5;
    private Rigidbody rb;
    public Vector3 PlayerLastPos;
    public GameObject Player;
    public bool isFacingRight;
    public float gravityScale;

    [Header("Animation Settings")]
    public Animator FlyBugAni;
    public AnimationClip FlyBugShoot;

    [Header("Detect Settings")]
    public bool SeePlayer;
    public float playerDetectRadius;
    public float AttackDetectRadius;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsObstacle;

    [Header("Patrol Settings")]
    [SerializeField]private Vector3 PatrolDir; 
    public float PatrolSpeed;
    public float ChangeDirFreq;
    [Header("Attack Settings")]
    public Transform GeneratePos;
    public float AttackRange;
    public float TraceRange;
    public bool canAttack;
    public Vector3 MovingDir;
    public float MovingSpeed;
    public float accelration;
    public float MaxSpeed;
    public float AttackCD;
    public float PreAttackTime;

    [Header("Search Settings")]
    public float SearchingMoveSpeed;
    public float SearchTime;
    public float RandomSearchTime;
    private bool randomSearch;

    [Header("Statement Settings")]
    public EnemyState currentState;
    public enum EnemyState{Patrol, Attacking, search, GetStabbed, Dead}

    [Header("Under Attack")]
    public bool getHit;
    public float repelTime;
    public Vector3 getHitDir;
    public float getHitForce;
    [Header("Dying Set")]
    private bool notDie = true;
    private float dieTimer = 0;
    public float dyingTime;
    public bool isFlyingBugSFXPlaying;
    public AudioSource flyingBugSFX;
    //private GameObject newEnemyAttack;
    void Start()
    {
        tempGetHit = GetComponent<tempGetHit>();
        rb = GetComponent<Rigidbody>();
        ChangePatrolDir();
        flyingBugSFX.Play();
        //attackTimer = 3;
    }
    
    void FixedUpdate()
    {
        if(currentState == EnemyState.Dead)
        {
            GravityInput();
        } 
    }
    void Update()
    {
        if(health<=0)
        {
            if(notDie)
            {   
                notDie = false;
                gameObject.tag = "DeadObject";
                gameObject.layer = LayerMask.NameToLayer("DeadObject");

                flyingBugSFX.Stop();
                GameObject sfx = Instantiate(Resources.Load("SoundPrefab/FlyingBugGetHit") as GameObject, transform.position, Quaternion.identity);
                currentState = EnemyState.Dead;
                FlyBugAni.SetTrigger("Die");
            }
            
        }

        

        getHitCheck();
        GetStabbedCheck();

        switch (currentState)
        {
            case EnemyState.Patrol:

                rb.velocity = PatrolDir*PatrolSpeed;
                if(Physics.CheckSphere(transform.position,playerDetectRadius,whatIsPlayer))
                {
                    CheckPlayerInSight();
                }

                if(SeePlayer)
                {
                    GameObject sfx = Instantiate(Resources.Load("SoundPrefab/FlyingBugDetect") as GameObject, transform.position, Quaternion.identity);
                    currentState = EnemyState.Attacking;
                    FlyBugAni.SetTrigger("StartMoving");
                    MovingDir = (Player.transform.position-transform.position).normalized;
                    ResetTimer();
                }

                if(Timer<ChangeDirFreq)
                {
                    Timer+=Time.deltaTime;
                }
                else
                {
                    ResetTimer();
                    ChangePatrolDir();
                    MovingSpeed = 0;
                }

            break;

            case EnemyState.Attacking:

                CheckPlayerInSight();

                FacingPlayer();

                float distanceToPlayer = Vector3.Distance(transform.position,Player.transform.position);
                if(canAttack)
                {
                    canAttack = false;
                    FlyBugAni.SetTrigger("Attack");
                    StartCoroutine(ShootAttack());
                }
                
                if(SeePlayer)
                {
                    MovingDir = (Player.transform.position-transform.position).normalized;
                    if(distanceToPlayer<AttackRange)
                    {
                        MovingSpeed -= accelration*Time.deltaTime;
                    }
                    else
                    {
                        MovingSpeed += accelration*Time.deltaTime;
                    }
                    MovingSpeed = Mathf.Clamp(MovingSpeed,-MaxSpeed,MaxSpeed);
                    rb.velocity = MovingDir*MovingSpeed;
                }
                else
                {
                    if(transform.position.y>Player.transform.position.y)
                    {
                        MovingDir = (MovingDir+Vector3.down).normalized;
                    }
                    else
                    {
                        MovingDir = (MovingDir+Vector3.up).normalized;
                    }
                    currentState = EnemyState.search;
                }
                
            break;
            case EnemyState.search:

                CheckPlayerInSight();
                if(SeePlayer)
                {
                    currentState = EnemyState.Attacking;
                    MovingDir = (Player.transform.position-transform.position).normalized;
                    ResetTimer();
                }

                
                if(!randomSearch)
                {
                    if(Timer<SearchTime)
                    {
                        Timer+=Time.deltaTime;
                        rb.velocity = MovingDir*SearchingMoveSpeed;
                    }
                    else
                    {
                        Timer = 0;
                        randomSearch = true;
                        Vector3 randomDir = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0).normalized;
                        MovingDir = randomDir;
                    }
                }
                else
                {
                    if(Timer<RandomSearchTime)
                    {
                        Timer+=Time.deltaTime;
                        rb.velocity = MovingDir*SearchingMoveSpeed;
                    }
                    else
                    {
                        ResetTimer();
                        currentState = EnemyState.Patrol;
                        FlyBugAni.SetTrigger("BackToIdle");
                    }
                }

            break;
            case EnemyState.Dead:

                
                if(rb.velocity.y>1.5f)
                {
                    rb.velocity = Vector3.Lerp(rb.velocity,Vector3.zero,0.9f);
                }
                rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x,0,0.9f),rb.velocity.y,0);
                
                if(dieTimer<dyingTime)
                {
                    dieTimer+=Time.deltaTime;
                }
                else
                {
                    
                    transform.GetChild(2).GetComponent<AirEnemyHealthBar>().DestroyUI();
                    Destroy(gameObject);
                }
                
            break;
        }
    }
    void FacingPlayer()
    {
        if(isFacingRight==false&&MovingDir.x<0)
        {
            Flip();
        }
        if(isFacingRight==true&&MovingDir.x>0)
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
    IEnumerator Repelling()
    {
        getHitDir = tempGetHit.KnockDir;
        for(float i =0 ; i <= repelTime; i+=Time.deltaTime)
		{
            rb.AddForce(getHitDir*getHitForce,ForceMode.VelocityChange);
			yield return 0;
		}
    }
    IEnumerator ShootAttack()
    {
        for(float i =0 ; i<= PreAttackTime; i+=Time.deltaTime)
		{
			yield return 0;
		}
        GameObject sfx = Instantiate(Resources.Load("SoundPrefab/FlyingBugAttack") as GameObject, transform.position, Quaternion.identity);
        GameObject AttackObj = Instantiate(AttackPf,GeneratePos.position,Quaternion.identity);
        Vector3 ShootDir = (Player.transform.position-GeneratePos.position).normalized;
        AttackObj.GetComponent<AirEnemyAttack>().attackDir = ShootDir;
        StartCoroutine(AttackCD_Count());
    }
    IEnumerator AttackCD_Count()
    {
        for(float i =0 ; i<= AttackCD; i+=Time.deltaTime)
		{
			yield return 0;
		}
		canAttack = true;
    }
    void ChangePatrolDir()
    {
        Vector3 randomDir = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0).normalized;
        PatrolDir = randomDir;
    }
    void CheckPlayerInSight()
    {
        Vector3 RayDir = Player.transform.position-transform.position;
        RaycastHit hit;
        
        if(Physics.Raycast(transform.position,RayDir,out hit,Mathf.Infinity,whatIsObstacle))
        {
            if(hit.collider.CompareTag("Player"))
            {
                SeePlayer = true;
                PlayerLastPos = hit.point;
                Debug.DrawLine(transform.position,hit.point,Color.red);
            }
            else
            {
                SeePlayer = false;

                Debug.DrawLine(transform.position,hit.point,Color.green);
            }
            
        }   
    }
    void getHitCheck()
    {
        if(tempGetHit.isHit)
        {
            print("repel!!");
            health--;
            StartCoroutine(Repelling());
            ResetTimer();
        }
    }
    void GetStabbedCheck()
    {
        if(GetComponent<tempGetHit>().isStabbed)
        {
            print("repel!!");
            health--;
            StartCoroutine(Repelling());
            ResetTimer();
        }
    }
    void ResetTimer()
    {
        Timer = 0;
    }
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,playerDetectRadius);
        
    }
    void GravityInput()
    {
        rb.AddForce(Physics.gravity*gravityScale,ForceMode.Acceleration);
    }
    void Launch()
    {
        // if(attackTimer >= attackTime)
        // {
        //     GameObject newEnemyAttack = Instantiate(AttackPf, GeneratePos.transform.position, Quaternion.identity);
        //     newEnemyAttack.GetComponent<AirEnemyAttack>().attackDir = (PlayerLastPos-transform.position).normalized;
        //     attackTimer = 0;
        // }
        
        // if(attackTimer < attackTime)
        // {
        //     attackTimer+=Time.deltaTime; 
        // }
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
}

