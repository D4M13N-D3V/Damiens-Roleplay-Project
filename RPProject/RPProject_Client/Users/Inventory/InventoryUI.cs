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

        private UIMenu menu;

        private Dictionary<int, int> quantitys = new Dictionary<int, int>();
        private Dictionary<int, UIMenuItem> menuItems = new Dictionary<int, UIMenuItem>();

        public InventoryUI()
        {
            EventHandlers["RefreshInventoryItems"] += new Action<List<dynamic>>(RefreshItems);
            Instance = this;
            SetupUI();
        }

        private async void SetupUI()
        {
            while (InteractionMenu.Instance == null)
            {
                await Delay(0);
            }
            menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenu(InteractionMenu.Instance._interactionMenu, "Inventory", "Access your inventory");
            InteractionMenu.Instance._menus.Add(menu);
        }

        private async void RefreshItems(List<dynamic> Items)
        {
            while (menu == null)
            {
                await Delay(0);
            }
            menu.Clear();
            Inventory.Clear();
            quantitys.Clear();
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            menu.Visible = true;
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

            foreach (var itemID in quantitys.Keys)
            {
                //Look in the list for a entryr matching the ID the nget the name from that row.
                var itemName = Inventory.Find(x => x.Id == itemID).Name;
                //Set the name of the sub menu title to the item name and the amount there is.
                var itemMenu = InteractionMenu.Instance._interactionMenuPool.AddSubMenu(menu, itemName + " [" + quantitys[itemID] + "]");
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

                    }
                    else if (item == itemDropButton)
                    {
                        Utility.Instance.KeyboardInput("How many items should be dropped", "", 2, new Action<string>((string result)=>
                            TriggerServerEvent("dropItem",itemID,Convert.ToInt16(result))
                        ));
                        itemMenu.Visible = false;
                        menu.Visible = true;
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
                            menu.Visible = true;
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

    }
}
 