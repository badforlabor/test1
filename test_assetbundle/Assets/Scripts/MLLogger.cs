/****************************************************************************
Copyright (c) 2013-2014,Dalian-LingYOU tech.
 This is not a free-ware .DO NOT use it without any authorization.
 * 
 * date : 1/15/2014 2:25:26 PM
 * author : ShengKai
 * purpose : 简单小日志系统，简单是美
****************************************************************************/
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace MLGame
{
    public class MLLogger
    {
        private static bool initialized = false;
        private static StreamWriter stream;

        public static bool LogToFile = false;

        private static void _Initialized()
        {
            initialized = true;

            string temporaryCachePath = Application.temporaryCachePath;
            //string temporaryCachePath = Application.dataPath + "/../log/";
            string destFileName = Path.Combine(temporaryCachePath, "log_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + ".txt");

            FileStream filestream = null;
            try
            {
                filestream = File.Open(destFileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                stream = new StreamWriter(filestream, new UTF8Encoding(false));
                stream.AutoFlush = true;
            }
            catch (System.Exception)
            {
                if (stream != null)
                {
                    stream.Close();
                }
                else if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public static void Destroy()
        {
            if (stream != null)
            {
                stream.Dispose();
                stream = null;
            }
        }

        public static void LogError(string content)
        {
            Debug.LogError("[Error] " + content);
            if (LogToFile)
            {
                if (!initialized)
                {
                    _Initialized();
                }

                stream.WriteLine("[Error] " + content);
            }
        }

        // !< 这是最最最严重的错误!会直接崩溃的,紫道不!?
        public static void LogFatal(string content)
        {
            Debug.LogError("[Fatal-Error] " + content);
            if (LogToFile)
            {
                if (!initialized)
                {
                    _Initialized();
                }

                stream.WriteLine("[Fatal-Error] " + content);
            }
        }

        public static void LogDebug(string content)
        {
            Debug.Log("[debug] " + content);
            if (LogToFile)
            {
                if (!initialized)
                {
                    _Initialized();
                }

                stream.WriteLine("[debug] " + content);
            }
        }

        public static void LogInfo(string content)
        {
            Debug.LogWarning("[Info] " + content);
            if (LogToFile)
            {
                if (!initialized)
                {
                    _Initialized();
                }

                stream.WriteLine("[Info] " + content);
            }
        }

        public static void LogWarning(string content)
        {
            Debug.LogWarning("[Warning] " + content);
            if (LogToFile)
            {
                if (!initialized)
                {
                    _Initialized();
                }

                stream.WriteLine("[Warning] " + content);
            }
        }
    }
}
