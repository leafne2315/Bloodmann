using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public Transform FrontCheck;
    private bool isContactPlayer;
    private bool isContactGround;
    public float checkRadius;
    public LayerMask WhatIsPlayer;
    public LayerMask WhatIsGround;
    public float speed;
    private Vector3 newDir;
    private ViewDetect EnemyView;
    public Transform[] PatrolPoint;
    public int PatrolNum;
    public enum EnemyState{Patrol,Scan,Alert,Chase,Attack}
    private EnemyState currentState;
    //public Vector3 MoveDir;
    private float ScanTimer;
    public float ScanTime;
    public float ScanAngle;
    public float RotatePersecond;
    //Alert
    [SerializeField]private float AlertValue;
    public float AlertIncreasePS;  
    public float AlertDecreasePS;
    //Chase
    public GameObject Target;
    public float ChasingSpeed;
    public float nextWaypointDistance = 3.0f; 
    Path path;
    int currentWaypoint = 0;
    bool reachEndOfPath = false;
    Seeker seeker;
    //Attack
    private float AttackTimer;
    public float preActionTime;
    public float AttackTime;
    private Vector2 AttackDir;
    public float AttackSpeed;
    public float AttackRange_Max;
    public float AttackRange_Min;
    public float AttackCD;
    private bool canAttack = true; 
    //UI
    public Image AwareUI;
    void Start()
    {
        EnemyView = GetComponent<ViewDetect>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        currentState = EnemyState.Patrol;
        PatrolNum = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(currentState)
        {
            case EnemyState.Patrol:
            break;
            case EnemyState.Chase:
                
            break;
            case EnemyState.Attack:
            break;
            default:
            break;
        }   
    }
    void Update()
    {
        isContactPlayer = Physics2D.OverlapCircle(FrontCheck.position,checkRadius,WhatIsPlayer);
        isContactGround = Physics2D.OverlapCircle(FrontCheck.position,checkRadius,WhatIsGround);

        switch(currentState)
        {
            case EnemyState.Patrol:
            
                rb.velocity = transform.right*speed;

                Vector3 targetPos = PatrolPoint[PatrolNum].position;
                transform.right = Vector3.Lerp(transform.right,(targetPos-transform.position).normalized,0.1f);

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

                if(EnemyView.Warning)
                {
                    if(AlertValue<100)
                        AlertValue += AlertIncreasePS*Time.deltaTime;
                    else
                    {
                        AlertValue = 100;
                        StateIn_Chase();
                        //EnemyView.SwitchDetectMode("Red");
                    }
                }
                else
                {
                    if(AlertValue>0)
                        AlertValue -= AlertDecreasePS*Time.deltaTime;
                    else
                    {
                        AlertValue = 0;
                    } 
                }

                if(EnemyView.RedWarning)
                {
                    AlertValue = 100;
                    StateIn_Chase();
                }

            break;
/*
            case EnemyState.Scan:

                ScanTimer+=Time.deltaTime;
                
                transform.right = Quaternion.AngleAxis(RotatePersecond*Time.deltaTime,Vector3.back)*transform.right;
                
            break;
*/
/*
            case EnemyState.Alert:

                transform.position = Vector3.MoveTowards(transform.position,EnemyView.PlayerLastPos,speed*Time.deltaTime);
                if(Vector3.Distance(EnemyView.PlayerLastPos,transform.position)>0.1f)
                    transform.right = (EnemyView.PlayerLastPos-transform.position).normalized;

                AlertValue -= DangerDecreasePs*Time.deltaTime;
                speed = 10;

                if(EnemyView.Warning)
                {
                    //currentState = EnemyState.Chase;
                    AlertValue = 100;
                }

                if(AlertValue<=0)
                {
                    AlertValue = 0;
                    currentState = EnemyState.Patrol;
                    EnemyView.SwitchDetectMode("Yellow");
                    speed = 5; 
                }
            break;
*/
            case EnemyState.Chase:
                
                
                aiPathfind();
                if(Vector2.Distance(transform.position,Target.transform.position)<Random.Range(AttackRange_Min,AttackRange_Max))
                {
                    if(canAttack)
                    {
                        currentState = EnemyState.Attack;
                        StateLeave_Chase();
                    }
                    
                }
                //rb.velocity = transform.right*10;
                //transform.right = Vector3.Lerp(transform.right,(EnemyView.PlayerLastPos-transform.position).normalized,0.8f);
                // if(Vector3.Distance(EnemyView.PlayerLastPos,transform.position)<0.5f && !EnemyView.Warning)
                // {
                //     currentState = EnemyState.Patrol;
                // }
                
            break;

            case EnemyState.Attack:
                AttackTimer+=Time.deltaTime;
                if(AttackTimer<preActionTime)
                {
                    print("preAction");
                    AttackDir = (Target.transform.position-transform.position).normalized;
                    transform.right = AttackDir;
                }
                else if(AttackTimer<preActionTime+AttackTime)
                {
                    rb.velocity = AttackDir*AttackSpeed;
                    
                    if(isContactPlayer)
                    {
                        GetComponent<BoxCollider2D>().isTrigger = true;
                    }
                    if(isContactGround)
                    {
                        rb.velocity = Vector2.zero;
                    }
                }
                else
                {
                    canAttack = false;
                    
                    AttackTimer = 0;
                    StartCoroutine(AttackCD_Count());
                    StateIn_Chase();
                }
            break;

            default:
            break;
        }
        AwareUI.fillAmount = AlertValue/100;
    }
    void PatrolDir_change()
    {

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

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint]-rb.position).normalized;
        transform.right = Vector3.Lerp(transform.right,(Vector3)direction,0.1f);
        rb.velocity = transform.right*ChasingSpeed;
        
        float distance = Vector2.Distance(rb.position,path.vectorPath[currentWaypoint]);
        if(distance<nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
    void StateIn_Chase()
    {
        currentState = EnemyState.Chase;
        Target = EnemyView.target;
        InvokeRepeating("UpdatePath",0,0.3f);
    }
    void StateLeave_Chase()
    {
        rb.velocity = Vector2.zero;
        CancelInvoke("UpdatePath");
    }
    IEnumerator AttackCD_Count()
	{
		for(float i =0 ; i<=AttackCD ; i+=Time.deltaTime)
		{

			yield return 0;
		}
		canAttack = true;
	}
    private void OnTriggerExit2D(Collider2D other)
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
    private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(FrontCheck.position,checkRadius);
	}
}
