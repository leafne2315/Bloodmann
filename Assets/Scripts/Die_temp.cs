using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die_temp : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 StartPos;
    public Rigidbody2D rb;
    private PlayerCtroller playerCtrl;
    public Transform GeneratePos;
    public GameObject EnemyPrefab;
    void Start()
    {
        StartPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        //
        playerCtrl = GetComponent<PlayerCtroller>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Sea"))
        {
            transform.position = StartPos;
            rb.velocity = Vector2.zero;
            GetComponent<PlayerCtroller>().currentGas = 100;
            GetComponent<PlayerCtroller>().Out_Of_Gas = false;
        }
        if(other.CompareTag("Enemy"))
        {
            transform.position = StartPos;
            rb.velocity = Vector2.zero;
            GetComponent<PlayerCtroller>().currentGas = 100;
            GetComponent<PlayerCtroller>().Out_Of_Gas = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(playerCtrl.isDash)
        {
            if(other.collider.CompareTag("Enemy"))
            {
                other.collider.transform.position = GeneratePos.position;
                other.collider.GetComponent<EnemyAI>().currentState = EnemyAI.EnemyState.Patrol;
                other.collider.GetComponent<EnemyAI>().AlertValue = 0;
            }
        }
        else
        {        
            if(other.collider.CompareTag("Enemy"))
            {
                transform.position = StartPos;
                rb.velocity = Vector2.zero;
                GetComponent<PlayerCtroller>().currentGas = 100;
                GetComponent<PlayerCtroller>().Out_Of_Gas = false;
            }
        }
    }
    void DestroyEnemy()
    {
        
    }
}
