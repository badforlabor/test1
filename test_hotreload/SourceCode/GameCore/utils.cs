/****************************************************************************
Copyright (c) 2013-2014,Dalian-LingYOU tech.
 This is not a free-ware .DO NOT use it without any authorization.
 * 
 * date : 11/11/2014 5:20:57 PM
 * author : Administrator
 * purpose : 
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace MLGame
{
    public class Utils
    {
        public static string Result = "";
        public static int Add(int a, int b)
        {
            return Add2();
            //return a + b* 2;
        }
        static void GetAllFiles(string dir, ref List<string> files, bool meta)
        {
            if (!Directory.Exists(dir))
            {
                return;
            }
            string[] curfiles = Directory.GetFiles(dir);
            foreach (var file in curfiles)
            {
                if (!meta && file.EndsWith(".meta"))
                {

                }
                else
                {
                    files.Add(file.Replace('\\', '/'));
                }
            }
            curfiles = Directory.GetDirectories(dir);
            foreach (var file in curfiles)
            {
                if (Directory.Exists(file))
                {
                    GetAllFiles(file, ref files, meta);
                }
            }
        }

        public static int Add2()
        {
            try
            {
                // 加载外部dll！
#if false
                // window下
                string dllDir = Application.dataPath + "/DLL/";
#else
                // android下
                string dllDir = Application.persistentDataPath + "/labor/";

                
                //List<string> files = new List<string>();
                //GetAllFiles(Application.dataPath, ref files, false);
                
                //File.Copy(dllDir + "GameCore_Script.dll", Application.dataPath + "/Managed/GameCore_Script.dll");
#endif
                FileStream fs = new System.IO.FileStream(dllDir + "GameCore_Script", FileMode.Open, FileAccess.Read);
                long length = fs.Length;
                byte[] data = new byte[length];
                fs.Read(data, 0, (int)length);
                fs.Close();
                Assembly dll = Assembly.Load(data);
                System.Type utils = dll.GetType("MLGame.Utils_Script");
                MethodInfo mi = utils.GetMethod("Add");
                return (int)mi.Invoke(null, new object[] { 3, 0 });
            }
            catch (Exception e)
            {
                Result = e.Message;
                Debug.LogError(e.Message);
            }
            return 0;
        }

        public static int Add3(int a, int b)
        {
            return a + b * 2;
        }
    }
}
