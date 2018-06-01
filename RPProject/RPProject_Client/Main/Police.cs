using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using roleplay.Main;
using roleplay.Users.Inventory;

namespace roleplay.Main
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

    public class PoliceUniformComponent
    {
        public int Component;
        public int Drawable;
        public int Texture;
        public int Pallet;
        public PoliceUniformComponent(int comp, int draw, int text, int pallet)
        {
            Component = comp;
            Drawable = draw;
            Texture = text;
            Pallet = pallet;
        }
    }

    public class Police : BaseScript
    {

        private readonly Dictionary<string, List<PoliceUniformComponent>> _maleUniforms =
            new Dictionary<string, List<PoliceUniformComponent>>()
            {
                ["LSPD"] = new List<PoliceUniformComponent>()
                {
                    new PoliceUniformComponent(3,0,0,0),
                    new PoliceUniformComponent(4,35,0,0),
                    new PoliceUniformComponent(5,0,0,0),
                    new PoliceUniformComponent(6,24,0,0),
                    new PoliceUniformComponent(7,0,0,0),
                    new PoliceUniformComponent(8,122,0,0),
                    new PoliceUniformComponent(11,55,0,0),
                },
                ["BCSO"] = new List<PoliceUniformComponent>()
                {
                    new PoliceUniformComponent(3,11,0,0),
                    new PoliceUniformComponent(4,10,0,0),
                    new PoliceUniformComponent(5,0,0,0),
                    new PoliceUniformComponent(6,24,0,0),
                    new PoliceUniformComponent(7,125,0,0),
                    new PoliceUniformComponent(8,122,0,0),
                    new PoliceUniformComponent(11,26,2,0),
                }
            };

        private readonly Dictionary<string, List<PoliceUniformComponent>> _femaleUniforms =
            new Dictionary<string, List<PoliceUniformComponent>>()
            {
                ["LSPD"] = new List<PoliceUniformComponent>()
                {
                    new PoliceUniformComponent(3,14,0,0),
                    new PoliceUniformComponent(4,34,0,0),
                    new PoliceUniformComponent(5,0,0,0),
                    new PoliceUniformComponent(6,25,0,0),
                    new PoliceUniformComponent(7,0,0,0),
                    new PoliceUniformComponent(8,152,0,0),
                    new PoliceUniformComponent(11,48,0,0),
                },
                ["BCSO"] = new List<PoliceUniformComponent>()
                {
                    new PoliceUniformComponent(3,0,0,0),
                    new PoliceUniformComponent(4,64,1,0),
                    new PoliceUniformComponent(5,0,0,0),
                    new PoliceUniformComponent(6,52,0,0),
                    new PoliceUniformComponent(7,95,0,0),
                    new PoliceUniformComponent(8,152,0,0),
                    new PoliceUniformComponent(11,27,2,0),
                }
            };

        public int CopCount = 0;
        private bool _onDuty = false;
        private string _rankName = "";
        private string _department = "";

        public Police()
        {
            EventHandlers["Police:SetOnDuty"] += new Action<dynamic>(OnDuty);
            EventHandlers["Police:SetOffDuty"] += new Action(OffDuty);
            EventHandlers["Police:RefreshOnDutyOfficers"] += new Action<dynamic>(RefreshCops);
        }
        private void RefreshCops(dynamic copCount)
        {
            CopCount = copCount;
        }

        private void OnDuty(dynamic data)
        {
            var department = Convert.ToString(data);
            _department = department;
            Utility.Instance.SendChatMessage("[POLICE]","You have gone on duty.",0,0,255);
            _onDuty = true;
            PoliceGear.Instance.MenuRestricted = false;
            GiveUniform();
        }

        private void OffDuty()
        {
            Utility.Instance.SendChatMessage("[POLICE]", "You have gone off duty.", 0, 0, 255);
            _rankName = "";
            _onDuty = false;
            PoliceGear.Instance.MenuRestricted = true;
            _department = "";
            TakeUniform();
        }

        private void GiveUniform()
        {
            if (_department=="USMS" || _department=="") { return; }
            if (API.GetEntityModel(API.PlayerPedId()) == API.GetHashKey("mp_m_freemode_01"))
            {
                foreach (var uniformParts in _maleUniforms[_department])
                {
                    API.SetPedComponentVariation(API.PlayerPedId(), uniformParts.Component, uniformParts.Drawable, uniformParts.Texture, uniformParts.Pallet);
                }
            }
            else if (API.GetEntityModel(API.PlayerPedId()) == API.GetHashKey("mp_f_freemode_01"))
            {
                foreach (var uniformParts in _femaleUniforms[_department])
                {
                    API.SetPedComponentVariation(API.PlayerPedId(), uniformParts.Component, uniformParts.Drawable, uniformParts.Texture, uniformParts.Pallet);
                }
            }
        }

        private void TakeUniform()
        {
            TriggerServerEvent("RefreshClothes");   
        }











        #region Items
        private void SetupItems()
        {
            InventoryProcessing.Instance.AddItemUse("Police Lock Tool(P)",PoliceLockTool);
        }

        public async void PoliceLockTool()
        {
            var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
            var vehicle = API.GetClosestVehicle(playerPos.X, playerPos.Y, playerPos.Z, 4, 0, 70);
            var random = new Random();
            var rdmInt = random.Next(3);
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            Game.PlayerPed.Task.PlayAnimation("misscarstealfinalecar_5_ig_3", "crouchloop");
            var lockPicking = true;
            async void CancelLockpick()
            {
                while (lockPicking)
                {
                    if (API.IsControlJustPressed(0, (int)Control.PhoneCancel))
                    {
                        lockPicking = false;
                        API.ClearPedTasks(API.PlayerPedId());
                    }
                    await Delay(0);
                }
            }
            CancelLockpick();
            await Delay(15000);
            lockPicking = false;
            Game.PlayerPed.Task.ClearAll(); 
            API.SetVehicleDoorsLocked(vehicle, 0);
            Utility.Instance.SendChatMessage("[LOCKPICK]", "You successfully unlock the vehicle with your tool!", 255, 0, 0);
        }

        public async void FingerprintScanner()
        {
            TriggerServerEvent("RequestID", Game.Player.ServerId);
            Game.PlayerPed.Task.PlayAnimation("mp_arresting","a_uncuff");
            await Delay(3000);
            Game.PlayerPed.Task.ClearAll();
            ClosestPlayerReturnInfo output;
            Utility.Instance.GetClosestPlayer(out output);
            if (output.Dist < 4)
            {
                TriggerServerEvent("RequestID",API.GetPlayerServerId(output.Pid));
            }
        }

        #endregion



    }

    public class PoliceGear : BaseStore
    {
        public PoliceGear() : base("Police Station", "Pick up your police gear here.", 60, 29,
            new List<Vector3>()
            {
                new Vector3(452.09893798828f,-979.99584960938f,30.689596176147f),
            },
            new Dictionary<string, int>()
            {
                ["Tazer(P)"] = 0,
                ["Nighstick(P)"] = 0,
                ["Binoculars(P)"] = 0,
                ["Police Lock Tool(P)"] = 0,
                ["Combat Pistol(P)"] = 0,
                ["Pump Shotgun(P)"] = 0,
                ["Carbine Rifle(P)"] = 0,
                ["Fingerprint Scanner(P)"] = 0,
                ["Spike Strips(P)"] = 0,
                ["Pistol Ammo"] = 10,
                ["Shotgun Ammo"] = 10,
                ["Rifle Ammo"] = 10
            })
        {
            MenuRestricted = false;
        }
    }

}
