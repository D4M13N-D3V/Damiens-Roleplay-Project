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
using client.Main.Users.Inventory;
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
        private string _rankName = "";
        private string _department = "";

        public static Police Instance;

        public Police()
        {
            Instance = this;
            StopDispatch();
            SetupItems();
            EventHandlers["Police:SetOnDuty"] += new Action<dynamic>(OnDuty);
            EventHandlers["Police:SetOffDuty"] += new Action(OffDuty);
            EventHandlers["Police:RefreshOnDutyOfficers"] += new Action<dynamic>(RefreshCops);
            EventHandlers["Police:MarineUnit"] += new Action(SpawnMarineUnit);
            EventHandlers["Police:AirUnit"] += new Action(SpawnAirUnit);
        }

        private void SpawnMarineUnit()
        {
            Utility.Instance.SpawnCar("Predator", i =>
            {
                VehicleManager.Instance.Cars.Add(i);
                Game.PlayerPed.SetIntoVehicle((Vehicle)Vehicle.FromHandle(i), VehicleSeat.Driver);
            });
        }

        private void SpawnAirUnit()
        {
            Utility.Instance.SpawnCar("Polmav", i =>
            {
                VehicleManager.Instance.Cars.Add(i);
                Game.PlayerPed.SetIntoVehicle((Vehicle)Vehicle.FromHandle(i), VehicleSeat.Driver);
            });
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

            CancelLockpick();
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