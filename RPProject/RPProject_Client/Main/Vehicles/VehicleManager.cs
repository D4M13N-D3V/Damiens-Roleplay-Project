using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace client.Main.Vehicles
{
    public class VehicleManager : BaseScript
    {
        public static VehicleManager Instance;

        public List<int> Cars = new List<int>();
        public bool isNearGarage = false;
        public VehicleManager()
        {

            API.DecorRegister("PIRP_VehicleOwner", 3);

            Instance = this;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            GarageCheck();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            SetupBlips();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            LowerTraffic();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            EventHandlers["PullCar"] += new Action<dynamic>(PullCar);
            EventHandlers["PutAwayCar"] += new Action<string>(PutAwayCar);
            EventHandlers["RepairCar"] += new Action(RepairCar);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            DrawMarkers();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }


        public bool OwnsCar(int vehHandle)
        {
            return API.DecorExistOn(vehHandle, "PIRP_VehicleOwner") && API.DecorGetInt(vehHandle, "PIRP_VehicleOwner") == Game.Player.ServerId;
        }

        private async Task LowerTraffic()
        {
            while (true)
            {
                API.SetRandomVehicleDensityMultiplierThisFrame(0.2f);
                API.SetVehicleDensityMultiplierThisFrame(0.2f);
                await Delay(0);
            }
        }

        private async void RepairCar()
        {
            if (Game.PlayerPed.IsInVehicle())
            {
                Utility.Instance.SendChatMessage("[ERROR]", "You can not repair vehicle while sitting in it.", 255, 0, 0);
                return;
            }

            if (Utility.Instance.GetDistanceBetweenVector3s(Game.PlayerPed.Position,
                    Utility.Instance.ClosestVehicle.Position) < 5 && API.GetVehicleDoorAngleRatio(Utility.Instance.ClosestVehicle.Handle, 4) != 0.0f)
            {
                Game.PlayerPed.Task.PlayAnimation("mini@repair", "fixing_a_player");
                await Delay(10000);
                Game.PlayerPed.Task.ClearAll();
                Utility.Instance.ClosestVehicle.EngineHealth = 1000;
                Utility.Instance.ClosestVehicle.IsEngineRunning = true;
                Utility.Instance.ClosestVehicle.Repair();
            }
        }

        #region Vehicle Garages
        private List<Vector3> _garages = new List<Vector3>()
        {
            new Vector3(-335.314972f,-773.932861f,33.3088455f),
            new Vector3(1190.778f,-1420.74365f,34.36095f),
            new Vector3(-1554.479f,-887.7129f,57.96044f),
            new Vector3(1644.71289f,3608.6062f,67.9901047f),
            new Vector3(133.553635f,6600.59f,31.8568935f),
            new Vector3(2127.9855957031f,4806.9521484375f,41.195922851563f),
            new Vector3(1687.9802246094f,3247.5837402344f,40.848697662354f),
            new Vector3(-1083.2529296875f,-2911.8312988281f,13.946918487549f),

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
            }
        }

        private async Task DrawMarkers()
        {
            while (true)
            {
                foreach (var pos in _garages)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(pos, Game.PlayerPed.Position) < 70)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, pos - new Vector3(0, 0, 0.8f), Vector3.Zero, Vector3.Zero, new Vector3(4, 4, 4), Color.FromArgb(255, 255, 255, 0));
                    }
                }
                await Delay(0);
            }
        }
        private async Task GarageCheck()
        {
            while (true)
            {
                var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
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
            if (!Cars.Any())
            {
                return false;
            }
            return true;
        }

        private void PutAwayCar(string plate)
        {
            var ped = Game.PlayerPed.Handle;
            if (isNearGarage && HasCarOut() && API.IsPedInAnyVehicle(ped, false) && OwnsCar(Game.PlayerPed.CurrentVehicle.Handle))
            {
                Utility.Instance.SendChatMessage("[VEHICLE MANAGER]", "You have put away your vehicle! [" + plate + "]", 0, 150, 40);
                var handle = Game.PlayerPed.CurrentVehicle.Handle;
                Vehicle.FromHandle(handle).Delete();
                TriggerServerEvent("SetCarStatus", plate, 0);
            }
        }

        private async void PullCar(dynamic carObj)
        {
            var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
            Debug.WriteLine(carObj.Model);
            var vehicle = (uint)API.GetHashKey(Convert.ToString(carObj.Model));
            API.RequestModel(vehicle);
            while (!API.HasModelLoaded(vehicle))
            {
                await Delay(1);
            }

            var id = API.CreateVehicle(vehicle, playerPos.X, playerPos.Y, playerPos.Z, 0, true, false);
            API.DecorSetInt(id, "PIRP_VehicleOwner", Game.Player.ServerId);
            API.SetEntityAsMissionEntity(id, true, true);
            API.SetVehicleHasBeenOwnedByPlayer(id, true);
            API.SetEntityHeading(id, API.GetEntityHeading(Game.PlayerPed.Handle));
            API.TaskWarpPedIntoVehicle(Game.PlayerPed.Handle, id, -1);
            API.SetVehicleNumberPlateText(id, Convert.ToString(carObj.Plate));
            API.SetVehicleOnGroundProperly(id);

            var blip = API.AddBlipForEntity(id);
            API.SetBlipAsFriendly(blip, true);
            API.SetBlipSprite(blip, 225);
            API.SetBlipColour(blip, 3);
            API.BeginTextCommandSetBlipName("STRING");
            API.AddTextComponentString("Personal Car");
            API.EndTextCommandSetBlipName(blip);

            #region Set Vehicle Mods
            API.SetVehicleModKit(id, 0);
            API.SetVehicleColours(id, Convert.ToInt16(carObj.ColorPrimary), Convert.ToInt16(carObj.ColorSecondary));

            API.SetVehicleNeonLightEnabled(id, 0, carObj.LeftNeons);
            API.SetVehicleNeonLightEnabled(id, 1, carObj.RightNeons);
            API.SetVehicleNeonLightEnabled(id, 2, carObj.FrontNeons);
            API.SetVehicleNeonLightEnabled(id, 3, carObj.BackNeons);

            API.SetVehicleNeonLightsColour(id, 255, 255, 255);// NEEDS TO BE FIXED

            API.SetVehicleWindowTint(id, Convert.ToInt16(carObj.WindowTint));
            API.SetVehicleWheelType(id, Convert.ToInt16(carObj.WheelType));

            API.SetVehicleMod(id, 0, carObj.VehicleMod1, false);
            API.SetVehicleMod(id, 1, carObj.VehicleMod2, false);
            API.SetVehicleMod(id, 2, carObj.VehicleMod3, false);
            API.SetVehicleMod(id, 3, carObj.VehicleMod4, false);
            API.SetVehicleMod(id, 4, carObj.VehicleMod5, false);
            API.SetVehicleMod(id, 5, carObj.VehicleMod6, false);
            API.SetVehicleMod(id, 6, carObj.VehicleMod7, false);
            API.SetVehicleMod(id, 7, carObj.VehicleMod8, false);
            API.SetVehicleMod(id, 8, carObj.VehicleMod9, false);
            API.SetVehicleMod(id, 9, carObj.VehicleMod10, false);
            API.SetVehicleMod(id, 10, carObj.VehicleMod11, false);
            API.SetVehicleMod(id, 11, carObj.VehicleMod12, false);
            API.SetVehicleMod(id, 12, carObj.VehicleMod13, false);
            API.SetVehicleMod(id, 13, carObj.VehicleMod14, false);
            API.SetVehicleMod(id, 14, carObj.VehicleMod15, false);
            API.SetVehicleMod(id, 15, carObj.VehicleMod16, false);
            API.SetVehicleMod(id, 16, carObj.VehicleMod17, false);
            API.SetVehicleMod(id, 17, carObj.VehicleMod18, false);
            API.SetVehicleMod(id, 18, carObj.VehicleMod19, false);
            API.SetVehicleMod(id, 19, carObj.VehicleMod20, false);
            API.SetVehicleMod(id, 20, carObj.VehicleMod21, false);
            API.SetVehicleMod(id, 21, carObj.VehicleMod22, false);
            API.SetVehicleMod(id, 22, carObj.VehicleMod23, false);
            API.SetVehicleMod(id, 23, carObj.VehicleMod24, false);
            API.SetVehicleMod(id, 24, carObj.VehicleMod25, false);
            API.SetVehicleMod(id, 25, carObj.VehicleMod26, false);
            API.SetVehicleMod(id, 26, carObj.VehicleMod27, false);
            API.SetVehicleMod(id, 27, carObj.VehicleMod28, false);
            API.SetVehicleMod(id, 28, carObj.VehicleMod29, false);
            API.SetVehicleMod(id, 29, carObj.VehicleMod30, false);
            API.SetVehicleMod(id, 30, carObj.VehicleMod31, false);
            API.SetVehicleMod(id, 31, carObj.VehicleMod32, false);
            API.SetVehicleMod(id, 32, carObj.VehicleMod33, false);
            API.SetVehicleMod(id, 33, carObj.VehicleMod34, false);
            API.SetVehicleMod(id, 34, carObj.VehicleMod35, false);
            API.SetVehicleMod(id, 35, carObj.VehicleMod36, false);
            API.SetVehicleMod(id, 36, carObj.VehicleMod37, false);
            API.SetVehicleMod(id, 37, carObj.VehicleMod38, false);
            API.SetVehicleMod(id, 38, carObj.VehicleMod39, false);
            API.SetVehicleMod(id, 39, carObj.VehicleMod40, false);
            API.SetVehicleMod(id, 40, carObj.VehicleMod41, false);
            API.SetVehicleMod(id, 41, carObj.VehicleMod42, false);
            API.SetVehicleMod(id, 42, carObj.VehicleMod43, false);
            API.SetVehicleMod(id, 43, carObj.VehicleMod44, false);
            API.SetVehicleMod(id, 44, carObj.VehicleMod45, false);
            API.SetVehicleMod(id, 45, carObj.VehicleMod46, false);
            API.SetVehicleMod(id, 46, carObj.VehicleMod47, false);
            API.SetVehicleMod(id, 47, carObj.VehicleMod48, false);
            API.SetVehicleMod(id, 48, carObj.VehicleMod49, false);
            API.SetVehicleMod(id, 49, carObj.VehicleMod50, false);
            #endregion
            API.SetModelAsNoLongerNeeded(vehicle);

            Utility.Instance.SendChatMessage("[VEHICLE MANAGER]", "You have pulled out your vehicle! [" + carObj.Plate + "]", 0, 150, 40);
        }


    }
}
