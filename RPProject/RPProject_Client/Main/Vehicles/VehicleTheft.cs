using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay.Main.Vehicles
{
    public class VehicleTheft : BaseScript
    {
        public static VehicleTheft Instance;

        public VehicleTheft()   
        {
            EventHandlers["HotwireCar"] += new Action(Hotwire);
            Instance = this;
        }

        private bool _canHotwire = false;

        public async void Hotwire()
        {
            if (API.IsPedInAnyVehicle(API.PlayerPedId(),false) && API.GetIsVehicleEngineRunning(API.GetVehiclePedIsIn(API.PlayerPedId(), false)))
            {
                if (!_canHotwire)
                {
                    return;
                };

                var veh = API.GetVehiclePedIsIn(API.PlayerPedId(), false);
                var random = new Random();
                var rdm = random.Next(0, 3);
                if (rdm == 2)
                {
                    _canHotwire = true;
                    API.SetVehicleDoorsLocked(veh, 4);
                    Utility.Instance.SendChatMessage("[VEHICLE MANAGER]", "You have started to try to hotwire the car!", 0, 140, 50);
                    API.TaskPlayAnim(API.PlayerPedId(), "mini@repair", "fixing_a_player", 8.0f, 0.0f, -1, 1, 0, false,
                        false, false);
                    await Delay(20000);
                    API.SetVehicleDoorsLocked(veh, 0);
                    API.SetVehiclePetrolTankHealth(veh, 1000);
                    API.SetVehicleEngineOn(veh, true, false, false);
                    Utility.Instance.SendChatMessage("[VEHICLE MANAGER]","You have hotwired the car sucessfully!",0,140,50);
                    await Delay(600000);
                    Utility.Instance.SendChatMessage("[VEHICLE MANAGER]", "You can now hotwire a car!", 0, 140, 50);
                }
                else
                {
                    Utility.Instance.SendChatMessage("[VEHICLE MANAGER]", "You messed up and fucked up the hotwiring!", 0, 140, 50);
                }
            }
        }

    }
}
