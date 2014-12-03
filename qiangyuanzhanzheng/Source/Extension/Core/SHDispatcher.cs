/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 12/1/2014 10:55:40 PM
 * author : Labor
 * purpose : 消息处理，以后都走消息
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    // !< 添加一个'KeyObject',提供对事件进行对象的识别
    // T表示一个枚举，E表示派发的参数(args)，例子参考SHEvents.cs
    public class SHDispatcher<T, E>
    {
        public delegate void SHEventDelegate(E args);

        protected Dictionary<object, Dictionary<T, ArrayList>> mEventsWithObject = new Dictionary<object, Dictionary<T, ArrayList>>();
        protected Dictionary<object, Dictionary<T, ArrayList>> mEventsToBeRemove = new Dictionary<object, Dictionary<T, ArrayList>>();
        protected Dictionary<object, Dictionary<T, ArrayList>> mEventsToBeAdd = new Dictionary<object, Dictionary<T, ArrayList>>();

        public void TickSpecial(float delta)
        {
            // !< 删除要单独处理,不然同一数据结构可能导致访问冲突.
            foreach (KeyValuePair<object, Dictionary<T, ArrayList>> i in mEventsToBeRemove)
            {
                Dictionary<T, ArrayList> events = null;
                if (mEventsWithObject.TryGetValue(i.Key, out events))
                {
                    foreach (KeyValuePair<T, ArrayList> j in i.Value)
                    {
                        ArrayList del = null;
                        if (events.TryGetValue(j.Key, out del))
                        {
                            foreach (SHEventDelegate k in j.Value)
                            {
                                del.Remove(k);
                            }
                        }
                    }
                }
            }
            mEventsToBeRemove.Clear();

            // !< 添加新的事件
            foreach (KeyValuePair<object, Dictionary<T, ArrayList>> i in mEventsToBeAdd)
            {
                Dictionary<T, ArrayList> events = null;
                if (!mEventsWithObject.TryGetValue(i.Key, out events))
                {
                    events = new Dictionary<T, ArrayList>();
                    mEventsWithObject.Add(i.Key, events);
                }
                foreach (KeyValuePair<T, ArrayList> j in i.Value)
                {
                    ArrayList slots = null;
                    if (!events.TryGetValue(j.Key, out slots))
                    {
                        slots = new ArrayList();
                        events.Add(j.Key, slots);
                    }

                    slots.AddRange(j.Value);
                }
            }
            mEventsToBeAdd.Clear();
        }

        public void RegisterEvent(T evt, SHEventDelegate callback)
        {
            RegisterEvent(this, evt, callback);
        }

        public void RegisterEventSafe(T evt, SHEventDelegate callback)
        {
            RegisterEventSafe(this, evt, callback);
        }

        public void FireEvent(T evt, E args)
        {
            FireEvent(this, evt, args);
        }

        public void UnregisterEvent(T evt, SHEventDelegate callback)
        {
            UnregisterEvent(this, evt, callback);
        }

        // !< 快速添加,不安全,但是当前帧肯定会加上
        public void RegisterEvent(object obj, T evt, SHEventDelegate callback)
        {
            Dictionary<T, ArrayList> events = null;
            if (mEventsWithObject.TryGetValue(obj, out events))
            {
                ArrayList del = null;
                if (events.TryGetValue(evt, out del))
                {
                    del.Add(callback);
                }
                else
                {
                    del = new ArrayList();
                    del.Add(callback);
                    events.Add(evt, del);
                }
            }
            else
            {
                events = new Dictionary<T, ArrayList>();
                ArrayList del = new ArrayList();
                del.Add(callback);
                events.Add(evt, del);
                mEventsWithObject.Add(obj, events);
            }

            // !< 删除mEventsToBeRemove
            if (mEventsToBeRemove.TryGetValue(obj, out events))
            {
                ArrayList del = null;
                if (events.TryGetValue(evt, out del))
                {
                    del.Remove(callback);
                }
            }
        }

        // !< 安全添加,防止崩溃,当在事件处理的过程中添加可能需要使用该函数
        public void RegisterEventSafe(object obj, T evt, SHEventDelegate callback)
        {
            Dictionary<T, ArrayList> events = null;
            if (mEventsToBeAdd.TryGetValue(obj, out events))
            {
                ArrayList del = null;
                if (events.TryGetValue(evt, out del))
                {
                    del.Add(callback);
                }
                else
                {
                    del = new ArrayList();
                    del.Add(callback);
                    events.Add(evt, del);
                }
            }
            else
            {
                events = new Dictionary<T, ArrayList>();
                ArrayList del = new ArrayList();
                del.Add(callback);
                events.Add(evt, del);
                mEventsToBeAdd.Add(obj, events);
            }

            // !< 删除mEventsToBeRemove
            if (mEventsToBeRemove.TryGetValue(obj, out events))
            {
                ArrayList del = null;
                if (events.TryGetValue(evt, out del))
                {
                    del.Remove(callback);
                }
            }
        }

        public void FireEvent(object obj, T evt, E args)
        {
            Dictionary<T, ArrayList> events = null;
            if (mEventsWithObject.TryGetValue(obj, out events))
            {
                ArrayList del = null;
                if (events.TryGetValue(evt, out del))
                {
                    foreach (SHEventDelegate evtdel in del)
                    {
                        evtdel(args);
                    }
                }
            }

        }

        public void UnregisterEvent(object obj, T evt, SHEventDelegate callback)
        {
            Dictionary<T, ArrayList> events = null;
            if (mEventsToBeRemove.TryGetValue(obj, out events))
            {
                ArrayList del = null;
                if (events.TryGetValue(evt, out del))
                {
                    del.Add(callback);
                }
                else
                {
                    del = new ArrayList();
                    del.Add(callback);
                    events.Add(evt, del);
                }
            }
            else
            {
                events = new Dictionary<T, ArrayList>();
                ArrayList del = new ArrayList();
                del.Add(callback);
                events.Add(evt, del);
                mEventsToBeRemove.Add(obj, events);
            }

            // !< 删除mEventsToBeAdd
            if (mEventsToBeAdd.TryGetValue(obj, out events))
            {
                ArrayList del = null;
                if (events.TryGetValue(evt, out del))
                {
                    del.Remove(callback);
                }
            }
        }

        // !< 目前用在UI清空的情况下...
        public void Reset()
        {
            // !< WARN:暂时无视'mInternalObject',实在需要再说!!
            mEventsWithObject.Clear();
            mEventsToBeRemove.Clear();
            mEventsToBeAdd.Clear();
        }
    }
}
