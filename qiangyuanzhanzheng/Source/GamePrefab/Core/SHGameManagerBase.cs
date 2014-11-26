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

        public void Awake()
        {
            
        }

        // Use this for initialization
        public void Start()
        {

        }

        // Update is called once per frame
        public void Update(float delta)
        {
            SHStats.Singleton.Tick(delta);
        }

        public void FixedUpdate(float delta)
        {

        }

        public void OnGUI()
        {
            SHStats.Singleton.OnGUI();
        }

        public void OnDestroy()
        {

        }
    }
}
