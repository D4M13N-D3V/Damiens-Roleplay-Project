using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using server.Main.Users;
using server.Main.Items;

namespace server.Main.Criminal.Drugs
{
    /// <summary>
    /// The manager class for drugs
    /// </summary>
    public class Drugs : BaseScript
    {
        /// <summary>
        /// All the infromation for all the drugs is held here.
        /// </summary>
        private Dictionary<DrugTypes, DrugInformation> ItemInfo = new Dictionary<DrugTypes, DrugInformation>()
        {
            [DrugTypes.Weed] = new DrugInformation(50, 3, 1000, 2600, 20, 120),
            [DrugTypes.Meth] = new DrugInformation(60, 3, 2000, 2500, 25, 80),
            [DrugTypes.Cocaine] = new DrugInformation(75, 3, 3000, 3600, 100, 250),
            [DrugTypes.Heroine] = new DrugInformation(75, 3, 1000, 1600, 25, 145),
            [DrugTypes.Acid] = new DrugInformation(80, 3, 2000, 3000, 60, 130),
            [DrugTypes.Lsd] = new DrugInformation(50, 3, 1000, 1300, 60, 130),
            [DrugTypes.Crack] = new DrugInformation(50, 3, 1000, 1300, 20, 90),
            [DrugTypes.Xanax] = new DrugInformation(50, 3, 1000, 1300, 20, 0),
            [DrugTypes.Oxy] = new DrugInformation(50, 3, 1000, 1300, 20, 30),
        };

        public Drugs()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            SetupItems();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }

        /// <summary>
        /// Dynamically creates all the items that need to be made.
        /// </summary>
        /// <returns></returns>
        private async Task SetupItems()
        {
            while (InventoryManager.Instance == null)
            {
                await Delay(0);
            }
            foreach (var drug in Enum.GetValues(typeof(DrugTypes)).Cast<DrugTypes>())
            {
                ItemManager.Instance.DynamicCreateItem(Convert.ToString(drug), "Narcotics.", ItemInfo[drug].SellSinglePrice, ItemInfo[drug].BuySinglePrice, ItemInfo[drug].SingleWeight, true);
                ItemManager.Instance.DynamicCreateItem("Bundle of " + Convert.ToString(drug), "Narcotics.", ItemInfo[drug].SellBulkPrice, ItemInfo[drug].BuyBulkPrice, ItemInfo[drug].BulkWeight, true);
            }
        }
    }
}
