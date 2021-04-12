using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource audioSource;
    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(!audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
