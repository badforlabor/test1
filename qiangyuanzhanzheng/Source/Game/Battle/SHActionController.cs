/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 12/19/2014 8:32:22 AM
 * author : Labor
 * purpose : 
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    // 接口，和ActionController组合使用
    interface SHIActionAgent
    {
        SHAgentType AgentType { get; }
        SHActionController Owner { get; }
        void Init(SHActionController ac);
        bool IsActive();
        void PhyTick();
    }
    class SHBaseActionAgent : MonoBehaviour, SHIActionAgent
    {
        virtual public SHAgentType AgentType { get { return 0; } }
        public SHActionController Owner { get; private set; }
        virtual public void Init(SHActionController ac)
        {
            Owner = ac;
        }
        virtual public bool IsActive()
        {
            return Owner.IsActive();
        }
        virtual public void PhyTick()
        {

        }
    }
    enum SHAgentType
    {
        SHAgent_None,
        SHAgent_MoveAgent,
        SHAgent_AI,
        SHAgent_Threat,
        SHAgent_Max
    }
    
    class SHActionController : SHActionControllerBase, SHIActor
    {
        public Transform ThisTransform { get; private set; }

        // 协助actioncontroller工作的
        public SHIActionAgent[] agents = new SHIActionAgent[(int)SHAgentType.SHAgent_Max];

        // 阵营
        public ECampType Camp = ECampType.ENone;

        public void OnInitActor()
        {
            ThisTransform = transform;
            SHActorManager.Singleton.AddActor(this);
        }
        public void OnDestroyActor()
        {
            SHActorManager.Singleton.RemoveActor(this);
        }
        public ECampType GetCamp()
        {
            return Camp;
        }
        public bool IsActive()
        {
            return false;
        }

        // 提供给外部的借口，读取csv表
        public void Init(int id, ECampType camp)
        {
            OnInitActor();

            Camp = camp;
        }
        public T AddActionAgent<T>() where T : MonoBehaviour
        {
            T a = gameObject.AddComponent<T>();
            if (!(a is SHIActionAgent))
            {
                throw new Exception("xxxxxxxxxx");
            }

            // 放进去了，没办法快速get出来啊，悲剧！
            SHIActionAgent b = (SHIActionAgent)a;
            b.Init(this);
            agents[(int)b.AgentType] = b;
            return a;
        }
        public SHMoveAgent MoveAgent
        {
            get { return agents[(int)SHAgentType.SHAgent_MoveAgent] as SHMoveAgent; }
        }
        public SHAIAgent AIAgent
        {
            get { return agents[(int)SHAgentType.SHAgent_AI] as SHAIAgent; }
        }
        public SHThreatAgent ThreatAgent
        {
            get { return agents[(int)SHAgentType.SHAgent_Threat] as SHThreatAgent; }
        }

        public void TakeDamage(SHIActor killer)
        { 
        
        }

        void FixedUpdate()
        {
            PhyTick();
        }
        void PhyTick()
        {
            foreach (var a in agents)
            {
                if (a == null)
                {
                    continue;
                }

                a.PhyTick();
            }
        }

        // 主角初始化按键
        public void InitInput()
        {
            SHInputClick.SpawnOne(gameObject, OnInputClick);
            SHInputMove.SpawnOne(gameObject, OnInputMove);
        }
        void OnInputClick(SHInputClick cb)
        { 
            // 获取指向目标
            Ray ray = Camera.main.ScreenPointToRay(cb.PressedScreenPos);
            RaycastHit hitresult;
            SHActionController target = null;
            if (Physics.Raycast(ray, out hitresult))
            {
                SHStats.Singleton.AddMsg("hit name=" + hitresult.collider.gameObject.name, Color.red);
                target = hitresult.collider.gameObject.GetComponent<SHActionController>();
            }

            if (target == null)
            {
                return;
            }
            if (target == this)
            {
                return;
            }

            // 开火
            //AIAgent.DoFire();
            AIAgent.AimToFire(target);
        }
        void OnInputMove(SHInputMove cb)
        {
            AIAgent.ProcessMove(cb);
        }
        void OnInputMove1(SHInputMove cb)
        {
            float delta = Time.deltaTime;

            // 如果角度近似，那么直接设置！
            if (Vector3.Angle(cb.ExpectDir, transform.forward) < (QuaternionSpeed * delta))
            {
                transform.forward = cb.ExpectDir;

            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(cb.ExpectDir), delta * RotateSpeed);
            }

            // 更改位置
            //cc.Move(transform.forward * delta * RunSpeed);
            // 行动思路有N种：
            //          1、边走边转，并沿着自身朝向走
            //          2、旋转完角度之后，再走
            //          3、边走边转，沿着玩家控制方向走。（个人觉得这个效果比较好）
            // 行走思路改成第3种
            cc.Move(cb.ExpectDir * delta * RunSpeed);

            // 设置摄像机的位置
            Ray ray = new Ray(transform.position + Vector3.up * CameraYOffset, CameraDirection);
            Camera.main.transform.position = ray.GetPoint(CameraDistance);
            Camera.main.transform.rotation = Quaternion.Euler(CameraRotation);
        }
    }
}
