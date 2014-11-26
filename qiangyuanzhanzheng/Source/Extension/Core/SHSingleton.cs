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
    public class SHSingleton<T> where T : class, new()
    {
        static protected T _inst = default(T);
        public static T Singleton
        {
            get
            {
                if (_inst == default(T))
                {
                    _inst = new T();
                }
                return _inst;
            }
        }
    }
}
