using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;
using roleplay.Main;
using roleplay.Main.Vehicles;
using roleplay.Users.Inventory;
using roleplay.Users.Login;

namespace roleplay.Main.Police
{
   

    public class EMSUniformComponent
    {
        public int Component;
        public int Drawable;
        public int Texture;
        public int Pallet;
        public EMSUniformComponent(int comp, int draw, int text, int pallet)
        {
            Component = comp;
            Drawable = draw;
            Texture = text;
            Pallet = pallet;
        }
    }

    public class EMS : BaseScript
    {
        //These still need to be changed to be ems instead of police
        private readonly Dictionary<string, List<EMSUniformComponent>> _maleUniforms =
            new Dictionary<string, List<EMSUniformComponent>>()
            {
                ["EMS"] = new List<EMSUniformComponent>()
                {
                    new EMSUniformComponent(3,30,1,0),
                    new EMSUniformComponent(4,23,8,0),
                    new EMSUniformComponent(5,0,0,0),
                    new EMSUniformComponent(6,24,0,0),
                    new EMSUniformComponent(7,127,0,0),
                    new EMSUniformComponent(8,129,0,0),
                    new EMSUniformComponent(11,250,1,0),
                }
            };

        private readonly Dictionary<string, List<EMSUniformComponent>> _femaleUniforms =
            new Dictionary<string, List<EMSUniformComponent>>()
            {
                ["EMS"] = new List<EMSUniformComponent>()
                {
                    new EMSUniformComponent(3,44,1,0),
                    new EMSUniformComponent(4,41,1,0),
                    new EMSUniformComponent(5,0,0,0),
                    new EMSUniformComponent(6,52,0,0),
                    new EMSUniformComponent(7,97,0,0),
                    new EMSUniformComponent(8,159,0,0),
                    new EMSUniformComponent(11,258,0,0),
                }
            };

        public int EMSCount = 0;
        private bool _onDuty = false;
        private string _rankName = "";
        private string _department = "";

        public static EMS Instance;

        public EMS()
        {
            Instance = this;
            StopDispatch();
            DisableAutospawn();
            DeathCheck();
            EventHandlers["EMS:SetOnDuty"] += new Action<dynamic>(OnDuty);
            EventHandlers["EMS:SetOffDuty"] += new Action(OffDuty);
            EventHandlers["EMS:RefreshOnDutyOfficers"] += new Action<dynamic>(RefreshCops);
            EventHandlers["Revive"] += new Action(Revive);
        }

        private async void DisableAutospawn()
        {
            await Delay(5000);
            Exports["spawnmanager"].setAutoSpawn(false);
        }

        private async void StopDispatch()
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
            EMSCount = copCount;
        }

        private void OnDuty(dynamic data)
        {
            var department = Convert.ToString(data);
            _department = department;
            Utility.Instance.SendChatMessage("[EMS]", "You have gone on duty.", 0, 255, 0);
            _onDuty = true;
            EMSGear.Instance.MenuRestricted = false;
            EMSGarage.Instance.MenuRestricted = false;
            GiveUniform();
        }

        private void OffDuty()
        {
            Utility.Instance.SendChatMessage("[EMS]", "You have gone off duty.", 0, 255, 0);
            _rankName = "";
            _onDuty = false;
            EMSGear.Instance.MenuRestricted = true;
            EMSGarage.Instance.MenuRestricted = true;
            _department = "";
            TakeUniform();
        }

