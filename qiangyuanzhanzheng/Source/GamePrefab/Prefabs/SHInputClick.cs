/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 4/12/2015 6:31:28 PM
 * author : Labor
 * purpose : 点击相关输入，进行施法的
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    // 处理点击的
    public class SHInputClick : MonoBehaviour
    {
        // 返回按下时的位置，时间
        public float PressedTime = 0;
        public Vector2 PressedScreenPos = Vector2.zero;

        System.Action<SHInputClick> mCallback = null;
        public static void SpawnOne(GameObject go, System.Action<SHInputClick> cb)
        {
            SHInputClick me = go.AddComponent<SHInputClick>();
            me.SetCallback(cb);
        }
        public void SetCallback(System.Action<SHInputClick> cb)
        {
            mCallback = cb;
        }


        void Update()
        {
            bool pressed = false;
            if (Input.GetKeyDown(KeyCode.J))
            {
                pressed = true;
                PressedScreenPos = Vector2.zero;
            }
            if (SHGameManagerBase.Singleton.IsPlayInMobile())
            {
                if (Input.touchCount > 0)
                {
                    foreach (var t in Input.touches)
                    {
                        if (t.phase == TouchPhase.Began)
                        {
                            if (PressedScreenPos.x * 2 > Screen.width)
                            {
                                pressed = true;
                                PressedScreenPos = t.position;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //if (Input.mousePosition.x * 2 > Screen.width)
                    {
                        pressed = true;
                        PressedScreenPos.x = Input.mousePosition.x;
                        PressedScreenPos.y = Input.mousePosition.y;
                    }
                }
            }

            if (pressed)
            {
                PressedTime = Time.deltaTime;
                SHDelegate.Exec(mCallback, this);
            }
        }
    }
}
