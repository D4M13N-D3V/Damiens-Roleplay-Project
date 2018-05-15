using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay.Main
{
    public class VehicleManager : BaseScript
    {
        public static VehicleManager Instance;

        public int car = -1;

        public VehicleManager()
        {
            Instance = this;
            EventHandlers["PullCar"] += new Action<string, string>(PullCar);
        }

        private async void PullCar(string model, string plate)
        {
            var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
            var ped = API.PlayerPedId();
            int nodeId = 0;
            Vector3 nodePos = new Vector3(0,0,0);
            API.GetRandomVehicleNode(playerPos.X,playerPos.Y,playerPos.Z,30.0f,false,false,false,ref nodePos,ref nodeId);
            var vehicle = (uint)API.GetHashKey(model);
            API.RequestModel(vehicle);
            while (!API.HasModelLoaded(vehicle))
            {
                await Delay(1);
            }
            var car = API.CreateVehicle(vehicle, nodePos.X, nodePos.Y, nodePos.Z, 0, true, false);
            API.SetVehicleNumberPlateText(car, plate);
            API.SetVehicleOnGroundProperly(car);

            var blip = API.AddBlipForEntity(car);
            API.SetBlipAsFriendly(blip, true);
            API.SetBlipSprite(blip, 225);
            API.SetBlipColour(blip, 3);
            API.BeginTextCommandSetBlipName("STRING");
            API.AddTextComponentString("Personal Car");
            API.EndTextCommandSetBlipName(blip);

            API.SetModelAsNoLongerNeeded(vehicle);
        }
    }
}
