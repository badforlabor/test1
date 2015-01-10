﻿/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 12/9/2014 7:31:12 AM
 * author : Labor
 * purpose : 控制移动的，调用Astar的AIPath
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

namespace SHGame
{
    class SHMoveAgent : AIPath
    {
        override protected void Awake () 
        {
            base.Awake();
            gameObject.AddComponent<FunnelModifier>();
        }
        public override void Update()
        {
            if (!canMove) { return; }

            Vector3 dir = CalculateVelocity(GetFeetPosition());

            //Rotate towards targetDirection (filled in by CalculateVelocity)
            RotateTowards(targetDirection);

            if (rvoController != null)
            {
                rvoController.Move(dir);
            }
            else
                if (navController != null)
                {
#if FALSE
			navController.SimpleMove (GetFeetPosition(),dir);
#endif
                }
                else if (controller != null)
                {
                    //controller.SimpleMove(dir);
                    controller.Move(dir * Time.deltaTime);
                }
                else if (rigid != null)
                {
                    rigid.AddForce(dir);
                }
                else
                {
                    transform.Translate(dir * Time.deltaTime, Space.World);
                }
        }

    }
}
