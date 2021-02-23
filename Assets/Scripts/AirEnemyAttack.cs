using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemyAttack : MonoBehaviour
{
    public Vector3 attackDir;
    private PlayerCtroller PlayerCtroller; 
    private AirEnemy AirEnemy;
    private Rigidbody rb;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //attackDir = (AirEnemy.PlayerLastPos-transform.position).normalized;
        rb.velocity = attackDir*speed;
    }
    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerCtroller>().gettingHit();

            if(!other.GetComponent<PlayerCtroller>().isInvincible)
            {
                Destroy(gameObject);
            }
            
            
        }
    }
}
