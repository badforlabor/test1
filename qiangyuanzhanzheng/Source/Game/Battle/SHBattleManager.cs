/****************************************************************************
Copyright (c) 2013-2025,大连-游你酷伴.
 This is not a free-ware .DO NOT use it without any authorization.
 * 坚持做有意思的游戏
 * 
 * date : 12/3/2014 7:43:11 AM
 * author : Labor
 * purpose : 管理游戏的
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SHGame
{
    class SHBattleManager : SHSingleton<SHBattleManager>
    {
        public void EnterGame()
        {
            SHSceneManager.Singleton.ChangeScene(EScene.Battle, OnEnterGame);
            //Application.LoadLevelAsync("city1");
        }
        void OnEnterGame()
        {
            SHLogger.Debug("[Battle] 切换场景完毕，开始加载游戏信息！");

            // 加载房间信息
            SHLogger.Info("[Battle] xxx load room.");
            SHResources.Singleton.Instance<UnityEngine.Object>("Rooms/huianchengzhen02");

            // 加载寻路网格
            Mesh mesh = SHResources.Singleton.Load<Mesh>("Rooms/NavMesh/huianchengzhen02");
            if (mesh == null)
            {
                SHLogger.Warning("[Battle] can't find nav mesh!");
            }
            if (AstarPath.active == null)
            {
                // AstarPath 被放在battle场景的对象A*中
                SHLogger.Error("[Battle] can't find astar!");
            }
            if (AstarPath.active.graphs.Length != 0)
            {
                SHLogger.Error("[Battle] astar is invalid!");
            }
            AstarPath.active.astarData.AddGraph(new Pathfinding.RecastGraph());

            // 加载触发器

            // 加载敌人
            AssembleMonsters();

            // 加载主角
            AssemblePlayers();

            // 加载UI
            SHLogger.Info("[Battle] xxx load UI.");
            GameObject go = new GameObject("UI_QuitGame");
            go.AddComponent<SHQuitBattleBehaviour>();
        }

        // 主角
        void AssemblePlayers()
        {
            GameObject hero = SHResources.Singleton.Instance<GameObject>("Characters/hero_no1");
            hero.AddComponent<SHActionControllerBase>();
        }

        // 怪
        void AssembleMonsters()
        {
            GameObject monster = SHResources.Singleton.Instance<GameObject>("Characters/hero_no1");
            monster.AddComponent<SHMoveAgent>();
        }
    }
}
