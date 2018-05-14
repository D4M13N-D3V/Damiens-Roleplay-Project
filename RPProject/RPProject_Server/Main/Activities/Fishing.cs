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
        }

        private const int DistanceCanFish = 20;

        private void GetFish([FromSource] Player ply, string type)
        {
            var user = UserManager.Instance.GetUserFromPlayer(ply);
            var itemId = 0;
            switch (type)
            {
                case "Trout":
                    itemId = 3;
                    break;
                case "Salmon":
                    itemId = 4;
                    break;
                case "Flounder":
                    itemId = 5;
                    break;
                case "Catfish":
                    itemId = 6;
                    break;
                case "Bass":
                    itemId = 7;
                    break;
            }

            InventoryManager.Instance.AddItem(itemId, 1, ply, true);
        }

        private void SellAllFish([FromSource] Player ply)
        {
            var user = UserManager.Instance.GetUserFromPlayer(ply);
            foreach (Item item in user.CurrentCharacter.Inventory)
            {
                if (item.Id == 3 || item.Id == 4 || item.Id == 5 || item.Id == 6 || item.Id == 7)
                {
                    MoneyManager.Instance.AddMoney(ply,MoneyTypes.Cash,item.SellPrice);
                    InventoryManager.Instance.RemoveItem(item.Id,1, ply);
                }
            }
        }
    }
}
