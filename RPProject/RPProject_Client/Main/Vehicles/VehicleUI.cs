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
    public class VehicleUI:BaseScript
    {
        public static VehicleUI Instance;
        public VehicleUI()
        {
            Instance = this;
            UICheck();
            EngineCheck();
        }

        private UIMenu _menu;
        private bool _insideMenuOpen = false;
        private bool _insideMenuCreated = false;
        private int _insideMenuIndex = 0;

        private bool lastCarEngineSet = false;
        private bool lastCarEngineState = false;

        private bool windowsDown = false;

        private async void EngineCheck()
        {
            while (true)
            {
                var veh = API.GetPlayersLastVehicle();
                if (API.IsPedInAnyVehicle(API.PlayerPedId(), false))
                {
                    var running = API.GetIsVehicleEngineRunning(veh);
                    await Delay(2000);
                    if (!API.IsPedInAnyVehicle(API.PlayerPedId(), false))
                    {
                        API.SetVehicleEngineOn(veh, running, true, true);
                    }
                }
                if (!API.IsPedInAnyVehicle(API.PlayerPedId(), true))
                {
                    var running = API.GetIsVehicleEngineRunning(veh);
                    if (API.IsVehicleEngineStarting(veh))
                    {
                        running = false;
                    }   
                    await Delay(2000);
                    if (API.IsPedInAnyVehicle(API.PlayerPedId(), false))
                    {
                        API.SetVehiclePetrolTankHealth(veh, 0);
                        API.SetVehicleEngineOn(veh, false, true, true);   
                        if (running)
                        {
                            API.SetVehiclePetrolTankHealth(veh, 1000);
                            API.SetVehicleEngineOn(veh, true, true, true);
                        }
                    }
                }
                await Delay(0);
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
                _insideMenuOpen = false;
                var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
                if (API.IsPedInAnyVehicle(API.PlayerPedId(), false))
                {
                    _insideMenuOpen = true;
                }

                if (_insideMenuOpen && !_insideMenuCreated)
                {
                    _insideMenuCreated = true;
                    _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenu(InteractionMenu.Instance._interactionMenu,
                        "Vehicle Interaction", "Interact with your vehicle.");
                    _insideMenuIndex = InteractionMenu.Instance._interactionMenu.MenuItems.Count - 1;

                    var engineButton = new UIMenuItem("~r~Toggle Engine", "Turn your vehicle on and off.");
                    if (API.GetIsVehicleEngineRunning(API.GetVehiclePedIsIn(API.PlayerPedId(), false)))
                    {
                        engineButton = new UIMenuItem("~g~Toggle Engine", "Turn your vehicle on and off.");
                    }

                    var lockButton = new UIMenuItem("~g~Toggle Lock", "Lock and unclocked your vehicle.");
                    var hoodButton = new UIMenuItem("~g~Pop Hood", "Lock and unclocked your vehicle.");
                    var trunkButton = new UIMenuItem("~g~Pop Trunk", "Lock and unclocked your vehicle.");
                    var windowsButton = new UIMenuItem("~g~Toggle Windows", "Toggle your windows up and down.");
                    var hotwireButton = new UIMenuItem("~r~Hotwire Car", "Attempt to hotwire the vehicle!");

                    _menu.AddItem(engineButton);
                    _menu.AddItem(lockButton);
                    _menu.AddItem(hoodButton);
                    _menu.AddItem(trunkButton);
                    _menu.AddItem(windowsButton);
                    _menu.AddItem(hotwireButton);

                    _menu.OnItemSelect += (sender, item, index) =>
                    {
                        var veh = API.GetVehiclePedIsIn(API.PlayerPedId(), false);
                        var plate = API.GetVehicleNumberPlateText(veh);
                        var myCar = false;
                        if (item == hoodButton)
                        {
                            if (Utility.Instance.IsDoorOpen(veh, 4))
                            {
                                Utility.Instance.SendChatMessage("[VEHICLE MANAGER]", "Reaches below the dash and pulls a lever that closes the hood with hydralics.", 0, 150, 40);
                                API.SetVehicleDoorShut(veh, 4, false);
                            }
                            else
                            {
                                Utility.Instance.SendChatMessage("[VEHICLE MANAGER]", "Reaches below the dash and pulls a lever that opens the hood.", 0, 150, 40);
                                API.SetVehicleDoorOpen(veh, 4, false, false);
                            }
                        }
                        else if (item == trunkButton)
                        {
                            if (Utility.Instance.IsDoorOpen(veh, 5))
                            {
                                Utility.Instance.SendChatMessage("[VEHICLE MANAGER]", "Reaches below the dash and pulls a lever that closes the trunk with hydralics.", 0, 150, 40);
                                API.SetVehicleDoorShut(veh, 5, true);
                            }
                            else
                            {
                                Utility.Instance.SendChatMessage("[VEHICLE MANAGER]", "Reaches below the dash and pulls a lever that opens the trunk.", 0, 150, 40);
                                API.SetVehicleDoorOpen(veh, 5, false, false);
                            }
                        }
                        else if (item == hotwireButton)
                        {
                            VehicleTheft.Instance.Hotwire();
                        }
                        else if (item == windowsButton)
                        {
                            if (windowsDown)
                            {
                                Utility.Instance.SendChatMessage("[VEHICLE MANAGER]", "Holds down the button that rolls down the windows of the car!", 0, 150, 40);
                                windowsDown = false;
                                API.RollDownWindows(veh);
                                windowsButton.Text = "~rToggle Windows";
                            }
                            else
                            {
                                Utility.Instance.SendChatMessage("[VEHICLE MANAGER]", "Holds down the button that rolls up the windows of the car!", 0, 150, 40);
                                windowsDown = true;
                                windowsButton.Text = "~g~Toggle Windows";
                                API.RollUpWindow(veh, 0);
                                API.RollUpWindow(veh, 1);
                                API.RollUpWindow(veh, 2);
                                API.RollUpWindow(veh, 3);
                            }
                        }
                        foreach (Item itemObj in InventoryUI.Instance.Inventory)
                        {
                            if (itemObj.Name.Split('-').Length>1 && itemObj.Name.Split('-')[1] == plate)
                            {
                                myCar = true;
                                if (item == engineButton)
                                {
                                    var vehicleRunning = API.IsVehicleEngineOn(veh);
                                    if (vehicleRunning)
                                    {
                                        lastCarEngineSet = true;
                                        lastCarEngineState = false;
                                        API.SetVehicleEngineOn(veh, false, false, false);
                                        API.SetVehiclePetrolTankHealth(veh, 0);
                                        item.Text = "~r~Toggle Engine";
                                        TriggerServerEvent("ActionCommandFromClient", "Twists the key to the left turning the engine of the vehicle off, and pulls the keys out, putting them into thier pocket.");
                                    }
                                    else
                                    {
                                        lastCarEngineSet = true;
                                        lastCarEngineState = true;
                                        API.SetVehiclePetrolTankHealth(veh, 1000);
                                        API.SetVehicleEngineOn(veh, true, false, false);
                                        item.Text = "~g~Toggle Engine";
                                        TriggerServerEvent("ActionCommandFromClient", "Pulls the keys out of thier pocket and inserts it into the ignition, turning it to the right, and turning the vehicle on.");
                                    }
                                }
                                else if (item == lockButton)
                                {
                                    var locked = API.GetVehicleDoorLockStatus(veh);
                                    if (locked==2)
                                    {
                                        API.SetVehicleDoorsLocked(veh,0);
                                        item.Text = "~g~Toggle Lock";
                                        TriggerServerEvent("ActionCommandFromClient", "Presses the button that unlocks the doors of the vehicle.");
                                    }
                                    else
                                    {
                                        TriggerServerEvent("ActionCommandFromClient", "Presses the button that locks the doors of the vehicle.");
                                        API.SetVehicleDoorsLocked(veh, 2);
                                        item.Text = "~r~Toggle Lock";
                                    }
                                }
                                break;
                            }
                        }

                        if (!myCar)
                        {
                            Utility.Instance.SendChatMessage("[VEHICLE MANAGER]", "This is not your car!", 0, 150, 40);
                        }



                    };

                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();

                }
                else if (!_insideMenuOpen && _insideMenuCreated)
                {
                    _insideMenuCreated = false;
                    _menu.Visible = false;
                    InteractionMenu.Instance._interactionMenu.Visible = false;
                    InteractionMenu.Instance._interactionMenu.RemoveItemAt(_insideMenuIndex);
                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();

                }
            }
        }
    }
}
    