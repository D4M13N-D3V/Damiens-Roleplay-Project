using CitizenFX.Core;
using roleplay.Users.Inventory;
using System.Collections.Generic;

namespace roleplay.Main
{        
    public class ItemManager :BaseScript
    {
        public static ItemManager Instance;

        public ItemManager()
        {
            Instance = this;
        }
        public Dictionary<int,Item> LoadedItems = new Dictionary<int, Item>();

        public Item DynamicCreateItem(string name, string desc, int buy, int sell, int weight, bool illegal)
        {
            var tmpItem = new Item();
            tmpItem.Id = LoadedItems.Count+1;
            tmpItem.Name = name;
            tmpItem.Description = desc;
            tmpItem.BuyPrice = buy;
            tmpItem.SellPrice = sell;
            tmpItem.Weight = weight;
            tmpItem.Illegal = illegal;
            LoadedItems.Add(tmpItem.Id,tmpItem);
            Utility.Instance.Log("A item was created dynamically. ["+name+"]");
            return tmpItem;
        }

        public Item GetItemByName(string name)
        {
            foreach (var item in LoadedItems.Values)
            {
                if (item.Name == name)
                {
                   return item;
                }
            }
            return null;
        }
    }
}
