using UnityEngine;

namespace LethalLevelLoader.Data.Save
{
    public struct SavedShipItemData
    {
        public ShipGrabbableItem shipGrabbableItem;
        public AllItemsListItemData itemAllItemsListData;

        public SavedShipItemData(int newItemAllItemsListIndex, Vector3 newItemPosition, int newItemScrapValue, int newItemAdditionalSavedData, AllItemsListItemData newItemAllItemsListData)
        {
            shipGrabbableItem = new(newItemAllItemsListIndex, newItemPosition, newItemScrapValue, newItemAdditionalSavedData);
            itemAllItemsListData = newItemAllItemsListData;
        }
    }
}
