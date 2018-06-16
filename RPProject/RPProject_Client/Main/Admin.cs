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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            Utility.Instance.SpawnCar(car, i =>
            {
                API.SetVehicleNumberPlateText(i, "ADMIN");
            } );
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
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
