using Unity.Netcode;

namespace LethalLevelLoader.Data.Levels
{
    public struct ExtendedLevelData : INetworkSerializable
    {
        public string UniqueIdentifier => uniqueIdentifier;
        public string uniqueIdentifier = string.Empty;
        public bool isHidden;
        public bool isLocked;


        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref uniqueIdentifier);
            serializer.SerializeValue(ref isHidden);
            serializer.SerializeValue(ref isLocked);
        }

        public ExtendedLevelData(ExtendedLevel extendedLevel)
        {
            uniqueIdentifier = extendedLevel.UniqueIdentificationName;
            isHidden = extendedLevel.IsRouteHidden;
            isLocked = extendedLevel.IsRouteLocked;
        }

        public void ApplySavedValues(ExtendedLevel extendedLevel)
        {
            extendedLevel.IsRouteHidden = isHidden;
            extendedLevel.IsRouteLocked = isLocked;
        }
    }
}
