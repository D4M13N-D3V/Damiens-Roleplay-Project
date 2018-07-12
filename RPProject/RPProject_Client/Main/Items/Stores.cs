using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;

namespace client.Main.Items
{
    public class BaseStore : BaseScript
    {
        public Dictionary<string, int> Items;
        public string StoreName;
        public string StoreDesc;
        public List<Vector3> Posistions;
        public bool MenuRestricted = false;
        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private UIMenu _menu;

        public BaseStore(string storeName, string storeDesc, int blip, int color, List<Vector3> pos, Dictionary<string, int> items)
        {
            StoreName = storeName;
            StoreDesc = storeDesc;
            Posistions = pos;
            Items = items;
            SetupBlips(blip, color);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            StoreCheck();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            DrawMarkers();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }

        public void SetRestricted(bool restricted)
        {
            MenuRestricted = restricted;
        }

        private async Task DrawMarkers()
        {
            while (true)
            {
                foreach (var pos in Posistions)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(pos, Game.PlayerPed.Position) < 30)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, pos - new Vector3(0, 0, 0.9f), Vector3.Zero, Vector3.Zero, Vector3.One, Color.FromArgb(255, 255, 255, 0));
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
                API.AddTextComponentString(StoreName);
                API.EndTextCommandSetBlipName(blip);
            }
        }

        private async Task StoreCheck()
        {
            while (true)
            {

                _menuOpen = false;
                foreach (var pos in Posistions)
                {
                    var dist = API.Vdist(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, pos.X, pos.Y, pos.Z);
                    if (dist < 2.5f && !MenuRestricted)
                    {
                        _menuOpen = true;
                    }
                }

                if (_menuOpen && !_menuCreated)
                {
                    _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(
                        InteractionMenu.Instance._interactionMenu, StoreName, StoreDesc, new PointF(5, Screen.Height / 2));
                    var buttons = new List<UIMenuItem>();
                    foreach (var item in Items)
                    {
                        var button = new UIMenuItem(item.Key, "Costs ~g~$" + item.Value);
                        buttons.Add(button);
                        _menu.AddItem(button);
                        _menu.OnItemSelect += (sender, selectedItem, index) =>
                        {
                            if (selectedItem == button)
                            {
                                TriggerServerEvent("BuyItemByName", selectedItem.Text);
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

    public class TwentyFourSevenDrinks : BaseStore
    {
        public TwentyFourSevenDrinks() : base("Drinks", "A store where you can buy drinks.", 52, 2,
            new List<Vector3>()
            {new Vector3(-2970.62109375f,387.94940185546f,15.043312072754f),
                new Vector3(-1486.8306884766f,-382.66729736328f,40.163436889648f),
                new Vector3(1137.353149414f,-979.49407958984f,46.415840148926f),
                new Vector3(1705.3275146484f,4932.0649414062f,42.06367111206f),
                new Vector3(1153.6125488282f,-323.9934387207f,69.205078125f),
                new Vector3(-1829.1737060546f,787.23034667968f,138.31108093262f),
                new Vector3(-1226.824584961f,-906.49768066406f,12.326355934144f),
                new Vector3(-717.10540771484f,-913.037109375f,19.215593338012f),
                new Vector3(541.08074951172f,2669.365234375f,42.156497955322f),
                new Vector3(1967.2169189454f,3745.4768066406f,32.34375f),
                new Vector3(2556.5556640625f,389.28247070312f,108.62296295166f),
                new Vector3(380.54251098632f,324.8773803711f,103.56638336182f),
                new Vector3(2681.6921386718f,3287.5456542968f,55.241134643554f),
                new Vector3(-3042.1843261718f,592.36938476562f,7.9089331626892f),
                new Vector3(-3242.1535644532f,1008.5739746094f,12.830711364746f),
                new Vector3(1169.1120605468f,2707.2734375f,38.15769958496f),
            },
            new Dictionary<string, int>()
            {
                ["Monster"] = 4,
                ["Redbull"] = 4,
                ["Mtn-Dew-Kickstart"] = 4,
                ["Mtn-Dew"] = 3,
                ["Lemonade"] = 3,
                ["Pink-Lemonade"] = 3,
                ["Coke"] = 2,
                ["Pepsi"] = 2,
                ["Sprite"] = 2,
                ["Juice"] = 2,
                ["Water"] = 1,
            })
        { }
    }

    public class TwentyFourSevenFood : BaseStore
    {
        public TwentyFourSevenFood() : base("Food", "A store where you can buy different food.", 52, 2,
            new List<Vector3>()
            {
                new Vector3(-2970.4289550782f,393.26190185546f,15.043310165406f),
                new Vector3(-1490.1849365234f,-379.78414916992f,40.163429260254f),
                new Vector3(1138.089477539f,-983.98107910156f,46.415832519532f),
                new Vector3(1702.0889892578f,4926.5063476562f,42.06364440918f),
                new Vector3(1159.3787841796f,-322.24493408204f,69.205078125f),
                new Vector3(-1824.9599609375f,791.83850097656f,138.19343566894f),
                new Vector3(-1222.4638671875f,-903.99389648438f,12.326355934144f),
                new Vector3(-711.34771728516f,-912.5220336914f,19.215591430664f),
                new Vector3(544.39959716796f,2668.8596191406f,42.156497955322f),
                new Vector3(1963.7283935546f,3743.6689453125f,32.34375f),
                new Vector3(2556.0268554688f,386.1379699707f,108.62296295166f),
                new Vector3(377.54934692382f,326.8624572754f,103.56639099122f),
                new Vector3(1733.0810546875f,6414.9223632812f,35.037227630616f),
                new Vector3(-3042.2905273438f,588.9028930664f,7.9089298248292f),
                new Vector3(-3243.708984375f,1005.2696533204f,12.830708503724f),
                new Vector3(1164.0760498046f,2706.7312011718f,38.157707214356f),

            },
            new Dictionary<string, int>()
            {
                ["PorkRinds"] = 4,
                ["Cheetos"] = 4,
                ["Doritos"] = 4,
                ["Pistachios"] = 3,
                ["Doughnut"] = 2,
                ["GummyBears"] = 2,
                ["IceCream"] = 1,
                ["Chocolate-Bar"] = 1,
            })
        { }
    }

    public class TwentyFourSevenCounter : BaseStore
    {
        public TwentyFourSevenCounter() : base("Counter Goods", "Buy tobacco, and other misc items here.", 52, 2,
            new List<Vector3>()
            {
                new Vector3(-1487.3585205078f,-379.77444458008f,40.163425445556f),
                new Vector3(1136.9680175782f,-982.08288574218f,46.41584777832f),
                new Vector3(1698.7567138672f,4925.0981445312f,42.063636779786f),
                new Vector3(1162.527709961f,-323.70434570312f,69.205078125f),
                new Vector3(-1223.9455566406f,-907.09210205078f,12.326354026794f),
                new Vector3(-708.53979492188f,-914.150390625f,19.215593338012f),
                new Vector3(546.92303466796f,2670.5454101562f,42.156494140625f),
                new Vector3(547.2582397461f,2670.4125976562f,42.156494140625f),
                new Vector3(1961.8131103516f,3741.4494628906f,32.34375f),
                new Vector3(2556.6828613282f,383.28713989258f,108.62295532226f),
                new Vector3(374.73919677734f,326.32650756836f,103.56639099122f),
                new Vector3(2678.7478027344f,3280.7219238282f,55.241134643554f),
                new Vector3(1730.1691894532f,6414.2041015625f,35.037231445312f),
                new Vector3(-3039.7141113282f,586.73748779296f,7.9089288711548f),
                new Vector3(-3242.1076660156f,1002.3307495118f,12.830706596374f),
                new Vector3(1166.046508789f,2708.7849121094f,38.157711029052f),

            },
            new Dictionary<string, int>()
            {
                ["Bobby-Pins"] = 250,
                ["Cigarette"] = 10
            })
        { }
    }

    public class Ammunation : BaseStore
    {
        public Ammunation() : base("Ammunation", "Buy weapons and other hunting gear.", 110, 2,
            new List<Vector3>()
            {
                new Vector3(1693.1379394532f,3758.9697265625f,34.705326080322f),
                new Vector3(251.58723449708f,-48.668949127198f,69.941055297852f),
                new Vector3(843.29119873046f,-1032.7292480468f,28.194858551026f),
                new Vector3(-330.34454345704f,6082.5659179688f,31.45477104187f),
                new Vector3(-663.0482788086f,-936.46765136718f,21.829235076904f),
                new Vector3(-1306.9487304688f,-392.7131652832f,36.69577407837f),
                new Vector3(-1117.7247314454f,2696.8181152344f,18.554134368896f),
                new Vector3(-3171.3623046875f,1086.3358154296f,20.838745117188f),
                new Vector3(2568.7775878906f,294.89547729492f,108.73487091064f),
                new Vector3(811.552734375f,-2156.404296875f,29.619018554688f),
                new Vector3(20.985027313232f,-1106.9747314453f,29.797027587891f),
                new Vector3(138.76893615722f,6581.67578125f,31.682182312012f)

            },
            new Dictionary<string, int>()
            {
                ["Binoculars"] = 500,
                ["SNS Pistol"] = 5000,
                ["Pistol .50"] = 8000,
                ["Pistol"] = 6000,
                ["Combat Pistol"] = 7000,
                ["Heavy Pistol"] = 7500,
                ["Double Action Revolver"] = 12000,
                ["Single Action Revolver"] = 15000,
                ["Hunting Rifle"] = 10000,
                ["Pump Shotgun"] = 30000,
                ["Shotgun Ammo"] = 10,
                ["Pistol Ammo"] = 10
            })
        { }
    }


    public class HardwareStore : BaseStore
    {
        public HardwareStore() : base("Hardware Store", "Buy tools and other items.", 478, 2,
            new List<Vector3>()
            {
                new Vector3(2749.0146484375f,3472.119140625f,55.67907333374f),
            },
            new Dictionary<string, int>()
            {
                ["Scuba Gear"] = 15000,
                ["Zipties"] = 1000,
                ["Lockpick"] = 200,
                ["Knife"] = 25,
                ["Hammer"] = 25,
                ["Fireaxe"] = 60,
                ["Crowbar"] = 50,
                ["Bottle"] = 5,
                ["Dagger"] = 40,
                ["Hatchet"] = 60,
                ["Machete"] = 60,
                ["Pool Cue"] = 60,
                ["Wrench"] = 30,
                ["Switchblade"] = 25,
                ["Brass Knuckles"] = 40,
            })
        { }
    }
}
