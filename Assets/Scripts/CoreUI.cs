using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CoreUI : MonoBehaviour
{
    private PlayerCtroller playerCtrScript;
    public GameObject Player;
    public Image[] cores;
    public Sprite fullCore;
    public Sprite emptyCore;
    // Start is called before the first frame update
    void Start()
    {
        playerCtrScript = Player.GetComponent<PlayerCtroller>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0; i<cores.Length; i++)
        {
            if(i<playerCtrScript.AttackRemain)
            {
                cores[i].sprite = fullCore;
            }
            else
            {
                cores[i].sprite = emptyCore;
            }
            
            if(i<playerCtrScript.FullRemain)
            {
                cores[i].enabled = true;
            }
            else
            {
                cores[i].enabled = false;
            }
        }
    }
}
