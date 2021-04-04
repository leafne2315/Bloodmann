using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AidKitCtrller : MonoBehaviour
{
    private PlayerCtroller playerCtrScript;
    public GameObject Player;
    public TextMeshProUGUI aidKitInt;
    // Start is called before the first frame update
    void Start()
    {
        playerCtrScript = Player.GetComponent<PlayerCtroller>();
    }

    // Update is called once per frame
    void Update()
    {
        aidKitInt.text = playerCtrScript.AidKitNum.ToString("00");
    }

    
}
