using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;

namespace roleplay.Main
{
    public class BaseStore : BaseScript
    {
        public Dictionary<string, int> Items;
        public string StoreName;
        public string StoreDesc;
        public List<Vector3> Posistions;

        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private UIMenu _menu;

        public BaseStore(string storeName, string storeDesc, int blip, int color, List<Vector3> pos, Dictionary<string, int> items)
        {
            StoreName = storeName;
            StoreDesc = storeDesc;
            Posistions = pos;
            Items = items;
            SetupBlips(blip,color);
            StoreCheck();
        }

        private void SetupBlips(int sprite, int color)
        {
            foreach (var var in Posistions)
            {
                var blip = API.AddBlipForCoord(var.X, var.Y, var.Z);
                API.SetBlipSprite(blip, sprite);
                API.SetBlipColour(blip, color);
                API.SetBlipScale(blip,0.6f);
                API.SetBlipAsShortRange(blip, true);
                API.BeginTextCommandSetBlipName("STRING");
                API.AddTextComponentString(StoreName);
                API.EndTextCommandSetBlipName(blip);
            }
        }

        private async void StoreCheck()
        {
            while (true)
            {
                _menuOpen = false;
                var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
                foreach (var pos in Posistions)
                {
                    var dist = API.Vdist(playerPos.X, playerPos.Y, playerPos.Z, pos.X, pos.Y, pos.Z);
                    if (dist < 2.5f)
                    {
                        _menuOpen = true;
                    }
                }

                if (_menuOpen && !_menuCreated)
                {
                    _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenu(
                        InteractionMenu.Instance._interactionMenu, StoreName, StoreDesc);
                    var buttons = new List<UIMenuItem>();
                    foreach (var item in Items)
                    {
                        var button = new UIMenuItem(item.Key,"Costs ~g~$"+item.Value);
                        buttons.Add(button);
                        _menu.AddItem(button);
                        _menu.OnItemSelect += (sender, selectedItem, index) =>
                        {
                            if (selectedItem == button)
                            {
                                TriggerServerEvent("BuyItemByName",selectedItem.Text);
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
                        if (item==_menu.ParentItem)
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

    public class TwentyFourSevenDrinks : BaseStore
    {
        public TwentyFourSevenDrinks() : base("Drinks", "A store where you can buy drinks.", 52, 2,
            new List<Vector3>()
            {
                new Vector3(33.294624328613f,-1346.4815673828f,29.497024536133f),
                new Vector3(-53.839450836182f,-1748.990234375f,29.421016693115f)
            },
            new Dictionary<string,int>()
            {
                ["Monster"]=4,
                ["Redbull"]=4,
                ["Mtn-Dew-Kickstart"]=4,
                ["Mtn-Dew"]=3,
                ["Lemonade"]=3,
                ["Pink-Lemonade"]=3,
                ["Coke"]=2,
                ["Pepsi"]=2,
                ["Sprite"]=2,
                ["Juice"]=2,
                ["Water"]=1,
            })
        { }
    }

    public class TwentyFourSevenFood : BaseStore
    {
        public TwentyFourSevenFood() : base("Food", "A store where you can buy different food.", 52, 2,
            new List<Vector3>()
            {
                new Vector3(29.63279914856f,-1345.2244873047f,29.49702835083f),
                new Vector3(-50.337833404541f,-1753.3913574219f,29.421003341675f),
            },
            new Dictionary<string,int>()
            {
                ["PorkRinds"]=4,
                ["Cheetos"]=4,
                ["Doritos"]=4,
                ["Pistachios"]=3,
                ["Doughnut"]=2,
                ["GummyBears"]=2,
                ["IceCream"]=1,
                ["Chocolate-Bar"]=1,
            })
        { }
    }

    public class TwentyFourSevenCounter : BaseStore
    {
        public TwentyFourSevenCounter() : base("Counter Goods", "Buy tobacco, and other misc items here.", 52, 2,
            new List<Vector3>()
            {
                new Vector3(25.71160697937f,-1346.6744384766f,29.49702835083f),
                new Vector3(-48.110900878906f,-1757.0367431641f,29.421016693115f),
            },
            new Dictionary<string, int>()
            {
                ["Binoculars"] = 500,
                ["Bobby-Pins"] = 250,
                ["Cigarette"] =10
            })
        { }
    }

}
