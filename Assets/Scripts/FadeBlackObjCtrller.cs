using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBlackObjCtrller : MonoBehaviour
{
    public bool fadeOut;
    public bool fadeIn;
    public float fadeSpeed;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            FadeOutObj();
        }

        if(fadeOut)
        {
            print("a");
            Color objectColor = this.GetComponent<MeshRenderer>().material.color;
            float fadeAmount = objectColor.a - (fadeSpeed*Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            this.GetComponent<MeshRenderer>().material.color = objectColor;

            if(objectColor.a <=0)
            {
                fadeOut = false;
            }
        }
    }

    public void FadeInObj()
    {
        fadeIn = true;
    }

    public void FadeOutObj()
    {
        fadeOut = true;
    }
}

