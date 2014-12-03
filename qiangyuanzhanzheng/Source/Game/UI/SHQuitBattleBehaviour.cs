/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 12/3/2014 7:51:47 AM
 * author : Labor
 * purpose : 退出游戏界面
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    class SHQuitBattleBehaviour : MonoBehaviour
    {
        void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 30), "退出游戏"))
            {
                SHLogger.Debug("退出游戏！");
                SHGameManager.Singleton.QuitBattle();
            }
        }
    }
}
