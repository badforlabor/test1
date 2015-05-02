/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 4/12/2015 11:09:36 PM
 * author : Labor
 * purpose : 对象池，防止反复创建对象，浪费cpu和内存
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    // 凡是有这个标志的，都将加入到缓冲池中。
    class SHPoolTag : MonoBehaviour
    {
        public string Path = "";
        public int SortID = 0;
    }
    class SHLevelPool : SHLevelSingleton<SHLevelPool>
    {
        public List<SHPoolTag> Storage = new List<SHPoolTag>();

        public void Test()
        {
            GameObject go = Spawn("base/mover");
            GameObject.Destroy(go);
            go = Spawn("base/mover");
            SHLevelPool.Singleton.TrashOne(go);
            go = Spawn("base/mover");
            SHLevelPool.Singleton.TrashOne(go);
        }
        public GameObject Spawn(string fullpath)
        {
            SHPoolTag ret = GetInStorage(fullpath);
            if (ret == null)
            {
                GameObject goTemplate = SHResources.Singleton.Load<GameObject>(fullpath);
                SHPoolTag pooltag = goTemplate.GetComponent<SHPoolTag>();
                if (pooltag == null)
                {
                    pooltag = goTemplate.AddComponent<SHPoolTag>();
                    pooltag.Path = fullpath;
                    pooltag.SortID = fullpath.GetHashCode();
                }

                GameObject go = (GameObject)GameObject.Instantiate(goTemplate);
                ret = go.GetComponent<SHPoolTag>();
            }
            else
            {
                RemoveFromStorage(ret);
            }

            return ret.gameObject;
        }
        public bool TrashOne(GameObject go)
        {
            SHPoolTag pooltag = go.GetComponent<SHPoolTag>();
            if (pooltag == null)
            {
                return false;
            }

            AddToStorage(pooltag);
            return true;
        }
        void AddToStorage(SHPoolTag pooltag)
        {
            pooltag.transform.parent = this.transform;
            pooltag.transform.localPosition = SHTrashManager.TrashPostion;

            Storage.Add(pooltag);
            SortStorage();
        }
        void SortStorage()
        {
            // 数据更改的时候，立刻排序，这样查找的时候，能够快一点（起始dictionary就是这个过程）
            Storage.Sort(delegate(SHPoolTag left, SHPoolTag right)
            {
                if (left.SortID < right.SortID)
                    return -1;
                else if (left.SortID > right.SortID)
                    return 1;
                else
                    return 0;
            });
        }
        void RemoveFromStorage(SHPoolTag pooltag)
        {
            pooltag.transform.parent = null;
            Storage.Remove(pooltag);
            SortStorage();
        }
        SHPoolTag GetInStorage(string path)
        {
            int sortID = path.GetHashCode();
            // 改用2分查找，这样能快点
            return Storage.Find(x => x.SortID == sortID);
        }
    }
}
