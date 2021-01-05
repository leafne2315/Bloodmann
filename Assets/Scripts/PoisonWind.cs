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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.right * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.right * 1000, Color.white);
            //Debug.Log("Did not Hit");
        }

        PoisonField.x = hit.distance/2;
 
        if(Physics.CheckBox(new Vector3((hit.point.x+transform.position.x)/2,hit.point.y, hit.point.z),PoisonField,Quaternion.identity,WhatIsPlayer))
        {
            poisonDamage = true;
            Player.GetComponent<PlayerCtroller>().HitByOtherComponent();
        }
        else
        {
            poisonDamage = false;
        }
    }

    void Damage()
    {
       
    }

    void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3((hit.point.x+transform.position.x)/2,hit.point.y, hit.point.z),2*PoisonField);
    }
}
