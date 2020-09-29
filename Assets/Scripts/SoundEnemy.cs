using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEnemy : MonoBehaviour
{
     public Transform redDetection;
    public Transform yellowDetection;
    public LayerMask whatIsPlayer;
    public float yellowDetectionRadius;
    public float redDetectionRadius;
    private Collider2D yellowWarning;
    private Collider2D redWarning;
    //public float yellowDetectTime;
    public float redDetectTime;
    private Rigidbody2D rb;
    private PlayerCtroller PlayerCtroller; 
    private Die_temp Die_temp;
    public Vector3 PlayerLastPos;
    private Vector2 playerDir;
    public float yellowSpeed;
    private Vector3 soundenemyStartPos;
    public float soundenemyDangerIncreasePS;
    public float soundenemyDangerDecreasePs;
    //public float redSpeed;
    void Start()
    {
        PlayerCtroller = GameObject.Find("Player").GetComponent<PlayerCtroller>();
        Die_temp = GameObject.Find("Player").GetComponent<Die_temp>();
        rb = GetComponent<Rigidbody2D>();
        soundenemyStartPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        yellowWarning = Physics2D.OverlapCircle(yellowDetection.position,yellowDetectionRadius,whatIsPlayer);
        if(yellowWarning)
        {    //判斷角色有無飛行
            if(PlayerCtroller.currentState == PlayerCtroller.PlayerState.AirDash||PlayerCtroller.currentState == PlayerCtroller.PlayerState.BugFly)
            {    
                PlayerLastPos = PlayerCtroller.transform.position;
                playerDir = (PlayerLastPos - rb.transform.position);
                yellowSpeed = 1;
                rb.velocity = playerDir * yellowSpeed; 
            }
        }
        else
        {
            yellowSpeed = 0;
            rb.velocity = playerDir * yellowSpeed;
        }
        redWarning = Physics2D.OverlapCircle(redDetection.position,redDetectionRadius,whatIsPlayer);
        if(redWarning)
        {
            if(PlayerCtroller.currentState == PlayerCtroller.PlayerState.AirDash||PlayerCtroller.currentState == PlayerCtroller.PlayerState.BugFly)
            {    
                if(redDetectTime <=100)
                {
                    redDetectTime += soundenemyDangerIncreasePS*Time.deltaTime;
                }
                else
                {   
                    redDetectTime = 100;  
                    //player死亡重置位置        
                    Die_temp.transform.position = Die_temp.StartPos;
                    Die_temp.rb.velocity = Vector2.zero;
                    PlayerCtroller.currentGas = 100;
                    PlayerCtroller.Out_Of_Gas = false;
                    //敵人重置位置
                    transform.position = soundenemyStartPos;
                    redDetectTime = 0;
                }           
            }
        }
        else
        {
            if(redDetectTime >0)
            {
                redDetectTime -= soundenemyDangerDecreasePs*Time.deltaTime;
            }
            else
            {
                redDetectTime = 0;
            }
        }
    }
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(yellowDetection.position, yellowDetectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(redDetection.position, redDetectionRadius);
    }
}
