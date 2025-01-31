namespace LethalLevelLoader
{
    public static class ItemManager
    {
        public static void RefreshDynamicItemRarityOnAllExtendedLevels()
        {
            foreach (ExtendedLevel extendedLevel in PatchedContent.ExtendedLevels)
                InjectCustomItemsIntoLevelViaDynamicRarity(extendedLevel);
        }
        public static void InjectCustomItemsIntoLevelViaDynamicRarity(ExtendedLevel extendedLevel, bool debugResults = false)
        {
            foreach (ExtendedItem extendedItem in PatchedContent.CustomExtendedItems)
            {
                if (!extendedItem.Item.isScrap) continue;

                string debugString = string.Empty;
                SpawnableItemWithRarity alreadyInjectedItem = null;
                foreach (SpawnableItemWithRarity spawnableItem in extendedLevel.SelectableLevel.spawnableScrap)
                {
                    if (spawnableItem.spawnableItem != extendedItem) continue;

                    alreadyInjectedItem = spawnableItem;
                    break;
                }

                int returnRarity = extendedItem.LevelMatchingProperties.GetDynamicRarity(extendedLevel);
                if (returnRarity <= 0)
                {
                    if (alreadyInjectedItem != null)
                    {
                        extendedLevel.SelectableLevel.spawnableScrap.Remove(alreadyInjectedItem);
                        debugString = "Removed " + extendedItem.Item.itemName + " From Planet: " + extendedLevel.NumberlessPlanetName;
                    }

                    if (debugResults)
                        DebugHelper.Log(debugString, DebugType.Developer);

                    continue;
                }

                if (alreadyInjectedItem != null)
                {
                    alreadyInjectedItem.rarity = returnRarity;
                    debugString = "Updated Rarity Of: " + extendedItem.Item.itemName + " To: " + returnRarity + " On Planet: " + extendedLevel.NumberlessPlanetName;
                }
                else
                {
                    SpawnableItemWithRarity newSpawnableItem = new SpawnableItemWithRarity();
                    newSpawnableItem.spawnableItem = extendedItem.Item;
                    newSpawnableItem.rarity = returnRarity;
                    extendedLevel.SelectableLevel.spawnableScrap.Add(newSpawnableItem);
                    debugString = "Added " + extendedItem.Item.itemName + " To Planet: " + extendedLevel.NumberlessPlanetName + " With A Rarity Of: " + returnRarity;
                }
                if (debugResults)
                    DebugHelper.Log(debugString, DebugType.Developer);
            }
        }
    }
}
