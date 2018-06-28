using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client.Main.Vehicles;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;

namespace client.Main.Housing
{
    public class Manager : BaseScript
    {
        public static Manager Instance;

        public Dictionary<int,House> Houses = new Dictionary<int, House>();

        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private UIMenu _menu;
        public House CurrentHouse = null;

        public Vector3 _playerPos;

        public Manager()
        {
            Instance = this;
            MenuLogic();
            DrawMarkers();
            GetPlayerPosEverySecond();
            EventHandlers["Housing:LoadHouses"] += new Action<List<dynamic>>(LoadHouses);
        }

        private async Task GetPlayerPosEverySecond()
        {
            while (true)
            {
                _playerPos = Game.PlayerPed.Position;
                await Delay(1000);
            }
        }

        private void LoadHouses(List<dynamic> objects)
        {
            Houses.Clear();
            foreach(var objt in objects)
            {
                var str = Convert.ToString(objt);
                var strings = str.Split('|');
                var id = int.Parse(strings[0]);
                Debug.WriteLine(strings[0]);
                var name = Convert.ToString(strings[1]);
                var desc = Convert.ToString(strings[2]);
                var pos = new Vector3(float.Parse(strings[3]), float.Parse(strings[4]), float.Parse(strings[5]));
                var forsale = bool.Parse(strings[6].ToUpper());
                var owner = strings[7];
                var price = int.Parse(strings[8]);
                Houses.Add(id, new House(id, name, desc, pos, forsale, owner, price));
            }
        }

        private async Task DrawMarkers()
        {
            while (true)
            {
                foreach (var house in Houses)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(house.Value.Position, _playerPos) < 30)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, house.Value.Position - new Vector3(0, 0, 0.95f), Vector3.Zero, Vector3.Zero, new Vector3(3, 3, 3), Color.FromArgb(255, 0, 150, 150));
                    }
                }
                await Delay(0);
            }
        }


        private async Task MenuLogic()
        {
            while (true)
            {

                _menuOpen = false;
                foreach (var house in Houses)
                {
                    var dist = API.Vdist(_playerPos.X, _playerPos.Y, _playerPos.Z, house.Value.Position.X, house.Value.Position.Y, house.Value.Position.Z);
                    if (dist < 6f)
                    {
                        _menuOpen = true;
                        if (CurrentHouse != null && CurrentHouse != house.Value)
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
                        CurrentHouse = house.Value;
                    }
                }
                if (_menuOpen && !_menuCreated)
                {
                    _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(
                        InteractionMenu.Instance._interactionMenu, "House ~p~(#"+CurrentHouse.Id+")~w~[~o~"+CurrentHouse.Name+"~w~]", "Housing Menu", new PointF(5, Screen.Height / 2));
                    var propertyNameLabel = new UIMenuItem("~p~("+CurrentHouse.Id+")" + CurrentHouse.Name);
                    var ownerLabel = new UIMenuItem("~o~"+CurrentHouse.Owner);
                    var priceLabel = new UIMenuItem("~y~Value/Price : $" + CurrentHouse.Price);
                    UIMenuItem forSaleLabel = null;
                    if (CurrentHouse.ForSale == true)
                    {
                        forSaleLabel = new UIMenuItem("~g~House Is For Sale!");
                    }
                    else
                    { 
                        forSaleLabel = new UIMenuItem("~r~House Is Not For Sale!");
                    }

                    var buyButton = new UIMenuItem("~b~Buy House!");
                    var setPriceButton = new UIMenuItem("~b~Set the price of the property!");
                    var sellButton = new UIMenuItem("~b~Put House Up For Sale!");
                    var removeSellButton = new UIMenuItem("~b~Remove House From Sales!");

                    _menu.AddItem(propertyNameLabel);
                    _menu.AddItem(ownerLabel);
                    _menu.AddItem(priceLabel);
                    _menu.AddItem(forSaleLabel);
                    _menu.AddItem(setPriceButton);
                    _menu.AddItem(sellButton);
                    _menu.AddItem(removeSellButton);
                    _menu.AddItem(buyButton);


                    _menu.OnItemSelect += (sender, item, index) =>
                    {
                        if (item == setPriceButton)
                        {
                            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                            Utility.Instance.KeyboardInput("The amount you want to set the price to.", "", 10,
                                s =>
                                {
                                    if (Int32.TryParse(s, out var result))
                                    {
                                        TriggerServerEvent("Housing:SetPrice", CurrentHouse.Id, result);
                                    }
                                    else
                                    {
                                        Utility.Instance.SendChatMessage("[Housing]","Invalid amount set.",255,255,0);
                                    }
                                });
                        }
                        else if (item == sellButton)
                        {
                            TriggerServerEvent("Housing:Sell", CurrentHouse.Id);
                        }
                        else if (item == removeSellButton)
                        {
                            TriggerServerEvent("Housing:StopSell", CurrentHouse.Id);
                        }
                        else if (item == buyButton)
                        {
                            TriggerServerEvent("Housing:Buy", CurrentHouse.Id);
                        }
                    };

                    _menuCreated = true;
                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                }
                else if (!_menuOpen && _menuCreated)
                {
                    _menuCreated = false;
                    CurrentHouse = null;
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
