using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX;
using roleplay.Main;
using CitizenFX.Core.Native;
using NativeUI;

namespace roleplay.Users.Inventory
{
    public class Item
    {
        public int Id = 1;
        public string Name = "Test Item";
        public string Description = "This is a item for testing. LUL. NOOB.";
        public int BuyPrice = 100;
        public int SellPrice = 100;
        public int Weight = 10;
        public bool Illegal = false;
    }

    public class InventoryUI : BaseScript
    {
        public static InventoryUI Instance;
        public List<Item> Inventory = new List<Item>();

        private UIMenu _menu;

        private Dictionary<int, int> quantitys = new Dictionary<int, int>();
        private Dictionary<int, UIMenuItem> _menuItems = new Dictionary<int, UIMenuItem>();

        private UIMenuItem _weight = null;
        private UIMenuItem _cashItem = null;
        private UIMenuItem _bankItem = null;
        private UIMenuItem _untaxedItem = null;

        public InventoryUI()
        {
            EventHandlers["RefreshInventoryItems"] += new Action<List<dynamic>,int,int,int,int,int>(RefreshItems);
            EventHandlers["RefreshMoney"] += new Action<int,int,int>(RefreshMoney);
            Instance = this;
            SetupUI();
        }

        private async void SetupUI()
        {
            while (InteractionMenu.Instance == null)
            {
                await Delay(0);
            }
            _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenu(InteractionMenu.Instance._interactionMenu, "Inventory", "Access your inventory");
            InteractionMenu.Instance._menus.Add(_menu);
        }

        private async void RefreshItems(List<dynamic> Items, int cash, int bank, int untaxed, int maxinv, int curinv)
        {
            while (_menu == null)
            {
                await Delay(0);
            }
            _menu.Clear();
            Inventory.Clear();
            quantitys.Clear();
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            _menu.Visible = true;
            //Cast the inventory items as items that are passed as dynamics.
            Inventory = Items.Select( x => new Item { Name = x.Name, Description = x.Description, BuyPrice = x.BuyPrice,
                SellPrice = x.SellPrice, Weight = x.Weight, Illegal = x.Illegal, Id = x.Id}).ToList();
            foreach (Item item in Inventory)
            {
                if (quantitys.ContainsKey(item.Id))
                {
                    quantitys[item.Id] = quantitys[item.Id] + 1;
                }
                else
                {
                    quantitys.Add(item.Id,1);
                }
            }

            _weight = new UIMenuItem("~o~"+curinv+"kg/"+maxinv+ "kg", "Current inventory weight and maximum weight.");
            _cashItem = new UIMenuItem("~g~$" + cash, "How much legal cash you have on your character.");
            _bankItem = new UIMenuItem("~b~$" + bank, "How much money you have in your bank account.");
            _untaxedItem = new UIMenuItem("~r~$" + untaxed,"How much illegal cash you have on your character.");

            _menu.AddItem(_weight);
            _menu.AddItem(_cashItem);
            _menu.AddItem(_bankItem);
            _menu.AddItem(_untaxedItem);

            foreach (var itemID in quantitys.Keys)
            {
                //Look in the list for a entryr matching the ID the nget the name from that row.
                var itemName = Inventory.Find(x => x.Id == itemID).Name;
                var itemDesc = Inventory.Find(x => x.Id == itemID).Description;
                //Set the name of the sub menu title to the item name and the amount there is.
                var itemMenu = InteractionMenu.Instance._interactionMenuPool.AddSubMenu(_menu, itemName + " [" + quantitys[itemID] + "]",itemDesc);
                var itemUseButton = new UIMenuItem("Use Item");
                var itemDropButton = new UIMenuItem("Drop Item");
                var itemGiveButton = new UIMenuItem("Give Item");
                itemMenu.AddItem(itemUseButton);
                itemMenu.AddItem(itemDropButton);
                itemMenu.AddItem(itemGiveButton);
                itemMenu.OnItemSelect += (sender, item, index) =>
                {
                    if (item == itemUseButton)
                    {
                        InventoryProcessing.Instance.Process(item, sender);
                    }
                    else if (item == itemDropButton)
                    {
                        Utility.Instance.KeyboardInput("How many items should be dropped", "", 2, new Action<string>((string result)=>
                            TriggerServerEvent("dropItem",itemID,Convert.ToInt16(result))
                        ));
                        itemMenu.Visible = false;
                        _menu.Visible = true;
                        InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                    }
                    else if (item == itemGiveButton)
                    {
                        ClosestPlayerReturnInfo output;
                        Utility.Instance.GetClosestPlayer(out output);
                        if (output.Dist < 5)
                        {
                            var pid = API.GetPlayerServerId(output.Pid);
                            Utility.Instance.KeyboardInput("How many items should be given", "", 2, new Action<string>((string result) =>
                                TriggerServerEvent("giveItem", pid ,itemID, Convert.ToInt16(result))
                            ));
                            itemMenu.Visible = false;
                            _menu.Visible = true;
                            InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                        }
                        else
                        {
                            TriggerEvent("chatMessage", "INVENTORY", new[] { 0, 255, 0 }, "No player is close enough to give anything to them!");
                        }
                    }
                };
            }

        }

        private void RefreshMoney(int cash, int bank, int untaxed)
        {
            _cashItem.Text = "~g~$" + cash;
            _bankItem.Text = "~b~$" + bank;
            _untaxedItem.Text = "~r~$" + untaxed;
        }

    }
}
 