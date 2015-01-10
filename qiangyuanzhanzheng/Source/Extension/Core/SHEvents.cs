/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 12/1/2014 11:18:07 PM
 * author : Labor
 * purpose : extension和gameprefab中派发的消息
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    // 核心的事件
    public enum SHEventType
    {
        None,

        SCENE_Change_Begin,
        SCENE_Change_End,

    }

    public class SHEventArgs
    {

    }


    public class SHCoreEvents : SHSingleton<SHDispatcher<SHEventType, SHEventArgs>>
    { 
    
    }

    class TestEvents
    {
        void Test()
        {
            SHCoreEvents.Singleton.RegisterEvent(SHEventType.None, DoTest);
        }
        void DoTest(SHEventArgs args)
        {
            SHCoreEvents.Singleton.UnregisterEvent(SHEventType.None, DoTest);
        }
    }

}
