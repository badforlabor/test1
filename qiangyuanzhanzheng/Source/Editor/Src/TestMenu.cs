﻿/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 11/22/2014 11:57:35 AM
 * author : Labor
 * purpose : 
****************************************************************************/
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    public class TestMenu
    {
        [MenuItem("ShGame/test1")]
        public static void ProcMenu()
        {
            SHLogger.Debug("XXXXXXXXXXXXXXXXXXXXX");
        }
    }
    [CustomEditor(typeof(SHList<MonoBehaviour>))]
    class SHListEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("123123123");
            EditorGUILayout.EndVertical();
        }
    }

}
