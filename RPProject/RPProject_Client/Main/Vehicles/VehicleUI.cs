using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;
using roleplay.Users.Inventory;

namespace roleplay.Main.Vehicles
{
    public class VehicleUI : BaseScript
    {
        public static VehicleUI Instance;
        public VehicleUI()
        {
            Instance = this;
            UICheck();
            LeaveEngineRunning();
            EngineCheck();
            EventHandlers["ToggleHood"] += new Action(() =>
            {
                if (Game.PlayerPed.IsInVehicle())
                {
                    ToggleHoodInside();
                }
                else
                {
                    ToggleHoodOutside();
                }
            });
            EventHandlers["ToggleTrunk"] += new Action(() =>
            {
                if (Game.PlayerPed.IsInVehicle())
                {
                    ToggleTrunkInside();
                }
                else
                {
                    ToggleTrunkOutside();
                }
            });
            EventHandlers["ToggleLock"] += new Action(() =>
            {
                if (Game.PlayerPed.IsInVehicle())
                {
                    LockVehicleInside();
                }
                else
                {
                    LockVehicleOutside();
                }
            });
            EventHandlers["ToggleEngine"] += new Action(() =>
            {
                if (Game.PlayerPed.IsInVehicle())
                {
                    ToggleEngine();
                }
            });
            EventHandlers["WindowsDown"] += new Action(() =>
            {
                if (Game.PlayerPed.IsInVehicle())
                {
                    RollWindowsDown();
                }
            });
            EventHandlers["WindowsUp"] += new Action(() =>
            {
                if (Game.PlayerPed.IsInVehicle())
                {
                    RollWindowsUp();
                }
            });


            EventHandlers["HotwireCar"] += new Action(Hotwire);

        }

        private UIMenu _menu = null;
        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private int _menuIndex = 0;
        public bool _insideMenu = false;

        private bool _canHotwire = true;
        private bool _hotWiring = false;

        private async void Hotwire()
        {
            if (_canHotwire && Game.PlayerPed.IsInVehicle())
            {
                _hotWiring = true;
                isHotwiring();
                async Task isHotwiring()
                {
                    while (_hotWiring)
                    {
                        Game.DisableControlThisFrame(0,Control.VehicleExit);
                        await Delay(0);
                    }
                }
                _canHotwire = false;
                Utility.Instance.SendChatMessage("[Hotwire]", "You start trying to hotwire the vehicle.", 255, 0, 0);
                Game.PlayerPed.Task.PlayAnimation("mini@repair", "fixing_a_player");
                await Delay(12000);
                Game.PlayerPed.Task.ClearAll();
                _hotWiring = false;
                API.SetVehicleEngineOn(Game.PlayerPed.CurrentVehicle.Handle, true, false, false);
                API.SetVehiclePetrolTankHealth(Game.PlayerPed.CurrentVehicle.Handle, 1000);
                Utility.Instance.SendChatMessage("[Hotwire]", "You have hotwired the vehicle.", 255, 0, 0);
                await Delay(600000);
                _canHotwire = true;
            }
            else
            {
                Utility.Instance.SendChatMessage("[Hotwire]", "You can not hotwire a vehicle, you have done so too recently.", 255, 0, 0);
            }
        }

        private async Task EngineCheck()
        {
            while (true)
            {
                await Delay(0);
            }
        }

        private async Task LeaveEngineRunning()
        {
            while (true)
            {
                var veh = API.GetPlayersLastVehicle();
                if (Game.PlayerPed.IsInVehicle())
                {
                    var running = API.GetIsVehicleEngineRunning(veh);
                    await Delay(2000);
                    if (!API.IsPedInAnyVehicle(Game.PlayerPed.Handle, false))
                    {
                        API.SetVehicleEngineOn(veh, running, true, true);
                    }
                }
                await Delay(1);
            }
        }

