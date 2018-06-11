using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay
{
    public class ClosestPlayerReturnInfo
    {
        public int Pid = 0;
        public int Ped = 0;
        public float Dist = 0;

        public ClosestPlayerReturnInfo(int pid, int ped, float dist)
        {
            Pid = pid;
            Ped = ped;
            Dist = dist;
        }
    }

    public class Utility : BaseScript
    {
        public static Utility Instance;

        public Utility()
        {
            Instance = this;
        }

        public void Log(string message)
        {
            Debug.WriteLine("[PINEAPPLE ISLAND ROLEPALY] [DEBUG LOG] " + message);
        }

        public void DrawTxt(float x, float y, float width, float height, float scale, string text, int r, int g, int b,
            int a, bool centered)
        {
            API.SetTextFont(4);
            API.SetTextProportional(false);
            API.SetTextScale(scale, scale);
            API.SetTextColour(r, g, b, a);
            API.SetTextEntry("STRING");
            API.SetTextOutline();
            API.AddTextComponentString(text);
            if (centered)
            {
                API.SetTextCentre(true);
            }

            API.DrawText(x, y);
        }

        public void DrawRct(float x, float y, float width, float height, int r, int g, int b, int a)
        {
            API.DrawRect(x + width / 2, y + height / 2, width, height, r, g, b, a);
        }

        public async Task KeyboardInput(string title, string defaultText, int maxlength, Action<string> cb)
        {
            API.DisableAllControlActions(0);
            API.AddTextEntry("FMMC_KEY_TIP1", title + ":");
            API.DisplayOnscreenKeyboard(1, "FMMC_KEY_TIP1", "", defaultText, "", "", "", maxlength);
            while (API.UpdateOnscreenKeyboard() != 1 && API.UpdateOnscreenKeyboard() != 2)
            {
                await Delay(0);
            }

            if (API.UpdateOnscreenKeyboard() != 2)
            {
                var result = API.GetOnscreenKeyboardResult();
                cb(result);
            }

            await (Delay(500));

            API.EnableAllControlActions(0);
        }

        public void GetClosestPlayer(out ClosestPlayerReturnInfo output)
        {
            var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
            int closestPlayer = -2;
            int closestPlayerPed = -2;
            float dist = 9999;
            for (int i = 0; i < 32; i++)
            {
                if (i != API.PlayerId())
                {
                    var otherPlayerPed = API.GetPlayerPed(i);
                    var pos = API.GetEntityCoords(otherPlayerPed, true);
                    var distance = API.Vdist(playerPos.X, playerPos.Y, playerPos.Z, pos.X, pos.Y, pos.Z);
                    if (distance < dist)
                    {
                        closestPlayer = i;
                        closestPlayerPed = otherPlayerPed;
                        dist = distance;
                    }
                }
            }

            output = new ClosestPlayerReturnInfo(closestPlayer, closestPlayerPed, dist);
        }

        public void GetClosestDeadPlayer(out ClosestPlayerReturnInfo output)
        {
            var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
            int closestPlayer = -2;
            int closestPlayerPed = -2;
            float dist = 9999;
            for (int i = 0; i < 32; i++)
            {
                if (i != API.PlayerId())
                {
                    var otherPlayerPed = API.GetPlayerPed(i);
                    var pos = API.GetEntityCoords(otherPlayerPed, true);
                    var distance = API.Vdist(playerPos.X, playerPos.Y, playerPos.Z, pos.X, pos.Y, pos.Z);
                    if (distance < dist && API.IsEntityDead(otherPlayerPed))
                    {
                        closestPlayer = i;
                        closestPlayerPed = otherPlayerPed;
                        dist = distance;
                    }
                }
            }

            output = new ClosestPlayerReturnInfo(closestPlayer, closestPlayerPed, dist);
        }

        public void GetPlayersInRadius(int player, int distance, out List<int> playerList)
        {
            var playersNearby = new List<int>();
            var playerPos = API.GetEntityCoords(API.GetPlayerPed(player), true);
            for (int i = 0; i < 32; i++)
            {
                if (i != player)
                {
                    var otherPlayerPed = API.GetPlayerPed(i);
                    var pos = API.GetEntityCoords(otherPlayerPed, true);
                    var dist = API.Vdist(playerPos.X, playerPos.Y, playerPos.Z, pos.X, pos.Y, pos.Z);
                    if (dist < distance)
                    {
                        playersNearby.Add(i);
                    }
                }
            }

            playerList = playersNearby;
        }

        public void SendChatMessage(string title, string message, int r, int g, int b)
        {
            TriggerEvent("chatMessage", title, new[] {r, g, b}, message);
        }

        public bool IsDoorOpen(int veh, int door)
        {
            if (API.GetVehicleDoorAngleRatio(veh, door) == 0)
            {
                return false;
            }

            return true;
        }

        public float GetDistanceBetweenVector3s(Vector3 a, Vector3 b)
        {
            return API.Vdist(a.X, a.Y, a.Z, b.X, b.Y, b.Z);
        }

        public async Task SpawnCar(string car, Action<int> cb)
        {
            var ped = Game.PlayerPed.Handle;
            var ply = API.PlayerId();
            var vehicle = (uint) API.GetHashKey(car);
            Debug.WriteLine(Convert.ToString(vehicle));
            API.RequestModel(vehicle);
            while (!API.HasModelLoaded(vehicle))
            {
                await Delay(1);
            }

            var coords = API.GetOffsetFromEntityInWorldCoords(ped, 0, 2f, 0);
            var spawnedCar = API.CreateVehicle(vehicle, coords.X, coords.Y, coords.Z, API.GetEntityHeading(ped), true,
                false);
            API.SetVehicleOnGroundProperly(spawnedCar);
            API.SetModelAsNoLongerNeeded(vehicle);
            API.SetEntityAsMissionEntity(spawnedCar, true, true);
            API.SetVehicleHasBeenOwnedByPlayer(spawnedCar, true);
            cb(spawnedCar);
        }

        public List<Vehicle> NearbyVehicles(float distance = 20)
        {
            var lppos = Game.PlayerPed.Position;

            // Distances are "distance sqr"
            distance *= distance;

            var ret = new List<Vehicle>();
            //Debug.WriteLine("Getting nearby vehicles...");
            foreach (var vid in new VehicleList())
            {
                var v = new Vehicle(vid);
                if (!v.Exists())
                {
                    continue;
                }

                if (v.Position.DistanceToSquared2D(lppos) <= distance)
                {
                    ret.Add(v);
                }
            }

            return ret.OrderBy(v => v.Position.DistanceToSquared(lppos)).ToList();
        }

        public List<Ped> NearbyPeds(float distance = 20)
        {
            var lppos = Game.PlayerPed.Position;

            // Distances are "distance sqr"
            distance *= distance;

            var ret = new List<Ped>();
            //Debug.WriteLine("Getting nearby vehicles...");
            foreach (var vid in new PedList())
            {
                var v = new Ped(vid);
                if (!v.Exists())
                {
                    continue;
                }

                if (v.Position.DistanceToSquared2D(lppos) <= distance)
                {
                    ret.Add(v);
                }
            }

            return ret.OrderBy(v => v.Position.DistanceToSquared(lppos)).ToList();
        }

        public Vehicle ClosestVehicle
        {
            get { return NearbyVehicles()[0]; }
            set
            {

            }
        }


        public Ped ClosestPed
        {
            get { return NearbyPeds()[1]; }
            set
            {

            }
        }

        private Dictionary<string, string> _zones = new Dictionary<string, string>()
        {
            ["AIRP"] = "Los Santos International Airport",
            ["ALAMO"] = "Alamo Sea",
            ["ALTA"] = "Alta",
            ["ARMYB"] = "Fort Zancudo",
            ["BANHAMC"] = "Banham Canyon Dr",
            ["BANNING"] = "Banning",
            ["BEACH"] = "Vespucci Beach",
            ["BHAMCA"] = "Banham Canyon",
            ["BRADP"] = "Braddock Pass",
            ["BRADT"] = "Braddock Tunnel",
            ["BURTON"] = "Burton",
            ["CALAFB"] = "Calafia Bridge",
            ["CANNY"] = "Raton Canyon",
            ["CCREAK"] = "Cassidy Creek",
            ["CHAMH"] = "Chamberlain Hills",
            ["CHIL"] = "Vinewood Hills",
            ["CHU"] = "Chumash",
            ["CMSW"] = "Chiliad Mountain State Wilderness",
            ["CYPRE"] = "Cypress Flats",
            ["DAVIS"] = "Davis",
            ["DELBE"] = "Del Perro Beach",
            ["DELPE"] = "Del Perro",
            ["DELSOL"] = "La Puerta",
            ["DESRT"] = "Grand Senora Desert",
            ["DOWNT"] = "Downtown",
            ["DTVINE"] = "Downtown Vinewood",
            ["EAST_V"] = "East Vinewood",
            ["EBURO"] = "El Burro Heights",
            ["ELGORL"] = "El Gordo Lighthouse",
            ["ELYSIAN"] = "Elysian Island",
            ["GALFISH"] = "Galilee",
            ["GOLF"] = "GWC and Golfing Society",
            ["GRAPES"] = "Grapeseed",
            ["GREATC"] = "Great Chaparral",
            ["HARMO"] = "Harmony",
            ["HAWICK"] = "Hawick",
            ["HORS"] = "Vinewood Racetrack",
            ["HUMLAB"] = "Humane Labs and Research",
            ["JAIL"] = "Bolingbroke Penitentiary",
            ["KOREAT"] = "Little Seoul",
            ["LACT"] = "Land Act Reservoir",
            ["LAGO"] = "Lago Zancudo",
            ["LDAM"] = "Land Act Dam",
            ["LEGSQU"] = "Legion Square",
            ["LMESA"] = "La Mesa",
            ["LOSPUER"] = "La Puerta",
            ["MIRR"] = "Mirror Park",
            ["MORN"] = "Morningwood",
            ["MOVIE"] = "Richards Majestic",
            ["MTCHIL"] = "Mount Chiliad",
            ["MTGORDO"] = "Mount Gordo",
            ["MTJOSE"] = "Mount Josiah",
            ["MURRI"] = "Murrieta Heights",
            ["NCHU"] = "North Chumash",
            ["NOOSE"] = "N.O.O.S.E",
            ["OCEANA"] = "Pacific Ocean",
            ["PALCOV"] = "Paleto Cove",
            ["PALETO"] = "Paleto Bay",
            ["PALFOR"] = "Paleto Forest",
            ["PALHIGH"] = "Palomino Highlands",
            ["PALMPOW"] = "Palmer-Taylor Power Station",
            ["PBLUFF"] = "Pacific Bluffs",
            ["PBOX"] = "Pillbox Hill",
            ["PROCOB"] = "Procopio Beach",
            ["RANCHO"] = "Rancho",
            ["RGLEN"] = "Richman Glen",
            ["RICHM"] = "Richman",
            ["ROCKF"] = "Rockford Hills",
            ["RTRAK"] = "Redwood Lights Track",
            ["SANAND"] = "San Andreas",
            ["SANCHIA"] = "San Chianski Mountain Range",
            ["SANDY"] = "Sandy Shores",
            ["SKID"] = "Mission Row",
            ["SLAB"] = "Stab City",
            ["STAD"] = "Maze Bank Arena",
            ["STRAW"] = "Strawberry",
            ["TATAMO"] = "Tataviam Mountains",
            ["TERMINA"] = "Terminal",
            ["TEXTI"] = "Textile City",
            ["TONGVAH"] = "Tongva Hills",
            ["TONGVAV"] = "Tongva Valley",
            ["VCANA"] = "Vespucci Canals",
            ["VESP"] = "Vespucci",
            ["VINE"] = "Vinewood",
            ["WINDF"] = "Ron Alternates Wind Farm",
            ["WVINE"] = "West Vinewood",
            ["ZANCUDO"] = "Zancudo River",
            ["ZP_ORT"] = "Port of South Los Santos",
            ["ZQ_UAR"] = "Davis Quartz"
        };

        public StreetNames GetVector3StreetNames(Vector3 pos)
        {
            var streetOne = new uint();
            var streetTwo = new uint();
            API.GetStreetNameAtCoord(pos.X, pos.Y, pos.Z, ref streetOne, ref streetTwo);
            var streetOneName = "";
            var streetTwoName = "";
            streetOneName = API.GetStreetNameFromHashKey(streetOne);
            streetTwoName = API.GetStreetNameFromHashKey(streetTwo);
            return new StreetNames(streetOneName,streetTwoName);
        }

        public string GetVector3ZoneName(Vector3 pos)
        {
            if (_zones.ContainsKey(API.GetNameOfZone(pos.X, pos.Y, pos.Z)))
            {
                return _zones[API.GetNameOfZone(pos.X, pos.Y, pos.Z)];
            }

            return "";
        }

    }

    public class StreetNames
    {
        public string Street1;
        public string Street2;

        public StreetNames(string street1, string street2)
        {
            Street1 = street1;
            Street2 = street2;
        }
    }

    public class VehicleList : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            int entity = 0;
            int handle = API.FindFirstVehicle(ref entity);
            yield return entity;

            while (API.FindNextVehicle(handle, ref entity))
            {
                yield return entity;
            }
            API.EndFindVehicle(handle);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class PedList : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            int entity = 0;
            int handle = API.FindFirstPed(ref entity);
            yield return entity;

            while (API.FindNextPed(handle, ref entity))
            {
                yield return entity;
            }
            API.EndFindPed(handle);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}