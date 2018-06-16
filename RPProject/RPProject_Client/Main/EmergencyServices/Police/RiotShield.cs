using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client.Main.Users.Inventory;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace client.Main.EmergencyServices.Police
{
    public class RiotShield : BaseScript
    {
        public static RiotShield Instance;

        public bool ShieldActive = false;
        public int ShieldEntity = 0;
        public bool HadPistol = false;

        public string AnimDict = "combat@gestures@gang@pistol_1h@beckon";
        public string AnimName = "0";

        public string Prop = "prop_ballistic_shield";
        public int Pistol = API.GetHashKey("WEAPON_PISTOL");

        public RiotShield()
        {
            Instance = this;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            ShieldLogic();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            EventHandlers["RiotShield"] += new Action(() =>
            {
                if (ShieldActive)
                {
                    DisableShield();
                }
                else
                {
                    if (InventoryUI.Instance.HasItem("Riot Shield(P)") > 0)
                    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                        EnableShield();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                    }
                }
            });
        }

        private async Task ShieldLogic()
        {
            while (true)
            {
                if (ShieldActive)
                {
                    if (!API.IsEntityPlayingAnim(Game.PlayerPed.Handle, AnimDict, AnimName, 1))
                    {
                        API.RequestAnimDict(AnimDict);
                        while (!API.HasAnimDictLoaded(AnimDict))
                        {
                            await Delay(100);
                        }

                        API.TaskPlayAnim(Game.PlayerPed.Handle, AnimDict, AnimName, 8.0f, 8.0f, -1, (2 + 16 + 32), 0.0f,
                            false, false, false);
                    }
                }

                await Delay(500);
            }
        }

        private async Task EnableShield()
        {
            API.RequestAnimDict(AnimDict);
            while (!API.HasAnimDictLoaded(AnimDict))
            {
                await Delay(100);
            }

            API.TaskPlayAnim(Game.PlayerPed.Handle, AnimDict, AnimName, 8.0f, 8.0f, -1, (2 + 16 + 32), 0.0f, false,
                false, false);

            API.RequestModel((uint)API.GetHashKey(Prop));
            while (!API.HasModelLoaded((uint)API.GetHashKey(Prop)))
            {
                await Delay(100);
            }

            ShieldEntity = API.CreateObject(API.GetHashKey(Prop), Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y,
                Game.PlayerPed.Position.Z, true, true, true);
            API.AttachEntityToEntity(ShieldEntity, Game.PlayerPed.Handle,
                API.GetEntityBoneIndexByName(Game.PlayerPed.Handle, "IK_L_HAND"),
                0.0f, -0.05f, -0.10f, -30.0f, 180.0f, 40.0f, false, false, true, false, 0, true);

            if (API.HasPedGotWeapon(Game.PlayerPed.Handle, (uint)Pistol, false) ||
                API.GetSelectedPedWeapon(Game.PlayerPed.Handle) == Pistol)
            {
                API.SetCurrentPedWeapon(Game.PlayerPed.Handle, (uint)Pistol, true);
                HadPistol = true;
            }
            else
            {
                API.GiveWeaponToPed(Game.PlayerPed.Handle, (uint)Pistol, 300, false, true);
                API.SetCurrentPedWeapon(Game.PlayerPed.Handle, (uint)Pistol, true);
                HadPistol = false;
            }

            ShieldActive = true;
            API.SetEnableHandcuffs(Game.PlayerPed.Handle, true);
        }

        private void DisableShield()
        {
            API.DeleteEntity(ref ShieldEntity);
            Game.PlayerPed.Task.ClearAll();
            if (!HadPistol)
            {
                API.RemoveWeaponFromPed(Game.PlayerPed.Handle, (uint)Pistol);
            }

            API.SetEnableHandcuffs(Game.PlayerPed.Handle, false);
            HadPistol = false;
            ShieldActive = false;
        }


    }
}
