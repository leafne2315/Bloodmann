using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 offset;
    public RectTransform rectTransform;
    public GameObject target;
    void Start()
    {
        
    }
    void Update()
    {
        if(target!=null)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
            rectTransform.position = screenPos + new Vector2(offset.x, offset.y);
        }
        else
        {
            gameObject.SetActive(false);
        }
        

        
        // if (screenPos.x > Screen.width || screenPos.x < 0 || screenPos.y > Screen.height || screenPos.y < 0) 
        //     rectTransform.gameObject.SetActive(false);
        // else 
        //     rectTransform.gameObject.SetActive(true);
    }
}
