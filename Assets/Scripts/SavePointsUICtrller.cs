using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SavePointsUICtrller : MonoBehaviour
{
    public GameObject restoreHPText;
    public GameObject addHPText;
    public Image restoreHPTextImage;
    public Image addHPTextImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (EventSystem.current.currentSelectedGameObject != null)
        // {
        //     Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        // }

        if(EventSystem.current.currentSelectedGameObject == restoreHPText)
        {
            restoreHPTextImage.enabled = true;
        }
        else
        {
            restoreHPTextImage.enabled = false;
        }

        if(EventSystem.current.currentSelectedGameObject == addHPText)
        {
            addHPTextImage.enabled = true;
        }
        else
        {
            addHPTextImage.enabled = false;
        }
    }
    public void RestoreHP()
    {
        
    }

    public void AddHP()
    {
        
    }
}
