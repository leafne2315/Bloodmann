using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavingAndLoad : MonoBehaviour
{
    // Start is called before the first frame update
    private int Hp;
    [SerializeField]private int Scene_index;
    [SerializeField]private int Hp_Max;
    [SerializeField]private int AidKitNum;
    [SerializeField]private int Blood;
    [SerializeField]private float posX;
    [SerializeField]private float posY;
    [SerializeField]private float posZ;
    [SerializeField]private Vector3 SavePos;

    [Header("Game Reset")]
    public int Origin_Hp_Max;
    public int Origin_AidKitNum;
    public int Origin_Blood;
    public Vector3 StartPos;
    private PlayerCtroller playerCtrl;
    private GameManager GM;
    void Awake()
    {
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerCtroller>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    void GetPlayerData()
    {
        Hp = playerCtrl.hp;
        Hp_Max = playerCtrl.hp_Max;
        AidKitNum = playerCtrl.AidKitNum;  

        Scene scene = SceneManager.GetActiveScene();
        Scene_index = scene.buildIndex; 
    }
    void GetPlayerSavePos()
    {

        SavePos = new Vector3(playerCtrl.transform.position.x,playerCtrl.transform.position.y,0.0f);
        posX = SavePos.x;
        posY = SavePos.y;
        posZ = SavePos.z;
    }
    
    public void SavePlayerDetail()
    {
        Hp = playerCtrl.hp;
        Hp_Max = playerCtrl.hp_Max;
        AidKitNum = playerCtrl.AidKitNum;

        PlayerPrefs.SetInt("myHP",Hp);
        PlayerPrefs.SetInt("myHP_Max",Hp_Max);
        PlayerPrefs.SetInt("AidKitNum",AidKitNum);

        print("Save Detail");
    }
    public void SavePlayerSavePos()
    {
        SavePos = new Vector3(playerCtrl.transform.position.x,playerCtrl.transform.position.y,0.0f);
        posX = SavePos.x;
        posY = SavePos.y;
        posZ = SavePos.z;

        PlayerPrefs.SetFloat("posX",posX);
        PlayerPrefs.SetFloat("posY",posY);
        PlayerPrefs.SetFloat("posZ",posZ);

        print("Save Position");
    }
    
    public void LoadPlayerDetail()
    {
        Hp = PlayerPrefs.GetInt("myHP");
        Hp_Max = PlayerPrefs.GetInt("myHP_Max");
        AidKitNum = PlayerPrefs.GetInt("AidKitNum");

        playerCtrl.hp = Hp_Max;
        playerCtrl.hp_Max = Hp_Max;
        playerCtrl.AidKitNum = AidKitNum;

        print("Load Detail");
    }
    public void LoadPlayerSavePos()
    {
        posX = PlayerPrefs.GetFloat("posX");
        posY = PlayerPrefs.GetFloat("posY");
        posZ = PlayerPrefs.GetFloat("posZ");

        SavePos = new Vector3(posX,posY,posZ);
        playerCtrl.transform.position = SavePos;

        print("Load Position");
    }
    public void ResetFile()
    {
        Hp = Origin_Hp_Max;
        Hp_Max = Origin_Hp_Max;
        AidKitNum = Origin_AidKitNum;

        SavePos = StartPos;
        
        posX = SavePos.x;
        posY = SavePos.y;
        posZ = SavePos.z;

        PlayerPrefs.SetInt("myHP",Hp_Max);
        PlayerPrefs.SetInt("Scene_Index",1);
        PlayerPrefs.SetInt("myHP_Max",Hp_Max);
        PlayerPrefs.SetInt("AidKitNum",AidKitNum);

        PlayerPrefs.SetFloat("posX",posX);
        PlayerPrefs.SetFloat("posY",posY);
        PlayerPrefs.SetFloat("posZ",posZ);
        
        PlayerPrefs.Save();
        print("reset data");
    }
}
