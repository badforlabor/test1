/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 11/22/2014 1:48:17 PM
 * author : Labor
 * purpose : 
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    public static class SHUtil
    {
        public static GameObject GetGO(string name)
        {
            GameObject go = GameObject.Find(name);
            if (go == null)
            {
                go = new GameObject(name);
            }
            return go;
        }
    }
}
