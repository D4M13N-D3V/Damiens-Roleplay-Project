using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using roleplay.Main.Users;

namespace roleplay.Main.Activities
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
            SetupItems();
        }

        private int _troutItemId;
        private int _salmonItemId;
        private int _flounderItemId;
        private int _catfishItemId;
        private int _bassItemId;

        private async void SetupItems()
        {
        
            var troutItem = ItemManager.Instance.DynamicCreateItem("Trout", "Fish that you caught!", 25, 25, 3, false);
            var salmonItem = ItemManager.Instance.DynamicCreateItem("Salmon", "Fish that you caught!", 25, 25, 3, false);
            var flounderItem = ItemManager.Instance.DynamicCreateItem("Flounder", "Fish that you caught!", 25, 25, 3, false);
            var catfishItem = ItemManager.Instance.DynamicCreateItem("Catfish", "Fish that you caught!", 25, 25, 3, false);
            var bassItem = ItemManager.Instance.DynamicCreateItem("Bass", "Fish that you caught!", 25, 25, 3, false);

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
                    if (item.Id == 3 || item.Id == 4 || item.Id == 5 || item.Id == 6 || item.Id == 7)
                    {
                        MoneyManager.Instance.AddMoney(ply, MoneyTypes.Cash, item.SellPrice);
                        InventoryManager.Instance.RemoveItem(item.Id, 1, ply);
                    }
                }
            }
        }
    }
}
