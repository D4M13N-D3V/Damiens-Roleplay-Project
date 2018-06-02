using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
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
        }

        private UIMenu _menu = null;
        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private int _menuIndex = 0;
        public bool _insideMenu = false;

        private async void EngineCheck()
        {
            while (true)
            {
                await Delay(0);
            }
        }

        private async void LeaveEngineRunning()
        {
            while (true)
            {
                var veh = API.GetPlayersLastVehicle();
                if (Game.PlayerPed.IsInVehicle())
                {
                    var running = API.GetIsVehicleEngineRunning(veh);
                    await Delay(2000);
                    if (!API.IsPedInAnyVehicle(API.PlayerPedId(), false))
                    {
                        API.SetVehicleEngineOn(veh, running, true, true);
                    }
                }
                await Delay(1);
            }
        }

        private async void UICheck()
        {
            while (InteractionMenu.Instance == null)
            {
                await Delay(0);
            }
            while (true)
            {
                await Delay(0);
                _menuOpen = false;
                var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
                var veh = API.GetClosestVehicle(playerPos.X, playerPos.Y, playerPos.Z, 6.0f, 0, 0);
                if (API.IsPedInAnyVehicle(API.PlayerPedId(), false))
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
                        _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenu(InteractionMenu.Instance._interactionMenu,
                            "Vehicle Interaction", "Interact with your vehicle.");
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
            var veh = API.GetClosestVehicle(playerPos.X, playerPos.Y, playerPos.Z, 5.0f, 0, 70);
            Debug.WriteLine(VehicleManager.Instance.car+" "+veh);
            if (API.DoesEntityExist(veh) && VehicleManager.Instance.car == veh)
            {
                if (!API.GetVehicleDoorsLockedForPlayer(veh, Game.Player.Handle))
                {
                    Utility.Instance.SendChatMessage("[VEHICLE]", "^1Locked", 255, 255, 0);
                    API.SetVehicleDoorsLockedForAllPlayers(veh, true);
                }
                else
                {
                    Utility.Instance.SendChatMessage("[VEHICLE]", "^2Unlocked", 255, 255, 0);
                    API.SetVehicleDoorsLockedForAllPlayers(veh, false);
                }
            }
            else
            {
                Utility.Instance.SendChatMessage("[VEHICLE]", "^1Vehicle too far away, or you are not the owner!", 255, 255, 0);
            }
        }

        private void ToggleTrunkOutside()
        {
            var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
            var veh = API.GetClosestVehicle(playerPos.X, playerPos.Y, playerPos.Z, 5.0f, 0, 70);
            if (API.DoesEntityExist(veh) && !API.GetVehicleDoorsLockedForPlayer(veh, Game.Player.Handle))
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
            var veh = API.GetClosestVehicle(playerPos.X, playerPos.Y, playerPos.Z, 5.0f, 0, 70);
            if (API.DoesEntityExist(veh) && !API.GetVehicleDoorsLockedForPlayer(veh, Game.Player.Handle))
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
            if (VehicleManager.Instance.car != veh) { Utility.Instance.SendChatMessage("[VEHICLE]","You do not own the car, you can not turn it on and off without hotwiring!",255,255,0); return; }
            if (API.GetIsVehicleEngineRunning(veh))
            {
                Utility.Instance.SendChatMessage("[VEHICLE]", "^1Engine Off", 255, 255, 0);
                API.SetVehiclePetrolTankHealth(veh, 0);
                API.SetVehicleEngineOn(veh,false,false,false);
            }
            else
            {
                Utility.Instance.SendChatMessage("[VEHICLE]", "^2Engine On", 255, 255, 0);
                API.SetVehicleFuelLevel(veh,1000);
                API.SetVehicleEngineOn(veh, true, false, false);
            }
        }

        private void LockVehicleInside()
        {
            var veh = API.GetVehiclePedIsIn(Game.PlayerPed.Handle, false);
            if (!API.GetVehicleDoorsLockedForPlayer(veh,Game.Player.Handle))
            {
                Utility.Instance.SendChatMessage("[VEHICLE]", "^1Locked", 255, 255, 0);
                API.SetVehicleDoorsLockedForAllPlayers(veh, true);
            }
            else
            {
                Utility.Instance.SendChatMessage("[VEHICLE]", "^2Unlocked", 255, 255, 0);
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
