/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 1/12/2016 8:17:03 AM
 * author : Labor
 * purpose : 控制摄像机的
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    public class camera_controller : MonoBehaviour
    {
        public GameObject Target = null;

        void Update()
        { 
            //
            if (Target != null)
            {
                Vector3 pos = Target.transform.position;
                pos.z = gameObject.transform.position.z;
                gameObject.transform.position = pos;
            }
        }
    }
}
