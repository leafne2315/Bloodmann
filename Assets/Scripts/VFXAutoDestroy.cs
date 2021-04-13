using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXAutoDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    private float Duration = 2;
    void Start()
    {
        Destroy(gameObject,Duration);
    }

    // Update is called once per frame
}
