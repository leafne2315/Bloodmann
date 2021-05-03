using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShowRange : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Lights;
    public float showingRange;
    public GameObject Player;
    void Start()
    {
        InvokeRepeating("checkLight",0.0f,0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void checkLight()
    {
        for(int i =0;i<Lights.Length;i++)
        {
            if(Vector3.Distance(Player.transform.position,Lights[i].transform.position)<showingRange)
            {
                Lights[i].gameObject.SetActive(true);
            }
            else
            {
                Lights[i].gameObject.SetActive(false);
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Player.transform.position,showingRange);
    }
}
