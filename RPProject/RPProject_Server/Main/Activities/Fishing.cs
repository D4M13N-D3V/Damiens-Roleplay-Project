using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using server.Main.Items;
using server.Main.Users;

namespace server.Main.Activities
{
    /// <summary>
    /// The different fish types.
    /// </summary>
    public enum FishTypes
    {
        Trout, Salmon, Flounder, Catfish, Bass
    }

    /// <summary>
    /// A manager class for fishing activity.
    /// </summary>
    public class Fishing:BaseScript
    {
        /// <summary>
        /// Singelton instance of the fishing class
        /// </summary>
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

        /// <summary>
        /// Item ID for the trout
        /// </summary>
        private int _troutItemId;
        /// <summary>
        /// Item id for the salmon
        /// </summary>
        private int _salmonItemId;
        /// <summary>
        /// Item id for the flounder
        /// </summary>
        private int _flounderItemId;
        /// <summary>
        /// Item id for the catfish
        /// </summary>
        private int _catfishItemId;
        /// <summary>
        /// ITem id for the bass
        /// </summary>
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

        /// <summary>
        /// Event handler for when the player catches a fish
        /// </summary>
        /// <param name="ply">The player triggering</param>
        /// <param name="type">Type of fish.</param>
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

        /// <summary>
        /// Event handler that sells all the fish in the players inventory.
        /// </summary>
        /// <param name="ply">The player who called it.</param>
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
