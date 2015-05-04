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
    public class SHPoolTag : MonoBehaviour
    {
        public string Path = "";
        public int SortID = 0;
    }
    class SHLevelPool : SHLevelSingleton<SHLevelPool>
    {
        static int CompareFunc(SHPoolTag left, SHPoolTag right)
        {
            return CompareFuncImpl(left.SortID, right);
        }
        static int CompareFuncImpl(int leftSortID, SHPoolTag right)
        {
                if (leftSortID < right.SortID)
                    return -1;
                else if (leftSortID > right.SortID)
                    return 1;
                else
                    return 0;
        }
        static int CompareFunc2(SHPoolTag left, SHPoolTag right)
        {
            return CompareFuncImpl2(left.Path, right);
        }
        static int CompareFuncImpl2(string leftPath, SHPoolTag right)
        {
            return string.Compare(leftPath, right.Path);
        }


        public SHList<SHPoolTag> Storage = new SHList<SHPoolTag>(CompareFunc);
        public SHList<SHPoolTag> TemplateStorage = new SHList<SHPoolTag>(CompareFunc2);
        
        public void Test()
        {
#if false
            GameObject go = Spawn("base/mover");
            GameObject.Destroy(go);
            go = Spawn("base/mover");
            SHLevelPool.Singleton.TrashOne(go);
            go = Spawn("base/mover");
            SHLevelPool.Singleton.TrashOne(go);
#endif
        }
        public GameObject Spawn(string fullpath)
        {
            return Spawn(fullpath, null);
        }
        public GameObject Spawn(string fullpath, System.Action<GameObject> TemplateCallback)
        {
            SHPoolTag ret = GetInStorage(fullpath);
            if (ret == null)
            {
                SHPoolTag tagTemplate = TemplateStorage.Find(delegate(SHPoolTag tag)
                {
                    return CompareFuncImpl2(fullpath, tag);
                });
                if (tagTemplate == null)
                {
                    SHLogger.Debug("[pool] xxxx new template:" + fullpath);
                    GameObject go = SHResources.Singleton.Instance<GameObject>(fullpath);
                    go.SetActive(false);

                    SHPoolTag pooltag = go.GetComponent<SHPoolTag>();
                    if (pooltag == null)
                    {
                        pooltag = go.AddComponent<SHPoolTag>();
                        pooltag.Path = fullpath;
                        pooltag.SortID = fullpath.GetHashCode();
                    }
                    // 修改模板的
                    if (TemplateCallback != null)
                    {
                        TemplateCallback(go);
                    }
                    tagTemplate = pooltag;

                    TemplateStorage.Add(tagTemplate);
                }
                GameObject goRet = (GameObject)GameObject.Instantiate(tagTemplate.gameObject);
                goRet.SetActive(true);
                ret = goRet.GetComponent<SHPoolTag>();
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
        }
        void RemoveFromStorage(SHPoolTag pooltag)
        {
            pooltag.transform.parent = null;
            Storage.Remove(pooltag);
        }
        SHPoolTag GetInStorage(string path)
        {
            int sortID = path.GetHashCode();
            // 改用2分查找，这样能快点
            return Storage.Find(delegate(SHPoolTag tag)
            {
                return CompareFuncImpl(sortID, tag);
            });
        }
    }
}
