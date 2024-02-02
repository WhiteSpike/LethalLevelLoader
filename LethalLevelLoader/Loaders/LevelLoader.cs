﻿using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEngine.AI;
using LethalLevelLoader.Tools;

namespace LethalLevelLoader
{
    public class LevelLoader
    {
        internal static List<MeshCollider> customLevelMeshCollidersList = new List<MeshCollider>();

        internal static async void EnableMeshColliders()
        {
            List<MeshCollider> instansiatedCustomLevelMeshColliders = new List<MeshCollider>();

            int counter = 0;
            foreach (MeshCollider meshCollider in UnityEngine.Object.FindObjectsOfType<MeshCollider>())
                if (meshCollider.gameObject.name.Contains(" (LLL Tracked)"))
                    instansiatedCustomLevelMeshColliders.Add(meshCollider);

            Task[] meshColliderEnableTasks = new Task[instansiatedCustomLevelMeshColliders.Count];

            foreach (MeshCollider meshCollider in instansiatedCustomLevelMeshColliders)
            {
                meshColliderEnableTasks[counter] = EnableMeshCollider(meshCollider);
                counter++;
            }

            await Task.WhenAll(meshColliderEnableTasks);

            //customLevelMeshCollidersList.Clear();
        }

        internal static async Task EnableMeshCollider(MeshCollider meshCollider)
        {
            meshCollider.enabled = true;
            meshCollider.gameObject.name.Replace(" (LLL Tracked)", "");
            await Task.Yield();
        }

        internal static void UpdateStoryLogs(ExtendedLevel extendedLevel, GameObject sceneRootObject)
        {
            foreach (StoryLog sceneStoryLog in sceneRootObject.GetComponentsInChildren<StoryLog>())
                foreach (StoryLogData storyLogData in extendedLevel.storyLogs)
                    if (sceneStoryLog.storyLogID == storyLogData.storyLogID)
                        sceneStoryLog.storyLogID = storyLogData.newStoryLogID;
        }
    }
}