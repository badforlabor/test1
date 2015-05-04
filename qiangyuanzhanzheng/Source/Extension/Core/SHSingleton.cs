/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 11/22/2014 11:36:53 AM
 * author : Labor
 * purpose : 
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    // 这个是全局唯一的（即游戏从启动到关闭的整个过程）
    public class SHSingleton<T> where T : class, new()
    {
        static protected T _inst = default(T);
        public static T Singleton
        {
            get
            {
                if (_inst == default(T))
                {
#if UNITY_EDITOR
                    GameObject go = SHUtil.GetGO("_Singleton");
                    go.tag = "PERSISTENT_TAG";
                    GameObject sig = new GameObject(typeof(T).Name);
                    sig.transform.parent = go.transform;
#endif
                    _inst = new T();
                }
                return _inst;
            }
        }
    }

    // 这个事场景内唯一的，即当场景切换的时候，就会销毁了。比如由游戏内切换到游戏外
    public class SHLevelSingleton<T> : MonoBehaviour where T : MonoBehaviour
    { 
        static protected T _inst = null;
        public static T Singleton
        {
            get 
            {
                if (_inst == null)
                {
                    GameObject sig = new GameObject(typeof(T).Name);
#if UNITY_EDITOR
                    GameObject go = SHUtil.GetGO("_LevelSingleton");
                    sig.transform.parent = go.transform;
#endif
                    sig.transform.position = Vector3.zero;
                    _inst = sig.AddComponent<T>();
                    SHLogger.Warning("[single] 创建单例:" + _inst.GetFullName());
                }

                return _inst;
            }
        }
        virtual protected void OnDestroy()
        {
            SHLogger.Warning("[single] 销毁单例:" + _inst.GetFullName());
            _inst = null;
        }
    }

    // 不能用这个，在editor中执行addcomponent的时候，unity就不认识了
    public class SHComponentSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static protected MonoBehaviour _inst = null;
        public static T Singleton
        {
            get { return (T)_inst; }
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
    }
}
