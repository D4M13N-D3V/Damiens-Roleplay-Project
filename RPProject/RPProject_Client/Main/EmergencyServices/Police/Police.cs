using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;
using client.Main.Users.Inventory;
using client.Main.Items;
#pragma warning disable CS0105 // The using directive for 'client.Main.Users.Inventory' appeared previously in this namespace
using client.Main.Users.Inventory;
#pragma warning restore CS0105 // The using directive for 'client.Main.Users.Inventory' appeared previously in this namespace
using client.Main.Vehicles;

namespace client.Main.EmergencyServices.Police
{
    public enum LEODepartments
    {
        LSPD,
        BCSO,
        LSCSO,
        SASP,
        SAHP,
        SAAO,
        USMS,
        FBI,
        DEA
    }

    public class Police : BaseScript
    {

        private readonly Dictionary<string, List<PoliceUniformComponent>> _maleUniforms =
            new Dictionary<string, List<PoliceUniformComponent>>()
            {
                ["SASP"] = new List<PoliceUniformComponent>()
                {
                    new PoliceUniformComponent(3, 0, 0, 0),
                    new PoliceUniformComponent(4, 10, 1, 0),
                    new PoliceUniformComponent(5, 0, 0, 0),
                    new PoliceUniformComponent(6, 24, 0, 0),
                    new PoliceUniformComponent(7, 0, 0, 0),
                    new PoliceUniformComponent(8, 122, 0, 0),
                    new PoliceUniformComponent(11, 55, 0, 0),
                }
            };

        private readonly Dictionary<string, List<PoliceUniformComponent>> _femaleUniforms =
            new Dictionary<string, List<PoliceUniformComponent>>()
            {
                ["SASP"] = new List<PoliceUniformComponent>()
                {
                    new PoliceUniformComponent(3, 14, 0, 0),
                    new PoliceUniformComponent(4, 64, 3, 0),
                    new PoliceUniformComponent(5, 3, 0, 0),
                    new PoliceUniformComponent(6, 52, 0, 0),
                    new PoliceUniformComponent(7, 0, 0, 0),
                    new PoliceUniformComponent(8, 152, 0, 0),
                    new PoliceUniformComponent(11, 48, 0, 0),
                }
            };

        public int CopCount = 0;
        public bool IsOnDuty = false;
#pragma warning disable CS0414 // The field 'Police._rankName' is assigned but its value is never used
        private string _rankName = "";
#pragma warning restore CS0414 // The field 'Police._rankName' is assigned but its value is never used
        private string _department = "";

        public static Police Instance;

        public Police()
        {
            Instance = this;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            StopDispatch();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            SetupItems();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            EventHandlers["Police:SetOnDuty"] += new Action<dynamic>(OnDuty);
            EventHandlers["Police:SetOffDuty"] += new Action(OffDuty);
            EventHandlers["Police:RefreshOnDutyOfficers"] += new Action<dynamic>(RefreshCops);
            EventHandlers["Police:MarineUnit"] += new Action(SpawnMarineUnit);
            EventHandlers["Police:AirUnit"] += new Action(SpawnAirUnit);
        }

