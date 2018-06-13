    using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;

namespace client.Main.Vehicles
{

    public class VehicleStoreEntryInformation
    {
        public string Model;
        public int Prices;
        public int Stock;
        public VehicleStoreEntryInformation(string model, int prices, int stock)
        {
            Model = model;
            Prices = prices;
            Stock = stock;
        }
    }

    public class VehicleStore : BaseScript
    {

        public static VehicleStore Instance;
        private UIMenu _menu;
        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private int _menuIndex = 0;


        private readonly Dictionary<string,string> _irlCars = new Dictionary<string, string>()
        {
            ["Gauntlet"] = "Chevrolet Camaro 2017 ZL1",
            ["Windsor "] = "Rolls Royce Wraith 2017",
            ["Banshee "] = "Subaru BRZ Rocket Bunny",
            ["Elgy2"] = "R35 Nissan GTR Convertible 2017",
            ["Buffalo2"] = "Audi RS4 Avant",
            ["Dukes"] = "Dodge Charger 1969",
            ["Buffalo"] = "Dodge Charger R/T 2015",
            ["Exemplar"] = "Porch Panarema 2017",
            ["Schafter2"] = "Mazda 6 2016",
            ["Contender"] = "Nissan Titan 2017",
            ["Bison"] = "Dodge Ram 1500 Extended Bed 1999",
            ["Stanier"] = " Crown Victoria 2008",
            ["Ballar"] = "Range Rover Evoque 2017",
            ["RancherXL"] = "Chevrolet Tahoe 2011",
            ["Granger"] = "Chevrolet Suburban 2017",
            ["ninef2"] = "Miata 1999",
            ["Slamvan"] = "Chevrolet C-10 Stepside",
            ["Burrito3"] = "Chevrolet G20",
            ["Sandking2"] = "1980 Ford Bronco",
            ["Oracle2"] = "2017 BMW M760i",
            ["Tailgater"] = "2012 Audi A8L W12",
            ["Mule"] = "Chvrolet G30",
            ["Feltzer2"] = "Mercedes SLS AMG",
            ["Dubsta"] = "2013 Mercedes-Benz G65 AMG",
            ["FMJ"] = "2017 Ford GT",
            ["Sentinel"] = "BMW M2",
            ["Rapidgt3"] = "Rapid GT Classic",
            ["Dominator"] = "Mustang GT",
            ["Phoenix"] = "1986 Chevrolet SS Monte Carlo"
        };

        private List<Vector3> _stores = new List<Vector3>()
        {
            new Vector3(-33.491161346436f,-1102.2437744141f,26.6f)
        };

        public VehicleStore()
        {
            Instance = this;
            SetupBlips();
            VehicleShopCheck();
            DrawMarkers();
            EventHandlers["UpdateVehicleStoreUI"] +=
                new Action<List<dynamic>>(UpdateVehicleStoreUI);
        }


        private List<VehicleStoreEntryInformation> vehicleStoreInfo = new List<VehicleStoreEntryInformation>();
        private void UpdateVehicleStoreUI(List<dynamic> info)
        {
            vehicleStoreInfo.Clear();
            foreach (var var in info)
            {
                vehicleStoreInfo.Add(new VehicleStoreEntryInformation(var.model,var.price,var.stock));
            }
        }


        private async Task DrawMarkers()
        {
            while (true)
            {
                foreach (var pos in _stores)
                {
                    World.DrawMarker(MarkerType.HorizontalCircleSkinny, pos, Vector3.Zero, Vector3.Zero, Vector3.One, Color.FromArgb(175, 255, 0, 0));
                }
                await Delay(0);
            }
        }
        private async Task VehicleShopCheck()
        {
            while (InteractionMenu.Instance == null)
            {
                await Delay(0);
            }

            while (true)
            {
                _menuOpen = false;
                var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
                foreach (Vector3 store in _stores)
                {
                    var distance = API.Vdist(store.X, store.Y, store.Z, playerPos.X, playerPos.Y, playerPos.Z);
                    if (distance < 6)
                    {
                        _menuOpen = true;
                    }
                }
                if (_menuOpen && !_menuCreated)
                {
                    _menuCreated = true;
                    SetupMenu();
                }
                else if (!_menuOpen && _menuCreated)
                {
                    _menuCreated = false;
                    InteractionMenu.Instance._interactionMenu.RemoveItemAt(_menuIndex);
                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                }
                await Delay(1000);
            }
        }

        private async Task SetupMenu()
        {
            await Delay(1000);
            _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(InteractionMenu.Instance._interactionMenu,
                "Vehicle Shop", "Buy your vehicles here!", new PointF(5, Screen.Height / 2));
            _menuIndex = InteractionMenu.Instance._interactionMenu.MenuItems.Count - 1;
            foreach (var info in vehicleStoreInfo)
            {
                Debug.WriteLine(info.Model);
                UIMenuItem item = null;
                if (_irlCars.ContainsKey(info.Model))
                {
                    item = new UIMenuItem("~b~" + _irlCars[info.Model] + "-~g~$" + info.Prices + "~r~ In Stock:" + info.Stock);
                }
                else
                {
                    item = new UIMenuItem("~b~" + info.Model + "-~g~$" + info.Prices + "~r~ In Stock:" + info.Stock);
                }
                    _menu.AddItem(item);
                _menu.OnItemSelect += (sender, selectedItem, index) =>
                {
                    if (selectedItem == item)
                    {
                        TriggerServerEvent("BuyVehicle", info.Model, info.Model);
                        InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                        _menuOpen = false;
                        _menuCreated = false;
                        InteractionMenu.Instance._interactionMenu.RemoveItemAt(_menuIndex);
                        InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                    }
                };
            }
            InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
        }


        private void SetupBlips()
        {
            foreach (Vector3 store in _stores)
            {
                var blip = API.AddBlipForCoord(store.X, store.Y, store.Z);
                API.SetBlipSprite(blip, 225);
                API.SetBlipColour(blip, 2);
                API.SetBlipScale(blip, 0.7f);
                API.SetBlipAsShortRange(blip, true);
                API.BeginTextCommandSetBlipName("STRING");
                API.AddTextComponentString("Vehicle Store");
                API.EndTextCommandSetBlipName(blip);
            }
        }

    }
}
