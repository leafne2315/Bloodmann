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
    private Rigidbody2D rb;
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
    private GameObject newEnemyAttack;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //attackTimer = 3;
    }

    
    void Update()
    {
        Collider2D shootDetection = Physics2D.OverlapCircle(transform.position,shootDetectRadius,whatIsPlayer);
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
                if(shootDetection)
                {
                    currentState = EnemyState.Attack;    
                }

                
            break;
            case EnemyState.Attack:
                
                if(shootDetection)
                {
                    PlayerLastPos = shootDetection.transform.position;
                }
                
                Launch();
                if(!shootDetection)
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
            newEnemyAttack = Instantiate(AttackPf, GeneratePos.transform.position, Quaternion.identity);
            attackTimer = 0;
        }
        
        if(attackTimer < attackTime)
        {
            attackTimer+=Time.deltaTime; 
        }
    }
}
