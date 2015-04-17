/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 4/17/2015 7:14:28 AM
 * author : Labor
 * purpose : 
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    class SHCuteInput : MonoBehaviour
    {
        const int AREA_LEFT = 0;
        const int AREA_RIGHT = 1;
        const int INVALID_FINGER = -1;


        Vector2 OldMousePos = Vector2.zero;
        Vector2 MouseDirection = Vector2.zero;

        // IOS最多支持5个手指
        float[] TouchTime = { 0, 0, 0, 0, 0 };

        public Vector3 ExpectDir = Vector3.zero;

        public float PressedTime = 0;
        public Vector2 PressedScreenPos = Vector2.zero;

        public System.Action<SHCuteInput> OnLockMoveArea = null;
        public System.Action<SHCuteInput> OnMoveOnMoveArea = null;
        public System.Action<SHCuteInput> OnFire = null;

        void Update()
        { 
            /*
             * 按键逻辑思路：
             *      在触摸屏上，分为左右两个区域，当一个区域检测到移动时，那么另一个区域就变成攻击区域
             *          如果检测时移动区域？当移动范围超过10像素之后，视为移动，将自身区域定位移动区域
             *              一旦处于移动区域后，即锁定该区域并一直视其为移动区域，直到玩家松开手指为止
             *          只检测移动区域，剩下的区域就是攻击区域，一旦区域更换后，需要UI提示。
             *          默认左侧区域为移动区域
             *          攻击区域支持点击（click）或者hold住射击（频率暂为0.2秒），不响应滑动（攻击要么点要么hold）
             *      在模拟器上，不分区域，上下左右控制操纵，鼠标点击控制攻击方向
             * *
             */
            float delta = Time.deltaTime;

            if (SHGameManagerBase.Singleton.IsPlayInMobile())
            {
                if (Input.touchCount > 2)
                { 
                    // 多余两个手指操纵的时候，不处理
                    SHStats.Singleton.AddMsg("touch finger count is too many. count=" + Input.touchCount);
                }
                else if (Input.touchCount > 0)
                {
                    int firstHold = 0;
                    bool bLockMove = false;
                    int LockMoveFingerID = INVALID_FINGER;
                    int MoveAreaID = AREA_LEFT; // 默认是左区域，1是右区域
                    foreach (var t in Input.touches)
                    {
                        if (t.phase == TouchPhase.Began)
                        { 
                            // 认为是按下，为攻击区域做准备，准备计算是不是click动作
                            TouchTime[t.fingerId] = 0.001f;
                        }

                        if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                        {
                            if (LockMoveFingerID == t.fingerId)
                            {
                                bLockMove = false;
                                LockMoveFingerID = INVALID_FINGER;
                            }
                            else
                            {
                                // 视为攻击区域的release
                                PressedScreenPos = t.position;
                                if (TouchTime[t.fingerId] < 0.1f)
                                {
                                    SHDelegate.Exec(OnFire, this);
                                }
                                TouchTime[t.fingerId] = 0;
                            }
                        }

                        if (t.phase == TouchPhase.Stationary)
                        {
                            if (LockMoveFingerID == t.fingerId)
                            {
                                // 视为移动区域的hold
                                SHDelegate.Exec(OnLockMoveArea, this);
                            }
                            else
                            {
                                // 视为攻击区域的hold
                                PressedScreenPos = t.position;
                                TouchTime[t.fingerId] += delta;
                                ProcessFire(ref TouchTime[t.fingerId]);
                            }
                        }

                        if (t.phase == TouchPhase.Moved)
                        {
                            if (!bLockMove)
                            {
                                bLockMove = true;
                                LockMoveFingerID = t.fingerId;
                                MoveAreaID = GetClickArea(t.position);
                            }
                            else
                            {
                                // 如果是在移动区域移动，那么通知玩家移动位置
                                if (GetClickArea(t.position) == MoveAreaID)
                                {
                                    SHDelegate.Exec(OnMoveOnMoveArea, this);
                                }
                                else
                                {
                                    // 否则，视为攻击区域的hold
                                    PressedScreenPos = t.position;
                                    TouchTime[t.fingerId] += delta;
                                    ProcessFire(ref TouchTime[t.fingerId]);
                                }
                            }
                        }
                    }

                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    TouchTime[0] = 0;
                }
                if (Input.GetMouseButtonUp(0) || Input.GetMouseButton(0))
                {
                    {
                        PressedScreenPos.x = Input.mousePosition.x;
                        PressedScreenPos.y = Input.mousePosition.y;
                        TouchTime[0] += delta;
                        ProcessFire(ref TouchTime[0]);
                    }
                }
                if (Input.GetKey(KeyCode.W)
                        || Input.GetKey(KeyCode.A)
                        || Input.GetKey(KeyCode.S)
                        || Input.GetKey(KeyCode.D))
                {
                    // 更改角度
                    float x = Input.GetAxis("Horizontal");  // 向右
                    float y = Input.GetAxis("Vertical");    // 向前

                    // 按键按下的表示方向(世界坐标系），比如按下w，d。那么方向为(1,1,0)，转化到u3d中，就是(-1, 0, 1)
                    ExpectDir = new Vector3(x, 0, y).normalized;
                    SHDelegate.Exec(OnMoveOnMoveArea, this);
                }
            }
        }
        void ProcessFire(ref float holdtime)
        {
            if (holdtime > 0.1f)
            {
                holdtime = 0;
                SHDelegate.Exec(OnFire, this);
            }
        }

        int GetClickArea(Vector2 pos)
        {
            return IsInLeftArea(pos) ? AREA_LEFT : AREA_RIGHT;
        }
        bool IsInLeftArea(Vector2 pos)
        {
            return (pos.x < Screen.width / 2);
        }
    }
}
