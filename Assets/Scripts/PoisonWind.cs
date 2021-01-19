using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonWind : MonoBehaviour
{
    [Header("Basic Settings")]
    public Vector3 PoisonField;
    public LayerMask WhatIsPlayer;
    public bool poisonDamage;
    public RaycastHit hit;
    public GameObject Player;
    //
    public GameObject tempDamageBlock;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit, Mathf.Infinity))
        {
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            
        }
        else
        {
            Debug.DrawRay(transform.position, transform.right * 1000, Color.white);
            Debug.Log("Did not Hit");
        }

        PoisonField.x = hit.distance/2;
 
        if(Physics.CheckBox(transform.position+transform.right*PoisonField.x,PoisonField,transform.localRotation,WhatIsPlayer))
        {
            Player.GetComponent<PlayerCtroller>().gettingHit();
        }
        //
        // tempDamageBlock.transform.position = new Vector3((hit.point.x+transform.position.x)/2,transform.position.y, transform.position.z);
        // tempDamageBlock.transform.localScale = PoisonField;
    }
    void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        
        Gizmos.matrix = Matrix4x4.TRS(transform.position+transform.right*PoisonField.x,transform.rotation,Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero,2*PoisonField);
    }
}
