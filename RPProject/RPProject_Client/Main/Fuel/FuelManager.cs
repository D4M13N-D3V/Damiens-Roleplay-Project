using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace client.Main.Fuel
{
    public class FuelManager : BaseScript
    {
        public static FuelManager Instance;
        private bool _refilling = false;
        private int _burnRateSlow = 1;
        private int _burnRateFast = 1;
        private List<GasStation> _gasStations = new List<GasStation>();

                public FuelManager()
        {
            Instance = this;

            //Setup The Fuel Levels Variables.
            API.DecorRegister("FuelLevel", 3);
            API.DecorRegister("MaximumFuel", 3);

            #region Setting up gas station locations
            #endregion

            Tick += new Func<Task>(async delegate
            {
                foreach (GasStation station in _gasStations)
                {
                    var playerPed = Game.PlayerPed.Handle;
                    var playerPos = API.GetEntityCoords(playerPed, true);
                    var distance = API.Vdist(playerPos.X, playerPos.Y, playerPos.Z, station.X, station.Y, station.Z);
                    if (distance < 25)
                    {

                    }
                    if (distance < 2)
                    {

                    }
                }
            });
        }

        public void SetupVehicleForFuel(Vehicle vehicle)
        {
            API.DecorSetInt(vehicle.Handle, "MaximumFuel", 100);
            API.DecorSetInt(vehicle.Handle, "FuelLevel", API.DecorGetInt(vehicle.Handle, "MaximumFuel"));
        }



        public int GetFuelLevel(Vehicle vehicle)
        {
            return API.DecorGetInt(vehicle.Handle, "FuelLevel");
        }

        public int GetFuelLevel(int vehicle)
        {
            return API.DecorGetInt(vehicle, "FuelLevel");
        }

        public int GetMaxFuelLevel(Vehicle vehicle)
        {
            return API.DecorGetInt(vehicle.Handle, "MaximumFuel");
        }

        public int GetMaxFuelLevel(int vehicle)
        {
            return API.DecorGetInt(vehicle, "MaximumFuel");
        }

    }
}
