using LethalLevelLoader.Data.Levels;
using LethalLevelLoader.Data.Save;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LethalLevelLoader
{
    internal static class SaveManager
    {
        public static LLLSaveFile currentSaveFile;
        public static bool parityCheck;

        internal static void InitializeSave()
        {
            if (!LethalLevelLoaderNetworkManager.networkManager.IsServer)
                return;

            currentSaveFile = new LLLSaveFile();
            currentSaveFile.Load();

            if (currentSaveFile.CurrentLevelName != null)
                DebugHelper.Log("Initialized LLL Save File, Current Level Was: " + currentSaveFile.CurrentLevelName + ", Current Vanilla Save Is: " + GameNetworkManager.Instance.currentSaveFileName, DebugType.User);
            else
                DebugHelper.Log("Initialized LLL Save File, Current Level Was: (Empty) " + ", Current Vanilla Save Is: " + GameNetworkManager.Instance.currentSaveFileName, DebugType.User);

            if (ES3.KeyExists("CurrentPlanetID", GameNetworkManager.Instance.currentSaveFileName))
                DebugHelper.Log("Vanilla CurrentSaveFileName Has Saved Current Planet ID: " + ES3.Load<int>("CurrentPlanetID", GameNetworkManager.Instance.currentSaveFileName), DebugType.Developer);

            // Compare saved "Steps Taken" statistic, to try to check whether the Vanilla and LethalLevelLoader saves are the same
            int originalStepsTaken = ES3.Load("Stats_StepsTaken", GameNetworkManager.Instance.currentSaveFileName, 0);

            if (originalStepsTaken == currentSaveFile.parityStepsTaken)
                parityCheck = true;
            else
            {
                DebugHelper.Log("Vanilla Save File Mismatch, LLL Steps Taken: " + currentSaveFile.parityStepsTaken + ", Vanilla Steps Taken: " + originalStepsTaken, DebugType.Developer);

                currentSaveFile.Reset();
                currentSaveFile.parityStepsTaken = originalStepsTaken;

                parityCheck = false;
            }

            if (currentSaveFile.extendedLevelSaveData == null) return;

            foreach (ExtendedLevelData extendedLevelData in currentSaveFile.extendedLevelSaveData)
                LethalLevelLoaderNetworkManager.Instance.SetExtendedLevelValuesServerRpc(extendedLevelData);
        }

        internal static void SaveGameValues()
        {
            currentSaveFile.itemSaveData = GetAllItemsListItemDataDict();
            currentSaveFile.parityStepsTaken = Patches.StartOfRound.gameStats.allStepsTaken;

            SaveAllLevels();

            currentSaveFile.Save();
        }

        internal static void SaveAllLevels()
        {
            currentSaveFile.extendedLevelSaveData = new List<ExtendedLevelData>();
            foreach (ExtendedLevel extendedLevel in PatchedContent.ExtendedLevels)
                currentSaveFile.extendedLevelSaveData.Add(new ExtendedLevelData(extendedLevel));
        }

        internal static void LoadShipGrabbableItems()
        {
            if (!parityCheck)
                return;

            // TODO: Config option to disable this process preferably

            List<SavedShipItemData> loadedShipItemData = GetConstructedSavedShipItemData(currentSaveFile.itemSaveData);
            FixMismatchedSavedItemData(loadedShipItemData);
            OverrideCurrentSaveFileItemData(loadedShipItemData);
        }
        internal static void OverrideCurrentSaveFileItemData(List<SavedShipItemData> savedShipItemDatas)
        {
            List<ShipGrabbableItem> shipGrabbableItems = new List<ShipGrabbableItem>();

            string currentSaveFileName = GameNetworkManager.Instance.currentSaveFileName;

            foreach (SavedShipItemData savedShipItemData in savedShipItemDatas)
            {
                ShipGrabbableItem shipGrabbableItem = savedShipItemData.shipGrabbableItem;
                shipGrabbableItems.Add(shipGrabbableItem);
            }
            if (ES3.KeyExists("shipGrabbableItems", currentSaveFileName))
                ES3.DeleteKey("shipGrabbableItems", currentSaveFileName);

            if (shipGrabbableItems.Count > 0)
                ES3.Save("shipGrabbableItems", shipGrabbableItems, currentSaveFileName);
        }

        internal static void FixMismatchedSavedItemData(List<SavedShipItemData> savedShipItemDatas)
        {
            Dictionary<int, AllItemsListItemData> itemDataDict = GetAllItemsListItemDataDict();

            int firstMismatch = 0;

            foreach (SavedShipItemData savedShipItemData in savedShipItemDatas)
            {
                ShipGrabbableItem shipGrabbableItem = savedShipItemData.shipGrabbableItem;

                int allitemsListIndex = shipGrabbableItem.identifier;
                if (!itemDataDict.ContainsKey(shipGrabbableItem.identifier))
                    break;

                AllItemsListItemData itemData = savedShipItemData.itemAllItemsListData;
                AllItemsListItemData newItemData = itemDataDict[allitemsListIndex];

                if (newItemData.itemName != itemData.itemName)
                    break;

                firstMismatch++;
            }

            if (firstMismatch >= savedShipItemDatas.Count)
                return;

            for (int i = firstMismatch; i < savedShipItemDatas.Count; i++)
            {
                SavedShipItemData savedShipItemData = savedShipItemDatas[i];
                ShipGrabbableItem shipGrabbableItem = savedShipItemData.shipGrabbableItem;

                int oldIndex = shipGrabbableItem.identifier;
                int newIndex = FixAllItemsListIndex(savedShipItemData.itemAllItemsListData, itemDataDict);

                shipGrabbableItem.identifier = newIndex;

                if (!itemDataDict.ContainsKey(newIndex))
                {
                    DebugHelper.Log($"Removing Item: [ {savedShipItemData.itemAllItemsListData.modName} ][ {savedShipItemData.itemAllItemsListData.itemName} ][ {savedShipItemData.itemAllItemsListData.itemObjectName} ][ #{oldIndex}", DebugType.User);
                    shipGrabbableItem.scrapValue = -1;
                    shipGrabbableItem.itemSaveData = -1;
                    continue;
                }

                AllItemsListItemData newItemData = itemDataDict[newIndex];

                if (oldIndex != newIndex)
                {
                    DebugHelper.Log($"Fixing Item ┌ {savedShipItemData.itemAllItemsListData.modName} ┬ {savedShipItemData.itemAllItemsListData.itemName} ┬ {savedShipItemData.itemAllItemsListData.itemObjectName} ┬ #{oldIndex}", DebugType.User);
                    DebugHelper.Log($"     -----> └ {newItemData.modName                           } ┴ {newItemData.itemName                           } ┴ {newItemData.itemObjectName                           } ┴ #{newIndex}", DebugType.User);
                }

                savedShipItemData.itemAllItemsListData = newItemData;

                if (!newItemData.isScrap && shipGrabbableItem.scrapValue >= 0)
                    shipGrabbableItem.scrapValue = -1;
                else if (newItemData.isScrap && shipGrabbableItem.scrapValue == -1)
                    shipGrabbableItem.scrapValue = 0; // Could generate a fitting scrap value here if desired

                if (!newItemData.saveItemVariable && shipGrabbableItem.itemSaveData >= 0)
                    shipGrabbableItem.itemSaveData = -1;
                else if (newItemData.saveItemVariable && shipGrabbableItem.itemSaveData == -1)
                    shipGrabbableItem.itemSaveData = 0; // Might cause problems with some items but what are you gonna do
            }
        }

        internal static int FixAllItemsListIndex(AllItemsListItemData itemData, Dictionary<int, AllItemsListItemData> itemDataDict)
        {
            // Priorities (higher number = higher priority)

            // Variable definitions:
            //  itemObjectName         : The name of the item's asset file.
            //  itemName               : The name of the item (field set in the item's ScriptableObject).
            //  modName                : The name of the mod the item is from.
            //  allItemsListIndex      : The items list index of this item.
            //  modItemsListIndex      : The items list index of this item relative to the modName.
            //  itemNameDuplicateIndex : The duplicate index of the item's itemName relative to the modName, e.g. if a mod has two items named "Item A" this will be 1 for the second.

            // Formulas (matching variables):
            //  64 -> modAuthor                          (Only if priority is >= 4)
            //  32 -> modName                            (Only if priority is >= 4)
            //  16 -> itemName & itemNameDuplicateIndex  (Exclusive with below)
            //   8 -> itemName                           (Exclusive with above)
            //   4 -> itemObjectName                     (Minimum to not be removed)
            //   2 -> modItemsListIndex & modName        (Only if priority is < 4)
            //   1 -> allItemsListIndex                  (Only if priority is < 4)

            // Possible values:
            // 116 -> modAuthor & modName & itemName & itemNameDuplicateIndex & itemObjectName
            // 112 -> modAuthor & modName & itemName & itemNameDuplicateIndex
            // 108 -> modAuthor & modName & itemName & itemObjectName
            // 104 -> modAuthor & modName & itemName
            // 100 -> modAuthor & modName & itemObjectName
            //  84 -> modAuthor & itemName & itemNameDuplicateIndex & itemObjectName
            //  80 -> modAuthor & itemName & itemNameDuplicateIndex
            //  76 -> modAuthor & itemName & itemObjectName
            //  72 -> modAuthor & itemName
            //  68 -> modAuthor & itemObjectName
            //  52 -> modName & itemName & itemNameDuplicateIndex & itemObjectName
            //  48 -> modName & itemName & itemNameDuplicateIndex
            //  44 -> modName & itemName & itemObjectName 
            //  40 -> modName & itemName
            //  36 -> modName & itemObjectName
            //  20 -> itemName & itemNameDuplicateIndex & itemObjectName
            //  16 -> itemName & itemNameDuplicateIndex
            //  12 -> itemName & itemObjectName
            //   8 -> itemName
            //   4 -> itemObjectName
            // Low values (removed by default):
            //   3 -> modItemsListIndex & modName & allitemsListIndex
            //   2 -> modItemsListIndex & modName
            //   1 -> allItemsListIndex

            List<Item> allItemsList = Patches.StartOfRound.allItemsList.itemsList;

            int matchedPriority = 0;
            int matchedIndex = -1;

            for (int newIndex = 0; newIndex < allItemsList.Count; newIndex++)
            {
                if (!itemDataDict.ContainsKey(newIndex))
                    break;

                int currentPriority = 0;
                AllItemsListItemData newItemData = itemDataDict[newIndex];

                if (newItemData.itemName == itemData.itemName)
                    if (newItemData.itemNameDuplicateIndex == itemData.itemNameDuplicateIndex)
                        currentPriority += 16;
                    else
                        currentPriority += 8;

                if (newItemData.itemObjectName == itemData.itemObjectName)
                    currentPriority += 4;

                if (currentPriority >= 4)
                {
                    if (newItemData.modAuthor == itemData.modAuthor)
                        currentPriority += 64;

                    if (CompareModNames(newItemData.modName, itemData.modName))
                        currentPriority += 32;
                }
                else
                {
                    if (newItemData.modItemsListIndex == itemData.modItemsListIndex && CompareModNames(newItemData.modName, itemData.modName))
                        currentPriority += 2;

                    if (newItemData.allItemsListIndex == itemData.allItemsListIndex)
                        currentPriority += 1;
                }

                if (currentPriority > matchedPriority)
                {
                    matchedPriority = currentPriority;
                    matchedIndex = newIndex;

                    if (matchedPriority == 64 + 32 + 16 + 4) // Max value
                        return matchedIndex;
                }
            }

            // TODO: Config option to disable removing items that aren't name matched?  
            if (matchedPriority >= 4)
                return matchedIndex;
            else
                return int.MaxValue; // Use MaxValue since vanilla loading code checks upper bounds
        }

        internal static Dictionary<int, AllItemsListItemData> GetAllItemsListItemDataDict()
        {
            Dictionary<int, AllItemsListItemData> items = new Dictionary<int, AllItemsListItemData>();
            int counter = 0;
            foreach (Item item in Patches.StartOfRound.allItemsList.itemsList)
            {
                item.TryGetExtendedItemInfo(out string modName, out string modAuthor, out int modItemIndex);
                int itemNameDuplicateIndex = GetItemNameDuplicateIndex(item, modName);

                items.Add(counter, new AllItemsListItemData(item.name, item.itemName, modName, modAuthor, counter, modItemIndex, itemNameDuplicateIndex, item.isScrap, item.saveItemVariable));
                counter++;
            }

            return items;
        }

        internal static bool TryGetExtendedItemInfo(this Item item, out string modName, out string modAuthor, out int modItemIndex)
        {
            int lowestNameAliases = int.MaxValue;
            modName = "";
            modAuthor = "";
            modItemIndex = -1;

            foreach (ExtendedMod extendedMod in PatchedContent.ExtendedMods)
            {
                if (lowestNameAliases <= extendedMod.ModNameAliases.Count)
                    continue;

                int modCounter = 0;

                foreach (ExtendedItem extendedItem in extendedMod.ExtendedItems)
                {
                    if (extendedItem.Item == item)
                    {
                        modName = string.Join(';', extendedMod.ModNameAliases);
                        modAuthor = extendedMod.AuthorName;
                        modItemIndex = modCounter;

                        lowestNameAliases = extendedMod.ModNameAliases.Count;
                        break;
                    }
                    modCounter++;
                }
            }

            return modItemIndex > -1;
        }

        internal static int GetItemNameDuplicateIndex(Item item, string modName)
        {
            if (modName != "")
            {
                foreach (ExtendedMod extendedMod in PatchedContent.ExtendedMods)
                    if (CompareModNames(extendedMod.ModName, modName))
                    {
                        int modCounter = 0;

                        foreach (ExtendedItem extendedItem in extendedMod.ExtendedItems)
                            if (extendedItem.Item == item)
                                break;
                            else if (extendedItem.Item.itemName == item.itemName)
                                modCounter++;

                        return modCounter;
                    }

                return 0;
            }
            else
            {
                int counter = 0;

                foreach (Item newItem in Patches.StartOfRound.allItemsList.itemsList)
                {
                    if (newItem == item)
                        return counter;

                    if (newItem.itemName == item.itemName && !item.TryGetExtendedItemInfo(out _, out _, out _))
                        counter++;
                }

                return counter;
            }
        }

        internal static bool CompareModNames(string modNameA, string modNameB)
        {
            var modNamesA = modNameA.Split(';');
            var modNamesB = modNameB.Split(';');

            return modNamesA.Intersect(modNamesB).Any();
        }
        internal static List<AllItemsListItemData> GetAllItemsListItemDatas(List<ShipGrabbableItem> items, Dictionary<int, AllItemsListItemData> itemDataDict)
        {
            List<AllItemsListItemData> result = new List<AllItemsListItemData>();

            foreach (ShipGrabbableItem id in items)
            {
                if (itemDataDict.ContainsKey(id.identifier))
                    result.Add(itemDataDict[id.identifier]);
                else
                    result.Add(new AllItemsListItemData("", "", "", "", id.identifier, -1, 0, false, false));
            }

            return result;
        }

        internal static List<SavedShipItemData> GetConstructedSavedShipItemData(Dictionary<int, AllItemsListItemData> itemDataDict)
        {
            List<SavedShipItemData> result = new List<SavedShipItemData>();

            string currentSaveFileName = GameNetworkManager.Instance.currentSaveFileName;

            if (!ES3.KeyExists("shipGrabbableItems", currentSaveFileName))
            {
                return result;
            }

            List<ShipGrabbableItem> shipGrabbableItems = ES3.Load<ShipGrabbableItem[]>("shipGrabbableItems", currentSaveFileName).ToList();
            List<AllItemsListItemData> shipGrabbableItemData = GetAllItemsListItemDatas(shipGrabbableItems, itemDataDict);

            for (int i = 0; i < shipGrabbableItems.Count; i++)
            {
                int newGrabbableItemID = shipGrabbableItems[i].identifier;
                Vector3 newGrabbableItemPos = shipGrabbableItems[i].position;
                int newShipScrapValue = 0;
                int newShipItemSaveData = 0;

                AllItemsListItemData newGrabbableItemData = shipGrabbableItemData[i];

                if (newGrabbableItemData.isScrap && shipGrabbableItems[i].scrapValue != -1)
                {
                    newShipScrapValue = shipGrabbableItems[i].scrapValue;
                }

                if (newGrabbableItemData.saveItemVariable && shipGrabbableItems[i].itemSaveData != -1)
                {
                    newShipItemSaveData = shipGrabbableItems[i].itemSaveData;
                }

                result.Add(new SavedShipItemData(newGrabbableItemID, newGrabbableItemPos, newShipScrapValue, newShipItemSaveData, newGrabbableItemData));
            }

            return result;
        }
    }
}
