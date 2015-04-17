/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 11/26/2014 10:26:27 PM
 * author : Labor
 * purpose : 资源管理：资源加载
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    class SHResourceManager
    {
    }
    public class SHResources : SHSingleton<SHResources>
    {
        // 加载
        public T Load<T>(string path) where T : UnityEngine.Object
        { 
            return Resources.Load<T>(path);
        }

        // 实例化
        public T Instance<T>(string path) where T : UnityEngine.Object
        {
            T obj = Load<T>(path);
            if (obj == null)
            {
                return null;
            }
            T ret = GameObject.Instantiate(obj) as T;
            return ret;
        }

        // 清理资源，切场景的时候调用
        public void OnSceneChanged()
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        virtual public void Update(float delta)
        { 
        
        }
    }
}
