/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 4/12/2015 11:15:19 PM
 * author : Labor
 * purpose : 所有销毁的物品都移动到一个垃圾堆，然后定点销毁
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    class SHTrashManager : SHLevelSingleton<SHTrashManager>
    {
        void Awake()
        {
            transform.localPosition = new Vector3(-1313, 0, 0);
        }

        public void Trash(SHIActor actor)
        {
            // 保持相对位置不变
            Vector3 pos = actor.ThisTransform.localPosition;
            actor.ThisTransform.parent = gameObject.transform;
            actor.ThisTransform.localPosition = pos;
        }
    }
}
