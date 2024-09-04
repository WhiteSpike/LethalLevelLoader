﻿using DunGen.Graph;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;

namespace LethalLevelLoader
{
    public static class PatchedContent
    {
        public static ExtendedMod VanillaMod { get; internal set; }

        public static List<string> AllLevelSceneNames { get; internal set; } = new List<string>();

        public static List<ExtendedMod> ExtendedMods { get; internal set; } = new List<ExtendedMod>();

        public static List<ExtendedContent> ExtendedContents { get; internal set; } = new List<ExtendedContent>();

        internal static Dictionary<string, ExtendedContent> UniqueIdentifiersDictionary = new Dictionary<string, ExtendedContent>();
        internal static Dictionary<SelectableLevel, ExtendedLevel> ExtendedLevelDictionary = new Dictionary<SelectableLevel, ExtendedLevel>();
        internal static Dictionary<DungeonFlow, ExtendedDungeonFlow> ExtendedDungeonFlowDictionary = new Dictionary<DungeonFlow, ExtendedDungeonFlow>();
        internal static Dictionary<Item, ExtendedItem> ExtendedItemDictionary = new Dictionary<Item, ExtendedItem>();
        internal static Dictionary<EnemyType, ExtendedEnemyType> ExtendedEnemyTypeDictionary = new Dictionary<EnemyType, ExtendedEnemyType>();
        internal static Dictionary<BuyableVehicle, ExtendedBuyableVehicle> ExtendedBuyableVehicleDictionary = new Dictionary<BuyableVehicle, ExtendedBuyableVehicle>();


        public static List<ExtendedLevel> ExtendedLevels { get; internal set; } = new List<ExtendedLevel>();

        public static List<ExtendedLevel> VanillaExtendedLevels
        {
            get
            {
                List<ExtendedLevel> list = new List<ExtendedLevel>();
                foreach (ExtendedLevel level in ExtendedLevels)
                    if (level.ContentType == ContentType.Vanilla)
                        list.Add(level);
                return (list);
            }
        }

        public static List<ExtendedLevel> CustomExtendedLevels
        {
            get
            {
                List<ExtendedLevel> list = new List<ExtendedLevel>();
                foreach (ExtendedLevel level in ExtendedLevels)
                    if (level.ContentType == ContentType.Custom)
                        list.Add(level);
                return (list);
            }
        }


        public static List<SelectableLevel> SelectableLevels
        {
            get
            {
                List<SelectableLevel> list = new List<SelectableLevel>();
                foreach (ExtendedLevel level in ExtendedLevels)
                    list.Add(level.SelectableLevel);
                return (list);
            }
        }

        public static List<SelectableLevel> MoonsCatalogue
        {
            get
            {
                List<SelectableLevel> list = new List<SelectableLevel>();
                foreach (SelectableLevel selectableLevel in OriginalContent.MoonsCatalogue)
                    list.Add(selectableLevel);
                foreach (ExtendedLevel level in ExtendedLevels)
                    if (level.ContentType == ContentType.Custom)
                        list.Add(level.SelectableLevel);
                return (list);
            }
        }



        public static List<ExtendedDungeonFlow> ExtendedDungeonFlows { get; internal set; } = new List<ExtendedDungeonFlow>();

        public static List<ExtendedDungeonFlow> VanillaExtendedDungeonFlows
        {
            get
            {
                List<ExtendedDungeonFlow> list = new List<ExtendedDungeonFlow>();
                foreach (ExtendedDungeonFlow dungeon in ExtendedDungeonFlows)
                    if (dungeon.ContentType == ContentType.Vanilla)
                        list.Add(dungeon);
                return (list);
            }
        }

        public static List<ExtendedDungeonFlow> CustomExtendedDungeonFlows
        {
            get
            {
                List<ExtendedDungeonFlow> list = new List<ExtendedDungeonFlow>();
                foreach (ExtendedDungeonFlow dungeon in ExtendedDungeonFlows)
                    if (dungeon.ContentType == ContentType.Custom)
                        list.Add(dungeon);
                return (list);
            }
        }



        public static List<ExtendedWeatherEffect> ExtendedWeatherEffects { get; internal set; } = new List<ExtendedWeatherEffect>();

        public static List<ExtendedWeatherEffect> VanillaExtendedWeatherEffects
        {
            get
            {
                List<ExtendedWeatherEffect> list = new List<ExtendedWeatherEffect>();
                foreach (ExtendedWeatherEffect effect in ExtendedWeatherEffects)
                    if (effect.contentType == ContentType.Vanilla)
                        list.Add(effect);
                return (list);
            }
        }

