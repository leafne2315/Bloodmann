using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingCurve : MonoBehaviour
{
    // Start is called before the first frame update
    LineRenderer Lr;
    public float Tveclocity;
    public float angle;
    public int resolution = 10;
    public float g;
    float radianAngle;
    public LayerMask WhatIsObstacle;
    void Awake()
    {
        Lr = GetComponent<LineRenderer>();
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        RenderCurve();
    }
    void RenderCurve()
    {
        Lr.positionCount = resolution;
        Lr.SetPositions(CalculateCurveArray());
    }

    Vector3[] CalculateCurveArray()
    {
        Vector3[] curveArray = new Vector3[resolution];
        //Vector3[] temp;
        radianAngle = Mathf.Deg2Rad*angle;
        float perT = 0.05f;
        for(int i = 0;i<resolution;i++)
        {
            
            curveArray[i] = CalculateCurvePoint(i*perT);

            if(i>0)
            {
                Vector2 rayDir = (curveArray[i]-curveArray[i-1]).normalized;
                float PtoP_Dist = Vector3.Distance(curveArray[i],curveArray[i-1]);
                RaycastHit2D hit = Physics2D.Raycast(curveArray[i-1],rayDir,PtoP_Dist,WhatIsObstacle);
                if(hit)
                {
                    curveArray[i] = hit.point;
                    Lr.positionCount = i;
                    break;
                }
            }
            
        }
        return curveArray;
    } 

    Vector3 CalculateCurvePoint(float pt)
    {
        float x = pt * Tveclocity*Mathf.Cos(radianAngle);
        float y = x * Mathf.Tan(radianAngle)-((g*x*x)/(2*Tveclocity*Tveclocity*Mathf.Cos(radianAngle)*Mathf.Cos(radianAngle)));

        return new Vector3(x,y) + transform.position;
    }
    public void ThwAngleChange(float tempAg)
    {
        angle = tempAg;
    }
}
