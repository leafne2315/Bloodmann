﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshProStarter : MonoBehaviour
{
    public GameObject textMeshProTyper;
    private TextMeshProTyper textMeshProTyperScript;
    // Start is called before the first frame update
    void Start()
    {
        textMeshProTyperScript = textMeshProTyper.GetComponent<TextMeshProTyper>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            textMeshProTyperScript.isActive = true;
        }
    }
    public void ActivateType()
    {
        textMeshProTyperScript.isActive = true;
    }
}
