using CitizenFX.Core;
using System.Collections.Generic;

namespace server.Main.Items
{
    /// <summary>
    /// manager for the items
    /// </summary>
    public class ItemManager :BaseScript
    {
        /// <summary>
        /// Singleton Instance.
        /// </summary>
        public static ItemManager Instance;

        public ItemManager()
        {
            Instance = this;
        }

        /// <summary>
        /// All items that are currently loaded into the game.
        /// </summary>
        public Dictionary<int,Item> LoadedItems = new Dictionary<int, Item>();

        /// <summary>
        /// dynamically creates a item
        /// </summary>
        /// <param name="name">name of the item</param>
        /// <param name="desc">desc of the item</param>
        /// <param name="buy">how much item is bought for</param>
        /// <param name="sell">how much stores sell item for</param>
        /// <param name="weight">weight of ite</param>
        /// <param name="illegal">if the item is illegal</param>
        /// <param name="isweapon">if the item is weapon</param>
        /// <returns>the item created</returns>
        public Item DynamicCreateItem(string name, string desc, int buy, int sell, int weight, bool illegal, bool isweapon = false)
        {
            var tmpItem = new Item();
            tmpItem.Id = LoadedItems.Count+1;
            tmpItem.Name = name;
            tmpItem.Description = desc;
            tmpItem.BuyPrice = buy;
            tmpItem.SellPrice = sell;
            tmpItem.Weight = weight;
            tmpItem.Illegal = illegal;
            tmpItem.IsWeapon = isweapon;
            LoadedItems.Add(tmpItem.Id,tmpItem);
            Utility.Instance.Log("A item was created dynamically. ["+name+"]");
            return tmpItem;
        }

        /// <summary>
        /// Get a item by its name
        /// </summary>
        /// <param name="name">name of item</param>
        /// <returns>item with matching name or null if none</returns>
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
