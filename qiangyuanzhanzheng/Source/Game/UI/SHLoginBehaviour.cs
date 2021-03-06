﻿/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 12/2/2014 8:18:49 AM
 * author : Labor
 * purpose : 
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    class SHLoginBehaviour : MonoBehaviour
    {
        bool Done = false;
        void OnGUI()
        {
            if (!Done && GUI.Button(new Rect(50, 50, 240, 100), "进入游戏"))
            {
                SHLogger.Debug("进入游戏！");
                SHBattleManager.Singleton.EnterGame();
                Done = true;
            }
        }
    }
}
