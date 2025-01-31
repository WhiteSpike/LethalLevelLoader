using UnityEngine;

namespace LethalLevelLoader.Data.Save
{
    public struct ShipGrabbableItem
    {
        public int identifier;
        public Vector3 position;
        public int scrapValue;
        public int itemSaveData;

        public ShipGrabbableItem(int identifier, Vector3 position, int scrapValue, int itemSaveData)
        {
            this.identifier = identifier;
            this.position = position;
            this.scrapValue = scrapValue;
            this.itemSaveData = itemSaveData;
        }
    }
}
