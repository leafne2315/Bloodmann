using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftLauncher : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Lift;
    private LiftController LiftCrtl;
    
    void Awake()
    {
        LiftCrtl = Lift.GetComponent<LiftController>();
    }
   
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Player"))
        {
            LiftCrtl.isActivated = true;
        }        
    }
}
