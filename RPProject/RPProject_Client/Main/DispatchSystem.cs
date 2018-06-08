using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay.Main
{
    public class DispatchSystem : BaseScript
    {
        public static DispatchSystem Instance;

        public DispatchSystem()
        {
            Instance = this;
            EventHandlers["911CallClient"] += new Action<string>(EmergencyCall);
            EventHandlers["311CallClient"] += new Action<string>(NonEmergencyCall);
            EventHandlers["AlertSound"] += new Action(AlertSound);
            EventHandlers["AlertSound2"] += new Action(AlertSound2);
            EventHandlers["AlertBlip"] += new Action<int, float, float, float>(AlertBlip);
        }

        private async void AlertBlip(int number, float x, float y, float z)
        {
            var blip = API.AddBlipForCoord(x,y,z);
            API.SetBlipSprite(blip, 205);
            API.SetBlipColour(blip, 5);
            API.SetBlipScale(blip, 0.6f);
            API.SetBlipAsShortRange(blip, false);
            API.BeginTextCommandSetBlipName("STRING");
            API.AddTextComponentString("Call #"+Convert.ToString(number));
            API.EndTextCommandSetBlipName(blip);
            await Delay(5000);
            while (Utility.Instance.GetDistanceBetweenVector3s(Game.PlayerPed.Position, new Vector3(x, y, z)) > 10)
            {
                await Delay(100);
            }
            API.RemoveBlip(ref blip);
        }

        private async void AlertSound()
        {
            for (int f = 0; f < 10; f++)
            {
                API.PlaySoundFrontend(-1, "Menu_Accept", "Phone_SoundSet_Default", true);
                await Delay(500);
            }
        }
        private void AlertSound2()
        {
            API.PlaySoundFrontend(-1, "Menu_Accept", "Phone_SoundSet_Default", true);
        }

        public void EmergencyCall(string message)
        {
            var names = Utility.Instance.GetVector3StreetNames(Game.PlayerPed.Position);
            TriggerServerEvent("911CallServer", message, names.Street1, names.Street2, Utility.Instance.GetVector3ZoneName(Game.PlayerPed.Position));
        }

        public void NonEmergencyCall(string message)
        {
            var names = Utility.Instance.GetVector3StreetNames(Game.PlayerPed.Position);
            TriggerServerEvent("311CallServer", message, names.Street1, names.Street2, Utility.Instance.GetVector3ZoneName(Game.PlayerPed.Position));
        }
    }
}
