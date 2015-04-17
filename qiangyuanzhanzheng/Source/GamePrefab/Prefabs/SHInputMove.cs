/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 4/12/2015 6:05:35 PM
 * author : Labor
 * purpose : 移动相关输入，键盘、鼠标、触摸屏，接受移动、处理、发消息
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    //处理移动的
    public class SHInputMove : MonoBehaviour
    {
        Vector2 OldMousePos = Vector2.zero;
        Vector2 MouseDirection = Vector2.zero;

        // IOS最多支持5个手指
        Touch[] OldTouchs = new Touch[5];
        int OldFinger = -1;

        public Vector3 ExpectDir = Vector3.zero;
        System.Action<SHInputMove> mCallback = null;

        public static void SpawnOne(GameObject go, System.Action<SHInputMove> cb)
        {
            SHInputMove me = go.AddComponent<SHInputMove>();
            me.SetCallback(cb);
        }
        public void SetCallback(System.Action<SHInputMove> cb)
        {
            mCallback = cb;
        }

        void Update()
        {
            Vector3 expectDir = Vector3.zero;

            if (Input.GetKey(KeyCode.W)
                    || Input.GetKey(KeyCode.A)
                    || Input.GetKey(KeyCode.S)
                    || Input.GetKey(KeyCode.D))
            {
                // 更改角度
                float x = Input.GetAxis("Horizontal");  // 向右
                float y = Input.GetAxis("Vertical");    // 向前

                // 按键按下的表示方向(世界坐标系），比如按下w，d。那么方向为(1,1,0)，转化到u3d中，就是(-1, 0, 1)
                expectDir = new Vector3(x, 0, y).normalized;

            }
            else
            {
                Vector2 deltaPos = Vector2.zero;
                // 处理鼠标移动或者手机上的屏幕移动
                if (SHGameManagerBase.Singleton.IsPlayInMobile())
                {
                    // 触控的
                    if (Input.touchCount > 0)
                    {
                        // 优先相应上一次相应过的手指
                        if (OldFinger >= 0)
                        {
                            foreach (var t in Input.touches)
                            {
                                if (t.fingerId == OldFinger)
                                {
                                    if (t.phase == TouchPhase.Moved)
                                    {
                                        deltaPos = t.position - OldTouchs[t.fingerId].position;
                                        if (deltaPos.magnitude > 0)
                                        {
                                            OldFinger = t.fingerId;
                                        }
                                    }
                                    else if (t.phase == TouchPhase.Stationary)
                                    {

                                    }
                                    else
                                    {
                                        OldFinger = -1;
                                    }
                                }
                                break;
                            }
                        }
                        // 如果没有旧的，那么就找一个新的
                        if (OldFinger == -1)
                        {
                            foreach (var t in Input.touches)
                            {
                                int oldi = t.fingerId;
                                if (t.phase == TouchPhase.Moved)
                                {
                                    deltaPos = t.position - OldTouchs[oldi].position;
                                    if (deltaPos.magnitude > 0)
                                    {
                                        OldFinger = t.fingerId;
                                        break;
                                    }
                                }
                                else
                                {
                                    OldTouchs[oldi] = t;
                                }
                            }
                        }

                        if (deltaPos.magnitude > 0)
                        {
                            MouseDirection = deltaPos;
                        }
                    }
                    else
                    {
                        MouseDirection = Vector2.zero;
                    }
                }
                else if(false)
                {
                    // 鼠标的
                    if (Input.GetMouseButton(0))
                    {
                        deltaPos.x = Input.mousePosition.x - OldMousePos.x;
                        deltaPos.y = Input.mousePosition.y - OldMousePos.y;
                        if (deltaPos.magnitude > 0)
                        {
                            MouseDirection = deltaPos;
                        }
                    }
                    else
                    {
                        MouseDirection = Vector2.zero;
                    }
                    OldMousePos = Input.mousePosition;
                }
                if (MouseDirection.magnitude > 0)
                {
                    expectDir = new Vector3(MouseDirection.x, 0, MouseDirection.y).normalized;
                }
            }
            ExpectDir = expectDir;

            if (ExpectDir.sqrMagnitude > 0)
            {
                SHDelegate.Exec(mCallback, this);                
            }
        }
    }

}
