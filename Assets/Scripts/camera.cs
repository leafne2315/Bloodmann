using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour 
{
	private Camera thisCamera;
	public Transform player;
	private Transform target;
	public BoxCollider2D limitBox;
	public float AreaX;
	public float AreaY; 
	public float DistX;
	public float DistY;
	private float top;
	private float btm;
	private float left;
	private float right;
	private float cameraInput_Y;
	public float speed;
	[Range(0,100)]public float BackSpeed; 
	private bool isMovingCamera;
	public float WatchingAreaY;
	public enum CameraState{Normal,Look}
	public CameraState currentState;
	private Vector3 OriginPos;
	// Use this for initialization
	void Start () 
	{
		target = player;
		thisCamera = Camera.main;
		currentState = CameraState.Normal;
		//getLimitDetail();
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
		
	}
	void LateUpdate () 
	{
		switch(currentState)
		{
			case CameraState.Normal:

				CameraFollowing();

				if(isMovingCamera)
				{
					OriginPos = transform.position;
					print(OriginPos);
					currentState = CameraState.Look;
				}

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
					}
				}

			break;

			default:
			break;
		}

		//transform.position = new Vector3(Mathf.Clamp(transform.position.x,left +100,right-100),Mathf.Clamp(transform.position.y,btm+100,top-100),transform.position.z);
	}
	void CameraFollowing()
	{
		if(target.position.y-transform.position.y >AreaY)
		{
			transform.position = new Vector3(transform.position.x,target.position.y-AreaY,transform.position.z);			
		}
		if(target.position.y-transform.position.y <-AreaY)
		{
			transform.position = new Vector3(transform.position.x,target.position.y+AreaY,transform.position.z);
		}
		if(target.position.x-transform.position.x >AreaX)
		{
			transform.position = new Vector3(target.position.x-AreaX,transform.position.y,transform.position.z);
		}
		if(target.position.x-transform.position.x <-AreaX)
		{
			transform.position = new Vector3(target.position.x+AreaX,transform.position.y,transform.position.z);
		}
	}

	void getLimitDetail()
	{
		top = limitBox.offset.y + (limitBox.size.y / 2f); 
    	btm = limitBox.offset.y - (limitBox.size.y / 2f);
    	left = limitBox.offset.x - (limitBox.size.x / 2f);
    	right = limitBox.offset.x + (limitBox.size.x /2f);
	}

	private void OnDrawGizmos()
	{
		Vector3 movingArea = new Vector3(2*AreaX,2*AreaY,0.0f);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(transform.position,movingArea);

	}
		
}