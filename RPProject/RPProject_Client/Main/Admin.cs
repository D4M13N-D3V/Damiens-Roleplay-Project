using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay.Main
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

        private async void SpawnCar(string car)
        {
            var ped = API.PlayerPedId();
            var ply = API.PlayerId();
            var vehicle = (uint)API.GetHashKey(car);
            API.RequestModel(vehicle);
            while (!API.HasModelLoaded(vehicle))
            {
                await Delay(1);
            }
            var coords = API.GetOffsetFromEntityInWorldCoords(ped, 0, 5.0f, 0);
            var spawnedCar = API.CreateVehicle(vehicle, coords.X, coords.Y, coords.Z, API.GetEntityHeading(ped), true,
                false);
            API.SetVehicleNumberPlateText(spawnedCar,"ADMIN LOL");
            API.SetVehicleOnGroundProperly(spawnedCar);
            API.SetModelAsNoLongerNeeded(vehicle);
        }

        private void TeleportToPlayer(dynamic ply)
        {
            var myPed = API.PlayerPedId();
            var otherPed = API.GetPlayerPed(API.GetPlayerFromServerId(Convert.ToInt16(ply)));
            var otherPedCoords = API.GetEntityCoords(otherPed,true);
            API.SetEntityCoords(myPed,otherPedCoords.X,otherPedCoords.Y,otherPedCoords.Z,false,false,false,false);
        }

        private void DeleteVehicle()
        {
            var myPed = API.PlayerPedId();

            if (API.IsPedInAnyVehicle(myPed, false))
            {
                var veh = API.GetVehiclePedIsIn(myPed, false);
                API.DeleteVehicle(ref veh);
                return;
            }

            var myCoords  = API.GetEntityCoords(myPed, true);
            var offset = API.GetOffsetFromEntityInWorldCoords(myPed, 0.0f, 20.0f, 0.0f);
            var rayHandle = API.CastRayPointToPoint(myCoords.X, myCoords.Y, myCoords.Z, myCoords.X, myCoords.Y,
                myCoords.Z, 10, myPed, 0);
            bool didHit = false;
            Vector3 startPos = new Vector3(0,0,0);
            Vector3 stopPos = new Vector3(0,0,0);
            int entityHit = 0;
            API.GetRaycastResult(rayHandle, ref didHit, ref startPos, ref stopPos, ref entityHit);

            if (entityHit != null)
            {
                API.DeleteVehicle(ref entityHit);
            }
                
        }
    }
}
