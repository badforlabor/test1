﻿/****************************************************************************
Copyright (c) 2013-2014,Dalian-LingYOU tech.
 This is not a free-ware .DO NOT use it without any authorization.
 * 
 * date : 11/11/2014 5:54:05 PM
 * author : Administrator
 * purpose : 
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MLGame
{
    class Utils_Script
    {
        public static int Add(int a, int b)
        {
            //GameObject go = Resources.Load<GameObject>("mono1");
            //GameObject.Instantiate(go);

            //return a + 1 + b * 2;
            return MLGame.Utils.Add3(a, b);
        }
    }
}
