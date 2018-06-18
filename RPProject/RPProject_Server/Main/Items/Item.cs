using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.Items
{
    /// <summary>
    /// Item interface.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// The id of the item
        /// </summary>
        public int Id = 1;
        /// <summary>
        /// The name of the item
        /// </summary>
        public string Name = "Test Item";
        /// <summary>
        /// The descirption of the item.
        /// </summary>
        public string Description = "This is a item for testing. LUL. NOOB.";
        /// <summary>
        /// The amount that the item is bought for by shops
        /// </summary>
        public int BuyPrice = 100;
        /// <summary>
        /// The amount the item  sells for from shops
        /// </summary>
        public int SellPrice = 100;
        /// <summary>
        /// The weight of the item
        /// </summary>
        public int Weight = 10;
        /// <summary>
        /// if the item is illegal
        /// </summary>
        public bool Illegal = false;
        /// <summary>
        /// is the item a weapon
        /// </summary>
        public bool IsWeapon = false;
    }
}
