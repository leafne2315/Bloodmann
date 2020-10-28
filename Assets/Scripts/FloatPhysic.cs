using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPhysic : MonoBehaviour
{
    float SurfaceHeight;
    float floatingLine;
    float floatSpeed;
    float FlowSpeed;
    float floatingRange;
    float Energy;
    public float Vy_0;
    private float Ek;
    private float Eu = 0;
    public GameObject Water;
    Rigidbody2D rb;
    float BounceDistance;
    bool sinking;
    bool canStartCal = false;
    void Awake()
    {
        SurfaceHeight = Water.transform.GetChild(0).transform.position.y;
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        sinking = false;
        transform.position = new Vector2(transform.position.x,Water.transform.GetChild(0).transform.position.y);
        StartCoroutine(Waiting());
        Ek = 0.5f*Mathf.Pow(Vy_0,2); 
        Energy = Ek;
    }
    void FixedUpdate()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        BounceDistanceCal();
        VelocityCal();
        print(sinking);
    }
    void VelocityCal()
    {

        float Vy = Mathf.Sqrt((2*(Energy-Eu)));
        print((2*(Energy-Eu)));
        //print((2*Energy-Mathf.Pow(BounceDistance,2)));
        print(Vy);
        
        if(sinking&&rb.velocity.y>-0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x,Vy);
            sinking =false;
        }
        else if(!sinking&&rb.velocity.y<0.1f)
        {
            sinking = true;
            rb.velocity = new Vector2(rb.velocity.x,Vy);
        }
        
    }

    void BounceDistanceCal()
    {
        float tempDis = transform.position.y-SurfaceHeight;

        BounceDistance = Mathf.Abs(tempDis);

        Eu = 0.5f*Mathf.Pow(BounceDistance,2);
        print(Eu);
        
        
    }
    IEnumerator Waiting()
    {
        yield return 0;
        canStartCal = true;
    }
}
