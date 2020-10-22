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
	// Use this for initialization
	void Start () 
	{
		target = player;
		thisCamera = Camera.main;
		//getLimitDetail();
	}
	void Update()
	{

	}
	// Update is called once per frame
	void LateUpdate () 
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

		
		//transform.position = new Vector3(Mathf.Clamp(transform.position.x,left +100,right-100),Mathf.Clamp(transform.position.y,btm+100,top-100),transform.position.z);
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