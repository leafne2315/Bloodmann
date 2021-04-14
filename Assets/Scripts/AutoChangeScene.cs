using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoChangeScene : MonoBehaviour
{
    // Start is called before the first frame update
    private LevelLoader LvLoader;
    private int SceneIndex;
    public int NextScene;
    void Awake()
    {
        LvLoader = GameObject.Find("LevelLoder").GetComponent<LevelLoader>();
    }
    void Start()
    {
        SceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ChangeScene();
        }
    }
    void ChangeScene()
    {
        StartCoroutine(LvLoader.LoadLevelWithDelay(NextScene,1.5f));
    }
    
}
