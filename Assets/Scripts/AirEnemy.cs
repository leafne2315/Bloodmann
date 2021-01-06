using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject AttackPf;
    public float attackTime;
    private float Timer;
    public int health;
    private Rigidbody rb;
    public Vector3 PlayerLastPos;
    public GameObject Player;

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
    public enum EnemyState{Patrol, Attacking, search, Dead}
    
    //private GameObject newEnemyAttack;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ChangePatrolDir();
        //attackTimer = 3;
    }
    void Update()
    {
        
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
                    currentState = EnemyState.Attacking;
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
                
                float distanceToPlayer = Vector3.Distance(transform.position,Player.transform.position);
                if(canAttack)
                {
                    canAttack = false;
                    StartCoroutine(AttackCD_Count());
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
                    }
                }

            break;
            case EnemyState.Dead:
                Destroy(gameObject);
            break;
        }
    }
    IEnumerator ShootAttack()
    {
        for(float i =0 ; i<= PreAttackTime; i+=Time.deltaTime)
		{
			yield return 0;
		}
        GameObject AttackObj = Instantiate(AttackPf,GeneratePos.position,Quaternion.identity);
        Vector3 ShootDir = (Player.transform.position-transform.position).normalized;
        AttackObj.GetComponent<AirEnemyAttack>().attackDir = ShootDir;
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
    void ResetTimer()
    {
        Timer = 0;
    }
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,playerDetectRadius);
        
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
}
