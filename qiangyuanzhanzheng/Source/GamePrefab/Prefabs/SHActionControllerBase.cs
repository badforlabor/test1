/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 11/22/2014 11:45:58 AM
 * author : Labor
 * purpose : 
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    public class SHActionControllerBase : MonoBehaviour
    {
        CharacterController cc = null;

        // Use this for initialization
        void Start()
        {
            SHLogger.Debug("cc=" + cc.GetFullName());
            cc = gameObject.GetComponent<CharacterController>();
            //SHLogger.Debug("cc=" + (cc == null ? "none" : cc.name));
            SHLogger.Debug("cc=" + cc.GetFullName());
        }

        public float RunSpeed = 7.0f;    // 7m/s
        public int RotateSpeed = 720;    // 720度/s
        public float QuaternionSpeed = 3.0f;

        public float CameraYOffset = 0;
        public float CameraDistance = 20;
        public Vector3 CameraDirection = new Vector3(0, 1, -1);
        public Vector3 CameraRotation = new Vector3(45, 0, 0);

        Vector2 OldMousePos = Vector2.zero;
        Vector2 MouseDirection = Vector2.zero;

        // IOS最多支持5个手指
        Touch[] OldTouchs = new Touch[5];
        int OldFinger = -1;

        // Update is called once per frame
        void Update()
        {

            float delta = Time.deltaTime;

            if (cc == null)
            {
                return;
            }

            Vector3 expectDir = Vector3.zero;

            if (Input.GetKeyUp(KeyCode.W))
            {
                SHStats.Singleton.AddMsg("keyup");
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
                                if(t.fingerId == OldFinger)
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
                else
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

            if (expectDir.magnitude > 0)
            {
                // 开始让角色转向按键对应的朝向
                // 如果角度近似，那么直接设置！
                if (Vector3.Angle(expectDir, transform.forward) < (QuaternionSpeed * delta))
                {
                    transform.forward = expectDir;

                }
                else
                {
                    //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(expectDir), Time.deltaTime * QuaternionSpeed);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(expectDir), Time.deltaTime * RotateSpeed);
                }

                // 更改位置
                cc.Move(transform.forward * Time.deltaTime * RunSpeed);


            }


            // 设置摄像机的位置
            Ray ray = new Ray(transform.position + Vector3.up * CameraYOffset, CameraDirection);
            Camera.main.transform.position = ray.GetPoint(CameraDistance);
            Camera.main.transform.rotation = Quaternion.Euler(CameraRotation);
        }
        void OnGUI()
        {
            Rect area = new Rect(10, 10, Screen.width, 20);
            GUI.Label(area, "SH-horizontal:" + Input.GetAxis("Horizontal"));

            area.yMin += 20;
            area.yMax += 20;
            GUI.Label(area, "vertical:" + Input.GetAxis("Vertical"));

            area.yMin += 20;
            area.yMax += 20;
            GUI.Label(area, "quaternion:" + gameObject.transform.rotation);

            area.yMin += 20;
            area.yMax += 20;
            GUI.Label(area, "euler:" + gameObject.transform.eulerAngles);

            area.yMin += 20;
            area.yMax += 20;
            GUI.Label(area, "forward:" + gameObject.transform.forward);

            area.yMin += 20;
            area.yMax += 20;
            GUI.Label(area, "right:" + gameObject.transform.right);


            area.yMin += 20;
            area.yMax += 20;
            GUI.Label(area, "mouse pos:" + Input.mousePosition);

            if (SHGameManagerBase.Singleton.IsPlayInMobile())
            {
                // 触控的
                if (Input.touchCount > 0)
                {
                    for (int i = 0; i < Input.touchCount; i++ )
                    {
                        Touch t = Input.touches[i];
                        area.yMin += 20;
                        area.yMax += 20;
                        GUI.Label(area, "touch[" + i + "], fingerid=" + t.fingerId + "pos=" + t.position + ", phase=" + t.phase);
                    }
                }
                else
                {
                    area.yMin += 20;
                    area.yMax += 20;
                    GUI.Label(area, "left mouse down:" + Input.GetMouseButtonDown(0));
                }
            }
            else
            {
                // 鼠标的
                area.yMin += 20;
                area.yMax += 20;
                GUI.Label(area, "left mouse down:" + Input.GetMouseButtonDown(0));
                area.yMin += 20;
                area.yMax += 20;
                GUI.Label(area, "left mouse up:" + Input.GetMouseButtonUp(0));
                area.yMin += 20;
                area.yMax += 20;
                GUI.Label(area, "left mouse down or up:" + Input.GetMouseButton(0));
            }
            area.yMin += 20;
            area.yMax += 20;
            GUI.Label(area, "move dir:" + MouseDirection.normalized);

            
        }
    }
}
