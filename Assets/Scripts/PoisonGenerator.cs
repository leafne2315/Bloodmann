using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonGenerator : MonoBehaviour
{
    public Transform GeneratePos;
    public GameObject Poison;
    private GameObject newPoison;
    private bool generateNext;

    void Start()
    {
        InvokeRepeating("Launch",0.5f,3.5f);
    }
    void Update()
    {
    

        
    }
    // Update is called once per frame
    void Launch()
    {
        newPoison = Instantiate(Poison, GeneratePos.transform.position, Quaternion.identity);
    }
   
    
}
