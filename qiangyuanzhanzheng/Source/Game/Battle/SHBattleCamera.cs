/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 4/16/2015 7:44:40 AM
 * author : Labor
 * purpose : 游戏内的主摄像机
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    public class SHBattleCamera : MonoBehaviour
    {
        static protected SHBattleCamera _inst = null;
        public static SHBattleCamera Singleton
        {
            get { return _inst; }
        }
        virtual protected void Awake()
        {
            if (_inst != null)
            {
                SHLogger.Error("[sys] xxxx 单例重复啦！" + _inst.GetFullName());
            }
            _inst = this;
            SHLogger.Warning("[single] awake 获得单例:" + _inst.GetFullName());
        }
        virtual protected void OnDestroy()
        {
            SHLogger.Warning("[single] 销毁单例:" + _inst.GetFullName());
            _inst = null;
        }     

        public float CameraYOffset = 0;
        public float CameraDistance = 20;
        public Vector3 CameraDirection = new Vector3(0, 1, -1);
        public Vector3 CameraRotation = new Vector3(45, 0, 0);

        public GameObject goLookAt = null;
        public void LookAt(GameObject go)
        {
            goLookAt = go;
        }
        void FixedUpdate()
        {
            if (goLookAt == null)
            {
                return;
            }


            Ray ray = new Ray(goLookAt.transform.position + Vector3.up * CameraYOffset, CameraDirection);
            Camera.main.transform.position = ray.GetPoint(CameraDistance);
            Camera.main.transform.rotation = Quaternion.Euler(CameraRotation);   
        }
    }
}
