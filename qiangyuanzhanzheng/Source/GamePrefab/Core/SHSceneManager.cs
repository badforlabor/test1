/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 11/26/2014 10:16:36 PM
 * author : Labor
 * purpose : 场景切换
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    public enum EScene
    { 
        None,     // 空场景，第一个场景，用来启动一些manager程序用的
        Login,    // 登陆场景，输入账号密码，UI处理等
        Battle,   // 战斗
    }
    public class SHSceneManager : SHSingleton<SHSceneManager>
    {
        EScene mOldScene = EScene.None;
        EScene mCurrentScene = EScene.None;

        AsyncOperation AsyncState = null;
        CallBackVoid mCallback = null;

        public void ChangeScene(EScene scene, CallBackVoid cb)
        {
            if (scene == EScene.None)
            {
                return;
            }

            if (mCurrentScene != scene)
            {
                mOldScene = mCurrentScene;
                mCurrentScene = scene;
                mCallback = cb;

                // 保留一些内容
                GameObject[] persis = GameObject.FindGameObjectsWithTag(SHNames.PersistantTag);
                if (persis != null)
                {
                    foreach (var p in persis)
                    {
                        GameObject.DontDestroyOnLoad(p);
                    }
                }

                SHCoreEvents.Singleton.FireEvent(SHEventType.SCENE_Change_Begin, null);

                // 开始切换场景
                AsyncState = Application.LoadLevelAsync((int)scene);
            }
        }
        public void Update(float delta)
        {
            if (AsyncState != null && AsyncState.isDone)
            {
                SHLogger.Info("[virgin] load scene done. prescene=" + mOldScene + ", now=" + mCurrentScene);
                SHResources.Singleton.OnSceneChanged();
                SHDelegate.Exec(mCallback);

                AsyncState = null;
                SHCoreEvents.Singleton.FireEvent(SHEventType.SCENE_Change_End, null);
            }
        }
    }
}
