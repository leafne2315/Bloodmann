using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextMeshProTyper : MonoBehaviour
{
    TextMeshProUGUI textMesh;
    public string[] textCharacter;
    public bool isActive;
    public float time;
    private float timer;
    int textCharacterCount;
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textCharacter = new string[textMesh.text.Length];
        for(int i = 0; i < textMesh.text.Length; i++)
        {
            textCharacter[i] = textMesh.text.Substring(i, 1);
        }
        textMesh.text = "";
        textCharacterCount = 0;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            if(textCharacterCount < textCharacter.Length)
            {
                timer += Time.deltaTime;
                if(timer >= time)
                {
                    textMesh.text += textCharacter[textCharacterCount];
                    textCharacterCount++;
                    timer = 0;
                }
            }

            if(textCharacterCount == textCharacter.Length)
            {
                if(transform.childCount >0)
                {
                    transform.GetChild(0).GetComponent<TextMeshProTyper>().isActive = true;
                    textCharacterCount ++;
                }
            }
        }
    }
}