        private void SpawnMarineUnit()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            Utility.Instance.SpawnCar("Predator", i =>
            {
                API.DecorSetInt(i, "PIRP_VehicleOwner", Game.Player.ServerId);
                Game.PlayerPed.SetIntoVehicle((Vehicle)Vehicle.FromHandle(i), VehicleSeat.Driver);
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }

        private void SpawnAirUnit()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            Utility.Instance.SpawnCar("Polmav", i =>
            {
                API.DecorSetInt(i, "PIRP_VehicleOwner", Game.Player.ServerId);
                Game.PlayerPed.SetIntoVehicle((Vehicle)Vehicle.FromHandle(i), VehicleSeat.Driver);
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }

        private async Task StopDispatch()
        {
            while (true)
            {
                for (int i = 0; i < 12; i++)
                {
                    API.EnableDispatchService(i, false);
                }

                API.SetPlayerWantedLevel(Game.Player.Handle, 0, false);
                API.SetPlayerWantedLevelNow(Game.Player.Handle, false);
                API.SetPlayerWantedLevelNoDrop(Game.Player.Handle, 0, false);

                await Delay(0);
            }
        }

        private void RefreshCops(dynamic copCount)
        {
            CopCount = copCount;
        }




#pragma warning disable CS0414 // The field 'Police._menuOpen' is assigned but its value is never used
        private bool _menuOpen = false;
#pragma warning restore CS0414 // The field 'Police._menuOpen' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'Police._menuCreated' is assigned but its value is never used
        private bool _menuCreated = false;
#pragma warning restore CS0414 // The field 'Police._menuCreated' is assigned but its value is never used
        private UIMenu _menu;

        private void OnDuty(dynamic data)
        {
            var department = Convert.ToString(data);
            _department = department;
            Utility.Instance.SendChatMessage("[Police]", "You have gone on duty.", 0, 0, 255);
            IsOnDuty = true;
            PoliceGear.Instance.SetRestricted(false);
            PoliceGarage.Instance.MenuRestricted = false;
            GiveUniform();
            TriggerEvent("setAsCopForDoors");





            _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(
                        InteractionMenu.Instance._interactionMenu, "Police Menu", "Police Menu", new PointF(5, Screen.Height / 2));

            var search = new UIMenuItem("Search Player");
            var confiscateWeapons = new UIMenuItem("Confiscate Weapons");
            var confiscateItems = new UIMenuItem("Confiscate Items");
            var dragButton = new UIMenuItem("Drag nearby cuffed/downed person");
            var cuffButton = new UIMenuItem("Cuff/Uncuff Nearby Person");

            _menu.AddItem(search);
            _menu.AddItem(confiscateWeapons);
            _menu.AddItem(confiscateItems);
            _menu.AddItem(dragButton);
            _menu.AddItem(cuffButton);

            _menu.OnItemSelect += async(sender, item, index) =>
            {
                if (item == search)
                {
                    Utility.Instance.GetClosestPlayer(out var info);
                    if (info.Dist < 5)
                    {
                        TriggerServerEvent("Police:SearchPlayer", API.GetPlayerServerId(info.Pid));
                    }
                }
                else if (item == confiscateWeapons)
                {
                    Utility.Instance.GetClosestPlayer(out var info);
                    if (info.Dist < 5)
                    {
                        TriggerServerEvent("ConfiscateWeapons", API.GetPlayerServerId(info.Pid));
                    }
                }
                else if (item == confiscateItems)
                {
                    Utility.Instance.GetClosestPlayer(out var info);
                    if (info.Dist < 5)
                    {
                        TriggerServerEvent("ConfiscateItems", API.GetPlayerServerId(info.Pid));
                    }
                }
                else if (item == dragButton)
                {
                    Utility.Instance.GetClosestPlayer(out var output);
                    if (output.Dist < 5)
                    {
                        TriggerServerEvent("DragRequest", API.GetPlayerServerId(output.Pid));
                    }
                }
                else if (item == cuffButton)
                {
                    Utility.Instance.GetClosestPlayer(out var output);
                    if (output.Dist < 5)
                    {
                        if (InventoryUI.Instance.HasItem("Zipties") > 0)
                        {
                            Game.PlayerPed.Task.PlayAnimation("mp_arresting", "a_uncuff");
                            await Delay(4000);
                            Game.PlayerPed.Task.ClearAll();
                            TriggerServerEvent("RestrainRequest", API.GetPlayerServerId(output.Pid), (int)RestraintTypes.Zipties);
                        }
                        else if (InventoryUI.Instance.HasItem("Handcuffs(P)") > 0)
                        {
                            Game.PlayerPed.Task.PlayAnimation("mp_arresting", "a_uncuff");
                            await Delay(4000);
                            Game.PlayerPed.Task.ClearAll();
                            TriggerServerEvent("RestrainRequest", API.GetPlayerServerId(output.Pid), (int)RestraintTypes.Handcuffs);
                        }
                    }
                    else
                    {
                        Utility.Instance.SendChatMessage("[Restraints]", "Not close enough to restrain anyone.", 0, 0, 255);
                    }
                }
            };

            _menuCreated = true;
            InteractionMenu.Instance._interactionMenuPool.RefreshIndex();


        }

        private void OffDuty()
        {
            Utility.Instance.SendChatMessage("[Police]", "You have gone off duty.", 0, 0, 255);
            _rankName = "";
            IsOnDuty = false;
            PoliceGear.Instance.SetRestricted(true);
            PoliceGarage.Instance.MenuRestricted = true;
            _department = "";
            TakeUniform();

            _menuCreated = false;
            if (_menu.Visible)
            {
                _menu.Visible = false;
                InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                InteractionMenu.Instance._interactionMenu.Visible = true;
            }
            var i = 0;
            foreach (var item in InteractionMenu.Instance._interactionMenu.MenuItems)
            {
                if (item == _menu.ParentItem)
                {
                    InteractionMenu.Instance._interactionMenu.RemoveItemAt(i);
                    break;
                }

                i++;
            }
            InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
    }

        private void GiveUniform()
        {
            if (_department == "USMS" || _department == "")
            {
                return;
            }

            if (API.GetEntityModel(Game.PlayerPed.Handle) == API.GetHashKey("mp_m_freemode_01"))
            {
                foreach (var uniformParts in _maleUniforms[_department])
                {
                    API.SetPedComponentVariation(Game.PlayerPed.Handle, uniformParts.Component, uniformParts.Drawable,
                        uniformParts.Texture, uniformParts.Pallet);
                }
            }
            else if (API.GetEntityModel(Game.PlayerPed.Handle) == API.GetHashKey("mp_f_freemode_01"))
            {
                foreach (var uniformParts in _femaleUniforms[_department])
                {
                    API.SetPedComponentVariation(Game.PlayerPed.Handle, uniformParts.Component, uniformParts.Drawable,
                        uniformParts.Texture, uniformParts.Pallet);
                }
            }
        }

        private void TakeUniform()
        {
            TriggerServerEvent("RefreshClothes");
        }

        private async Task SetupItems()
        {
            while (InventoryProcessing.Instance == null)
            {
                await Delay(0);
            }

            InventoryProcessing.Instance.AddItemUse("Police Lock Tool(P)", PoliceLockTool);
            InventoryProcessing.Instance.AddItemUse("Fingerprint Scanner(P)", FingerprintScanner);
            InventoryProcessing.Instance.AddItemUse("GSR Kit(P)", GSRKit);
        }

        public void GSRKit()
        {
            Utility.Instance.GetClosestPlayer(out var output);
            if (output.Dist < 4)
            {
                if (API.DecorGetBool(output.Ped, "GSR_Active"))
                {
                    TriggerServerEvent("ActionCommandFromClient", "Swabs the persons hand and puts it in a capsule, shaking it. The liquid in the capsule turns red indicating that they had fired a firearm recently.");
                }
                else
                {
                    TriggerServerEvent("ActionCommandFromClient", "Swabs the persons hand and puts it in a capsule, shaking it. The liquid in the capsule turns blue indicating that they havent fired a firearm recently.");
                }
            }
        }

        public async void PoliceLockTool()
        {
            var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
            var vehicle = Utility.Instance.ClosestVehicle.Handle;
            if (Utility.Instance.GetDistanceBetweenVector3s(playerPos, API.GetEntityCoords(vehicle, false)) > 4)
            {
                Utility.Instance.SendChatMessage("[Police Lock Tool]", "You are too far away from a vehicle.", 255, 0,
                    0);
                return;
            }

            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            Game.PlayerPed.Task.PlayAnimation("misscarstealfinalecar_5_ig_3", "crouchloop");
            var lockPicking = true;

            async Task CancelLockpick()
            {
                while (lockPicking)
                {
                    if (API.IsControlJustPressed(0, (int) Control.PhoneCancel))
                    {
                        lockPicking = false;
                        API.ClearPedTasks(Game.PlayerPed.Handle);
                    }

                    await Delay(0);
                }
            }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CancelLockpick();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            Utility.Instance.SendChatMessage("[Police Lock Tool]", "You start using your tool to unlock the vehicle.",
                255, 0, 0);
            await Delay(15000);
            lockPicking = false;
            Game.PlayerPed.Task.ClearAll();
            API.SetVehicleDoorsLocked(vehicle, 0);
            Utility.Instance.SendChatMessage("[Police Lock Tool]",
                "You successfully unlock the vehicle with your tool!", 255, 0, 0);
        }

        public async void FingerprintScanner()
        {
            ClosestPlayerReturnInfo output;
            Utility.Instance.GetClosestPlayer(out output);
            if (output.Dist < 4)
            {
                Game.PlayerPed.Task.PlayAnimation("mp_arresting", "a_uncuff");
                await Delay(3000);
                Game.PlayerPed.Task.ClearAll();
                TriggerServerEvent("FingerPrintScannerRequest", API.GetPlayerServerId(output.Pid));
            }
            else
            {
                Utility.Instance.SendChatMessage("[Fingeprint Scanner]", "You are not close enough to a player!", 255,
                    0, 0);
            }
        }
    }

}