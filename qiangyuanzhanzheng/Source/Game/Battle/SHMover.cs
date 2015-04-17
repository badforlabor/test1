/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 12/17/2014 6:42:16 AM
 * author : Labor
 * purpose : 非瞬间击中的子弹，雷的投射器等
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    class SHMover : MonoBehaviour, SHIActor
    {
        public Transform ThisTransform { get; private set; }
        public void OnInitActor()
        {
            ThisTransform = transform;
            SHActorManager.Singleton.AddActor(this);
        }
        public ECampType GetCamp()
        {
            return Owner.GetCamp();
        }



        //public Vector3 Velocity = Vector3.zero;    // 速度方向，这个废掉，就是Gameobject的朝向！
        public float LifeTime = 5;                 // 生命周期
        public float VerticalSpeed = 0;            // 垂直方向的速度
        public float HorizentalSpeed = 200;          // 水平方向的速度
        public bool UseGravity = false;             // 是否使用重力（影响垂直方向）

        bool PendingDestroyed = false;

        public SHActionController Owner = null;

        public void Init(SHActionController owner, int id)
        {
            OnInitActor();

            Owner = owner;
        }

        void FixedUpdate()
        {
            PhyTick();
        }
        void PhyTick()
        {
            if (PendingDestroyed)
            {
                return;
            }

            float delta = Time.deltaTime;

            // 移动物体
            if (LifeTime > 0)
            {
                LifeTime -= delta;
                Vector3 moved = HorizentalSpeed * delta * transform.forward + (-1) * VerticalSpeed * delta * transform.up;
#if false
                transform.Translate(moved, Space.World);
#elif true
                // 约定场景中的可碰撞物体至少是0.5米宽
                if (moved.sqrMagnitude > 0.25)
                {
                    // 射线检测

                    RaycastHit[] hitinfos = Physics.RaycastAll(ThisTransform.position,// + ThisTransform.forward * 1,
                                ThisTransform.forward, moved.magnitude);

                    foreach (var hitinfo in hitinfos)
                    {
                        if (Physics.GetIgnoreLayerCollision(hitinfo.collider.gameObject.layer, gameObject.layer))
                        {

                        }
                        else
                        {
                            OnTriggerEnter(hitinfo.collider);
                        }
                        if (!IsActive())
                        {
                            break;
                        }
                    }
                    transform.Translate(moved, Space.World);
                }
                else
                {
                    transform.Translate(moved, Space.World);
                }
#else
                // 不好用！
                // 一次移动0.5个单位，要不会移动太长会导致打不中（比如一步移动了3米，而可碰撞物体才1米宽，那么就会导致直接越过可碰撞物体）
                Vector3 deltaMoved = moved.normalized * 0.5f;
                do
                {
                    // 移动自己
                    transform.Translate(deltaMoved, Space.World);
                    //transform.localPosition += moved;
                    moved -= deltaMoved;
                    if (moved.sqrMagnitude < deltaMoved.sqrMagnitude)
                    {
                        deltaMoved = moved;
                    }
                }
                while (IsActive() && moved.sqrMagnitude > 0);
                if(moved.sqrMagnitude > 0)
                {
                    SHLogger.Warning("xxxxxxxxxxxxxxxxxxxxxx");
                }
#endif
            }
            if (LifeTime < 0)
            {
                DestroyMe();
            }
        }
        bool IsActive()
        {
            return !PendingDestroyed;
        }

        // 产生的碰撞反应
        void OnCollisionEnter(Collision col)
        {
            SHLogger.Debug("[collision] enter. col.name=" + col.gameObject.GetFullName());
        }
        void OnCollisionExit(Collision col)
        {
            SHLogger.Debug("[collision] exit. col.name=" + col.gameObject.GetFullName());
        }
        void OnCollisionStay(Collision col)
        {
            SHLogger.Debug("[collision] stay. col.name=" + col.gameObject.GetFullName());
        }
        void OnTriggerEnter(Collider col)
        {
            SHLogger.Debug("[collision] trigger enter. trigger.name=" + col.gameObject.GetFullName());

            SHActionController ac = col.GetComponent<SHActionController>();
            if (ac == null || ac.GetCamp() == Owner.GetCamp())
            {
                return;
            }

            if (IsActive())
            {
                DestroyMe();
            }
        }
        void OnTriggerExit(Collider col)
        {
            SHLogger.Debug("[collision] trigger exit. trigger.name=" + col.gameObject.GetFullName());
        }
        void OnTriggerStay(Collider col)
        {
            SHLogger.Debug("[collision] trigger stay. trigger.name=" + col.gameObject.GetFullName());
        }

        // 销毁自己
        void DestroyMe()
        {
            PendingDestroyed = true;

            SHActorManager.Singleton.RemoveActor(this);
        }
    }
}
