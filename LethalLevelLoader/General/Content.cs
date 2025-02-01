using DunGen.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace LethalLevelLoader
{
    public static class PatchedContent
    {
        public static ExtendedMod VanillaMod { get; internal set; }

        public static List<string> AllLevelSceneNames { get; internal set; } = new List<string>();

        public static List<ExtendedMod> ExtendedMods { get; internal set; } = new List<ExtendedMod>();
        internal static Dictionary<string, ExtendedContent> UniqueIdentifiersDictionary = new Dictionary<string, ExtendedContent>();
        internal static Dictionary<SelectableLevel, ExtendedLevel> ExtendedLevelDictionary = new Dictionary<SelectableLevel, ExtendedLevel>();
        internal static Dictionary<DungeonFlow, ExtendedDungeonFlow> ExtendedDungeonFlowDictionary = new Dictionary<DungeonFlow, ExtendedDungeonFlow>();
        internal static Dictionary<Item, ExtendedItem> ExtendedItemDictionary = new Dictionary<Item, ExtendedItem>();
        internal static Dictionary<EnemyType, ExtendedEnemyType> ExtendedEnemyTypeDictionary = new Dictionary<EnemyType, ExtendedEnemyType>();
        internal static Dictionary<BuyableVehicle, ExtendedBuyableVehicle> ExtendedBuyableVehicleDictionary = new Dictionary<BuyableVehicle, ExtendedBuyableVehicle>();


        public static List<ExtendedLevel> ExtendedLevels { get; internal set; } = new List<ExtendedLevel>();

        internal static List<T> GetFilteredExtendedContent<T>(this List<T> collection, ContentType contentType) where T : ExtendedContent
        {
            if (contentType == ContentType.Any) return new List<T>(collection);

            List <T> list = new List<T>();
            foreach (T content in collection)
            {
                    if (content.ContentType == contentType)
                        list.Add(content);
                    break;
            }
            return (list);
        }
        internal static List<SelectableLevel> GetSelectableLevels(ContentType contentType)
        {
            List<SelectableLevel> list = new List<SelectableLevel>();
            foreach (ExtendedLevel level in ExtendedLevels)
            {
                switch (contentType)
                {
                    case ContentType.Any: list.Add(level.SelectableLevel); break;
                    default:
                        {
                            if (level.ContentType == contentType)
                                list.Add(level.SelectableLevel);
                            break;
                        }
                }
            }
            return (list);
        }

        public static List<ExtendedLevel> VanillaExtendedLevels
        {
            get
            {
                return ExtendedLevels.GetFilteredExtendedContent(ContentType.Vanilla);
            }
        }

        public static List<ExtendedLevel> CustomExtendedLevels
        {
            get
            {
                return ExtendedLevels.GetFilteredExtendedContent(ContentType.Custom);
            }
        }

        [Obsolete("Use PatchedContent.SelectableLevels instead.")] // probably used by no mod, but this is public so we should be careful
        public static List<SelectableLevel> SeletectableLevels
        {
            get { return (SelectableLevels); }
        }



        public static List<SelectableLevel> SelectableLevels
        {
            get
            {
                return GetSelectableLevels(ContentType.Any);
            }
        }

        public static List<SelectableLevel> MoonsCatalogue
        {
            get
            {
                List<SelectableLevel> list = new List<SelectableLevel>(OriginalContent.MoonsCatalogue);
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
                return ExtendedDungeonFlows.GetFilteredExtendedContent(ContentType.Vanilla);
            }
        }

        public static List<ExtendedDungeonFlow> CustomExtendedDungeonFlows
        {
            get
            {
                return ExtendedDungeonFlows.GetFilteredExtendedContent(ContentType.Custom);
            }
        }



        public static List<ExtendedWeatherEffect> ExtendedWeatherEffects { get; internal set; } = new List<ExtendedWeatherEffect>();

        public static List<ExtendedWeatherEffect> VanillaExtendedWeatherEffects
        {
            get
            {
                return ExtendedWeatherEffects.GetFilteredExtendedContent(ContentType.Vanilla);
            }
        }

        public static List<ExtendedWeatherEffect> CustomExtendedWeatherEffects
        {
            get
            {
                return ExtendedWeatherEffects.GetFilteredExtendedContent(ContentType.Custom);
            }
        }



        public static List<ExtendedItem> ExtendedItems { get; internal set; } = new List<ExtendedItem>();

        public static List<ExtendedItem> CustomExtendedItems
        {
            get
            {
                return ExtendedItems.GetFilteredExtendedContent(ContentType.Custom);
            }
        }



        public static List<ExtendedEnemyType> ExtendedEnemyTypes { get; internal set; } = new List<ExtendedEnemyType>();

        public static List<ExtendedEnemyType> CustomExtendedEnemyTypes
        {
            get
            {
                return ExtendedEnemyTypes.GetFilteredExtendedContent(ContentType.Custom);
            }
        }

        public static List<ExtendedEnemyType> VanillaExtendedEnemyTypes
        {
            get
            {
                return ExtendedEnemyTypes.GetFilteredExtendedContent(ContentType.Vanilla);
            }
        }

        public static List<ExtendedBuyableVehicle> ExtendedBuyableVehicles { get; internal set; } = new List<ExtendedBuyableVehicle>();

        public static List<ExtendedBuyableVehicle> CustomExtendedBuyableVehicles
        {
            get
            {
                return ExtendedBuyableVehicles.GetFilteredExtendedContent(ContentType.Custom);
            }
        }

        public static List<ExtendedBuyableVehicle> VanillaExtendedBuyableVehicles
        {
            get
            {
                return ExtendedBuyableVehicles.GetFilteredExtendedContent(ContentType.Vanilla);
            }
        }


        public static List<AudioMixer> AudioMixers { get; internal set; } = new List<AudioMixer>();

        public static List<AudioMixerGroup> AudioMixerGroups { get; internal set; } = new List<AudioMixerGroup>();

        public static List<AudioMixerSnapshot> AudioMixerSnapshots { get; internal set; } = new List<AudioMixerSnapshot>();


        //Items

        public static List<Item> Items { get; internal set; } = new List<Item>();

        //Enemies

        public static List<EnemyType> Enemies { get; internal set; } = new List<EnemyType>();


        public static void RegisterExtendedDungeonFlow(ExtendedDungeonFlow extendedDungeonFlow)
        {
            extendedDungeonFlow.ConvertObsoleteValues();
            if (string.IsNullOrEmpty(extendedDungeonFlow.name))
            {
                DebugHelper.LogWarning("Tried to register ExtendedDungeonFlow with missing name! Setting to DungeonFlow name for safety!", DebugType.Developer);
                extendedDungeonFlow.name = extendedDungeonFlow.DungeonFlow.name;
            }
            //AssetBundleLoader.RegisterNewExtendedContent(extendedDungeonFlow, extendedDungeonFlow.name);
            LethalBundleManager.RegisterNewExtendedContent(extendedDungeonFlow, null);
        }

        public static void RegisterExtendedLevel(ExtendedLevel extendedLevel)
        {
            //AssetBundleLoader.RegisterNewExtendedContent(extendedLevel, extendedLevel.name);
            LethalBundleManager.RegisterNewExtendedContent(extendedLevel, null);
        }

        public static void RegisterExtendedMod(ExtendedMod extendedMod)
        {
            DebugHelper.Log("Registering ExtendedMod: " + extendedMod.ModName + " Manually.", DebugType.IAmBatby);
            //AssetBundleLoader.RegisterExtendedMod(extendedMod);
            LethalBundleManager.RegisterExtendedMod(extendedMod, null);
        }

        internal static void SortExtendedMods()
        {
            ExtendedMods = new List<ExtendedMod>(ExtendedMods.OrderBy(o => o.ModName).ToList());

            foreach (ExtendedMod extendedMod in ExtendedMods)
            {
                extendedMod.SortRegisteredContent();
            }
        }

        internal static void PopulateContentDictionaries()
        {
            foreach (ExtendedLevel extendedLevel in ExtendedLevels)
            {
                TryAdd(ExtendedLevelDictionary, extendedLevel.SelectableLevel, extendedLevel);
                TryAddUUID(extendedLevel);
            }
            foreach (ExtendedDungeonFlow extendedDungeonFlow in ExtendedDungeonFlows)
            {
                TryAdd(ExtendedDungeonFlowDictionary, extendedDungeonFlow.DungeonFlow, extendedDungeonFlow);
                TryAddUUID(extendedDungeonFlow);
            }
            foreach (ExtendedItem extendedItem in ExtendedItems)
            {
                TryAdd(ExtendedItemDictionary, extendedItem.Item, extendedItem);
                TryAddUUID(extendedItem);
            }
            foreach (ExtendedEnemyType extendedEnemyType in ExtendedEnemyTypes)
            {
                TryAdd(ExtendedEnemyTypeDictionary, extendedEnemyType.EnemyType, extendedEnemyType);
                TryAddUUID(extendedEnemyType);
            }
            foreach (ExtendedBuyableVehicle extendedBuyableVehicle in ExtendedBuyableVehicles)
            {
                TryAdd(ExtendedBuyableVehicleDictionary, extendedBuyableVehicle.BuyableVehicle, extendedBuyableVehicle);
                TryAddUUID(extendedBuyableVehicle);
            }
        }

        internal static void TryAddUUID(ExtendedContent extendedContent)
        {
            TryAdd(UniqueIdentifiersDictionary, extendedContent.UniqueIdentificationName, extendedContent);
        }

        internal static bool TryAdd<T1,T2>(Dictionary<T1, T2> dict, T1 key, T2 value)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);
                return (true);
            }
            else
            {
                DebugHelper.LogError("Could not add " + key.ToString() + " to dictionary.", DebugType.Developer);
                return (false);
            }

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
    }

    public static class OriginalContent
    {
        public static StartOfRound StartOfRound => Patches.StartOfRound;
        public static RoundManager RoundManager => Patches.RoundManager;
        public static Terminal Terminal => Patches.Terminal;
        public static TimeOfDay TimeOfDay => Patches.TimeOfDay;
        
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

        //Spawnable Objects

        public static List<SpawnableOutsideObject> SpawnableOutsideObjects { get; internal set; } = new List<SpawnableOutsideObject>();

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
