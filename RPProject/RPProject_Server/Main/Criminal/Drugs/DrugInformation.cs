using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.Criminal.Drugs
{
    /// <summary>
    /// The types of drugs
    /// </summary>
    public enum DrugTypes { Cocaine, Heroine, Weed, Acid, Lsd, Meth, Crack, Xanax, Oxy }

    /// <summary>
    /// A class holding all the information for a drug.
    /// </summary>
    public class DrugInformation
    {
        /// <summary>
        /// How much a bulk piece weighs.
        /// </summary>
        public int BulkWeight;
        /// <summary>
        /// How much a single piece weighs
        /// </summary>
        public int SingleWeight;
        /// <summary>
        /// How much a bulk piece costs
        /// </summary>
        public int BuyBulkPrice;
        /// <summary>
        /// How much a bulk piece sells for
        /// </summary>
        public int SellBulkPrice;
        /// <summary>
        /// How much a single piece costs
        /// </summary>
        public int BuySinglePrice;
        /// <summary>
        /// How much a single piece sells for
        /// </summary>
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
