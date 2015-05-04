/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 5/2/2015 9:18:18 PM
 * author : Labor
 * purpose : 排序号的List，查找速度快
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    public delegate int IntPredicate<T>(T obj);
    [System.Serializable]
    public class SHList<T> : List<T>
    {
        Comparison<T> mComparison = null;
        public SHList(Comparison<T> comparison)
        {
            mComparison = comparison;
        }

        new public void Add(T obj)
        {
            base.Add(obj);
            Sort();
        }
        new public void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            Sort();
        }
        new public void Remove(T obj)
        {
            base.Remove(obj);
            Sort();
        }
        new public void Sort()
        {
            base.Sort(mComparison);
        }
        public T Find(T temp)
        {
            // 2分查找
            int maxcnt = (this.Count + 1) / 2;
            int low = 0, high = this.Count - 1;
            int mid = 0;
            while(low <= high)
            {
                mid = low + (high - low) / 2;

                T right = this[mid];
                int va = mComparison(temp, right);

                if (va == 0)
                {
                    return right;;
                }

                if (va < 0)
                {
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }

            return default(T);
        }
        public T Find(IntPredicate<T> match)
        {
            // 2分查找
            int maxcnt = (this.Count + 1) / 2;
            int low = 0, high = this.Count - 1;
            int mid = 0;
            while (low <= high)
            {
                mid = low + (high - low) / 2;

                T right = this[mid];
                int va = match(right);

                if (va == 0)
                {
                    return right;
                }

                if (va < 0)
                {
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }

            return default(T);
        }


        // 测试函数
        public static void Test()
        {
            SHList<int> a = new SHList<int>(delegate(int left, int right)
                {
                    if (left < right)
                        return -1;
                    else if (left > right)
                        return 1;
                    else
                        return 0;
                });
            a.Add(1);
            a.Add(5);
            a.Add(3);
            a.Add(2);
            a.Add(4);

            // 确保都能找到
            int ret = a.Find(2);
            ret = a.Find(1);
            ret = a.Find(3);
            ret = a.Find(4);
            ret = a.Find(5);
        }
    }
}
