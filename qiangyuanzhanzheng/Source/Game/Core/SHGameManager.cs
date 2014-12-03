/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 11/26/2014 11:33:35 PM
 * author : Labor
 * purpose : 
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    class SHGameManager : SHGameManagerBase
    {
        public SHGameManager()
        {
            _inst = this;
        }
        public static new SHGameManager Singleton
        {
            get { return (SHGameManager)_inst; }
        }

        override protected void Init()
        {
            SHLogger.Info("[virgin] change scene to login.");
            SHSceneManager.Singleton.ChangeScene(EScene.Login, OnToLogin);
        }

        override public void Update(float delta)
        {
            base.Update(delta);
        }

        // 由于不是虚函数，将不会调用此函数！
        string ShowMe()
        {
            return ("my name is 'gm'");
        }

        void OnToLogin()
        {
            GameObject go = new GameObject("UI_Entry");
            go.AddComponent<SHLoginBehaviour>();
        }
        public void QuitBattle()
        {
            SHLogger.Info("[battle] 离开游戏，进入到登陆");
            SHSceneManager.Singleton.ChangeScene(EScene.Login, OnToLogin);
        }
    }
}
