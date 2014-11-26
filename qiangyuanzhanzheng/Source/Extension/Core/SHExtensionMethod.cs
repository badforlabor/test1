/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 11/22/2014 1:44:42 PM
 * author : Labor
 * purpose : C#特性
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    public static class SHExtensionMethod
    {
        public static string GetFullName(this UnityEngine.Object obj)
        {
            return (obj == null ? "none" : obj.ToString());
        }
    }
}
