using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewDetect : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform OriginPos;
    private Vector2 RayDir;
    private Vector2 RayDir_02;
    [SerializeField]private Color RayColor = Color.yellow;
    [SerializeField]private float RayLength = 20;
    public float YellowRange;
    public float RedRange;
    public LayerMask TargetLayer;
    public float DetectAngle;
    [Range(1, 50)]public float accuracy;
    [SerializeField]private float Ray_RotatePerSecond;
    private float currentAngle;
    public LayerMask ignoreYourself;
    public LayerMask ignoreLayer01;
    public bool Warning;
    public bool RedWarning;
    public Vector3 PlayerLastPos;
    public GameObject target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Detecting(DetectAngle,YellowRange,Color.yellow,ref Warning);
        Detecting(DetectAngle,RedRange,Color.red,ref RedWarning);
        currentAngle += Ray_RotatePerSecond*Time.deltaTime;
    }
    public void SwitchDetectMode(string color)
    {   
        switch(color)
        {
            case "Yellow":
                RayLength = YellowRange;
                RayColor = Color.yellow;
            break;

            case "Red":
                RayLength = RedRange;
                RayColor = Color.red;
            break;

            default:
                RayColor = Color.gray;
            break;
        }
    }
    public void Detecting(float angle,float range,Color ray,ref bool trigger)
    {
        float subAngle = angle / accuracy;
        for(int i=0;i<accuracy;i++)
        {
            RayDir = transform.right;
            RayDir = Quaternion.AngleAxis(-DetectAngle/2 + Mathf.Repeat(Ray_RotatePerSecond*Time.time + i*subAngle,angle),Vector3.back)*RayDir;
            RaycastHit2D hit = Physics2D.Raycast(OriginPos.position,RayDir,range, ~(ignoreYourself|ignoreLayer01));
            if(hit)
            {
                if(hit.collider)
                {
                    Debug.DrawLine(OriginPos.position,hit.point,ray);
                }

                if(hit.collider.CompareTag("Player"))
                {
                    trigger = true;
                    PlayerLastPos = hit.collider.transform.position;
                    target = hit.collider.gameObject;
                    break;
                }
                else
                {
                    trigger = false;
                }
                //Debug.Log(hit.collider.name);
            }
            else
            {
                Debug.DrawLine(OriginPos.position,OriginPos.position+new Vector3(RayDir.x,RayDir.y,0)*range,ray);
            }
        }
    }
}
