using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;

namespace roleplay.Main
{
    public class VehicleStore : BaseScript
    {

        public static VehicleStore Instance;
        private UIMenu _menu;
        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private int _menuIndex = 0;
        
        private List<Vector3> _stores = new List<Vector3>()
        {
            new Vector3(-33.491161346436f,-1102.2437744141f,26.6f)
        };

        #region Vehicle Prices
        private readonly Dictionary<string, int> _vehiclePrices = new Dictionary<string, int>()
        {
            {"Huntley",0}
        };
        #endregion

        public VehicleStore()
        {
            Instance = this;
            SetupBlips();
            VehicleShopCheck();
            DrawMarkers();
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
                var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
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
                await Delay(0);
            }
        }

        private void SetupMenu()
        {
            _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenu(InteractionMenu.Instance._interactionMenu,
                "Vehicle Shop", "Buy your vehicles here!");
            _menuIndex = InteractionMenu.Instance._interactionMenu.MenuItems.Count - 1;
            foreach (string key in _vehiclePrices.Keys)
            {
                var item = new UIMenuItem("~b~"+API.GetDisplayNameFromVehicleModel(Convert.ToUInt32(API.GetHashKey(key)))+"-~g~$"+_vehiclePrices[key]);
                    _menu.AddItem(item);
                _menu.OnItemSelect += (sender, selectedItem, index) =>
                {
                    if (selectedItem == item)
                    {
                        TriggerServerEvent("BuyVehicle",key,key);
                        InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
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
