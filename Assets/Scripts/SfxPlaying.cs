using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxPlaying : MonoBehaviour
{
    public void HealingSfx()
    {
        GameObject sfx = Instantiate(Resources.Load("SoundPrefab/PlayerAfterHealing") as GameObject, transform.position, Quaternion.identity);
    }
}
