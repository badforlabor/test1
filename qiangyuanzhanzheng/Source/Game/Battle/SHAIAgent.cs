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

        SHAIStateMachine StateMachine = null;

        public override void Init(SHActionController ac)
        {
            base.Init(ac);

            RegisteStates();
        }
        void RegisteStates()
        {
            StateMachine = new SHAIStateMachine();
            AIState_Idle idlestate = new AIState_Idle(this);
            AIState_Run runstate = new AIState_Run(this);
            AIState_Dead deadstate = new AIState_Dead(this);

            // 注册起始状态是idle
            StateMachine.RegisteBegin(idlestate);

            // idle -> run
            StateMachine.Registe(idlestate, runstate, delegate()
            {
                return StateMachine.HoldData.state == EState.AI_RUNATTACK;
            });
            // idle -> dead
            StateMachine.Registe(idlestate, deadstate, delegate()
            {
                return StateMachine.HoldData.state == EState.AI_DEAD;
            });
            // run -> dead
            StateMachine.Registe(runstate, deadstate, delegate()
            {
                return StateMachine.HoldData.state == EState.AI_DEAD;
            });
            // run -> idle
            StateMachine.Registe(runstate, idlestate, delegate()
            {
                return StateMachine.HoldData.state == EState.AI_RUNATTACK;
            });

            // 设置起始状态
            StateMachine.SetNextState(EState.AI_IDLE);
        }

        public void SetNextState(EState state)
        {
            // 在下一帧的时候，才会切换状态
            StateMachine.SetNextState(state);
        }

        public void DoFire()
        {
            SHMover mover = SHMoverManager.Singleton.SpawnOne(Owner);
        }   
        public void AimToFire(SHActionController ac)
        {
            AimToFire(ac.ThisTransform.position);
        }
        public void AimToFire(Vector3 pos)
        {
            SavedAttackDir = (pos - Owner.ThisTransform.position);
            SavedAttackDir.y = Owner.ThisTransform.position.y;
            SavedMoveDir.Normalize();
            bSavedAttackDir = true;
            LastFireTime = FireStateHoldTime;
        }

        Vector3 SavedMoveDir = Vector3.zero;
        Vector3 SavedAttackDir = Vector3.zero;
        bool bSavedAttackDir = false;
        const float FireStateHoldTime = 1f; // 一旦开火，那么间隔1s之后再转向运动方向
        float LastFireTime = 0;

        public void ProcessMove(SHInputMove cb)
        {
            ProcessMove(cb.ExpectDir);
        }
        public void ProcessMove(Vector3 dir)
        {
            SavedMoveDir = dir;
        }

        override public void PhyTick()
        {
            StateMachine.PhyTick();


            float delta = Time.deltaTime;

            LastFireTime -= delta;

            EState state = StateMachine.GetCurState();

            if (state != EState.AI_DEAD)
            {
                // 如果有攻击方向，那么转向攻击方向，然后攻击
                if (bSavedAttackDir)
                {
                    if (!Owner.MoveAgent.TickRotateTowards(SavedAttackDir))
                    {
                        DoFire();
                        bSavedAttackDir = false;
                    }
                }
                else if (LastFireTime > 0)
                {

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

        /// 状态相关：静止、跑和攻击、死亡
        /// 
    }
}
