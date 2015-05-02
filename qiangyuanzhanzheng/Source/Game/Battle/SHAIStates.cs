/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 4/25/2015 8:26:41 AM
 * author : Labor
 * purpose : 划分状态，让事情变得简单处理
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    enum EState
    {
        EState_Invalid, // 起始状态
        EState_A,
        EState_B,

        AI_IDLE,
        AI_RUNATTACK,
        AI_DEAD,

        EStateMax,
    }
    class StateBase
    {
        virtual public EState StateTag
        {
            get { return EState.EStateMax; }
        }
        virtual public void OnStart()
        {
        }
        virtual public void OnEnd()
        {
        }
        virtual public void PhyTick()
        {         
        }
    }
    class InvalidState : StateBase
    {
        public override EState StateTag
        {
            get
            {
                return EState.EState_Invalid;
            }
        }
    }
    delegate bool OP();
    class NextStateOP
    {
        public StateBase NextState;
        public OP op;
    }
    class StateHolder
    {
        public StateBase Owner = null;
        public List<NextStateOP> NextStates = new List<NextStateOP>();
    }
    class StateData
    {
        public EState state = EState.EState_Invalid;
    }
    class StateMachine
    {
        StateHolder[] states = new StateHolder[(int)EState.EStateMax];
        StateHolder CurState = null;
        public StateData HoldData = null;
        bool bDirty = false;
        public StateMachine(StateData sd)
        {
            bDirty = false;
            HoldData = sd;
        }
        // 注册起始状态
        public void RegisteBegin(StateBase begin)
        {
            if (CurState == null)
            {
                InvalidState invalid = new InvalidState();
                Registe(invalid, begin, delegate() { return true; });
                CurState = states[(int)invalid.StateTag];
            }
        }
        // 启动后，先注册所有的状态
        // 注册所有状态的流程
        public void Registe(StateBase cur, StateBase next, OP op)
        {
            if (cur.StateTag == EState.EStateMax || next.StateTag == EState.EStateMax)
            {
                // 非法的状态
                return;
            }
            if (states[(int)cur.StateTag] == null)
            {
                states[(int)cur.StateTag] = new StateHolder();
                states[(int)cur.StateTag].Owner = cur;
            }
            if (states[(int)cur.StateTag].Owner != cur)
            {
                // 非法的状态
                return;
            }

            if (states[(int)next.StateTag] == null)
            {
                states[(int)next.StateTag] = new StateHolder();
                states[(int)next.StateTag].Owner = next;
            }
            if (states[(int)next.StateTag].Owner != next)
            {
                // 非法的状态
                return;
            }

            NextStateOP nextop = new NextStateOP();
            nextop.NextState = next;
            nextop.op = op;
            states[(int)cur.StateTag].NextStates.Add(nextop);
        }
        public void DoCheck()
        {
            bDirty = false;
            if (CurState == null)
            {
                return;
            }
            foreach (var a in CurState.NextStates)
            {
                // 发现条件满足，那么就切换状态
                if (a.op != null && a.op())
                {
                    CurState.Owner.OnEnd();
                    CurState = states[(int)a.NextState.StateTag];
                    if (CurState != null)
                    {
                        CurState.Owner.OnStart();
                    }
                    break;
                }
            }
        }
        public void PhyTick()
        {
            if (CurState != null && CurState.Owner != null)
            {
                CurState.Owner.PhyTick();
            }

            if (bDirty)
            {
                DoCheck();
            }
        }
        public void SetNextState(EState state)
        {
            HoldData.state = state;
            bDirty = true;
        }
        public EState GetCurState()
        {
            if (CurState != null && CurState.Owner != null)
            {
                return CurState.Owner.StateTag;
            }
            return EState.EState_Invalid;
        }
    }

    class SHAIStateMachine : StateMachine
    {
        public SHAIStateMachine() : base(new StateData())
        {
                 
        }
    }

    class AIStateBase : StateBase
    {
        protected SHAIAgent AIAgent = null;
        protected AIStateBase(SHAIAgent ai)
        {
            AIAgent = ai;
        }
    }
    class AIState_Idle : AIStateBase
    {
        public AIState_Idle(SHAIAgent ai)
            : base(ai)
        { 
        
        }
        public override EState StateTag
        {
            get
            {
                return EState.AI_IDLE;
            }
        }
        public override void OnStart()
        {
            SHLogger.Debug("[aistate] xxx idle start." + AIAgent.gameObject);
        }
        public override void OnEnd()
        {
            SHLogger.Debug("[aistate] xxx idle end." + AIAgent.gameObject);
        }
        public override void PhyTick()
        {

        }
    }
    class AIState_Run : AIStateBase
    {
        public AIState_Run(SHAIAgent ai)
            : base(ai)
        {

        }
        public override EState StateTag
        {
            get
            {
                return EState.AI_RUNATTACK;
            }
        }
        public override void OnStart()
        {
            SHLogger.Debug("[aistate] xxx run start." + AIAgent.gameObject);
        }
        public override void OnEnd()
        {
            SHLogger.Debug("[aistate] xxx run end." + AIAgent.gameObject);
        }
        public override void PhyTick()
        {

        }
    }
    class AIState_Dead : AIStateBase
    {
        public AIState_Dead(SHAIAgent ai)
            : base(ai)
        {

        }
        public override EState StateTag
        {
            get
            {
                return EState.AI_DEAD;
            }
        }
        public override void OnStart()
        {
            SHLogger.Debug("[aistate] xxx dead start." + AIAgent.gameObject);
        }
        public override void OnEnd()
        {
            SHLogger.Debug("[aistate] xxx dead end." + AIAgent.gameObject);
        }
        public override void PhyTick()
        {            
            
        }
    }
}
