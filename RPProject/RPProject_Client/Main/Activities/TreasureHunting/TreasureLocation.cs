using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace client.Main.Activities.TreasureHunting
{
    public class TreasureLocation
    {
        public Vector3 Posistion;
        public List<string> PossibleItems;
        public bool CanLoot = true;

        public TreasureLocation(Vector3 pos, List<string> items)
        {
            Posistion = pos;
            PossibleItems = items;
        }
    }
}
