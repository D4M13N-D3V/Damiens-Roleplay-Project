using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using server.Main.Items;
using server.Main.Users;

namespace server.Main.Activities
{
    public class Hunting : BaseScript
    {
        public static Hunting Instance;

        public Hunting()
        {
            Instance = this;
            SetupItems();
            EventHandlers["HuntingReward"] += new Action<Player>(GiveHuntingReward);
            EventHandlers["SellHuntingHides"] += new Action<Player>(SellHuntingHides);
            EventHandlers["SellHuntingMeats"] += new Action<Player>(SellHuntingMeats);
        }

        private void SetupItems()
        {
            ItemManager.Instance.DynamicCreateItem("Animal Skin", "Animal Skin", 300, 300, 15, false, false);
            ItemManager.Instance.DynamicCreateItem("Animal Meat", "Animal Meat", 200, 200, 8, false, false);
        }

        private void SellHuntingHides([FromSource] Player player)
        {
            InventoryManager.Instance.SellItemByName("Animal Skin", player);
        }

        private void SellHuntingMeats([FromSource] Player player)
        {
            InventoryManager.Instance.SellItemByName("Animal Meat", player);
        }

        private void GiveHuntingReward([FromSource] Player player)
        {
            InventoryManager.Instance.AddItem(player, "Animal Skin", 1);
            InventoryManager.Instance.AddItem(player, "Animal Meat", 1);
        }
    }
}
