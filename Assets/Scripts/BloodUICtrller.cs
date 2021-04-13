using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BloodUICtrller : MonoBehaviour
{
    private PlayerCtroller playerCtrScript;
    public GameObject Player;
    private float gainingBlood;
    private float targetBlood;
    public TextMeshProUGUI BloodAmountUI;
    public float gainSpeed;
    void Awake()
    {
        playerCtrScript = Player.GetComponent<PlayerCtroller>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BloodAmountUI.text = playerCtrScript.Blood.ToString();
    }
}
