using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay.Main.Vehicles
{
    public class VehicleManager : BaseScript
    {
        public static VehicleManager Instance;

        public int car = -1;
        public bool isNearGarage = false;
        public VehicleManager()
        {
            Instance = this;
            GarageCheck();
            SetupBlips();
            EventHandlers["PullCar"] += new Action<dynamic>(PullCar);
            EventHandlers["PutAwayCar"] += new Action<string>(PutAwayCar);
        }

        #region Vehicle Garages
        private List<Vector3> _garages = new List<Vector3>()
        {
            new Vector3(-335.314972f,-773.932861f,33.3088455f),
            new Vector3(1190.778f,-1420.74365f,34.36095f),
            new Vector3(-1554.479f,-887.7129f,57.96044f),
            new Vector3(1644.71289f,3608.6062f,67.9901047f),
            new Vector3(133.553635f,6600.59f,31.8568935f)
        };
        #endregion

        private void SetupBlips()
        {
            foreach (Vector3 garage in _garages)
            {
                var blip = API.AddBlipForCoord(garage.X, garage.Y, garage.Z);
                API.SetBlipSprite(blip, 357);
                API.SetBlipColour(blip, 2);
                API.SetBlipScale(blip, 0.7f);
                API.SetBlipAsShortRange(blip, true);
                API.BeginTextCommandSetBlipName("STRING");
                API.AddTextComponentString("Vehicle Garage");
                API.EndTextCommandSetBlipName(blip);

                var blip2 = API.AddBlipForCoord(garage.X, garage.Y, garage.Z);
                API.SetBlipSprite(blip2, 161);
                API.SetBlipColour(blip2, 2);
                API.SetBlipScale(blip2, 0.7f);
                API.SetBlipAsShortRange(blip, true);
                API.BeginTextCommandSetBlipName("STRING");
                API.AddTextComponentString("Vehicle Garage");
                API.EndTextCommandSetBlipName(blip2);
            }
        }

        private async void GarageCheck()
        {
            while (true)
            {
                var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
                isNearGarage = false;
                foreach (Vector3 garage in _garages)
                {
                    if (API.Vdist(playerPos.X, playerPos.Y, playerPos.Z, garage.X, garage.Y, garage.Z) < 20)
                    {
                        isNearGarage = true;
                    }
                }
                await Delay(0);
            }
        }

        private bool HasCarOut()
        {
            if (car == -1)
            {
                return false;
            }
            return true;
        }

        private void PutAwayCar(string plate)
        {
            var ped = API.PlayerPedId();
            if (isNearGarage && HasCarOut() && API.GetVehicleNumberPlateText(car) == plate && API.IsPedInAnyVehicle(ped,false) && API.GetVehiclePedIsIn(ped,false)==car)
            {
                Utility.Instance.SendChatMessage("[VEHICLE MANAGER]","You have put away your vehicle! ["+plate+"]",0,150,40);
                API.DeleteVehicle(ref car);
                TriggerServerEvent("SetCarStatus", plate, 0);
                car = -1;
            }
        }

        private async void PullCar(dynamic carObj)
        {
            var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
            var vehicle = (uint)API.GetHashKey(Convert.ToString(carObj.Model));
            API.RequestModel(vehicle);
            while (!API.HasModelLoaded(vehicle))
            {
                await Delay(1);
            }
            car = API.CreateVehicle(vehicle, playerPos.X, playerPos.Y, playerPos.Z, 0, true, false);
            API.SetEntityAsMissionEntity(car,true,true);
            API.SetEntityHeading(car,API.GetEntityHeading(API.PlayerPedId()));
            API.TaskWarpPedIntoVehicle(API.PlayerPedId(),car,-1);
            API.TaskWarpPedIntoVehicle(API.PlayerPedId(),car,-1);
            API.SetVehicleNumberPlateText(car, Convert.ToString(carObj.Plate));
            API.SetVehicleOnGroundProperly(car);

            var blip = API.AddBlipForEntity(car);
            API.SetBlipAsFriendly(blip, true);
            API.SetBlipSprite(blip, 225);
            API.SetBlipColour(blip, 3);
            API.BeginTextCommandSetBlipName("STRING");
            API.AddTextComponentString("Personal Car");
            API.EndTextCommandSetBlipName(blip);
    
            #region Set Vehicle Mods
            API.SetVehicleModKit(car,0);
            API.SetVehicleColours(car, Convert.ToInt16(carObj.ColorPrimary), Convert.ToInt16(carObj.ColorSecondary));

            API.SetVehicleNeonLightEnabled(car, 0, carObj.LeftNeons);
            API.SetVehicleNeonLightEnabled(car, 1, carObj.RightNeons);
            API.SetVehicleNeonLightEnabled(car, 2, carObj.FrontNeons);
            API.SetVehicleNeonLightEnabled(car, 3, carObj.BackNeons);
            
            API.SetVehicleNeonLightsColour(car,255,255,255);// NEEDS TO BE FIXED

            API.SetVehicleWindowTint(car, Convert.ToInt16(carObj.WindowTint));
            API.SetVehicleWheelType(car, Convert.ToInt16(carObj.WheelType));

            API.SetVehicleMod(car, 0, carObj.VehicleMod1, false);
            API.SetVehicleMod(car, 1, carObj.VehicleMod2, false);
            API.SetVehicleMod(car, 2, carObj.VehicleMod3, false);
            API.SetVehicleMod(car, 3, carObj.VehicleMod4, false);
            API.SetVehicleMod(car, 4, carObj.VehicleMod5, false);
            API.SetVehicleMod(car, 5, carObj.VehicleMod6, false);
            API.SetVehicleMod(car, 6, carObj.VehicleMod7, false);
            API.SetVehicleMod(car, 7, carObj.VehicleMod8, false);
            API.SetVehicleMod(car, 8, carObj.VehicleMod9, false);
            API.SetVehicleMod(car, 9, carObj.VehicleMod10, false);
            API.SetVehicleMod(car, 10, carObj.VehicleMod11, false);
            API.SetVehicleMod(car, 11, carObj.VehicleMod12, false);
            API.SetVehicleMod(car, 12, carObj.VehicleMod13, false);
            API.SetVehicleMod(car, 13, carObj.VehicleMod14, false);
            API.SetVehicleMod(car, 14, carObj.VehicleMod15, false);
            API.SetVehicleMod(car, 15, carObj.VehicleMod16, false);
            API.SetVehicleMod(car, 16, carObj.VehicleMod17, false);
            API.SetVehicleMod(car, 17, carObj.VehicleMod18, false);
            API.SetVehicleMod(car, 18, carObj.VehicleMod19, false);
            API.SetVehicleMod(car, 19, carObj.VehicleMod20, false);
            API.SetVehicleMod(car, 20, carObj.VehicleMod21, false);
            API.SetVehicleMod(car, 21, carObj.VehicleMod22, false);
            API.SetVehicleMod(car, 22, carObj.VehicleMod23, false);
            API.SetVehicleMod(car, 23, carObj.VehicleMod24, false);
            API.SetVehicleMod(car, 24, carObj.VehicleMod25, false);
            API.SetVehicleMod(car, 25, carObj.VehicleMod26, false);
            API.SetVehicleMod(car, 26, carObj.VehicleMod27, false);
            API.SetVehicleMod(car, 27, carObj.VehicleMod28, false);
            API.SetVehicleMod(car, 28, carObj.VehicleMod29, false);
            API.SetVehicleMod(car, 29, carObj.VehicleMod30, false);
            API.SetVehicleMod(car, 30, carObj.VehicleMod31, false);
            API.SetVehicleMod(car, 31, carObj.VehicleMod32, false);
            API.SetVehicleMod(car, 32, carObj.VehicleMod33, false);
            API.SetVehicleMod(car, 33, carObj.VehicleMod34, false);
            API.SetVehicleMod(car, 34, carObj.VehicleMod35, false);
            API.SetVehicleMod(car, 35, carObj.VehicleMod36, false);
            API.SetVehicleMod(car, 36, carObj.VehicleMod37, false);
            API.SetVehicleMod(car, 37, carObj.VehicleMod38, false);
            API.SetVehicleMod(car, 38, carObj.VehicleMod39, false);
            API.SetVehicleMod(car, 39, carObj.VehicleMod40, false);
            API.SetVehicleMod(car, 40, carObj.VehicleMod41, false);
            API.SetVehicleMod(car, 41, carObj.VehicleMod42, false);
            API.SetVehicleMod(car, 42, carObj.VehicleMod43, false);
            API.SetVehicleMod(car, 43, carObj.VehicleMod44, false);
            API.SetVehicleMod(car, 44, carObj.VehicleMod45, false);
            API.SetVehicleMod(car, 45, carObj.VehicleMod46, false);
            API.SetVehicleMod(car, 46, carObj.VehicleMod47, false);
            API.SetVehicleMod(car, 47, carObj.VehicleMod48, false);
            API.SetVehicleMod(car, 48, carObj.VehicleMod49, false);
            API.SetVehicleMod(car, 49, carObj.VehicleMod50, false);
            #endregion
            API.SetModelAsNoLongerNeeded(vehicle);

            Utility.Instance.SendChatMessage("[VEHICLE MANAGER]", "You have pulled out your vehicle! [" + carObj.Plate + "]", 0, 150, 40);
        }


    }
}
