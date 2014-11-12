/****************************************************************************
Copyright (c) 2013-2014,Dalian-LingYOU tech.
 This is not a free-ware .DO NOT use it without any authorization.
 * 
 * date : 11/12/2014 11:46:13 AM
 * author : Administrator
 * purpose : 
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MLGame
{
    public class mono1 : MonoBehaviour
    {
        public int a = 0;

        void OnGUI()
        {
            Color old = GUI.color;
            GUI.color = Color.red;
            Rect area = new Rect(10, 100, Screen.width, 20);
            GUI.Label(area, "mono1");
            GUI.color = old;
        }
    }
}
