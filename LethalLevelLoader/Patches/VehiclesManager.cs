using System.Collections.Generic;
using System.Linq;

namespace LethalLevelLoader
{
    public static class VehiclesManager
    {
        internal static void PatchVanillaVehiclesLists()
        {
            Patches.Terminal.buyableVehicles = PatchedContent.ExtendedBuyableVehicles.Select(v => v.BuyableVehicle).ToArray();
            Patches.StartOfRound.VehiclesList = PatchedContent.ExtendedBuyableVehicles.Select(v => v.BuyableVehicle.vehiclePrefab).ToArray();
        }

        internal static void SetBuyableVehicleIDs()
        {
            foreach (ExtendedBuyableVehicle extendedBuyableVehicle in PatchedContent.ExtendedBuyableVehicles)
                extendedBuyableVehicle.VehicleID = -1;

            int vehicleID = 0;
            SetSellableVehicleIDs(PatchedContent.VanillaExtendedBuyableVehicles, ref vehicleID);
            SetSellableVehicleIDs(PatchedContent.CustomExtendedBuyableVehicles, ref vehicleID);
        }

        internal static void SetSellableVehicleIDs<T>(this List<T> collection, ref int vehicleID) where T : ExtendedBuyableVehicle
        {
            foreach (ExtendedBuyableVehicle extendedBuyableVehicle in collection)
            {
                extendedBuyableVehicle.VehicleID = vehicleID;

                if (extendedBuyableVehicle.BuyableVehicle.vehiclePrefab.TryGetComponent(out VehicleController vehicleController))
                    vehicleController.vehicleID = extendedBuyableVehicle.VehicleID;

                vehicleID++;
            }
        }
    }
}
