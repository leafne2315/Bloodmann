using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Vector3 Dir ;
    public float gravity = 10.0f;
    public float Speed = 15.0f;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ZA_Gravity();
        
    }
    public void getDir(Vector3 throwDir)
    {
        Dir = throwDir;
    }
    void ZA_Gravity()
    {
        rb.AddForce(Vector2.down*gravity);
    }
}
