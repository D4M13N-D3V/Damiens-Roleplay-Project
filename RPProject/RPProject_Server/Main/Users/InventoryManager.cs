using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace roleplay.Main.Users
{
    public class InventoryManager:BaseScript
    {
        private static InventoryManager instance;
        public InventoryManager()
        {
        }
        public static InventoryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InventoryManager();
                }
                return instance;
            }
        }
        public bool AddItem(Player player, int itemId)
        {
            var Items = UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory;
            var tmpItem = ItemManager.Instance.LoadedItems[itemId];
            if (CheckWeightAddition(player,tmpItem.Weight))
            {
                Items.Add(tmpItem);
                RefreshWeight(player);
            }
            return false;
        }

        public bool RemoveItem(Player player,int itemId)
        {
            var Items = UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory;
            var tmpItem = ItemManager.Instance.LoadedItems[itemId];
            if (HasItem(player,itemId))
            {
                Items.Remove(tmpItem);
                RefreshWeight(player);
            }
            return false;
        }

        public bool HasItem(Player player,int itemId)
        {
            var Items = UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Inventory;
            foreach (Item item in Items)
            {
                if (item.Id == itemId)
                {
                    return true;
                }
            }
            return false;
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
    }
}