        private async Task UICheck()
        {
            while (InteractionMenu.Instance == null)
            {
                await Delay(0);
            }
            while (true)
            {
                await Delay(0);
                _menuOpen = false;
                var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
                if (API.IsPedInAnyVehicle(Game.PlayerPed.Handle, false))
                {
                    _menuOpen = true;
                    if (_insideMenu == false)
                    {
                        _insideMenu = true;
                        _menuCreated = false;
                    }
                }
                else
                {
                    _menuOpen = true;
                    if (_insideMenu)
                    {
                        _insideMenu = false;
                        _menuCreated = false;
                    }
                }

                if (_menuOpen && !_menuCreated)
                {
                    _menuCreated = true;


                    if (_menu == null)
                    {
                        _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(InteractionMenu.Instance._interactionMenu,
                            "Vehicle Interaction", "Interact with your vehicle.", new PointF(5, Screen.Height / 2));
                        _menuIndex = InteractionMenu.Instance._interactionMenu.MenuItems.Count - 1;
                    }
                    else
                    {
                        _menu.Clear();
                    }
                    if (!_insideMenu)
                    {
                        var trunkButton = new UIMenuItem("Trunk", "Opens/Closes nearest vehicles trunk.");
                        var hoodButton = new UIMenuItem("Hood", "Opens/Closes the nearest vehicles hood.");
                        var lockButton = new UIMenuItem("Lock", "Locks/Unlocks the nearest vehicle.");
                        _menu.AddItem(trunkButton);
                        _menu.AddItem(hoodButton);
                        _menu.AddItem(lockButton);
                        _menu.OnItemSelect += (sender, item, index) =>
                        {
                            if (item == trunkButton)
                            {
                                ToggleTrunkOutside();
                            }
                            else if (item == hoodButton)
                            {
                                ToggleHoodOutside();
                            }
                            else if (item == lockButton)
                            {
                                LockVehicleOutside();
                            }
                        };
                    }
                    else if (_insideMenu)
                    {
                        var trunkButton = new UIMenuItem("Trunk", "Opens/Closes vehicles trunk.");
                        var hoodButton = new UIMenuItem("Hood", "Opens/Closes the vehicles hood.");
                        var lockButton = new UIMenuItem("Lock", "Locks/Unlocks the vehicle.");
                        var engineButton = new UIMenuItem("Engine", "Turn vehicle engine on/off.");
                        var windowsUpButton = new UIMenuItem("Windows Up", "Turn vehicle engine on/off.");
                        var windowsDownButton = new UIMenuItem("Windows Down", "Turn vehicle engine on/off.");
                        _menu.AddItem(trunkButton);
                        _menu.AddItem(hoodButton);
                        _menu.AddItem(lockButton);
                        _menu.AddItem(engineButton);
                        _menu.AddItem(windowsUpButton);
                        _menu.AddItem(windowsDownButton);
                        _menu.OnItemSelect += (sender, item, index) =>
                        {
                            if (item == trunkButton)
                            {
                                ToggleTrunkInside();
                            }
                            else if (item == hoodButton)
                            {
                                ToggleHoodInside();
                            }
                            else if (item == lockButton)
                            {
                                LockVehicleInside();
                            }
                            else if (item == engineButton)
                            {
                                ToggleEngine();
                            }
                            else if (item == windowsUpButton)
                            {
                                RollWindowsUp();
                            }
                            else if (item == windowsDownButton)
                            {
                                RollWindowsDown();
                            }

                        };
                    }

                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                }
                else if (!_menuOpen && _menuCreated)
                {
                    _menuCreated = false;
                    InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                    InteractionMenu.Instance._interactionMenu.RemoveItemAt(_menuIndex);
                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                    _menu = null;
                }
            }
        }

        private void LockVehicleOutside()
        {
            var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
            var veh = Utility.Instance.ClosestVehicle.Handle;
            if (Utility.Instance.GetDistanceBetweenVector3s(playerPos,API.GetEntityCoords(veh,false))<5 && VehicleManager.Instance.Cars.Contains(veh))
            {
                if (!API.GetVehicleDoorsLockedForPlayer(veh, Game.Player.Handle))
                {
                    Utility.Instance.SendChatMessage("[Vehicle]", "^1Locked", 255, 255, 0);
                    API.SetVehicleDoorsLockedForAllPlayers(veh, true);
                }
                else
                {
                    Utility.Instance.SendChatMessage("[Vehicle]", "^2Unlocked", 255, 255, 0);
                    API.SetVehicleDoorsLockedForAllPlayers(veh, false);
                }
            }
            else
            {
                Utility.Instance.SendChatMessage("[Vehicle]", "^1Vehicle too far away, or you are not the owner!", 255, 255, 0);
            }
        }

