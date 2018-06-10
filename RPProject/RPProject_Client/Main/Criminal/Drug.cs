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
using roleplay.Users.Inventory;

namespace roleplay.Main.Criminal
{
    public enum DrugTypes { Cocaine, Meth, Weed, Acid, Lsd, Heroine, Crack, Xanax, Oxy}

    public class DrugInformation
    {
        public int BulkWeight;
        public int SingleWeight;
        public int BuyBulkPrice;
        public int SellBulkPrice;
        public int BuySinglePrice;
        public int SellSinglePrice;
        public DrugInformation(int bulkweight, int singleweight, int bulkbuy, int bulksell, int singlebuy, int singlesell)
        {
            BulkWeight = bulkweight;
            SingleWeight = singleweight;
            BuyBulkPrice = bulkbuy;
            SellBulkPrice = bulksell;
            BuySinglePrice = singlebuy;
            SellSinglePrice = singlesell;
        }
    }

    public class Drug : BaseScript
    {
        private Dictionary<DrugTypes, DrugInformation> ItemInfo = new Dictionary<DrugTypes, DrugInformation>()
        {
            [DrugTypes.Weed] = new DrugInformation(50, 3, 1000, 1800, 20, 100), 
            [DrugTypes.Meth] = new DrugInformation(60, 3, 2000, 2200, 25, 60),
            [DrugTypes.Cocaine] = new DrugInformation(75, 3, 3000, 3300, 100, 250),
            [DrugTypes.Heroine] = new DrugInformation(75, 3, 1000, 1300, 25, 145),
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

        public bool MenuRestricted = false;
        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private UIMenu _menu;
        private bool _bulkMenu = false;

        public Drug(Vector3 buyPos, Vector3 sellPos, DrugTypes type)
        {
            _bulkBuyPos = buyPos;
            _singleBuyPos = sellPos;

            Type = type;
            Instance = this;
            DrawMarkers();
            Logic();
        }

        private async Task DrawMarkers()
        {
            while (true)
            {
                if (Utility.Instance.GetDistanceBetweenVector3s(Game.PlayerPed.Position, _bulkBuyPos) < 30)
                {
                    World.DrawMarker(MarkerType.HorizontalCircleSkinny, _bulkBuyPos-new Vector3(0,0,0.8f), Vector3.Zero, Vector3.Zero, new Vector3(3,3,3), Color.FromArgb(175, 255, 0, 0));
                }
                if (Utility.Instance.GetDistanceBetweenVector3s(Game.PlayerPed.Position, _singleBuyPos) < 30)
                {
                    World.DrawMarker(MarkerType.HorizontalCircleSkinny, _singleBuyPos - new Vector3(0, 0, 1.1f), Vector3.Zero, Vector3.Zero, new Vector3(3, 3, 3), Color.FromArgb(175, 255, 0, 0));
                }
                await Delay(0);
            }
        }

        private async Task Logic()
        {
            while (true)
            {
                if (Utility.Instance.GetDistanceBetweenVector3s(_bulkBuyPos, Game.PlayerPed.Position) < 5)
                {
                    _menuOpen = true;
                    _bulkMenu = true;
                }
                else if (Utility.Instance.GetDistanceBetweenVector3s(_singleBuyPos, Game.PlayerPed.Position) < 5)
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
                        var buyButton = new UIMenuItem("Buy Bulk Package ~r~("+ItemInfo[Type].BuyBulkPrice+ ")");
                        _menu.AddItem(buyButton);
                        _menu.OnItemSelect += (sender, selectedItem, index) =>
                        {
                            if (selectedItem==buyButton)
                            {
                                TriggerServerEvent("BuyItemByName", "Bundle of "+ Convert.ToString(Type));
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
                                TriggerServerEvent("SellItemByName", "Bundle of " + Convert.ToString(Type));
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

                await Delay(0);
            }
        }
    }

    public class Weed : Drug
    {
        public Weed() : base(
            new Vector3(2218.37646f, 5612.836f, 54.69646f),
            new Vector3(-1173.23523f, -1573.29883f, 4.51461172f),
            DrugTypes.Weed)
        {

        }
    }
    

    public class Meth : Drug
    {
        public Meth() : base(
            new Vector3(1395.281f, 3608.274f, 38.9419022f),
            new Vector3(60.799774169922f, 3718.8859863281f, 39.746185302734f),
            DrugTypes.Meth)
        {

        }
    }

    public class Cocaine : Drug
    {
        public Cocaine() : base(
            new Vector3(-2032.3114f, -1038.63586f, 5.882404f),
            new Vector3(-1502.6539306641f, 137.2939453125f, 55.653125762939f),
            DrugTypes.Cocaine)
        {

        }
    }
    
    public class DrugSelling : BaseScript
    {
        public DrugSelling Instance;

        private bool _isSelling = false;
        private Random _random = new Random();

        public DrugSelling()
        {
            Instance = this;
            API.DecorRegister("HasBoughtDrugs", 2);
            EventHandlers["StartSellingDrugs"] += new Action(DrugToggle);
        }
        
            
        private async void DrugToggle()
        {
            if (_isSelling)
            {
                _isSelling = false;
                Game.PlayerPed.Task.ClearAll();
                await Delay(1000);
                Game.PlayerPed.Task.ClearAll();
                await Delay(1000);
                Game.PlayerPed.Task.ClearAll();
            }
            else
            {
                _isSelling = true;
                StartSellingDrugs();
            }
        }

        private async Task StartSellingDrugs()
        {
            DrugSellingAnim();
            while (_isSelling)
            {
                if (!API.IsPedUsingScenario(Game.PlayerPed.Handle, "WORLD_HUMAN_DRUG_DEALER_HARD") && !API.IsPedUsingScenario(Game.PlayerPed.Handle, "WORLD_HUMAN_DRUG_DEALER"))
                {
                    API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "WORLD_HUMAN_DRUG_DEALER_HARD", 0, true);
                }
                var ped = API.GetRandomPedAtCoord(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y,
                    Game.PlayerPed.Position.Z, 6.0f, 6.0f, 6.0f, 26);
                var hasBought = API.DecorGetBool(ped, "HasBoughtDrugs");
                if (API.DoesEntityExist(ped) && hasBought == false)
                {
                    var randomChance = _random.Next(3);
                    if (randomChance == 0)
                    {
                        foreach (var drug in Enum.GetValues(typeof(DrugTypes)).Cast<DrugTypes>())
                        {
                            if (InventoryUI.Instance.HasItem(Convert.ToString(drug)) > 0)
                            {
                                Game.PlayerPed.Task.PlayAnimation("mp_arresting", "a_uncuff");
                                await Delay(4000);
                                Game.PlayerPed.Task.ClearAll();
                                TriggerServerEvent("SellItemByName", Convert.ToString(drug));
                                API.DecorSetBool(ped, "HasBoughtDrugs", true);
                                break;
                            }
                        }
                    }
                    else
                    {
                        API.DecorSetBool(ped, "HasBoughtDrugs", true);
                        API.SetPedScream(ped);
                        if (randomChance == 2)
                        {
                            //Police Alert Code.
                        }
                    }
                }
                await Delay(1500);
            }
            Game.PlayerPed.Task.ClearAll();
            Game.PlayerPed.Task.ClearAll();
        }

        private void DrugSellingAnim()
        {
            var rdmAnim = _random.Next(2);
            if (rdmAnim == 0)
            {
                API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "WORLD_HUMAN_DRUG_DEALER", 0, true);
            }
            else
            {
                API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "WORLD_HUMAN_DRUG_DEALER_HARD", 0, true);
            }
        }
    }

}
