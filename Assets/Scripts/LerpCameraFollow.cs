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
    void Awake()
    {
        mainCam = Camera.main;
        PlayerScript = Player.GetComponent<PlayerCtroller>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, Target.position + offset , ref cameraVelocity, SmoothTime);
    }
    void Update()
    {
        if(PlayerScript.facingRight)
        {
            offset = new Vector3(LeftX_Pivot,offset.y,offset.z);
        }
        else
        {
            offset = new Vector3(RightX_Pivot,offset.y,offset.z);
        }

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
