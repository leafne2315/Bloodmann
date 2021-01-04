using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform GeneratePos;
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
    public float AttackRange;
    public float TraceRange;
    public bool canAttack;
    public Vector3 MovingDir;
    public float MovingSpeed;
    public float accelration;

    [Header("Statement Settings")]
    public EnemyState currentState;
    public enum EnemyState{Patrol, Attacking, Dead}
    
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
                }

            break;

            case EnemyState.Attacking:

                CheckPlayerInSight();
                
                float distanceToPlayer = Vector3.Distance(transform.position,Player.transform.position);

                rb.velocity = MovingDir*MovingSpeed;

                if(distanceToPlayer<AttackRange)
                {
                    MovingSpeed-= accelration*Time.deltaTime;
                }
                else
                {
                    MovingSpeed+= accelration*Time.deltaTime;
                }
                
                if(SeePlayer)
                {
                    if(canAttack)
                    {
                        //Attack!!
                    }
                }
                else
                {
                    
                }
                
                
            break;
            case EnemyState.Dead:
                Destroy(gameObject);
            break;
        }
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
            }
            else
            {
                SeePlayer = false;
            }
        }   
    }
    void ResetTimer()
    {
        Timer = 0;
    }
    private void OnDrawGizmos() 
    {
        
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
