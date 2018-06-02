using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay.Main
{
    public class Stance : BaseScript
    {
        public Stance()
        {
        }

        private bool crouched = false;
        private bool proned = false;

        private async void StanceLogic()
        {
            while (true)
            {
                Game.DisableControlThisFrame(0,Control.LookBehind);
                if (Game.IsControlJustPressed(0, Control.LookBehind) && !Game.PlayerPed.IsDead &&!Game.PlayerPed.IsInVehicle())
                {
                    if (!crouched && !proned)
                    {
                        crouched = true;
                        API.RequestAnimSet("move_ped_crouched");

                        while (!API.HasAnimSetLoaded("move_ped_crouched"))
                        {
                            await Delay(0);
                        }
                        API.SetPedMovementClipset(Game.PlayerPed.Handle,"move_ped_crouched",0.25f);
                    }
                    else if (crouched && !proned)
                    {
                        crouched = false;
                        proned = true;
                        API.RequestAnimSet("move_ped_crouched");

                        while (!API.HasAnimSetLoaded("move_ped_crouched"))
                        {
                            await Delay(0);
                        }

                        API.SetPedMovementClipset(Game.PlayerPed.Handle, "move_ped_crouched", 0.25f);
                    }
                    else if (!crouched && proned)
                    {
                        API.ResetPedMovementClipset(Game.PlayerPed.Handle, 0);
                    }
                }
                await Delay(0);
            }
        }
    }
}
