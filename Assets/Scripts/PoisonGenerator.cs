using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonGenerator : MonoBehaviour
{
    public Transform GeneratePos;
    public GameObject Poison;
    void Start()
    {
        InvokeRepeating("Launch", 0, 0.8f);
    }

    // Update is called once per frame
    void Launch()
    {
        Instantiate(Poison, GeneratePos.transform.position, Quaternion.identity);
    }
}
