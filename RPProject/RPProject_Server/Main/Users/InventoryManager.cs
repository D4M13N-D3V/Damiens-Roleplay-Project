using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using CitizenFX.Core;
using Newtonsoft.Json;

namespace roleplay.Main.Users
{
    public class InventoryManager:BaseScript
    {
        public static InventoryManager Instance;
        public InventoryManager()
        {
            Instance = this;
            EventHandlers["dropItem"] += new Action<Player, string, int, bool>(RemoveItem);
            EventHandlers["giveItem"] += new Action<Player, int, string,int>(GiveItem);
            EventHandlers["BuyItemByName"] += new Action<Player, string>(BuyItemByName);
            EventHandlers["SellItemByName"] += new Action<Player, string>(SellItemByName);
        }
        
        public void AddItem([FromSource] Player player, string itemName, int quantity)
        {
            var tmpItem = ItemManager.Instance.GetItemByName(itemName);
            for (int i = 0; i < quantity; i++)
            {
                if (CheckWeightAddition(player, tmpItem.Weight))
                {
                    if (tmpItem.Description == "Vehicle Keys")
                    {
                        Debug.Write("TESTESTESTSETSETSET");
                        var splitName = tmpItem.Name.Split('|');
                        var veh = VehicleManager.Instance.LoadedVehicles[splitName[1]];
                        veh.RegisteredOwner =
                            UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.FirstName + " " +
                            UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.LastName;
                        VehicleManager.Instance.LoadedVehicles[splitName[1]] = veh;
                        DatabaseManager.Instance.ExecuteAsync("UPDATE VEHICLES SET vehicle='" + JsonConvert.SerializeObject(veh) + "' WHERE id=" + veh.id + ";");
                    }
                    UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory.Add(tmpItem);
                }
            }
            Utility.Instance.SendChatMessage(player,"[Inventory]", " You have picked up " + itemName + "[" + quantity + "]", 0,255,0);
            RefreshWeight(player);
            RefreshItems(player);
        }

        public void ConfiscateItems(Player player)
        {
            var tgtUser = UserManager.Instance.GetUserFromPlayer(player);
            var inventory = tgtUser.CurrentCharacter.Inventory;
            foreach (var itme in tgtUser.CurrentCharacter.Inventory)
            {
                if (itme.Illegal)
                {
                    inventory.Remove(itme);
                }
            }
            tgtUser.CurrentCharacter.Inventory = inventory;
            RefreshWeight(player);
            RefreshItems(player);
        }

        public void ConfiscateWeapons(Player player)
        {
            var tgtUser = UserManager.Instance.GetUserFromPlayer(player);
            var inventory = tgtUser.CurrentCharacter.Inventory;
            foreach (var itme in tgtUser.CurrentCharacter.Inventory)
            {
                if (itme.Description=="A firearm.")
                {
                    inventory.Remove(itme);
                }
            }
            tgtUser.CurrentCharacter.Inventory = inventory;
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
                    if (tmpItem.Description == "Vehicle Keys")
                    {
                        Debug.Write("TESTESTESTSETSETSET");
                        var splitName = tmpItem.Name.Split('-');
                        var veh = VehicleManager.Instance.LoadedVehicles[splitName[1]];
                        veh.RegisteredOwner =
                            UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.FirstName + " " +
                            UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.LastName;
                        VehicleManager.Instance.LoadedVehicles[splitName[1]] = veh;
                        DatabaseManager.Instance.ExecuteAsync("UPDATE VEHICLES SET vehicle='" + JsonConvert.SerializeObject(veh) + "' WHERE id=" + veh.id + ";");
                    }
                    UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory.Add(tmpItem);
                }
            }
            Utility.Instance.SendChatMessage(player, "[Inventory]", " You have picked up " + ItemManager.Instance.LoadedItems[itemId].Name + "[" + quantity + "]", 0, 255, 0);
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
                    if (tmpItem.Description == "Vehicle Keys")
                    {
                        Debug.Write("TESTESTESTSETSETSET");
                        var splitName = tmpItem.Name.Split('-');
                        var veh = VehicleManager.Instance.LoadedVehicles[splitName[1]];
                        veh.RegisteredOwner =
                            UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.FirstName + " " +
                            UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.LastName;
                        VehicleManager.Instance.LoadedVehicles[splitName[1]] = veh;
                        DatabaseManager.Instance.ExecuteAsync("UPDATE VEHICLES SET vehicle='" + JsonConvert.SerializeObject(veh) + "' WHERE id=" + veh.id + ";");
                    }
                    UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory.Add(tmpItem);

