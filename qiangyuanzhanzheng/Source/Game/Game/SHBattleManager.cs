/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 12/3/2014 7:43:11 AM
 * author : Labor
 * purpose : 管理游戏的
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    class SHBattleManager : SHSingleton<SHBattleManager>
    {
        public void EnterGame()
        {
            SHSceneManager.Singleton.ChangeScene(EScene.Battle, OnEnterGame);
        }
        void OnEnterGame()
        {
            SHLogger.Debug("[Battle] 切换场景完毕，开始加载游戏信息！");

            SHLogger.Info("[Battle] xxx load room.");
            SHResources.Singleton.Instance<UnityEngine.Object>("Rooms/huianchengzhen02");

            SHLogger.Info("[Battle] xxx load UI.");
            GameObject go = new GameObject("UI_QuitGame");
            go.AddComponent<SHQuitBattleBehaviour>();
        }
    }
}
