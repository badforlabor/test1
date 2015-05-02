/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 4/23/2015 11:13:52 PM
 * author : Labor
 * purpose : 
****************************************************************************/
#define PRETTY_FORMATER

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

namespace SHGame
{
    static class Debug
    { 
        public static void LogError(string msg)
        {
        
        }
    }

    public static class JsonHelper
    {
        static public T loadObjectFromJsonFile<T>(string path)
        {
            try
            {
                TextReader reader = new StreamReader(path);
                if (reader == null)
                {
                    Debug.LogError("Cannot find " + path);
                    reader.Close();
                    return default(T);
                }

                T data = ToObject<T>(reader.ReadToEnd());
                if (data == null)
                {
                    Debug.LogError("Cannot read data from " + path);
                }

                reader.Close();

                return data;
            }
            catch (System.Exception e)
            {
                return default(T);
            }

        }

        static public void saveObjectToJsonFile(object data, string path)
        {
            TextWriter tw = new StreamWriter(path);
            if (tw == null)
            {
                Debug.LogError("Cannot write to " + path);
                return;
            }

#if PRETTY_FORMATER
            string jsonStr = JsonFormatter.PrettyPrint(ToJson(data));
#else
            string jsonStr = JsonMapper.ToJson(data);
#endif
            tw.Write(jsonStr);
            tw.Flush();
            tw.Close();
        }
        public static string ToJson(object obj)
        {
            return JsonMapper.ToJson(obj);
        }
        public static T ToObject<T>(string json)
        {
            return JsonMapper.ToObject<T>(json);
        }
	
	
    }
}
