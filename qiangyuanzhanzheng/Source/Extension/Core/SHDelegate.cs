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
    // 使用System.Action
    //public delegate void CallBackVoid();

    // 一个参数
    // 使用System.Action<T>
    //public delegate void CallBackInt(int a);
    //public delegate void CallBackFloat(float b);

    public class SHDelegate
    {
        // 默认调完callback之后，将指针清空，要不会导致有引用关系，gc清除不掉
        // 写逻辑的时候，默认调用这个函数！不要调用非ref的，除非特殊情况
        public static void Exec(ref System.Action ac)
        {
            Exec(ac);
            ac = null;
        }
        public static void Exec<T>(ref System.Action<T> ac, T obj)
        {
            Exec(ac, obj);
            ac = null;
        }


        public static void Exec(System.Action ac)
        { 
            if(ac != null)
            {
                ac();
            }
        }
        public static void Exec<T>(System.Action<T> ac, T obj)
        {
            if (ac != null)
            {
                ac(obj);
            }
        }

        void _Test()
        {
            System.Action<object> cb = delegate(object a)
            {

            };
            Exec(cb, new object());
            Exec(ref cb, new object());

            System.Action cb2 = delegate() { };
            Exec(cb2);
            Exec(ref cb2);
        }
    }
}
