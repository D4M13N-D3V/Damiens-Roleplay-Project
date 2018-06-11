using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace client.Main
{
    public class Admin : BaseScript
    {
        public Admin Instance;

        public Admin()
        {
            Instance = this;
            EventHandlers["AdminSpawnCar"] += new Action<string>(SpawnCar);
            EventHandlers["TeleportToPlayer"] += new Action<dynamic>(TeleportToPlayer);
            EventHandlers["DeleteVehicle"] += new Action(DeleteVehicle);
        }

        private void SpawnCar(string car)
        {
            Utility.Instance.SpawnCar(car, i =>
            {
                API.SetVehicleNumberPlateText(i, "ADMIN");
            } );
        }

        private void TeleportToPlayer(dynamic ply)
        {
            var myPed = Game.PlayerPed.Handle;
            var otherPed = API.GetPlayerPed(API.GetPlayerFromServerId(Convert.ToInt16(ply)));
            var otherPedCoords = API.GetEntityCoords(otherPed,true);
            API.SetEntityCoords(myPed,otherPedCoords.X,otherPedCoords.Y,otherPedCoords.Z,false,false,false,false);
        }

        private void DeleteVehicle()
        {
            var myPed = Game.PlayerPed.Handle;

            if (API.IsPedInAnyVehicle(myPed, false))
            {
                var veh = API.GetVehiclePedIsIn(myPed, false);
                API.DeleteVehicle(ref veh);
                return;
            }
            Utility.Instance.ClosestVehicle.Delete();
        }
    }
}
