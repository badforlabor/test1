/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 11/22/2014 11:38:59 AM
 * author : Labor
 * purpose : 
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    public class SHGameManagerBase : SHSingleton<SHGameManagerBase>
    {
        public RuntimePlatform Platform = RuntimePlatform.WindowsEditor;

        public bool IsPlayInMobile()
        {
            return (!Application.isEditor && (Platform == RuntimePlatform.Android || Platform == RuntimePlatform.IPhonePlayer));
        }
        
        // 把逻辑代码转到Game工程中。
        void ReInit()
        {
            SHLogger.Debug("[game-manager] before reinit, inst=" + SHGameManagerBase.Singleton.GetFullName()
                    + ", showme=" + SHGameManagerBase.Singleton.ShowMe());
            
            System.Activator.CreateInstance("Game", "SHGame.SHGameManager");
            SHGameManagerBase.Singleton.Init();

            SHLogger.Debug("[game-manager] after reinit, inst=" + SHGameManagerBase.Singleton.GetFullName()
                    + ", showme=" + SHGameManagerBase.Singleton.ShowMe());
        }

        public void Awake()
        {
            
        }

        // Use this for initialization
        public void Start()
        {
            SHLogger.Info("[virgin] game manager start.");

            SHLogger.Info("[virgin] begin init.");

            SHLogger.Info("[virgin] end init.");

            ReInit();

        }
        virtual protected void Init()
        {

        }

        // Update is called once per frame
        virtual public void Update(float delta)
        {
            SHStats.Singleton.Update(delta);
            SHSceneManager.Singleton.Update(delta);
        }

        virtual public void FixedUpdate(float delta)
        {

        }

        virtual public void OnGUI()
        {
            SHStats.Singleton.OnGUI();
        }

        virtual public void OnDestroy()
        {

        }
        string ShowMe()
        {
            return ("my name is 'base'");
        }
    }
}
