using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEnemy : MonoBehaviour
{
    public Transform redDetection;
    public Transform yellowDetection;
    public LayerMask whatIsPlayer;
    public float yellowDetectionRadius;
    public float redDetectionRadius;
    private Collider2D playerInYellow;
    private Collider2D playerInRed;
    //public float yellowDetectTime;
    public float redDetectTime;
    private Rigidbody2D rb;
    private PlayerCtroller PlayerCtroller; 
    //private Die_temp Die_temp;
    public Vector3 PlayerLastPos;
    private Vector2 playerDir;
    public float speed;
    private Vector3 soundenemyStartPos;
    public float soundEnemyDangerIncreasePS;
    public float soundEnemyDangerDecreasePs;
    public Image AwareUI;
    //public float redSpeed;
    void Start()
    {
        PlayerCtroller = GameObject.Find("Player").GetComponent<PlayerCtroller>();
        //Die_temp = GameObject.Find("Player").GetComponent<Die_temp>();
        rb = GetComponent<Rigidbody2D>();
        soundenemyStartPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        AwareUI.fillAmount = redDetectTime/100;
        playerInYellow = Physics2D.OverlapCircle(yellowDetection.position,yellowDetectionRadius,whatIsPlayer);
        playerInRed = Physics2D.OverlapCircle(redDetection.position,redDetectionRadius,whatIsPlayer);
        if(playerInYellow)
        {    //判斷角色有無飛行
            if(PlayerCtroller.isDash)
            {    
                PlayerLastPos = PlayerCtroller.transform.position;
                playerDir = (PlayerLastPos - rb.transform.position).normalized;
                rb.velocity = playerDir * speed;
            }
        }
        else
        {
            rb.velocity = playerDir * 0;
        }
        if(playerInRed && PlayerCtroller.isDash)
        {          
            if(redDetectTime <=100)
            {
                redDetectTime += soundEnemyDangerIncreasePS*Time.deltaTime;
            }
            else
            {   
                redDetectTime = 100;  
                //player死亡重置位置        
                //PlayerCtroller.Die();
                //敵人重置位置
                transform.position = soundenemyStartPos;
                redDetectTime = 0;
            }           
        }
        else
        {
            rb.velocity = playerDir * 0;
            if(redDetectTime >0)
            {
                redDetectTime -= soundEnemyDangerDecreasePs*Time.deltaTime;
            }
            else
            {
                redDetectTime = 0;
            }
        }
    }
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(yellowDetection.position, yellowDetectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(redDetection.position, redDetectionRadius);
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}

