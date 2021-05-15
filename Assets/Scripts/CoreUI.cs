using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// public class CoreUI : MonoBehaviour
// {
//     private PlayerCtroller playerCtrScript;
//     public GameObject Player;
//     public Image core1;
//     public Image core2;
//     // public Sprite fullCore;
//     // public Sprite emptyCore;
//     // public Sprite core1;
//     // public Sprite emptyCore1;
//     // Start is called before the first frame update
//     void Start()
//     {
//         playerCtrScript = Player.GetComponent<PlayerCtroller>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // for (int i=0; i<cores.Length; i++)
//         // {
//         //     if(i<playerCtrScript.AttackRemain)
//         //     {
//         //         cores[i].sprite = fullCore;
//         //     }
//         //     else
//         //     {
//         //         cores[i].sprite = emptyCore;
//         //     }
            
//         //     if(i<playerCtrScript.FullRemain)
//         //     {
//         //         cores[i].enabled = true;
//         //     }
//         //     else
//         //     {
//         //         cores[i].enabled = false;
//         //     }
//         // }

//         // if(playerCtrScript.AttackRemain == 2)
//         // {
//         //     core1.enabled = true;
//         //     core2.enabled = true;
//         // }

//         // if(playerCtrScript.AttackRemain == 1)
//         // {
//         //     core1.enabled = true;
//         //     core2.enabled = false;      
//         // }

//         // if(playerCtrScript.AttackRemain == 0)
//         // {
//         //     core1.enabled = false;
//         //     core2.enabled = false;
//         // }
//     }
// }
public class CoreUI : MonoBehaviour
{
    private PlayerCtroller playerCtrScript;
        public GameObject Player;
        public Image[] cores;
        public Sprite fullCore;
        public Sprite FirstCore;
        public Sprite emptyCore;
        public Sprite firstEmptyCore;
        private Vector3 originalScale;
        public GameObject coresObj;
        // Start is called before the first frame update
        void Start()
        {
            playerCtrScript = Player.GetComponent<PlayerCtroller>();
            originalScale = coresObj.transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {
            for (int i=0; i<cores.Length; i++)
            {
                if(i<playerCtrScript.AttackRemain)
                {
                    if(i==0)
                    {
                        cores[i].sprite = FirstCore;
                        cores[i].SetNativeSize();
                        //transform.GetChild(i).transform.localScale = originalScale;
                    }
                    else
                    {
                        cores[i].sprite = fullCore;
                        cores[i].SetNativeSize();
                        //transform.GetChild(i).transform.localScale = originalScale;
                    }
                }
                else
                {
                    if(i==0)
                    {
                        cores[i].sprite = firstEmptyCore;
                        cores[i].SetNativeSize();
                        //transform.GetChild(i).transform.localScale = originalScale;
                    }
                    else
                    {
                        cores[i].sprite = emptyCore;
                        cores[i].SetNativeSize();
                        //transform.GetChild(i).transform.localScale = originalScale;
                    }
                    
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
