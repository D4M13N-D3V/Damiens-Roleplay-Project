using System.Collections.Generic;

namespace roleplay.Main.Users.CharacterClasses
{
    public class CharacterInventory
    {
        public User User;
        public List<Item> Items = new List<Item>();
        
        public bool AddItem(int itemId)
        {
            var tmpItem = ItemManager.Instance.LoadedItems[itemId];
            if (CheckWeightAddition(tmpItem.Weight))
            {
                Items.Add(tmpItem);
                RefreshWeight();
            }
            return false;
        }

        public bool RemoveItem(int itemId)
        {
            var tmpItem = ItemManager.Instance.LoadedItems[itemId];
            if (HasItem(itemId))
            {
                Items.Remove(tmpItem);
                RefreshWeight();
            }
            return false;
        }

        public bool HasItem(int itemId)
        {
            foreach (Item item in Items)
            {
                if (item.Id == itemId)
                {
                    return true;
                }
            }
            return false;
        }
        

        public void RefreshWeight()
        {
            var totalWeight = 0;
            foreach (Item item in Items)
            {
                totalWeight =+ item.Weight;
            }
            User.CurrentCharacter.CurrentInventory = totalWeight;
        }
        
        public bool CheckWeightAddition(int amount)
        {
            if (User.CurrentCharacter.CurrentInventory + amount < User.CurrentCharacter.MaximumInventory)
            {
                return true;
            }
            return false;
        }
    }
}