        private void ToggleTrunkOutside()
        {
            var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
            var veh = Utility.Instance.ClosestVehicle.Handle;
            if(Utility.Instance.GetDistanceBetweenVector3s(playerPos, API.GetEntityCoords(veh, false)) < 5 && !API.GetVehicleDoorsLockedForPlayer(veh, Game.Player.Handle))
            {
                if (API.GetVehicleDoorAngleRatio(veh, 5) == 0.0f)
                {
                    API.SetVehicleDoorOpen(veh, 5, false, false);
                }
                else
                {
                    API.SetVehicleDoorShut(veh, 5, false);
                }
            }
        }

        private void ToggleHoodOutside()
        {
            var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
            var veh = Utility.Instance.ClosestVehicle.Handle;
            if (Utility.Instance.GetDistanceBetweenVector3s(playerPos, API.GetEntityCoords(veh, false)) < 5 && !API.GetVehicleDoorsLockedForPlayer(veh, Game.Player.Handle))
            {
                if (API.GetVehicleDoorAngleRatio(veh, 4) == 0.0f)
                {
                    API.SetVehicleDoorOpen(veh, 4, false, false);
                }
                else
                {
                    API.SetVehicleDoorShut(veh, 4, false);
                }
            }
        }

        private void RollWindowsDown()
        {
            var veh = API.GetVehiclePedIsIn(Game.PlayerPed.Handle, false);
            API.RollDownWindow(veh, 0);
            API.RollDownWindow(veh, 1);
            API.RollDownWindow(veh, 2);
            API.RollDownWindow(veh, 3);
        }

        private void RollWindowsUp()
        {
            var veh = API.GetVehiclePedIsIn(Game.PlayerPed.Handle, false);
            API.RollUpWindow(veh, 0);
            API.RollUpWindow(veh, 1);
            API.RollUpWindow(veh, 2);
            API.RollUpWindow(veh, 3);
        }
        
        private void ToggleEngine()
        {
            var veh = API.GetVehiclePedIsIn(Game.PlayerPed.Handle, false);
            if (API.GetIsVehicleEngineRunning(veh))
            {
                Utility.Instance.SendChatMessage("[Vehicle]", "^1Engine Off", 255, 255, 0);
                API.SetVehiclePetrolTankHealth(veh, 0);
                API.SetVehicleEngineOn(veh,false,false,false);
            }
            else
            {
                if (!VehicleManager.Instance.Cars.Contains(veh)) { Utility.Instance.SendChatMessage("[Vehicle]", "You do not own the car, you can not turn it on and off without hotwiring!", 255, 255, 0); return; }
                Utility.Instance.SendChatMessage("[Vehicle]", "^2Engine On", 255, 255, 0);
                API.SetVehicleFuelLevel(veh,1000);
                API.SetVehicleEngineOn(veh, true, false, false);
            }
        }

        private void LockVehicleInside()
        {
            var veh = API.GetVehiclePedIsIn(Game.PlayerPed.Handle, false);
            if (!API.GetVehicleDoorsLockedForPlayer(veh,Game.Player.Handle))
            {
                Utility.Instance.SendChatMessage("[Vehicle]", "^1Locked", 255, 255, 0);
                API.SetVehicleDoorsLockedForAllPlayers(veh, true);
            }
            else
            {
                Utility.Instance.SendChatMessage("[Vehicle]", "^2Unlocked", 255, 255, 0);
                API.SetVehicleDoorsLockedForAllPlayers(veh, false);
            }
        }

        private void ToggleTrunkInside()
        {
            var veh = API.GetVehiclePedIsIn(Game.PlayerPed.Handle, false);
            if (API.GetVehicleDoorAngleRatio(veh, 5) == 0.0f)
            {
                API.SetVehicleDoorOpen(veh, 5, false, false);
            }
            else
            {
                API.SetVehicleDoorShut(veh, 5, false);
            }
        }

        private void ToggleHoodInside()
        {
            var veh = API.GetVehiclePedIsIn(Game.PlayerPed.Handle, false);
            if (API.GetVehicleDoorAngleRatio(veh, 4) == 0.0f)
            {
                API.SetVehicleDoorOpen(veh, 4, false, false);
            }
            else
            {
                API.SetVehicleDoorShut(veh, 4, false);
            }
        }

    }
}
