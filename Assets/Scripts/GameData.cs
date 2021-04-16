﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData 
{
    public static bool DeadOnce;
    public static bool CanFly ;
    public static bool CanDash ;
    public static bool CanRoll ;
    public static bool CanRecover = true;
    public static bool FirstBlood;
    public static bool CanAttack = true;

    public static void ResetBool()
    {
        DeadOnce = false;
        CanFly = false;
        CanDash = false;
        CanRoll = false;
        FirstBlood = false;

    }
}