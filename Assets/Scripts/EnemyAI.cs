using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    private Vector3 newDir;
    private ViewDetect EnemyView;
    public Transform[] PatrolPoint;
    public int PatrolNum;
    public enum EnemyState{Patrol,Scan,Alert,Trigger}
    private EnemyState currentState;
    //public Vector3 MoveDir;
    private float ScanTimer;
    public float ScanTime;
    public float ScanAngle;
    public float RotatePersecond;
    //Alert
    [SerializeField]private float AlertValue;
    public float AlertIncreasePS;  
    public float AlertDecreasePS;
    //Trigger
    private float DangerValue;
    public float DangerIncreasePS;
    public float DangerDecreasePs; 
    //UI
    public Image AwareUI;
    void Start()
    {
        EnemyView = GetComponent<ViewDetect>();
        currentState = EnemyState.Patrol;
        PatrolNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case EnemyState.Patrol:
            
                transform.position += transform.right*speed*Time.deltaTime;

                Vector3 targetPos = PatrolPoint[PatrolNum].position;
                transform.right = Vector3.Lerp(transform.right,(targetPos-transform.position).normalized,0.1f);

                if(Vector3.Distance(targetPos,transform.position)<0.2f)
                {
                    if(PatrolNum<PatrolPoint.Length-1)
                    {
                        PatrolNum++;
                    }
                    else
                    {
                        PatrolNum = 0;
                    }
                    //currentState = EnemyState.Scan;
                }

                if(EnemyView.Warning)
                {
                    if(AlertValue<100)
                        AlertValue += AlertIncreasePS*Time.deltaTime;
                    else
                    {
                        AlertValue = 100;
                        currentState = EnemyState.Alert;
                        EnemyView.SwitchDetectMode("Red");
                    }
                }
                else
                {
                    if(AlertValue>0)
                        AlertValue -= AlertDecreasePS*Time.deltaTime;
                    else
                    {
                        AlertValue = 0;
                    } 
                }

            break;

            case EnemyState.Scan:

                ScanTimer+=Time.deltaTime;
                
                transform.right = Quaternion.AngleAxis(RotatePersecond*Time.deltaTime,Vector3.back)*transform.right;
                
            break;

            case EnemyState.Alert:

                transform.position = Vector3.MoveTowards(transform.position,EnemyView.PlayerLastPos,speed*Time.deltaTime);
                if(Vector3.Distance(EnemyView.PlayerLastPos,transform.position)>0.1f)
                    transform.right = (EnemyView.PlayerLastPos-transform.position).normalized;

                AlertValue -= DangerDecreasePs*Time.deltaTime;
                speed = 10;

                if(EnemyView.Warning)
                {
                    //currentState = EnemyState.Trigger;
                    AlertValue = 100;
 
                }

                if(AlertValue<=0)
                {
                    AlertValue = 0;
                    currentState = EnemyState.Patrol;
                    EnemyView.SwitchDetectMode("Yellow");
                    speed = 5; 
                }
                

            break;

            case EnemyState.Trigger:
                print("KiLLLLLLLLLLLL!!!!");
            break;

            default:
            break;
        }
        AwareUI.fillAmount = AlertValue/100;
    }
    void PatrolDir_change()
    {

    }
}
