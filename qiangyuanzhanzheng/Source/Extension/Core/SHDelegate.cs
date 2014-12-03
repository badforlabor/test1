/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 11/26/2014 10:19:08 PM
 * author : Labor
 * purpose : 定义代理类型，让其通用
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    // 没有参数
    public delegate void CallBackVoid();

    // 一个参数
    public delegate void CallBackInt(int a);
    public delegate void CallBackFloat(float b);

    public class SHDelegate
    {
        public static void Exec(CallBackVoid cb)
        {
            if (cb != null)
            {
                cb();
            }
        }
    }
}
