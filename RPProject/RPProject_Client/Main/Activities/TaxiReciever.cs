using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace client.Main.Activities
{
    public class TaxiReciever : BaseScript
    {
        public static TaxiReciever Instance;

        private bool _canMark = false;

        public TaxiReciever()
        {
            Instance = this;
            EventHandlers["TaxiBroadcast"] += new Action<dynamic,dynamic,dynamic>(RecieveTaxiBroadcast);
        }

        private async void RecieveTaxiBroadcast(dynamic x, dynamic y, dynamic z)
        {
            var street = Utility.Instance.GetVector3StreetNames(new Vector3(x, y, z));
            var zone = Utility.Instance.GetVector3ZoneName(new Vector3(x, y, z));
            if (Game.PlayerPed.IsInVehicle() && Game.PlayerPed.CurrentVehicle.Model == VehicleHash.Taxi ||
                Game.PlayerPed.LastVehicle.Model == VehicleHash.Taxi)
            {
                Utility.Instance.SendChatMessage("[Taxi]","You have a call at "+street+" in "+zone+" [PRESS Y TO MARK IT WITHIN THE NEXT 10 SECONDS]",255,255,0);
                TriggerEvent("AlertSound");
                _canMark = true;
                TurnOffCanMark();
                while (_canMark)
                {
                    if (Game.IsControlJustPressed(0, Control.MpTextChatTeam))
                    {
                        Utility.Instance.SendChatMessage("[Taxi]", "You have accepted the call, it has been set on your GPS.", 255, 255, 0);
                        API.SetNewWaypoint(x,y);
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