        private void GiveUniform()
        {
            if (_department == "USMS" || _department == "") { return; }
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




        private bool _dead = false;
        private const int _respawnTime = 300;
        private int _respawnTimeLeft = _respawnTime;
        private List<Vector3> Hospitals = new List<Vector3>()
            {
                new Vector3(1155.26f, -1520.82f, 34.9f),
                new Vector3(295.411f, -1446.88f, 30.5f),
                new Vector3(361.145f, -584.414f, 29.2f),
                new Vector3(-449.639f, -340.586f, 34.8f),
                new Vector3(-874.467f, -307.896f, 39.8f),
                new Vector3(-677.135f, 310.275f, 83.5f),
                new Vector3(1839.39f, 3672.78f, 34.6f),
                new Vector3(-242.968f, 6326.29f, 32.8f)
            };
        private async void DeathCheck()
        {
            while (true)
            {
                if (Game.PlayerPed.Health <= 0 && !_dead)
                {
                    Dead();
                }
                await Delay(1000);
            }
        }

        private async void Dead()
        {
            _dead = true;
            Game.PlayerPed.Health = Game.PlayerPed.MaxHealth;
            API.NetworkResurrectLocalPlayer(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, 0, true, false);
            Game.PlayerPed.IsInvincible = true;
            RespawnTimer();
            while (_dead)
            {
                Game.PlayerPed.Ragdoll(2000,RagdollType.Normal);
                await Delay(0);
            }
            Game.PlayerPed.IsInvincible = false;
            Game.PlayerPed.CancelRagdoll();
        }

        public bool NeedsPills = false;
        private async void Revive()
        {
            Game.PlayerPed.ResetVisibleDamage();
            _dead = false;
            API.RequestAnimSet("move_injured_generic");
            Weapons.Instance.RefreshWeapons();
            while (!API.HasAnimSetLoaded("move_injured_generic"))
            {
                await Delay(0);
            }
            API.SetPedMovementClipset(Game.PlayerPed.Handle,"move_injured_generic", 1);
            NeedsPills = true;
            PedDamage.Instance.ResetInjuries();
            while (NeedsPills)
            {
                Game.DisableControlThisFrame(0,Control.Sprint);
                await Delay(0);
            }
        }

        private async void RespawnTimer()
        {
            _respawnTimeLeft = _respawnTime;
            UI();
            async void UI()
            {
                while (_dead)
                {
                    if (_respawnTimeLeft == 0)
                    {
                        Utility.Instance.DrawTxt(0.5f, 0.3f, 0, 0, 1.5f, "Press E to respawn.", 255, 190, 190, 255, true);
                        if (Game.IsControlJustPressed(0, Control.Context))
                        {
                            Respawn();
                        }
                    }
                    else
                    {
                        Utility.Instance.DrawTxt(0.5f, 0.3f, 0, 0, 1.5f, _respawnTimeLeft + " left until you can respawn.", 255, 190, 190, 255, true);
                    }
                    await Delay(0);
                }
            }

            while (_dead)
            {
                _respawnTimeLeft = _respawnTimeLeft - 1;
                if (_respawnTimeLeft <= 0)
                {
                    return;
                }
                await Delay(1000);
            }
        }

        private async void Respawn()
        {
            PedDamage.Instance.ResetInjuries();
            Game.PlayerPed.ResetVisibleDamage();
            _dead = false;
            Vector3 closestSpot = Hospitals[0];
            float closestSpotDistance = 10000000;
            foreach (var spot in Hospitals)
            {
                var dist = Utility.Instance.GetDistanceBetweenVector3s(Game.PlayerPed.Position, spot);
                if (dist < closestSpotDistance)
                {
                    closestSpotDistance = dist;
                    closestSpot = spot;
                }
            }
            var targetPos = closestSpot + new Vector3(0, 0, 1);
            Game.PlayerPed.Position = targetPos;
            API.RequestAnimSet("move_injured_generic");
            Weapons.Instance.RefreshWeapons();
            while (!API.HasAnimSetLoaded("move_injured_generic"))
            {
                await Delay(0);
            }
            API.SetPedMovementClipset(Game.PlayerPed.Handle, "move_injured_generic", 1);
        }

    }

    public class EMSGear : BaseStore
    {
        public static EMSGear Instance;
        public EMSGear() : base("Hospital", "Pick up your EMS gear here.", 61, 24,
            new List<Vector3>()
            {
                new Vector3(1155.26f,-1520.82f,34.9f),
                new Vector3(295.411f,-1446.88f,30.5f),
                new Vector3(361.145f,-584.414f,29.2f),
                new Vector3(-449.639f,-340.586f,34.8f),
                new Vector3(-874.467f,-307.896f,39.8f),
                new Vector3(-677.135f,310.275f,83.5f),
                new Vector3(1839.39f,3672.78f,34.6f),
                new Vector3(-242.968f,6326.29f,32.8f)
            },
            new Dictionary<string, int>()
            {
                ["Bandages(EMS)"] = 0,
                ["Binoculars(EMS)"] = 0,
                ["Scuba Gear(EMS)"] = 0,
                ["Medical Supplies(EMS)"] = 0,
                ["Pain Killers(EMS)"] = 0,
                ["First Aid Kit(EMS)"] = 0
            })
        {
            Instance = this;
            MenuRestricted = true;
        }
    }

    public class EMSGarage : BaseScript
    {
        public static EMSGarage Instance;
        public List<dynamic> Vehicles;
        public List<Vector3> Posistions = new List<Vector3>()
        {
            new Vector3(-237.292f,6332.39f,32.8f),
            new Vector3(1842.64f,3667.10f,33.9f),
            new Vector3(-657.582f,293.92f,81.9f),
            new Vector3(-876.029f,-300.418f,39.9f),
            new Vector3(1169.01f,-1509.82f,34.9f),
            new Vector3(303.086f,-1439.04f,30.3f),
            new Vector3(364.135f,-591.145f,29.1f),
            new Vector3(-475.254f,-352.322f,34.8f),
        };
        public bool MenuRestricted = true;
        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private UIMenu _menu;

        private bool CarIsOut = true;

        public EMSGarage()
        {
            Instance = this;
            SetupBlips(61, 24);
            GarageCheck();

            EventHandlers["UpdateEMSCars"] += new Action<List<dynamic>>(delegate (List<dynamic> list)
            {
                Vehicles = list;
            });
            DrawMarkers();
        }


