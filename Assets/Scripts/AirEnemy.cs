using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float playerDetectRadius;
    public float shootDetectRadius;
    public float speed;
    public int health;
    private Rigidbody rb;
    public LayerMask whatIsPlayer;
    public Vector3 PlayerLastPos;
    public EnemyState currentState;
    public float attackTime;
    public float attackTimer;
    public enum EnemyState
    {
        Normal, Attack, Dead 
    }
    public Transform GeneratePos;
    public GameObject AttackPf;
    //private GameObject newEnemyAttack;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //attackTimer = 3;
    }

    
    void Update()
    {
        Collider[] shootDetections = Physics.OverlapSphere(transform.position,shootDetectRadius, whatIsPlayer);
        //Collider2D playerDetection = Physics2D.OverlapCircle(playerDetect.position,playerDetectRadius,whatIsPlayer);
        
        switch (currentState)
        {
            case EnemyState.Normal:
                
                
                if(health <= 0)
                {
                    currentState = EnemyState.Dead;
                }
                
                // if(playerDetection)
                // {
                //     currentState = EnemyState.Move;
                // }   
                foreach(var shootDetection in shootDetections)
                {
                    currentState = EnemyState.Attack;    
                }

                
            break;
            case EnemyState.Attack:
                
                foreach(var shootDetection in shootDetections)
                {
                    currentState = EnemyState.Attack;    
                }
                
                Launch();
                if(shootDetections == null)
                {
                    currentState = EnemyState.Normal;
                }   
                
            break;
            case EnemyState.Dead:
                Destroy(gameObject);
            break;
        }
    }
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerDetectRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootDetectRadius);
    }
    void Launch()
    {
        if(attackTimer >= attackTime)
        {
            GameObject newEnemyAttack = Instantiate(AttackPf, GeneratePos.transform.position, Quaternion.identity);
            newEnemyAttack.GetComponent<AirEnemyAttack>().attackDir = (PlayerLastPos-transform.position).normalized;
            attackTimer = 0;
        }
        
        if(attackTimer < attackTime)
        {
            attackTimer+=Time.deltaTime; 
        }
    }
}
