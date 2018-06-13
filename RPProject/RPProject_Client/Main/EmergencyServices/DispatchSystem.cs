using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using client.Main.EmergencyServices.Police;

namespace client.Main.EmergencyServices
{
    public class DispatchSystem : BaseScript
    {
        public static DispatchSystem Instance;

        private List<string> _blacklistedWeapons = new List<string>
        {
            "WEAPON_UNARMED",
            "WEAPON_STUNGUN",
            "WEAPON_KNIFE",
            "WEAPON_KNUCKLE",
            "WEAPON_NIGHTSTICK",
            "WEAPON_HAMMER",
            "WEAPON_BAT",
            "WEAPON_GOLFCLUB",
            "WEAPON_CROWBAR",
            "WEAPON_BOTTLE",
            "WEAPON_DAGGER",
            "WEAPON_HATCHET",
            "WEAPON_MACHETE",
            "WEAPON_FLASHLIGHT",
            "WEAPON_SWITCHBLADE",
            "WEAPON_FIREEXTINGUISHER",
            "WEAPON_PETROLCAN",
            "WEAPON_SNOWBALL",
            "WEAPON_FLARE",
            "WEAPON_BALL",
            "WEAPON_MOLOTOV",
            "WEAPON_STUNGUN"
        };

        private readonly List<string> _possibleShotsfiredCallMessages = new List<string>()
        {
            "Help! Help! Theres a maniac with a gun!",
            "Someone is shooting, send police!",
            "Oh my god some crazy asshole with a gun!",
            "This guy just shot for some reason please send police",
            "Send some officers over here, this guy just shot his gun!",
            "HELP! HELP! HELP! HELP! SOMEONE IS SHOOTING THIER GUN!"
        };

        private readonly List<string> _possibleStolenvehicleCallMessages = new List<string>()
        {
            "I just saw some guy stealing a car.",
            "Just saw a guy steal this guys car.",
            "Can you send help theres a guy stealing cars.",
            "Please send a cop, i dont know what to do, this guy is stealing cop cars.",
            "This dude is stealing cars, please send some people.",
            "Crazy guy is over hear snatching cars, send help!"
        };

        public DispatchSystem()
        {
            Instance = this;
            EventHandlers["911CallClient"] += new Action<string>(EmergencyCall);
            EventHandlers["911CallClientAnonymous"] += new Action<string>(EmergencyCallAnonymous); 
             EventHandlers["311CallClient"] += new Action<string>(NonEmergencyCall);
            EventHandlers["AlertSound"] += new Action(AlertSound);
            EventHandlers["AlertSound2"] += new Action(AlertSound2);
            EventHandlers["AlertBlip"] += new Action<int, float, float, float>(AlertBlip);
            EventHandlers["RemoveBlip"] += new Action<int>(RemoveBlip);
            ShotsFiredLogic();
            VehicleStolenLogic();
        }

        private void RemoveBlip(int i)
        {
            if (blips.ContainsKey(i))
            {
                blips.Remove(i);
            }
        }

        private Dictionary<int, int> blips = new Dictionary<int, int>();

        private async Task ShotsFiredLogic()
        {
            while (true)
            {
                var weaponBlacklisted = false;  
                foreach (var weapon in _blacklistedWeapons)
                {
                    if (Game.PlayerPed.Weapons.Current.Model == API.GetHashKey(weapon))
                    {
                        weaponBlacklisted = true;
                        break;
                    }
                }

                if (Game.PlayerPed.IsShooting && !weaponBlacklisted && !Police.Police.Instance.IsOnDuty)
                {
                    var random = new Random();
                    var randomIndex = random.Next(0, _possibleShotsfiredCallMessages.Count - 1);
                    TriggerEvent("911CallClientAnonymous", _possibleShotsfiredCallMessages[randomIndex]);
                    await Delay(5000);
                }
                await Delay(10);
            }
        }

        private async Task VehicleStolenLogic()
        {
            while (true)
            {
                if (Game.PlayerPed.IsJacking || Game.PlayerPed.IsTryingToEnterALockedVehicle)
                {
                    var random = new Random();
                    var randomIndex = random.Next(0, _possibleShotsfiredCallMessages.Count - 1);
                    TriggerEvent("911CallClientAnonymous", _possibleStolenvehicleCallMessages[randomIndex] + "( "+Game.PlayerPed.VehicleTryingToEnter.DisplayName+","+Game.PlayerPed.VehicleTryingToEnter.Mods.LicensePlate+" )");
                    await Delay(5000);
                }
                await Delay(10);
            }
        }

        private async void AlertBlip(int number, float x, float y, float z)
        {
            var blip = API.AddBlipForCoord(x,y,z);
            API.SetBlipSprite(blip, 205);
            API.SetBlipColour(blip, 5);
            API.SetBlipScale(blip, 1f);
            API.SetBlipAsShortRange(blip, false);
            API.BeginTextCommandSetBlipName("STRING");
            API.AddTextComponentString("Call #"+Convert.ToString(number));
            API.EndTextCommandSetBlipName(blip);
            blips.Add(number, blip);
            await Delay(300000);
            API.RemoveBlip(ref blip);
        }

        private async void AlertSound()
        {
            for (int f = 0; f < 2; f++)
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
        public void EmergencyCallAnonymous(string message)
        {
            var names = Utility.Instance.GetVector3StreetNames(Game.PlayerPed.Position);
            TriggerServerEvent("911CallServerAnonymous", message, names.Street1, names.Street2, Utility.Instance.GetVector3ZoneName(Game.PlayerPed.Position));
        }

        public void NonEmergencyCall(string message)
        {
            var names = Utility.Instance.GetVector3StreetNames(Game.PlayerPed.Position);
            TriggerServerEvent("311CallServer", message, names.Street1, names.Street2, Utility.Instance.GetVector3ZoneName(Game.PlayerPed.Position));
        }
    }
}
