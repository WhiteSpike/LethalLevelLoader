namespace LethalLevelLoader.Data.Save
{
    public struct AllItemsListItemData
    {
        public string itemObjectName;
        public string itemName;
        public string modName;
        public string modAuthor;
        public int allItemsListIndex;
        public int modItemsListIndex;
        public int itemNameDuplicateIndex;
        public bool isScrap;
        public bool saveItemVariable;

        public AllItemsListItemData(string newItemObjectName, string newItemName, string newModName, string newModAuthor, int newAllItemsListIndex, int newModItemsListIndex, int newItemNameDuplicateIndex, bool newIsScrap, bool newSaveItemVariable)
        {
            itemObjectName = newItemObjectName;
            itemName = newItemName;
            modName = newModName;
            modAuthor = newModAuthor;
            allItemsListIndex = newAllItemsListIndex;
            modItemsListIndex = newModItemsListIndex;
            itemNameDuplicateIndex = newItemNameDuplicateIndex;
            isScrap = newIsScrap;
            saveItemVariable = newSaveItemVariable;
        }
    }
}
