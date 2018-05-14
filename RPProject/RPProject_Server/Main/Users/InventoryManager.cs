using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using CitizenFX.Core;

namespace roleplay.Main.Users
{
    public class InventoryManager:BaseScript
    {
        public static InventoryManager Instance;
        public InventoryManager()
        {
            Instance = this;
            EventHandlers["dropItem"] += new Action<Player, int, int>(RemoveItem);
            EventHandlers["giveItem"] += new Action<Player, int, int,int>(GiveItem);
        }

        public void AddItem([FromSource] Player player, int itemId, int quantity)
        {
            var tmpItem = ItemManager.Instance.LoadedItems[itemId];
            for (int i = 0; i < quantity; i++)
            {
                if (CheckWeightAddition(player, tmpItem.Weight))
                {
                    UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory.Add(tmpItem);
                }
            }
            Utility.Instance.SendChatMessage(player,"INVENTORY", " You have picked up " + ItemManager.Instance.LoadedItems[itemId].Name + "[" + quantity + "]", 0,255,0);
            RefreshWeight(player);
            RefreshItems(player);
        }

        public void AddItem(int itemId, int quantity, Player player)
        {
            var tmpItem = ItemManager.Instance.LoadedItems[itemId];
            for (int i = 0; i < quantity; i++)
            {
                if (CheckWeightAddition(player, tmpItem.Weight))
                {
                    UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory.Add(tmpItem);
                }
            }
            Utility.Instance.SendChatMessage(player, "INVENTORY", " You have picked up " + ItemManager.Instance.LoadedItems[itemId].Name + "[" + quantity + "]", 0, 255, 0);
            RefreshWeight(player);
            RefreshItems(player);
        }

        public bool AddItem(int itemId, int quantity, Player player, bool given)
        {
            var tmpItem = ItemManager.Instance.LoadedItems[itemId];
            for (int i = 0; i < quantity; i++)
            {
                if (CheckWeightAddition(player, tmpItem.Weight))
                {
                    UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory.Add(tmpItem);

                    if (given == false)
                    {
                        Utility.Instance.SendChatMessage(player, "INVENTORY", " You have pickedup " + ItemManager.Instance.LoadedItems[itemId].Name + "[" + quantity + "]", 0, 255, 0);
                    }
                    RefreshWeight(player);
                    RefreshItems(player);
                    return true;
                }
            }
            return false;
        }


        public void RemoveItem([FromSource]Player player, int itemId, int quantity)
        {
            var inv = UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory;
            for (int i = 0; i < quantity; i++)
            {
                foreach (Item item in inv)
                {
                    if (item.Id==itemId)
                    {
                        inv.Remove(item);
                    }
                   break;
                }
            }
            UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory = inv;
            RefreshWeight(player);
            RefreshItems(player);
            Utility.Instance.SendChatMessage(player, "INVENTORY", " You have dropped " + ItemManager.Instance.LoadedItems[itemId].Name + "[" + quantity + "]", 0, 255, 0);
        }

        public void RemoveItem( int itemId, int quantity, Player player)
        {
            var inv = UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory;
            for (int i = 0; i < quantity; i++)
            {
                foreach (Item item in inv)
                {
                    Debug.Write("AWETWAETAEWTAWETWAETAWETAWETAWETAWETEWATWEAQTEAWSTAWETwa4");
                    if (item.Id == itemId)
                    {
                        inv.Remove(item);
                    }
                    break;
                }
            }
            UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory = inv;
            Debug.WriteLine(Convert.ToString(UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory.Count));
            RefreshWeight(player);  
            RefreshItems(player);

            Utility.Instance.SendChatMessage(player, "INVENTORY", " You have dropped " + ItemManager.Instance.LoadedItems[itemId].Name + "[" + quantity + "]", 0, 255, 0);
        }
        
        public void GiveItem([FromSource] Player player, int recieve, int itemID, int quantity)
        {
            var plyList = new PlayerList();
            var recievingPlayer = plyList[recieve];
            if (player != null && recievingPlayer != null)
            {
                var user = UserManager.Instance.GetUserFromPlayer(player);
                var matchingItems = user.CurrentCharacter.Inventory.Select(x => x.Id==itemID );
                if (matchingItems.Count() >= quantity)
                {
                    RemoveItem(itemID, quantity, player);
                    AddItem(itemID, quantity, recievingPlayer,true);
                    Utility.Instance.SendChatMessage(player, "INVENTORY", " You have given " + ItemManager.Instance.LoadedItems[itemID].Name + "[" + quantity + "] to " + recievingPlayer.Name + ".", 0, 255, 0);
                    Utility.Instance.SendChatMessage(recievingPlayer, "INVENTORY", " You have been given " + ItemManager.Instance.LoadedItems[itemID].Name + "[" + quantity + "] by " + player.Name + ".", 0, 255, 0);
                }
            }
        }
        
        public void RefreshWeight(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var character = user.CurrentCharacter;
            var Items = user.CurrentCharacter.Inventory;
            var totalWeight = 0;
            foreach (Item item in Items)
            {
                totalWeight = +item.Weight;
            }
            character.CurrentInventory = totalWeight;
        }

        public bool CheckWeightAddition(Player player, int amount)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var character = user.CurrentCharacter;
            if (character.CurrentInventory + amount < character.MaximumInventory)
            {
                return true;
            }
            return false;
        }

        public void RefreshItems(Player player)
        {
            if (player != null)
            {
                var user = UserManager.Instance.GetUserFromPlayer(player);
                if (user != null)
                {
                    var inv = new List<ExpandoObject>();

                    foreach (Item item in user.CurrentCharacter.Inventory)
                    {
                        dynamic obj = new ExpandoObject();
                        obj.Id = item.Id;
                        obj.Name = item.Name;
                        obj.Description = item.Description;
                        obj.BuyPrice = item.BuyPrice;
                        obj.SellPrice = item.SellPrice;
                        obj.Weight = item.Weight;
                        obj.Illegal = false;
                        inv.Add(obj);
                    }

                    TriggerClientEvent(user.Source, "RefreshInventoryItems", inv);
                }
            }
        }

        public void RefreshItems(User user)
        {
            if (user != null)
            {
                var inv = new List<ExpandoObject>();

                foreach (Item item in user.CurrentCharacter.Inventory)
                {
                    dynamic obj = new ExpandoObject();
                    obj.Id = item.Id;
                    obj.Name = item.Name;
                    obj.Description = item.Description;
                    obj.BuyPrice = item.BuyPrice;
                    obj.SellPrice = item.SellPrice;
                    obj.Weight = item.Weight;
                    obj.Illegal = false;
                    inv.Add(obj);
                }

                TriggerClientEvent(user.Source, "RefreshInventoryItems", inv, user.CurrentCharacter.Money.Cash,user.CurrentCharacter.Money.Bank,user.CurrentCharacter.Money.UnTaxed,user.CurrentCharacter.MaximumInventory,user.CurrentCharacter.CurrentInventory);
            }
        }
    }
}
