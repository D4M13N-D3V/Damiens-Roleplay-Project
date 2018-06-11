using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace roleplay.Main
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
