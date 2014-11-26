/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 11/22/2014 11:45:35 PM
 * author : Labor
 * purpose : 
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    public class SHFPS : MonoBehaviour
    {
        int fps = 0;
        float Interval = 1;

        int frame = 0;
        float time = 0;

        void Start()
        {
            Application.targetFrameRate = 100;
            QualitySettings.vSyncCount = 0;
        }

        void Update()
        {
            time -= Time.deltaTime;
            frame++;

            if (time <= 0)
            {
                fps = frame;
                frame = 0;
                time = Interval;
            }
        }
        void OnGUI()
        {
            Color old = GUI.color;
            if (fps >= 25)
            {
                GUI.color = Color.green;
            }
            else
            {
                GUI.color = Color.red;
            }

            Rect area = new Rect(Screen.width - 100, 10, Screen.width, 20);
            GUI.Label(area, "FPS:" + fps);

            GUI.color = old;

            area.yMin += 20;
            area.yMax += 20;
            GUI.Label(area, "limit:" + Application.targetFrameRate);
        }
    }
}
