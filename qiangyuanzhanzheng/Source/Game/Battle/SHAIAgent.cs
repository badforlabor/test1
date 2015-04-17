/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 12/19/2014 7:38:29 AM
 * author : Labor
 * purpose : 控制AI行为的，如：何时逃跑，何时开火，如何寻找敌人等
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    class SHAIAgent : SHBaseActionAgent
    {
        override public SHAgentType AgentType { get { return SHAgentType.SHAgent_AI; } }

        public void DoFire()
        {
            SHMover mover = SHMoverManager.Singleton.SpawnOne(Owner);
        }   
        public void AimToFire(SHActionController ac)
        {
            SavedAttackDir = (ac.ThisTransform.position - Owner.ThisTransform.position).normalized;
            bSavedAttackDir = true;
        }

        Vector3 SavedMoveDir = Vector3.zero;
        Vector3 SavedAttackDir = Vector3.zero;
        bool bSavedAttackDir = false;

        public void ProcessMove(SHInputMove cb)
        {
            SavedMoveDir = cb.ExpectDir;
        }

        override public void PhyTick()
        {
            float delta = Time.deltaTime;

            // 如果有攻击方向，那么转向攻击方向，然后攻击
            if (bSavedAttackDir)
            {
                if (!Owner.MoveAgent.TickRotateTowards(SavedAttackDir))
                {
                    DoFire();
                    bSavedAttackDir = false;
                }
            }
            else if (SavedMoveDir.sqrMagnitude > 0)
            {
                Owner.MoveAgent.TickRotateTowards(SavedMoveDir);
            }

            if (SavedMoveDir.sqrMagnitude > 0)
            {
                Owner.cc.Move(SavedMoveDir * delta * Owner.RunSpeed);
                Owner.UpdateCamera();
                SavedMoveDir = Vector3.zero;
            }

        }
    }
}
