using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using roleplay.Users.Inventory;

namespace roleplay.Main
{
    public enum RestraintTypes { Handcuffs, Zipties, Hobblecuff }

    public class Restraints : BaseScript
    {
        public bool Restrained = false;
        public RestraintTypes RestraintType = RestraintTypes.Handcuffs;

        public bool Drag = false;
        public int OfficerDrag = -1;
        public bool WasDragged = false;

        public Restraints()
        {
            EventHandlers["Restrained"] += new Action<dynamic>(GetRestrained);
            EventHandlers["Dragged"] += new Action<dynamic>(GetDragged);
            EventHandlers["Forced"] += new Action(GetForced);
            RestrainerFunctionality();
            DraggingFunctionality();
            ForcingIntoVehicleFunctionality();
            SearchFunctionality();
        }

        private async void SearchFunctionality()
        {
            while (true)
            {
                Utility.Instance.GetClosestPlayer(out var output);
                if (output.Dist < 5 && Game.IsControlPressed(0, Control.Context))
                {
                    if (Game.IsControlJustPressed(0, Control.MeleeAttackLight))
                    {
                        TriggerServerEvent("Police:SearchPlayer", API.GetPlayerServerId(output.Pid));
                    }
                }
                await Delay(0);
            }
        }

        #region Forcing Into Vehicle
        private async void ForcingIntoVehicleFunctionality()
        {
            while (true)
            {
                API.DisableControlAction(0, (int)Control.MeleeAttackLight, true);
                if (Game.IsControlPressed(0, Control.Aim))
                {
                    if (Game.IsDisabledControlJustPressed(0, Control.MeleeAttackLight))
                    {
                        Utility.Instance.GetClosestPlayer(out var output);
                        if (output.Dist < 5)
                        {
                            TriggerServerEvent("ForceEnterVehicleRequest", API.GetPlayerServerId(output.Pid));
                        }
                    }
                }
                await Delay(0);
            }
        }

        private void GetForced()
        {
            if (Restrained)
            {
                if (Game.PlayerPed.IsInVehicle())
                {
                    Game.PlayerPed.Task.ClearAllImmediately();
                    var pos = Game.PlayerPed.Position + new Vector3(2, 2, 0);
                    API.SetEntityCoords(Game.PlayerPed.Handle,pos.X,pos.Y,pos.Z,false,false,false,false);
                }
                else
                {
                    var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
                    var veh = API.GetClosestVehicle(playerPos.X, playerPos.Y, playerPos.Z, 5.0f, 0, 70);
                    if (API.DoesEntityExist(veh))
                    {
                        if (API.IsVehicleSeatFree(veh, 1))
                        {
                            API.SetPedIntoVehicle(Game.PlayerPed.Handle, veh, 1);
                        }
                        else if (API.IsVehicleSeatFree(veh, 2))
                        {
                            API.SetPedIntoVehicle(Game.PlayerPed.Handle, veh, 2);
                        }
                    }
                }
            }
        }
        #endregion

        #region Dragging

        private async void DraggingFunctionality()
        {
            while (true)
            {
                if (Game.IsControlPressed(0, Control.Aim))
                {
                    if (Game.IsControlJustPressed(0, Control.Context))
                    {
                        Utility.Instance.GetClosestPlayer(out var output);
                        if (output.Dist < 5)
                        {
                            TriggerServerEvent("DragRequest",API.GetPlayerServerId(output.Pid));
                        }
                    }
                }
                await Delay(0);
            }
        }

        private void GetDragged(dynamic target)
        {
            if (Restrained)
            {
                OfficerDrag = API.GetPlayerFromServerId(target);
                Drag = !Drag;

                if (Drag)
                {
                    API.AttachEntityToEntity(Game.PlayerPed.Handle, API.GetPlayerPed(OfficerDrag), 4103, 0.00f, 0.48f, 0.0f, 0.0f, 0.0f, 0.0f, false, false, false, false, 2, true);
                }
                else
                {
                    API.DetachEntity(Game.PlayerPed.Handle,true,false);
                }
            }
        }


        #endregion

