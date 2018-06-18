using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace server.Main.Activities.TreasureHunting
{
    /// <summary>
    /// Location for treasure.
    /// </summary>
    public class TreasureLocation
    {
        /// <summary>
        /// The posistion of the location.
        /// </summary>
        public Vector3 Posistion;
        
        /// <summary>
        /// List of strings cooresponding to name of items that can be looted.
        /// </summary>
        public List<string> PossibleItems;

        public TreasureLocation(Vector3 pos, List<string> items)
        {
            Posistion = pos;
            PossibleItems = items;
        }
    }
}
