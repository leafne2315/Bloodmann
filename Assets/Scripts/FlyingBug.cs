using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBug : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Circle Dance")]
    [SerializeField]private float DanceSpeed ;
    private Vector3 DanceDir;
    public float CycleTime;
    public float radius;
    public Transform CenterPos;
    private Vector3 CentDir;
    private Vector3 currentDir;
    private Vector3 currentVel;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentDir = Vector3.right;
        radius = Vector3.Distance(transform.position,CenterPos.position);
        
        DanceSpeed = radius*2*Mathf.PI/CycleTime;
        currentVel = currentDir*DanceSpeed;
        print(currentVel);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = currentVel;
        
        CentDir = (CenterPos.position-transform.position);
        currentVel = 2*Mathf.PI/CycleTime*CentDir;
        print(currentVel);
        
    }
}
