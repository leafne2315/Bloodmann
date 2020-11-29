using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour {

	public float fallMultiplier = 2.5f;

	Rigidbody rb;
	void Awake()
	{
		rb = GetComponent<Rigidbody>();	
	}
	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	void FixedUpdate()
	{
		if(rb.velocity.y>0&&!Input.GetButton("PS4-x"))
		{
			rb.AddForce(Physics.gravity*5.0f*(fallMultiplier-1),ForceMode.Acceleration);
		}
	}
	void Update ()
	{
		
	}
}
