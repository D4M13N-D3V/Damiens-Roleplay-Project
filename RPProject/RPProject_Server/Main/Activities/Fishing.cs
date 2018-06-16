using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using server.Main.Items;
using server.Main.Users;

namespace server.Main.Activities
{
    public enum FishTypes
    {
        Trout, Salmon, Flounder, Catfish, Bass
    }
    public class Fishing:BaseScript
    {
        public static Fishing Instance;

        public Fishing()
        {
            Instance = this;
            EventHandlers["GetFish"] += new Action<Player, string>(GetFish);
            EventHandlers["SellAllFish"] += new Action<Player>(SellAllFish);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            SetupItems();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }

        private int _troutItemId;
        private int _salmonItemId;
        private int _flounderItemId;
        private int _catfishItemId;
        private int _bassItemId;

        private async Task SetupItems()
        {
        
            var troutItem = ItemManager.Instance.DynamicCreateItem("Trout", "Fish that you caught!", 15, 15, 3, false);
            var salmonItem = ItemManager.Instance.DynamicCreateItem("Salmon", "Fish that you caught!", 20, 20, 4, false);
            var flounderItem = ItemManager.Instance.DynamicCreateItem("Flounder", "Fish that you caught!", 25, 25, 5, false);
            var catfishItem = ItemManager.Instance.DynamicCreateItem("Catfish", "Fish that you caught!", 30, 30, 6, false);
            var bassItem = ItemManager.Instance.DynamicCreateItem("Bass", "Fish that you caught!", 35, 35, 7, false);

            await Delay(3000);

            _troutItemId = troutItem.Id;
            _salmonItemId = salmonItem.Id; ;
            _flounderItemId = flounderItem.Id;
            _catfishItemId = catfishItem.Id;
            _bassItemId = bassItem.Id;
        }

        private void GetFish([FromSource] Player ply, string type)
        {
            var itemId = 0;
            switch (type)
            {
                case "Trout":
                    itemId = _troutItemId;
                    break;
                case "Salmon":
                    itemId = _salmonItemId;
                    break;
                case "Flounder":
                    itemId = _flounderItemId;
                    break;
                case "Catfish":
                    itemId = _catfishItemId;
                    break;
                case "Bass":
                    itemId = _bassItemId;
                    break;
            }

            InventoryManager.Instance.AddItem(itemId, 1, ply, true);
        }

        private void SellAllFish([FromSource] Player ply)
        {
            var user = UserManager.Instance.GetUserFromPlayer(ply);
            var inv = user.CurrentCharacter.Inventory;
            lock (inv)
            {
                foreach (Item item in inv)
                {
                    if (item.Name == "Trout" || item.Name == "Salmon" || item.Name == "Flounder" || item.Name == "Catfish" || item.Name == "Bass")
                    {
                        MoneyManager.Instance.AddMoney(ply, MoneyTypes.Cash, item.SellPrice);
                        InventoryManager.Instance.RemoveItem(item.Name, 1, ply);
                    }
                }
            }
        }
    }
}
