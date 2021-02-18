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
    Seeker seeker;
    public bool SeePlayer;
    public float playerDetectRadius;
    public float AttackDetectRadius;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsObstacle;
    private ViewDetect EnemyView;

    [Header("Patrol Settings")]
    [SerializeField]private Vector3 PatrolDir; 
    public float PatrolSpeed;
    public float PatrolingSpeed;
    public float ChangeDirFreq;
    public Transform[] PatrolPoint;
    public int PatrolNum;
    Path path;
    bool reachEndOfPath = false;
    int currentWaypoint = 0;
    public GameObject Target;
    public float nextWaypointDistance = 3.0f;
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
    public enum EnemyState{Patrol, Attacking, search, MoveBack, Dead}

    [Header("Under Attack")]
    public bool getHit;
    public float repelTime;
    public Vector3 getHitDir;
    public float getHitForce;
    [Header("Dying Set")]
    private bool notDie = true;
    private float dieTimer = 0;
    public float dyingTime;
    //private GameObject newEnemyAttack;
    void Start()
    {
        EnemyView = GetComponent<ViewDetect>();
        tempGetHit = GetComponent<tempGetHit>();
        rb = GetComponent<Rigidbody>();
        seeker = GetComponent<Seeker>();
        ChangePatrolDir();
        PatrolNum = 0;
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

                currentState = EnemyState.Dead;
                FlyBugAni.SetTrigger("Die");
            }
            
        }
        if(tempGetHit.isHit)
        {
            print("repel!!");
            health--;
            StartCoroutine(Repelling());
        }

        switch (currentState)
        {
            case EnemyState.Patrol:

                
                FacingPlayer();
                rb.velocity = MovingDir*PatrolingSpeed;
                Vector3 targetPos = PatrolPoint[PatrolNum].position;
                //MovingDir = Vector3.Lerp(MovingDir,(targetPos-transform.position).normalized,0.1f);
                MovingDir = (targetPos-transform.position).normalized;

                if(Vector3.Distance(targetPos,transform.position)<0.2f)
                {
                    if(PatrolNum<PatrolPoint.Length-1)
                    {
                        PatrolNum++;
                    }
                    else
                    {
                        PatrolNum = 0;
                    }
                    //currentState = EnemyState.Scan;
                }
                //rb.velocity = PatrolDir*PatrolSpeed;
                
                // if(Physics.CheckSphere(transform.position,playerDetectRadius,whatIsPlayer))
                // {
                //     CheckPlayerInSight();
                // }
                if(EnemyView.RedWarning)
                {
                    CheckPlayerInSight();
                }


                if(SeePlayer)
                {
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
                    StateIn_Search();
                }
                
            break;
            case EnemyState.search:
                aiPathfind();
                CheckPlayerInSight();
                //FacingPlayer();
                if(SeePlayer)
                {
                    currentState = EnemyState.Attacking;
                    
                    MovingDir = (Player.transform.position-transform.position).normalized;
                    ResetTimer();
                    StateLeave_Search();
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
                        //currentState = EnemyState.Patrol;
                        currentState = EnemyState.MoveBack;
                        StateLeave_Search();
                        seeker.StartPath(rb.position,PatrolPoint[0].position,OnPathComplete);
                        //FlyBugAni.SetTrigger("BackToIdle");
                        FlyBugAni.SetTrigger("StartMoving");
                    }
                }

            break;

            case EnemyState.MoveBack:
            aiPathfind();
            FacingPlayer();
            //rb.velocity = *PatrolingSpeed;
            
            if(Vector2.Distance(transform.position,PatrolPoint[0].position)<2.0f)
            {
                currentState = EnemyState.Patrol;
            }
                
            // if(Physics.CheckSphere(transform.position,playerDetectRadius,whatIsPlayer))
            // {
            //     CheckPlayerInSight();
            // }

            if(EnemyView.RedWarning)
            {
                CheckPlayerInSight();
            }

            

            if(SeePlayer)
            {
                //currentState = EnemyState.Attacking;
                StateIn_Search();
                FlyBugAni.SetTrigger("StartMoving");
                MovingDir = (Player.transform.position-transform.position).normalized;
                ResetTimer();
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
            getHit = true;
        }
        else
        {
            getHit = false;
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

     void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    void UpdatePath()
    {
        seeker.StartPath(rb.position,Target.transform.position,OnPathComplete);
    }
    void aiPathfind()
    {
        if(path == null)
            return;
        if(currentWaypoint>= path.vectorPath.Count)
        {
            reachEndOfPath = true;
            return;
        }
        else
        {
            reachEndOfPath = false;
        }
        MovingDir = ((Vector3)path.vectorPath[currentWaypoint]-rb.position).normalized;
        
        rb.velocity = MovingDir*PatrolingSpeed;

        //transform.right = Vector3.Lerp(transform.right,(Vector3)direction,0.1f);
        

        float distance = Vector2.Distance(rb.position,path.vectorPath[currentWaypoint]);
        if(distance<nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }
    void StateIn_Search()
    {
        currentState = EnemyState.search;
        //Target = EnemyView.target;
        
        InvokeRepeating("UpdatePath",0,0.3f);
    }
    void StateLeave_Search()
    {
        rb.velocity = Vector2.zero;
        CancelInvoke("UpdatePath");
    }
}
