using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LerpCameraFollow : MonoBehaviour
{
    public Transform Target;
    public float SmoothTime = 0.01f;
    private Vector3 cameraVelocity = Vector3.zero;
    private Camera mainCam;
    public Vector3 offset;
    public float LeftX_Pivot;
    public float RightX_Pivot;
    private PlayerCtroller PlayerScript;
    public GameObject Player;
    public enum CameraState{Normal,Look}
    public CameraState currentState;
    [Header("View Change")]
	private Vector3 OriginPos;
    private float cameraInput_Y;
    public float speed;
    [Range(0,100)]public float BackSpeed; 
	private bool isMovingCamera;
	public float WatchingAreaY;
    
    void Awake()
    {
        mainCam = Camera.main;
        PlayerScript = Player.GetComponent<PlayerCtroller>();

        currentState = CameraState.Normal;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        switch(currentState)
        {
            case CameraState.Normal:

                CameraMainMove();

                if(isMovingCamera&&PlayerScript.isStill)
				{
					OriginPos = transform.position;
					print(OriginPos);
					currentState = CameraState.Look;
                    PlayerScript.LastState = PlayerScript.currentState;
                    PlayerScript.currentState = PlayerCtroller.PlayerState.Idle;
				}

            break; 

            case CameraState.Look:

            break;

            default:
            break;
        }
        
        //transform.position = Vector3.SmoothDamp(transform.position, Target.position + offset , ref cameraVelocity, SmoothTime);
    }
    void Update()
    {
        cameraInput_Y = Input.GetAxis("PS4-R-Vertical");

		if(Input.GetAxis("PS4-R-Vertical")!=0)
		{
			isMovingCamera = true;
		}
		else
		{
			isMovingCamera = false;
	
    	}

        DoublePivot();
        FollowSpeed_Change();
    }
    void LateUpdate()
    {
        switch(currentState)
        {
            case CameraState.Normal:

            break;

            case CameraState.Look:

                float distToTargetY = Mathf.Abs(transform.position.y-OriginPos.y);
				if(distToTargetY<WatchingAreaY)
				{
					transform.position += new Vector3(0,cameraInput_Y * speed * Time.deltaTime,0);
				}

				if(!isMovingCamera)
				{
					transform.position = Vector3.MoveTowards(transform.position,OriginPos,BackSpeed * Time.deltaTime);

					if(Vector3.Distance(transform.position,OriginPos)<0.05f)
					{
						currentState = CameraState.Normal;
                        PlayerScript.currentState = PlayerScript.LastState;
					}
				}
            break;
        }
    }
    void CameraMainMove()
    {
        float positionX = Mathf.SmoothDamp(transform.position.x,Target.position.x+offset.x,ref cameraVelocity.x,SmoothTime);
        float positionY = Mathf.SmoothDamp(transform.position.y,Target.position.y+offset.y,ref cameraVelocity.y,0.1f);
        transform.position = new Vector3(positionX,positionY,offset.z);
    }
    void DoublePivot()
    {
        if(PlayerScript.facingRight)
        {
            if(PlayerScript.currentState == PlayerCtroller.PlayerState.Attach)
            {
                offset = new Vector3(RightX_Pivot,offset.y,offset.z);
            }
            else
            {
                offset = new Vector3(LeftX_Pivot,offset.y,offset.z);
            }
        }
        else
        {
            if(PlayerScript.currentState == PlayerCtroller.PlayerState.Attach)
            {
                offset = new Vector3(LeftX_Pivot,offset.y,offset.z);
            }
            else
            {
                offset = new Vector3(RightX_Pivot,offset.y,offset.z);
            }
        }
    }
    void FollowSpeed_Change()
    {
        if(PlayerScript.moveInput_X!=0)
        {
            SmoothTime -= 0.5f*Time.deltaTime;
            SmoothTime = Mathf.Clamp(SmoothTime,0.5f,0.8f);
        }
        else
        {
            SmoothTime =0.8f;
        }
    }
}
