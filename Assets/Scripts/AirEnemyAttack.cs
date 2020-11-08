using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemyAttack : MonoBehaviour
{
    public Vector2 attackDir;
    private PlayerCtroller PlayerCtroller; 
    private AirEnemy AirEnemy;
    private Rigidbody2D rb;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        PlayerCtroller = GameObject.Find("Player").GetComponent<PlayerCtroller>();
        AirEnemy = GameObject.Find("AirEnemy").GetComponent<AirEnemy>();
        rb = GetComponent<Rigidbody2D>();

        attackDir = (AirEnemy.PlayerLastPos-transform.position).normalized;
        rb.velocity = attackDir*speed;
    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        if(other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
