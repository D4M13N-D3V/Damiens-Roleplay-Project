

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using client;
using client.Main;
using client.Main.Vehicles;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;

namespace client.Main.Criminal.Informant
{

    public class Informant : BaseScript
    {
        public static Informant Instance;

        public List<InformantInfo> Information = new List<InformantInfo>()
        {
            new InformantInfo("Meth Bulk Pickup", "Desert meth lab.", 5000),
            new InformantInfo("Meth Bulk Sell/Singles Pickup", "Bikers live here.", 6000),
            new InformantInfo("Weed Bulk Pickup", "Farm near the mountains", 8000),
            new InformantInfo("Weed Bulk Sell/Singles Pickup", "Shop on the beach",10000),
            new InformantInfo("Cocaine Bulk Pickup","Boat.",5000),
            new InformantInfo("Cocaine Bulk Sell/Singles","Big ass party house.",6000),
        };

        public List<Vector3> Posistions = new List<Vector3>()
        {
            new Vector3(-2194.2731933594f,4290.6640625f,49.173866271973f),
        };
        
        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private UIMenu _menu;

        public Informant()
        {
            Instance = this;
            SetupBlips(304, 4);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            InformantCheck();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            DrawMarkers();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            GetPlayerPosEverySecond();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }

        public Vector3 _playerPos;
        private async Task GetPlayerPosEverySecond()
        {
            while (true)
            {
                _playerPos = Game.PlayerPed.Position;
                await Delay(1000);
            }
        }


        private async Task DrawMarkers()
        {
            while (true)
            {
                foreach (var pos in Posistions)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(pos, _playerPos) < 30)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, pos - new Vector3(0, 0, 0.8f), Vector3.Zero, Vector3.Zero, new Vector3(2, 2, 2), Color.FromArgb(255, 180, 0, 0));
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
                API.SetBlipScale(blip, 1f);
                API.SetBlipAsShortRange(blip, true);
                API.BeginTextCommandSetBlipName("STRING");
                API.AddTextComponentString("Informant");
                API.EndTextCommandSetBlipName(blip);
            }
        }

        private async Task InformantCheck()
        {
            while (true)
            {

                _menuOpen = false;
                foreach (var pos in Posistions)
                {
                    var dist = API.Vdist(_playerPos.X, _playerPos.Y, _playerPos.Z, pos.X, pos.Y, pos.Z);
                    if (dist < 6f)
                    {
                        _menuOpen = true;
                    }
                }

                if (_menuOpen && !_menuCreated)
                {
                    _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(
                        InteractionMenu.Instance._interactionMenu, "Informant", "Buy information to find criminal spots.", new PointF(5, Screen.Height / 2));
                    foreach (var info in Information)
                    {
                        var button = new UIMenuItem(info.Title,Convert.ToString(info.Price));
                        _menu.AddItem(button);
                        _menu.OnItemSelect += (sender, item, index) =>
                        {
                            if (item == button)
                            {
                                TriggerServerEvent("BuyInformerInformation", info.Title);
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
                await Delay(1000);
            }
        }
    }

}
