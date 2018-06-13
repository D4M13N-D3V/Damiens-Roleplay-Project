using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Main.Criminal.Drugs
{
    public enum DrugTypes { Cocaine, Meth, Weed, Acid, Lsd, Heroine, Crack, Xanax, Oxy }
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
}
