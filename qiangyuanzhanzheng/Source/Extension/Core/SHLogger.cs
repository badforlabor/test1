/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 11/22/2014 12:28:24 PM
 * author : Labor
 * purpose : 日志。等发布版本的时候，可以根据情况，过滤掉一些日志！
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    public class SHLogger
    {
        public static void LogDebug(string content)
        {
            Debug.Log("[debug]" + content);
        }
        public static void LogInfo(string content)
        {
            Debug.Log("[info]" + content);
        }
        public static void LogWarning(string content)
        {
            Debug.LogWarning("[warning]" + content);
        }
        public static void LogError(string content)
        {
            Debug.LogError("[error]" + content);
        }
    }
}
