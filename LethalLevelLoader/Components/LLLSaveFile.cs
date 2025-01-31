using LethalLevelLoader.Data.Levels;
using LethalLevelLoader.Data.Save;
using LethalModDataLib.Base;
using System.Collections.Generic;

namespace LethalLevelLoader
{
    public class LLLSaveFile : ModDataContainer
    {
        public string CurrentLevelName { get; internal set; } = string.Empty;

        public int parityStepsTaken;
        public Dictionary<int, AllItemsListItemData> itemSaveData = new Dictionary<int, AllItemsListItemData>();
        public List<ExtendedLevelData> extendedLevelSaveData = new List<ExtendedLevelData>();

        public LLLSaveFile()
        {
            //OptionalPrefixSuffix = name;
        }

        public void Reset()
        {
            CurrentLevelName = string.Empty;
            parityStepsTaken = 0;
            itemSaveData = new Dictionary<int, AllItemsListItemData>();
            extendedLevelSaveData = new List<ExtendedLevelData>();
        }
    }
}
