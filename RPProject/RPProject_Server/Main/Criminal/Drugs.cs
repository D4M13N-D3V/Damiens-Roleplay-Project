using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using roleplay.Main.Users;

namespace roleplay.Main.Criminal
{
    public enum DrugTypes { Cocaine, Heroine, Weed, Acid, Lsd, Meth, Crack, Xanax, Oxy }

    public class DrugInformation
    {
        public int BulkWeight;
        public int SingleWeight;
        public int BuyBulkPrice;
        public int SellBulkPrice;
        public int BuySinglePrice;
        public int SellSinglePrice;
        public DrugInformation(int bulkweight, int singleweight, int bulkbuy, int bulksell, int singlebuy, int singlesell)
        {
            BulkWeight = bulkweight;
            SingleWeight = singleweight;
            BuyBulkPrice = bulkbuy;
            SellBulkPrice = bulksell;
            BuySinglePrice = singlebuy;
            SellSinglePrice = singlesell;
        }
    }

    public class Drugs : BaseScript
    {
        private Dictionary<DrugTypes, DrugInformation> ItemInfo = new Dictionary<DrugTypes, DrugInformation>()
        {
            [DrugTypes.Weed] = new DrugInformation(50, 3, 1000, 1300, 20, 30),
            [DrugTypes.Meth] = new DrugInformation(60, 3, 2000, 2500, 25, 40),
            [DrugTypes.Cocaine] = new DrugInformation(75, 3, 3000, 4000, 100, 130),
            [DrugTypes.Heroine] = new DrugInformation(75, 3, 1000, 1300, 25, 40),
            [DrugTypes.Acid] = new DrugInformation(80, 3, 2000, 3000, 60, 80),
            [DrugTypes.Lsd] = new DrugInformation(50, 3, 1000, 1300, 20, 30),
            [DrugTypes.Crack] = new DrugInformation(50, 3, 1000, 1300, 20, 30),
            [DrugTypes.Xanax] = new DrugInformation(50, 3, 1000, 1300, 20, 30),
            [DrugTypes.Oxy] = new DrugInformation(50, 3, 1000, 1300, 20, 30),
        };

        public Drugs()
        {
            SetupItems();
        }

        private async void SetupItems()
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
