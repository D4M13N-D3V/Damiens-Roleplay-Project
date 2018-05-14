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
            DatabaseManager.Instance.EndQuery(data);
        }
    }
}
