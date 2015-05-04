/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 11/23/2014 12:02:45 AM
 * author : Labor
 * purpose : 方便调试。
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{

    public class SHStats : SHSingleton<SHStats>
    {
        protected static long UNIQUE_ID = 1;
        class Element
        {
            public string content;
            public Color color;
            public float LifeTime;
        };
        Dictionary<string, Element> elements = new Dictionary<string, Element>();
        protected void AddMsgImpl(string key, string content, Color color, float lifetime)
        {
            if (IsReleaseVersion())
            {
                return;
            }

            UNIQUE_ID++;

            Element one = null;
            if (!elements.TryGetValue(key, out one))
            {
                one = new Element();
                elements.Add(key, one);
            }
            one.content = content;
            one.color = color;
            one.LifeTime = lifetime;
        }
        protected bool IsReleaseVersion()
        {
            return false;
        }

        public void AddMsg(string key, string content, Color color, float lifetime = 3.0f)
        {
            AddMsgImpl(key, content, color, lifetime);
        }
        public void AddMsg(string key, string content, float lifetime = 3.0f)
        {
            AddMsg(key, content, Color.green, 3.0f);
        }
        public void AddMsg(string content, float lifetime = 3.0f)
        {
            AddMsg("" + UNIQUE_ID, content, lifetime);
        }
        public void AddMsg(string content, Color color, float lifetime = 3.0f)
        {
            AddMsg("" + UNIQUE_ID, content, color, lifetime);
        }
        public void Update(float deltaTime)
        {
            if (IsReleaseVersion())
            {
                return;
            }

            // 过期的删除掉
            List<string> dels = new List<string>();
            foreach (var one in elements)
            {
                one.Value.LifeTime -= deltaTime;

                if (one.Value.LifeTime <= 0)
                {
                    dels.Add(one.Key);
                }
            }
            foreach (var del in dels)
            {
                elements.Remove(del);
            }
        }
        public void OnGUI()
        {
            if (IsReleaseVersion())
            {
                return;
            }

            Rect area = new Rect(10, 10, Screen.width, 20);
            foreach (var one in elements)
            {
                GUI.color = one.Value.color;
                GUI.Label(area, one.Value.content);
                area.yMin += 20;
                area.yMax += 20;
            }
            GUI.color = Color.white;
        }

    }

    public class SHLitTimer
    {
        protected float BeginTime;
        public SHLitTimer()
        {
            Reset();
        }
        public void Reset()
        {
            BeginTime = Time.realtimeSinceStartup;
        }
        public bool IsTime(float delta)
        {
            return IsTime((double)delta);
        }
        public bool IsTime(double delta)
        {
            if (Time.realtimeSinceStartup - BeginTime >= delta)
            {
                BeginTime = Time.realtimeSinceStartup;
                return true;
            }
            else
            {
                return false;
            }
        }
    };
}
