using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client.Main.EmergencyServices;
using client.Main.EmergencyServices.Police;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using NativeUI;

namespace client.Main.Criminal.Drugs
{
    public class Drug : BaseScript
    {
        private Dictionary<DrugTypes, DrugInformation> ItemInfo = new Dictionary<DrugTypes, DrugInformation>()
        {
            [DrugTypes.Weed] = new DrugInformation(50, 3, 1000, 2600, 20, 120),
            [DrugTypes.Meth] = new DrugInformation(60, 3, 2000, 2500, 25, 80),
            [DrugTypes.Cocaine] = new DrugInformation(75, 3, 3000, 3600, 100, 250),
            [DrugTypes.Heroine] = new DrugInformation(75, 3, 1000, 1600, 25, 145),
            [DrugTypes.Acid] = new DrugInformation(80, 3, 2000, 3000, 60, 130),
            [DrugTypes.Lsd] = new DrugInformation(50, 3, 1000, 1300, 60, 130),
            [DrugTypes.Crack] = new DrugInformation(50, 3, 1000, 1300, 20, 90),
            [DrugTypes.Xanax] = new DrugInformation(50, 3, 1000, 1300, 20, 0),
            [DrugTypes.Oxy] = new DrugInformation(50, 3, 1000, 1300, 20, 30),
        };
        public Drug Instance;

        private readonly Vector3 _bulkBuyPos;
        private readonly Vector3 _singleBuyPos;

        public DrugTypes Type;

        public string DrugName;

        private Random _random = new Random();

        private List<string> _buyingBulkMessages = new List<string>()
        {
            "I saw some guy loading a bunch of stuff in his vehicle, is super shady, handed off some money too",
            "This guy just gave some dude a fuck ton of cash and is loading something into his car",
            "Send the cops i think i just witnessed something crazy, they gave this guy money, then put something in his car"
        };
        private List<string> _sellingBulkMessages = new List<string>()
        {
            "Just saw a guy drop off a bunch of shit then was given a MASSIVE amount of cash, dont know whats going on should prob send a cop",
            "This guy just unloaded some wierd looking stuff, and then was given a massive amount of cash, whats going on? Can you send a cop?",
            "Super sketchy guy just dropped off a package and took a bunch of money, can you send a cop please?"
        };

        public bool MenuRestricted = false;
        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private UIMenu _menu;
        private bool _bulkMenu = false;

        private Vector3 _playerPos;

        public Drug(Vector3 buyPos, Vector3 sellPos, DrugTypes type)
        {
            _bulkBuyPos = buyPos;
            _singleBuyPos = sellPos;

            Type = type;
            Instance = this;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            DrawMarkers();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            Logic();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            GetPlayerPosEverySecond();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
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
                if (Utility.Instance.GetDistanceBetweenVector3s(_playerPos, _bulkBuyPos) < 30)
                {
                    World.DrawMarker(MarkerType.HorizontalCircleSkinny, _bulkBuyPos - new Vector3(0, 0, 0.8f), Vector3.Zero, Vector3.Zero, new Vector3(3, 3, 3), Color.FromArgb(175, 255, 0, 0));
                }
                if (Utility.Instance.GetDistanceBetweenVector3s(_playerPos, _singleBuyPos) < 30)
                {
                    World.DrawMarker(MarkerType.HorizontalCircleSkinny, _singleBuyPos - new Vector3(0, 0, 0.8f), Vector3.Zero, Vector3.Zero, new Vector3(3, 3, 3), Color.FromArgb(175, 255, 0, 0));
                }
                await Delay(0);
            }
        }

        private async Task Logic()
        {
            while (true)
            {
                if (Utility.Instance.GetDistanceBetweenVector3s(_bulkBuyPos, _playerPos) < 5 && !Game.PlayerPed.IsInVehicle())
                {
                    _menuOpen = true;
                    _bulkMenu = true;
                }
                else if (Utility.Instance.GetDistanceBetweenVector3s(_singleBuyPos, _playerPos) < 5 && !Game.PlayerPed.IsInVehicle())
                {
                    _menuOpen = true;
                    _bulkMenu = false;
                }
                else
                {
                    _menuOpen = false;
                }

                if (_menuOpen && !_menuCreated)
                {
                    if (_bulkMenu)
                    {
                        _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(
                            InteractionMenu.Instance._interactionMenu, Convert.ToString(Type), "Buy bulk " + Convert.ToString(Type) + " from here.", new PointF(5, Screen.Height / 2));
                        var buyButton = new UIMenuItem("Buy Bulk Package ~r~(" + ItemInfo[Type].BuyBulkPrice + ")");
                        _menu.AddItem(buyButton);
                        _menu.OnItemSelect += (sender, selectedItem, index) =>
                        {
                            if (selectedItem == buyButton)
                            {
                                if (Police.Instance.CopCount >= 1)
                                {
                                    TriggerServerEvent("BuyItemByName", "Bundle of " + Convert.ToString(Type));
                                    if (_random.Next(0, 3) == 1)
                                    {
                                        TriggerEvent("911CallClientAnonymous",
                                            _buyingBulkMessages[_random.Next(0, _buyingBulkMessages.Count)]);
                                    }
                                }
                                else
                                {
                                    Utility.Instance.SendChatMessage("[Drugs]", "Not enough cops on.", 255, 0, 0);
                                }
                            }
                        };
                    }
                    else
                    {
                        _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(
                            InteractionMenu.Instance._interactionMenu, Convert.ToString(Type), "Buy bulk " + Convert.ToString(Type) + " from here.", new PointF(5, Screen.Height / 2));
                        var sellBulk = new UIMenuItem("Sell Bulk Package" + "~g~(" + ItemInfo[Type].SellBulkPrice + ")");
                        var buySingle = new UIMenuItem("Buy Single" + "~r~(" + ItemInfo[Type].BuySinglePrice + ")");
                        _menu.AddItem(sellBulk);
                        _menu.AddItem(buySingle);
                        _menu.OnItemSelect += (sender, selectedItem, index) =>
                        {
                            if (selectedItem == sellBulk)
                            {
                                Sell();
                                async Task Sell()
                                {
                                    Game.PlayerPed.IsPositionFrozen = true;
                                    await Delay(6000);
                                    Game.PlayerPed.IsPositionFrozen = false;
                                    TriggerServerEvent("SellItemByName", "Bundle of " + Convert.ToString(Type));
                                    if (_random.Next(0, 3) == 1)
                                    {
                                        TriggerEvent("911CallClientAnonymous", _sellingBulkMessages[_random.Next(0, _sellingBulkMessages.Count)]);
                                    }
                                }
                            }
                            if (selectedItem == buySingle)
                            {
                                TriggerServerEvent("BuyItemByName", Convert.ToString(Type));
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
