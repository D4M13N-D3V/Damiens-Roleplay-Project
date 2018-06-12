using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace client.Main
{
    public class Admin : BaseScript
    {
        public Admin Instance;

        public Admin()
        {
            Instance = this;
            EventHandlers["AdminSpawnCar"] += new Action<string>(SpawnCar);
            EventHandlers["DeleteVehicle"] += new Action(DeleteVehicle);
        }

        private void SpawnCar(string car)
        {
            Utility.Instance.SpawnCar(car, i =>
            {
                API.SetVehicleNumberPlateText(i, "ADMIN");
            } );
        }

        private void DeleteVehicle()
        {
            var myPed = Game.PlayerPed.Handle;

            if (API.IsPedInAnyVehicle(myPed, false))
            {
                var veh = API.GetVehiclePedIsIn(myPed, false);
                API.DeleteVehicle(ref veh);
                return;
            }
            Utility.Instance.ClosestVehicle.Delete();
        }
    }
}
