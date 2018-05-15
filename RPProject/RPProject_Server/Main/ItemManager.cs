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

            ReloadItems();
        }

        public bool ItemsLoaded = false;
        public Dictionary<int,Item> LoadedItems = new Dictionary<int, Item>();

        public async void ReloadItems()
        {
            while (Utility.Instance == null)
            {
                await Delay(0);
            }
            LoadedItems.Clear();
            Utility.Instance.Log("Loading Items..........................");
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM ITEMS");
            while(data.Read())
            {
                var tmpItem = new Item();
                tmpItem.Id = Convert.ToInt32(data["id"]);
                tmpItem.Name = Convert.ToString(data["name"]);
                tmpItem.Description = Convert.ToString(data["description"]);
                tmpItem.BuyPrice = Convert.ToInt32(data["buyprice"]);
                tmpItem.SellPrice = Convert.ToInt32(data["sellprice"]);
                tmpItem.Weight = Convert.ToInt32(data["weight"]);
                tmpItem.Illegal = Convert.ToBoolean(data["illegal"]);
                LoadedItems.Add(Convert.ToInt32(data["id"]),tmpItem);
                Utility.Instance.Log(tmpItem.Name+" has been loaded and added.");
            }
            ItemsLoaded = true;
            DatabaseManager.Instance.EndQuery(data);
        }

        public Item DynamicCreateItem(string name, string desc, int buy, int sell, int weight, bool illegal)
        {
            var tmpItem = new VehicleKeysItem();
            tmpItem.Id = LoadedItems.Last().Key+1; // Get the last id, and add one to it.
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
    }
}
