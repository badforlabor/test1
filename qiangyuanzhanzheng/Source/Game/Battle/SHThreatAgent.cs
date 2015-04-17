/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 12/19/2014 7:41:49 AM
 * author : Labor
 * purpose : 仇恨系统，很简单，每隔一段时间扫描一下四周的敌人
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    /*
     * 设计：
     *      1、每秒检测一下四周的敌人
     *      2、当收到伤害时，记录一下该敌人，并将敌人信息广播给自己的组成员
     *      3、每帧都有威胁值的衰减
     * */
    class SHThreatAgent : SHBaseActionAgent
    {
        class SHThreat
        {
            float ThreatValue;
            SHIActor ThreatActor;
        }
        List<SHThreat> Threats = new List<SHThreat>();
        SHLitTimer LitTimerExplorer = new SHLitTimer();
        public float ExplorerDuration = 1.0f;
        public float ExplorerRadius = 10.0f;


        void OnTakeDamage(SHIActor killer)
        {

        }
        void Update()
        {
            float delta = Time.deltaTime;

            if (LitTimerExplorer.IsTime(ExplorerDuration))
            {
                DoExplorer();
            }
        }
        void DoExplorer()
        {
            List<SHIActor> actors = SHActorManager.Singleton.GetObjectByShape_Circle(Owner, ETargetType.EEnemy, Owner.ThisTransform.position, ExplorerRadius);

        }
    }
}
