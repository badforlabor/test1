/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 12/19/2014 7:56:59 AM
 * author : Labor
 * purpose : 
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    public enum ECampType
    {
        ENone,          // 非法的
        ERed,         // 红色阵营，红警中的共和军，自己是共和军，红蓝队，红在前，是自己
        EBlue,        // 蓝色阵营，红警中的同盟军
        ENeutral,     // 中立阵营，可以被红蓝队攻击
        EUnbeatable,  // 不可被攻击阵营，不可以被任何人攻击
    }
    enum ETargetType
    { 
        EFriend,
        EEnemy,
        ENpc,   // 只检索unbeatable类型的
    }
    enum EExplorerShape
    {
        ECircle,
        ERectangle,
    }

    // 场景里面所有可以被攻击的物体都被称作actor
    interface SHIActor
    {
        Transform ThisTransform { get; }
        void OnInitActor();
        ECampType GetCamp();
    }

    class SHActorManager : SHLevelSingleton<SHActorManager>
    {
        List<SHIActor> AllActors = new List<SHIActor>();

        public void AddActor(SHIActor actor)
        {
            AllActors.Add(actor);
        }
        public void RemoveActor(SHIActor actor)
        {
            AllActors.Remove(actor);

            SHTrashManager.Singleton.Trash(actor);
        }

        // 探寻四周
        public List<SHIActor> GetObjectByShape_Circle(SHIActor self, ETargetType target, Vector3 start, float r)
        {
            return GetObjectByShape(self, EExplorerShape.ECircle, target, start, r, r);
        }
        List<SHIActor> GetObjectByShape(SHIActor self, EExplorerShape shape, ETargetType target, Vector3 start, float l, float w)
        {
            List<SHIActor> ret = new List<SHIActor>();

            switch (shape)
            { 
                case EExplorerShape.ECircle:
                    {
                        foreach (var actor in AllActors)
                        {
                            if (ETargetType.EFriend == target)
                            {
                                if (actor.GetCamp() != self.GetCamp())
                                {
                                    continue;
                                }
                            }
                            else if (ETargetType.EEnemy == target)
                            {
                                if (actor.GetCamp() == ECampType.EUnbeatable
                                        || actor.GetCamp() == self.GetCamp())
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (actor.GetCamp() != ECampType.EUnbeatable)
                                {
                                    continue;
                                }
                            }

                            if ((actor.ThisTransform.position - start).magnitude < w)
                            {
                                ret.Add(actor);
                            }
                        }
                    }
                    break;
            }

            return ret;
        }
    }
}
