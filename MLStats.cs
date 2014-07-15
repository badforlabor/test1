/****************************************************************************
 Copyright (c) 2013-2014,Dalian-LingYOU tech.
 This is not a free-ware .DO NOT use it without any authorization.
 * 
 * date : 7/1/2014 21:46:39 AM
 * author : Labor
 * purpose : 直接在屏幕上显示调试信息，类似于现在在屏幕上看到的FPS
****************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLGame
{
    public class MLStats : MLSingleton<MLStats>, MLINotObfuscator
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
        public void Tick(float deltaTime)
        {
            if (IsReleaseVersion())
            {
                return;
            }

            // 过期的删除掉
            List<string> dels = new List<string>();
            foreach(var one in elements)
            {
                one.Value.LifeTime -= deltaTime;

                if(one.Value.LifeTime <= 0)
                {
                    dels.Add(one.Key);
                }
            }
            foreach(var del in dels)
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
            foreach(var one in elements)
            {
                GUI.color = one.Value.color;
                GUI.Label(area, one.Value.content);
                area.yMin += 20;
                area.yMax += 20;
            }
            GUI.color = Color.white;
        }

    }

    public class LitTimer
    {
        protected float BeginTime;
        public LitTimer()
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
