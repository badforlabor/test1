/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 4/12/2015 11:09:36 PM
 * author : Labor
 * purpose : 
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    class SHMoverManager : SHLevelSingleton<SHMoverManager>
    {
        public SHMover SpawnOne(SHActionController owner)
        {
            GameObject go = SHLevelPool.Singleton.Spawn("base/mover", delegate(GameObject goTemplate)
            {
                if (goTemplate.GetComponent<SHMover>() == null)
                {
                    goTemplate.layer = LayerMask.NameToLayer(SHNames.LayerMover);
                    goTemplate.AddComponent<SHMover>();

                    // 生成特效
                    GameObject goEffect = new GameObject("goEffect");
                    goEffect.transform.parent = goTemplate.transform;
                }
            });
            go.transform.parent = this.gameObject.transform;
            SHMover ret = go.GetComponent<SHMover>();
            
            // 异步生成特效！

            ret.Init(owner, 0);
            ret.transform.position = owner.transform.position + owner.transform.up * 1f;
            ret.transform.rotation = owner.transform.rotation;

            return ret;
        }
    }
}
