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
                    await Delay(500);
                    if (API.IsPedInAnyVehicle(API.PlayerPedId(), false))
                    {
                        API.SetVehicleEngineOn(veh, running, true, true);
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
                    List<dynamic> doorOptions = new List<dynamic>() { "All", "Front Left", "Front Right", "Rear Left", "Rear Right" };
                    var doorSlider = new UIMenuSliderItem("Door(s) : All",doorOptions,0);
                    var windowSlider= new UIMenuSliderItem("Window(s) : All", doorOptions, 0);

                    _menu.AddItem(engineButton);
                    _menu.AddItem(lockButton);
                    _menu.AddItem(hoodButton);
                    _menu.AddItem(trunkButton);
                    _menu.AddItem(doorSlider);
                    _menu.AddItem(windowSlider);

                    _menu.OnItemSelect += (sender, item, index) =>
                    {
                        var veh = API.GetVehiclePedIsIn(API.PlayerPedId(), false);
                        var plate = API.GetVehicleNumberPlateText(veh);
                        var myCar = false;
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
                                    }
                                    else
                                    {
                                        API.SetVehicleDoorsLocked(veh, 2);
                                        item.Text = "~r~Toggle Lock";
                                    }
                                }
                                else if (item == hoodButton)
                                {

                                }
                                else if (item == trunkButton)
                                {

                                }
                                break;
                            }
                        }

                        if (!myCar)
                        {
                            Utility.Instance.SendChatMessage("[VEHICLE MANAGER]", "This is not your car!", 0, 150, 40);
                        }



                    };

                    _menu.OnSliderChange += (sender, item, index) =>
                    {
                        if (item == doorSlider)
                        {
                            doorSlider.Text = "Open/Close Door(s) : " + doorOptions[index];
                        }
                        else if (item == windowSlider)
                        {

                            windowSlider.Text = "Open/Close Windows(s) : " + doorOptions[index];
                        }
                    };
                    _menu.OnSliderSelect += (sender, item, index) => { };

                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();

                }
                else if (!_insideMenuOpen && _insideMenuCreated)
                {
                    _insideMenuCreated = false;
                    _menu.Visible = false;
                    InteractionMenu.Instance._interactionMenu.RemoveItemAt(_insideMenuIndex);
                    InteractionMenu.Instance._interactionMenu.Visible = true;
                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                }
                await Delay(0);
            }
        }
    }
}