        #region Restraining
        private async void RestrainerFunctionality()
        {
            while (true)
            {
                if (Game.IsControlPressed(0, Control.Aim))
                {
                    if (Game.IsControlJustPressed(0, Control.SelectWeaponHeavy))
                    {
                        Utility.Instance.GetClosestPlayer(out var output);
                        if (output.Dist < 5)
                        {
                            if (InventoryUI.Instance.HasItem("Zipties") > 0)
                            {
                                TriggerServerEvent("RestrainRequest", API.GetPlayerServerId(output.Pid), (int)RestraintTypes.Zipties);
                            }
                            else if (InventoryUI.Instance.HasItem("Handcuffs(P)") > 0)
                            {
                                TriggerServerEvent("RestrainRequest", API.GetPlayerServerId(output.Pid), (int)RestraintTypes.Handcuffs);
                            }
                        }
                        else
                        {
                            Utility.Instance.SendChatMessage("[Restraints]", "Not close enough to restrain anyone.", 0, 0, 255);
                        }
                    }
                    if (Game.IsControlJustPressed(0, Control.SelectWeaponSpecial))
                    {
                        Utility.Instance.GetClosestPlayer(out var output);
                        if (output.Dist < 5)
                        {
                            if (InventoryUI.Instance.HasItem("Hobblecuffs(P)") > 0)
                            {
                                TriggerServerEvent("RestrainRequest", API.GetPlayerServerId(output.Pid),
                                    (int) RestraintTypes.Hobblecuff);
                            }
                        }
                        else
                        {
                            Utility.Instance.SendChatMessage("[Restraints]","Not close enough to restrain anyone.",0,0,255);
                        }
                    }
                }
                await Delay(0);
            }
        }

        private void GetRestrained(dynamic type)
        {
            RestraintType = (RestraintTypes)Convert.ToInt32(type);
            Restrained = !Restrained;
            if (Restrained)
            {
                API.SetPedComponentVariation(Game.PlayerPed.Handle, 7, 41, 0, 0);
                API.SetEnableHandcuffs(Game.PlayerPed.Handle,true);
                API.SetCurrentPedWeapon(Game.PlayerPed.Handle, (uint)API.GetHashKey("WEAPON_UNARMED"),true);
                API.SetPedPathCanUseLadders(Game.PlayerPed.Handle, false);
                Animation();
            }
            else
            {
                API.SetPedComponentVariation(Game.PlayerPed.Handle, 7, 0, 0, 0);
                API.SetEnableHandcuffs(Game.PlayerPed.Handle, false);
                API.SetPedPathCanUseLadders(Game.PlayerPed.Handle, true);
            }
        }

        private async void Animation()
        {
            API.RequestAnimDict("mp_arresting");
            while (!API.HasAnimDictLoaded("mp_arresting"))
            {
                await Delay(1);
            }

            if(RestraintType == RestraintTypes.Zipties)
            {
                Reset();
                async void Reset()
                {
                    await Delay(600000);
                    Utility.Instance.SendChatMessage("[Restraints]","You have broken free from your zipties",255,255,0);
                    Restrained = false;
                }
            }

            while (Restrained)
            {
                API.DisableControlAction(0, 140, true);
                API.DisableControlAction(0, 141, true);
                API.DisableControlAction(0, 142, true);
                API.DisableControlAction(0, 23, true);
                switch (RestraintType)
                {
                    case RestraintTypes.Handcuffs:
                        Game.PlayerPed.Task.PlayAnimation("mp_arresting", "idle", -1, -1, AnimationFlags.UpperBodyOnly);
                        break;
                    case RestraintTypes.Hobblecuff:
                        Game.PlayerPed.Task.PlayAnimation("mp_arresting", "idle", -1, -1, AnimationFlags.None);
                        break;
                    case RestraintTypes.Zipties:
                        Game.PlayerPed.Task.PlayAnimation("mp_arresting", "idle", -1, -1, AnimationFlags.UpperBodyOnly);
                        break;
                }
                await Delay(0);
            }
            API.EnableControlAction(0, 140, true);
            API.EnableControlAction(0, 23, true);
            API.EnableControlAction(0, 141, true);
            API.EnableControlAction(0, 142, true);
            Game.PlayerPed.Task.ClearAll();
        }
        #endregion
    }
}
