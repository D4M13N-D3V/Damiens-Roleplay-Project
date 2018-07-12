using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace client.Main.Activities
{
    public class TowReciever : BaseScript
    {
        public static TowReciever Instance;

        private bool _canMark = false;

        public TowReciever()
        {
            Instance = this;
            EventHandlers["TowBroadcast"] += new Action<dynamic, dynamic, dynamic>(RecieveTowBroadcast);
        }

        private async void RecieveTowBroadcast(dynamic x, dynamic y, dynamic z)
        {
            var street = Utility.Instance.GetVector3StreetNames(new Vector3(x, y, z));
            var zone = Utility.Instance.GetVector3ZoneName(new Vector3(x, y, z));
            if (Game.PlayerPed.IsInVehicle() && Game.PlayerPed.CurrentVehicle.Model == VehicleHash.Flatbed || Game.PlayerPed.CurrentVehicle.Model == VehicleHash.TowTruck || Game.PlayerPed.CurrentVehicle.Model == VehicleHash.TowTruck2 ||
                Game.PlayerPed.LastVehicle.Model == VehicleHash.Flatbed || Game.PlayerPed.LastVehicle.Model == VehicleHash.TowTruck || Game.PlayerPed.LastVehicle.Model == VehicleHash.TowTruck2)
            {
                Utility.Instance.SendChatMessage("[Tow]", "You have a call at " + street + " in " + zone + " [PRESS Y TO MARK IT WITHIN THE NEXT 10 SECONDS]", 255, 255, 0);
                TriggerEvent("AlertSound");
                _canMark = true;
                TurnOffCanMark();
                while (_canMark)
                {
                    if (Game.IsControlJustPressed(0, Control.MpTextChatTeam))
                    {   
                        Utility.Instance.SendChatMessage("[Tow]","You have accepted the call, it has been set on your GPS.",255,255,0);
                        API.SetNewWaypoint(x, y);
                        _canMark = false;
                    }
                    await Delay(0);
                }
            }
        }

        private async Task TurnOffCanMark()
        {
            await Delay(10000);
            _canMark = false;
        }
    }
}
