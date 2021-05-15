using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCtrller : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isOpen;
    public bool isOver;
    public float openTime = 5.0f;
    public float Dist;
    private float speed;
    private Rigidbody rb;
    private float timer;
    void Start()
    {
        if(GameData.isDoorOpen)
        {
            transform.position = transform.position + Dist*Vector3.up;
        }

        speed = Dist/openTime;        
    }

    // Update is called once per frame
    void Update()
    {
        if(isOpen&!isOver)
        {
            if(timer<openTime)
            {
                timer+=Time.deltaTime;
                transform.position += Vector3.up*speed*Time.deltaTime;
            }
            else
            {
                timer = 0;
                isOver = true;
                
            }
        }
    }

}