                    if (given == false)
                    {
                        Utility.Instance.SendChatMessage(player, "[Inventory]", " You have pickedup " + ItemManager.Instance.LoadedItems[itemId].Name + "[" + quantity + "]", 0, 255, 0);
                    }
                    RefreshWeight(player);
                    RefreshItems(player);
                    return true;
                }
            }
            return false;
        }


        public void RemoveItem([FromSource]Player player, string itemName, int quantity, bool putOnGround = false)
        {
            var inv = UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory.ToList();
            for (int i = 0; i < quantity; i++)
            {
                foreach (Item item in inv)
                {
                    if (item.Name == itemName)
                    {
                        inv.Remove(item);
                        break;
                    }
                }
            }
            UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory = inv;
            RefreshWeight(player);
            RefreshItems(player);
            RPCommands.Instance.ActionCommand("Has dropped " + itemName + "[" + quantity + "]",player);
        }

        public void RemoveItem( string itemName, int quantity, Player player, bool putOnGround = false)
        {
            var inv = UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory.ToList();
            for (int i = 0; i < quantity; i++)
            {
                foreach (Item item in inv)
                {
                    if (item.Name == itemName)
                    {
                        inv.Remove(item);
                        break;
                    }
                }
            }
            UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory = inv;
            RefreshWeight(player);
            RefreshItems(player);
            RPCommands.Instance.ActionCommand("Has dropped " + itemName + "[" + quantity + "]", player);
        }

        public void BuyItemByName([FromSource] Player player, string itemName)
        {
            var item = ItemManager.Instance.GetItemByName(itemName);
            if (MoneyManager.Instance.GetMoney(player, MoneyTypes.Cash) >= item.SellPrice)
            {
                MoneyManager.Instance.RemoveMoney(player, MoneyTypes.Cash, item.SellPrice);
                //DatabaseManager.Instance.Execute("INSERT INTO MDT_BankStatement (Name,ItemName,Amount,Date) VALUES('"+UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.FullName+"','"+itemName+"','"+item.SellPrice+"');");
                AddItem(item.Id, 1, player);
            }
            else if (MoneyManager.Instance.GetMoney(player, MoneyTypes.Bank) >= item.SellPrice)
            {
                MoneyManager.Instance.RemoveMoney(player, MoneyTypes.Bank, item.SellPrice);
                //DatabaseManager.Instance.Execute("INSERT INTO MDT_BankStatement (ItemName,Amount,Date) VALUES('" + itemName + "','" + item.SellPrice + "');");
                AddItem(item.Id, 1, player);
            }
        }

        public void SellItemByName([FromSource] Player player, string itemName)
        {
            var tgtUser = UserManager.Instance.GetUserFromPlayer(player);
            foreach (var invitem in tgtUser.CurrentCharacter.Inventory)
            {
                if (invitem.Name == itemName)
                {
                    RemoveItem(itemName,1,player);
                    if (invitem.Illegal)
                    {
                        MoneyManager.Instance.AddMoney(player,MoneyTypes.Cash,invitem.BuyPrice);
                    }
                    else
                    {
                        MoneyManager.Instance.AddMoney(player, MoneyTypes.Bank, invitem.BuyPrice);
                    }
                    return;
                }
            }
        }


        public void GiveItem([FromSource] Player player, int recieve, string itemName, int quantity)
        {
            var plyList = new PlayerList();
            var recievingPlayer = plyList[recieve];
            var itemID = ItemManager.Instance.GetItemByName(itemName).Id;
            if (player != null && recievingPlayer != null)
            {
                var user = UserManager.Instance.GetUserFromPlayer(player);
                var matchingItems = user.CurrentCharacter.Inventory.Select(x => x.Id==itemID );
                if (matchingItems.Count() >= quantity)
                {
                    RemoveItem(itemName, quantity, player);
                    AddItem(itemID, quantity, recievingPlayer,true);
                    Utility.Instance.SendChatMessage(player, "[Inventory]", " You have given " + ItemManager.Instance.LoadedItems[itemID].Name + "[" + quantity + "] to " + recievingPlayer.Name + ".", 0, 255, 0);
                    Utility.Instance.SendChatMessage(recievingPlayer, "[Inventory]", " You have been given " + ItemManager.Instance.LoadedItems[itemID].Name + "[" + quantity + "] by " + player.Name + ".", 0, 255, 0);
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
                totalWeight = totalWeight + item.Weight;
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

                    TriggerClientEvent(user.Source, "RefreshInventoryItems", inv, user.CurrentCharacter.Money.Cash, user.CurrentCharacter.Money.Bank, user.CurrentCharacter.Money.UnTaxed, user.CurrentCharacter.MaximumInventory, user.CurrentCharacter.CurrentInventory);
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