        public static List<ExtendedWeatherEffect> CustomExtendedWeatherEffects
        {
            get
            {
                List<ExtendedWeatherEffect> list = new List<ExtendedWeatherEffect>();
                foreach (ExtendedWeatherEffect effect in ExtendedWeatherEffects)
                    if (effect.contentType == ContentType.Custom)
                        list.Add(effect);
                return (list);
            }
        }



        public static List<ExtendedItem> ExtendedItems { get; internal set; } = new List<ExtendedItem>();

        public static List<ExtendedItem> CustomExtendedItems
        {
            get
            {
                List<ExtendedItem> returnList = new List<ExtendedItem>();
                foreach (ExtendedItem item in ExtendedItems)
                    if (item.ContentType == ContentType.Custom)
                        returnList.Add(item);
                return (returnList);
            }
        }



        public static List<ExtendedEnemyType> ExtendedEnemyTypes { get; internal set; } = new List<ExtendedEnemyType>();

        public static List<ExtendedEnemyType> CustomExtendedEnemyTypes
        {
            get
            {
                List<ExtendedEnemyType> returnList = new List<ExtendedEnemyType>();
                foreach (ExtendedEnemyType extendedEnemyType in ExtendedEnemyTypes)
                    if (extendedEnemyType.ContentType == ContentType.Custom)
                        returnList.Add(extendedEnemyType);
                return (returnList);
            }
        }

        public static List<ExtendedEnemyType> VanillaExtendedEnemyTypes
        {
            get
            {
                List<ExtendedEnemyType> returnList = new List<ExtendedEnemyType>();
                foreach (ExtendedEnemyType extendedEnemyType in ExtendedEnemyTypes)
                    if (extendedEnemyType.ContentType == ContentType.Vanilla)
                        returnList.Add(extendedEnemyType);
                return (returnList);
            }
        }


        public static List<ExtendedStoryLog> ExtendedStoryLogs { get; internal set; } = new List<ExtendedStoryLog>();

        public static List<ExtendedFootstepSurface> ExtendedFootstepSurfaces { get; internal set; } = new List<ExtendedFootstepSurface>();

        public static List<ExtendedBuyableVehicle> ExtendedBuyableVehicles { get; internal set; } = new List<ExtendedBuyableVehicle>();

        public static List<ExtendedBuyableVehicle> CustomExtendedBuyableVehicles
        {
            get
            {
                List<ExtendedBuyableVehicle> returnList = new List<ExtendedBuyableVehicle>();
                foreach (ExtendedBuyableVehicle extendedBuyableVehicle in ExtendedBuyableVehicles)
                    if (extendedBuyableVehicle.ContentType == ContentType.Custom)
                        returnList.Add(extendedBuyableVehicle);
                return (returnList);
            }
        }

        public static List<ExtendedBuyableVehicle> VanillaExtendedBuyableVehicles
        {
            get
            {
                List<ExtendedBuyableVehicle> returnList = new List<ExtendedBuyableVehicle>();
                foreach (ExtendedBuyableVehicle extendedBuyableVehicle in ExtendedBuyableVehicles)
                    if (extendedBuyableVehicle.ContentType == ContentType.Vanilla)
                        returnList.Add(extendedBuyableVehicle);
                return (returnList);
            }
        }


        public static List<AudioMixer> AudioMixers { get; internal set; } = new List<AudioMixer>();

        public static List<AudioMixerGroup> AudioMixerGroups { get; internal set; } = new List<AudioMixerGroup>();

        public static List<AudioMixerSnapshot> AudioMixerSnapshots { get; internal set; } = new List<AudioMixerSnapshot>();

        private static Dictionary<Type, IList> _extendedContentListsDict;
        internal static Dictionary<Type, IList> ExtendedContentsLists
        {
            get
            {
                if (_extendedContentListsDict == null)
                    PopulateExtendedContentsListDict();
                return (_extendedContentListsDict);
            }
        }


        public static void RegisterExtendedMod(ExtendedMod extendedMod)
        {
            DebugHelper.Log("Registering ExtendedMod: " + extendedMod.ModName + " Manually.", DebugType.Developer);
            ContentManager.RegisterExtendedMod(extendedMod);
        }

        internal static void SortExtendedMods()
        {
            ExtendedMods = new List<ExtendedMod>(ExtendedMods.OrderBy(o => o.ModName).ToList());

            foreach (ExtendedMod extendedMod in ExtendedMods)
                extendedMod.SortRegisteredContent();
        }

        internal static void PopulateContentDictionaries()
        {
            foreach (ExtendedContent extendedContent in ExtendedContents)
                TryAdd(UniqueIdentifiersDictionary, extendedContent.UniqueIdentificationName, extendedContent);
        }

