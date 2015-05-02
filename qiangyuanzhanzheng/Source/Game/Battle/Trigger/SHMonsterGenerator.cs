/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 4/19/2015 7:38:24 AM
 * author : Labor
 * purpose : 定时生成怪物的
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    public class SHMonsterGenerator : MonoBehaviour
    {
        public float Interval = 1;
        public float timer = 1;

        void Awake()
        {
            timer = Interval;
        }

        void FixedUpdate()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    SHActionController ac = SHBattleManager.Singleton.AssembleOneMonster();
                    ac.ThisTransform.position = this.transform.position;
                    ac.MoveAgent.target = SHBattleInfo.Singleton.HERO.ThisTransform;

                    timer = Interval;
                }
            }
        }

    }
}
