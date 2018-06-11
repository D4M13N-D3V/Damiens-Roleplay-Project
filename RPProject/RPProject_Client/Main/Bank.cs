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
using roleplay.Main.Police;
using roleplay.Main.Vehicles;

namespace roleplay.Main
{
    public class Bank : BaseScript
    {

        public static Bank Instance;
        public List<Vector3> Posistions = new List<Vector3>()
        {
            new Vector3(150.266f,-1040.203f,29.374f),
            new Vector3(-1212.980f,-330.841f,37.787f),
            new Vector3(-2962.582f,482.627f,15.703f),
            new Vector3(-112.202f,6469.295f,31.626f),
            new Vector3(314.187f,-278.621f,54.170f),
            new Vector3(-351.534f,-49.529f,49.042f),
            new Vector3(241.727f,220.706f,106.286f),
        };
        public bool MenuRestricted = false;
        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private UIMenu _menu;

        public Vector3 _playerPos;

        public Bank()
        {
            Instance = this;
            SetupBlips(108, 2);
            GarageCheck();
            DrawMarkers();
            GetPlayerPosEverySecond();
        }

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
                    if (Utility.Instance.GetDistanceBetweenVector3s(pos, Game.PlayerPed.Position) < 30)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, pos - new Vector3(0, 0, 1.1f), Vector3.Zero, Vector3.Zero, Vector3.One, Color.FromArgb(255, 0, 255, 0));
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
                API.AddTextComponentString("Bank");
                API.EndTextCommandSetBlipName(blip);
            }
        }

        private async Task GarageCheck()
        {
            while (true)
            {
                _menuOpen = false;
                foreach (var pos in Posistions)
                {
                    var dist = API.Vdist(_playerPos.X, _playerPos.Y, _playerPos.Z, pos.X, pos.Y, pos.Z);
                    if (dist < 3f && !MenuRestricted)
                    {
                        _menuOpen = true;
                    }
                }

                if (_menuOpen && !_menuCreated)
                {
                    _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(
                        InteractionMenu.Instance._interactionMenu, "Bank", "Access your bank account.", new PointF(5, Screen.Height / 2));
                    var depositButton = new UIMenuItem("Deposit");
                    var withdrawlButton = new UIMenuItem("Withdrawl");
                    var transferButton = new UIMenuItem("Transfer");
                    _menu.AddItem(depositButton);
                    _menu.AddItem(withdrawlButton);
                    _menu.AddItem(transferButton);
                    _menu.OnItemSelect += (sender, selectedItem, index) =>
                    {
                        if (selectedItem == depositButton)
                        {
                            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                            Utility.Instance.KeyboardInput(
                                "Amount of money to deposit into your bank account.", "", 10,
                                delegate (string s)
                                {
                                    TriggerServerEvent("DepositMoney", Convert.ToInt32(s));
                                });
                        }
                        if (selectedItem == withdrawlButton)
                        {
                            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                            Utility.Instance.KeyboardInput(
                                "Amount of money to withdrawl from your bank account.", "", 10,
                                delegate (string s)
                                {
                                    TriggerServerEvent("WithdrawMoney", Convert.ToInt32(s));
                                });
                        }
                        if (selectedItem == transferButton)
                        {
                            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                            Utility.Instance.KeyboardInput(
                                "The ID of the player to send the money to", "", 10,
                                delegate (string idS)
                                {
                                    Utility.Instance.KeyboardInput(
                                        "Amount of money to transfer from your bank account.", "", 10,
                                        delegate (string amountS)
                                        {
                                            if (Int32.TryParse(idS, out var id) &&
                                                Int32.TryParse(amountS, out var amount))
                                            {
                                                TriggerServerEvent("TransferMoney", amount, id);
                                            }
                                        });
                                });

                        }
                    };
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
}