        internal static void TryAdd<T1,T2>(Dictionary<T1, T2> dict, T1 key, T2 value)
        {
            if (!dict.ContainsKey(key))
                dict.Add(key, value);
            else
                DebugHelper.LogError("Could not add " + key.ToString() + " to dictionary.", DebugType.Developer);
        }

        public static bool TryGetExtendedContent(SelectableLevel selectableLevel, out ExtendedLevel extendedLevel)
        {
            return (ExtendedLevelDictionary.TryGetValue(selectableLevel, out extendedLevel));
        }

        public static bool TryGetExtendedContent(DungeonFlow dungeonFlow, out ExtendedDungeonFlow extendedDungeonFlow)
        {
            return (ExtendedDungeonFlowDictionary.TryGetValue(dungeonFlow, out extendedDungeonFlow));
        }

        public static bool TryGetExtendedContent(Item item, out ExtendedItem extendedItem)
        {
            return (ExtendedItemDictionary.TryGetValue(item, out extendedItem));
        }

        public static bool TryGetExtendedContent(EnemyType enemyType, out ExtendedEnemyType extendedEnemyType)
        {
            return (ExtendedEnemyTypeDictionary.TryGetValue(enemyType, out extendedEnemyType));
        }

        public static bool TryGetExtendedContent(BuyableVehicle buyableVehicle, out ExtendedBuyableVehicle extendedBuyableVehicle)
        {
            return (ExtendedBuyableVehicleDictionary.TryGetValue(buyableVehicle, out extendedBuyableVehicle));
        }

        public static bool TryGetExtendedContent<T>(string uniqueIdentifierName, out T extendedContent) where T : ExtendedContent
        {
            extendedContent = null;
            if (UniqueIdentifiersDictionary.TryGetValue(uniqueIdentifierName, out ExtendedContent result))
                extendedContent = result as T;
            return (extendedContent != null);
        }

        private static void PopulateExtendedContentsListDict()
        {
            _extendedContentListsDict = new Dictionary<Type, IList>();
            AddExtendedContentsListToDict(ExtendedLevels);
            AddExtendedContentsListToDict(ExtendedDungeonFlows);
            AddExtendedContentsListToDict(ExtendedItems);
            AddExtendedContentsListToDict(ExtendedEnemyTypes);
            AddExtendedContentsListToDict(ExtendedWeatherEffects);
            AddExtendedContentsListToDict(ExtendedStoryLogs);
            AddExtendedContentsListToDict(ExtendedFootstepSurfaces);
            AddExtendedContentsListToDict(ExtendedBuyableVehicles);
        }

        private static void AddExtendedContentsListToDict<T>(List<T> extendedContentsList) where T : ExtendedContent
        {
            _extendedContentListsDict.Add(typeof(T), extendedContentsList);
        }
    }

    public static class OriginalContent
    {
        //Levels

        public static List<SelectableLevel> SelectableLevels { get; internal set; } = new List<SelectableLevel>();

        public static List<SelectableLevel> MoonsCatalogue { get; internal set; } = new List<SelectableLevel>();

        //Dungeons

        public static List<DungeonFlow> DungeonFlows { get; internal set; } = new List<DungeonFlow>();

        //Items

        public static List<Item> Items { get; internal set; } = new List<Item>();

        public static List<ItemGroup> ItemGroups { get; internal set; } = new List<ItemGroup>();

        //Enemies

        public static List<EnemyType> Enemies { get; internal set; } = new List<EnemyType>();

        public static List<BuyableVehicle> BuyableVehicles { get; internal set; } = new List<BuyableVehicle>();

        //Spawnable Objects

        public static List<GameObject> SpawnableOutsideObjects { get; internal set; } = new List<GameObject>();

        public static List<GameObject> SpawnableMapObjects { get; internal set; } = new List<GameObject>();

        //Audio

        public static List<AudioMixer> AudioMixers { get; internal set; } = new List<AudioMixer>();

        public static List<AudioMixerGroup> AudioMixerGroups { get; internal set; } = new List<AudioMixerGroup>();

        public static List<AudioMixerSnapshot> AudioMixerSnapshots { get; internal set; } = new List<AudioMixerSnapshot>();

        public static List<LevelAmbienceLibrary> LevelAmbienceLibraries { get; internal set; } = new List<LevelAmbienceLibrary>();

        public static List<ReverbPreset> ReverbPresets { get; internal set; } = new List<ReverbPreset>();

        //Terminal

        public static List<TerminalKeyword> TerminalKeywords { get; internal set; } = new List<TerminalKeyword>();

        public static List<TerminalNode> TerminalNodes { get; internal set; } = new List<TerminalNode>();
    }
}