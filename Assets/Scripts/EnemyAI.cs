using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    private Vector3 newDir;
    private ViewDetect EnemyView;
    public Transform[] PatrolPoint;
    public int PatrolNum;
    public enum EnemyState{Patrol,Alert,Trigger}
    private EnemyState currentState;
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

            if(Vector3.Distance(targetPos,transform.position)<0.5f)
            {
                if(PatrolNum<PatrolPoint.Length-1)
                {
                    PatrolNum++;
                }
                else
                {
                    PatrolNum = 0;
                }
            }
            break;

            case EnemyState.Alert:

            break;

            case EnemyState.Trigger:

            break;

            default:
            break;
        }
    }
    void PatrolDir_change()
    {

    }
}