        private async void DrawMarkers()
        {
            while (true)
            {
                foreach (var pos in Posistions)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(pos, Game.PlayerPed.Position) < 30)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, pos - new Vector3(0, 0, 1.1f), Vector3.Zero, Vector3.Zero, new Vector3(2,2,2), Color.FromArgb(255, 255, 255, 0));
                    }
                }
                await Delay(0);
            }
        }

        private void SetupBlips(int sprite, int color)
        {
            foreach (var var in Posistions)
            {
                var blip = API.AddBlipForCoord(var.X, var.Y, var.Z);
                API.SetBlipSprite(blip, sprite);
                API.SetBlipColour(blip, color);
                API.SetBlipScale(blip, 0.6f);
                API.SetBlipAsShortRange(blip, true);
                API.BeginTextCommandSetBlipName("STRING");
                API.AddTextComponentString("EMS Garage");
                API.EndTextCommandSetBlipName(blip);
            }
        }

        private async void GarageCheck()
        {
            while (true)
            {

                _menuOpen = false;
                var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
                foreach (var pos in Posistions)
                {
                    var dist = API.Vdist(playerPos.X, playerPos.Y, playerPos.Z, pos.X, pos.Y, pos.Z);
                    if (dist < 6f && !MenuRestricted)
                    {
                        _menuOpen = true;
                    }
                }

                if (_menuOpen && !_menuCreated)
                {
                    _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenu(
                        InteractionMenu.Instance._interactionMenu, "EMS Garage", "Pull out your EMS vehicles.");
                    var putawayButton = new UIMenuItem("Put away car");
                    _menu.AddItem(putawayButton);

                    _menu.OnItemSelect += (sender, selectedItem, index) =>
                    {
                        if (selectedItem == putawayButton)
                        {
                            if (Game.PlayerPed.IsInVehicle() && VehicleManager.Instance.car == Game.PlayerPed.CurrentVehicle.Handle &&
                                CarIsOut)
                            {
                                API.DeleteVehicle(ref VehicleManager.Instance.car);
                                VehicleManager.Instance.car = -1;
                            }
                        }
                    };
                    var buttons = new List<UIMenuItem>();
                    foreach (var item in Vehicles)
                    {
                        var button = new UIMenuItem(item);
                        buttons.Add(button);
                        _menu.AddItem(button);
                        _menu.OnItemSelect += (sender, selectedItem, index) =>
                        {
                            if (selectedItem == button)
                            {
                                Utility.Instance.SpawnCar(selectedItem.Text, delegate (int i)
                                {
                                    CarIsOut = true;
                                    API.SetVehicleNumberPlateText(i, "EMS");
                                    API.ToggleVehicleMod(i, 18, true);
                                    VehicleManager.Instance.car = i;
                                    API.TaskWarpPedIntoVehicle(Game.PlayerPed.Handle, i, -1);
                                });
                            }
                        };
                    }
                    _menuCreated = true;
                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                }
                else if (!_menuOpen && _menuCreated)
                {
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

                await Delay(0);
            }
        }
    }

    public class Hospital : BaseScript
    {
        public static Hospital Instance;

        public bool InHospital = false;
        public int TimeLeft = 0;

        private readonly Vector3 _inPos = new Vector3(331.889771f, -585.050354f, 43.3529778f);
        private readonly Vector3 _outPos = new Vector3(308.729767f, -591.7288f, 43.291893f);
            
        public Hospital()
        {
            Instance = this;
            EventHandlers["Hospital"] += new Action<dynamic>(JailFunc);
            EventHandlers["Unhospital"] += new Action(UnjailFunc);
        }

        private void JailFunc(dynamic timeDynamic)
        {
            Game.PlayerPed.Weapons.RemoveAll();
            Game.PlayerPed.IsInvincible = true;
            Game.PlayerPed.CanSwitchWeapons = false;

            API.SetEntityCoords(Game.PlayerPed.Handle, _inPos.X, _inPos.Y, _inPos.Z, false, false, false, false);
            TimeLeft = timeDynamic;
            InHospital = true;
            Loop();
            Draw();
        }

        private async void Draw()
        {
            while (InHospital)
            {
                Utility.Instance.DrawTxt(0.5f, 0.05f, 0, 200, 1, "Time Left : " + TimeLeft, 255, 255, 255, 255, true);
                await Delay(0);
            }
        }

        private async void Loop()
        {
            while (InHospital)
            {
                Game.PlayerPed.Weapons.RemoveAll();
                Game.PlayerPed.IsInvincible = true;
                Game.PlayerPed.CanSwitchWeapons = false;
                TimeLeft = TimeLeft - 1;
                var pos = Game.PlayerPed.Position;
                if (API.Vdist(pos.X, pos.Y, pos.Z, _inPos.X, _inPos.Y, _inPos.Z) > 30)
                {
                    API.SetEntityCoords(Game.PlayerPed.Handle, _inPos.X, _inPos.Y, _inPos.Z, false, false, false, false);
                }
                TriggerServerEvent("UpdateHospitalTime", TimeLeft);
                await Delay(1000);

                if (TimeLeft == 0)
                {
                    break;
                }
            }
            UnjailFunc();
        }
        private void UnjailFunc()
        {
            API.SetEntityCoords(Game.PlayerPed.Handle, _outPos.X, _outPos.Y, _outPos.Z, false, false, false, false);
            InHospital = false;
            Game.PlayerPed.IsInvincible = false;
            Game.PlayerPed.CanSwitchWeapons = true;
            Weapons.Instance.RefreshWeapons();
        }

    }
}

