using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPhysic : MonoBehaviour
{
    [SerializeField]float SurfaceHeight;
    [SerializeField]float OriginHeight;
    public float noiseScale;
    float floatSpeed;
    public GameObject Water;
    Rigidbody rb;
    private float GForce;
    private float Buoyancy;
    public float FloatParameter;
    [SerializeField]private float RisistForce;
    public float WaterRisistForce;
    public float MixRisistForce;
    [Range(0.0f,1.0f)]public float density;
    [SerializeField]private float OwnSizeHeight;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Water = GameObject.Find("Water");
        SurfaceHeight = Water.transform.GetChild(0).transform.position.y;
        OriginHeight = SurfaceHeight;
        rb = GetComponent<Rigidbody>();
        OwnSizeHeight = GetComponent<Collider>().bounds.size.y;
    }
    void Start()
    {
        GForce = OwnSizeHeight*density*FloatParameter;
    }
    void FixedUpdate()
    {
        rb.AddForce(Vector2.down*GForce);
        rb.AddForce(Vector2.up*Buoyancy);
    }
    void Update()
    {
        BuoyancyChange();
        AddRisistance();
        // print("GForce"+GForce);
        // print("Buoyancy"+Buoyancy);
        Wave();
    }
    
    void BuoyancyChange()
    {
        float SinkDist = SurfaceHeight-(transform.position.y-OwnSizeHeight/2);
        
        Buoyancy = Mathf.Clamp(SinkDist,0.0f,OwnSizeHeight)*FloatParameter;
        
    }
    void AddRisistance()
    {
        float SinkDist = SurfaceHeight-(transform.position.y-OwnSizeHeight/2);

        if(SinkDist>0)
        {
            if(rb.velocity.y<0)
            {
                rb.AddForce(Vector2.up*RisistForce);
            }
            else
            {
                rb.AddForce(Vector2.down*RisistForce);
            }
        }

        if(SinkDist<OwnSizeHeight && SinkDist>0)
        {
            RisistForce = WaterRisistForce*(SinkDist/OwnSizeHeight);
        }
        else
        {
            RisistForce = WaterRisistForce;
        }
        
    }
    void Wave()
    {
        SurfaceHeight = OriginHeight + noiseScale * (Mathf.PerlinNoise(Time.time,0.0f)-0.5f);
    }
}
