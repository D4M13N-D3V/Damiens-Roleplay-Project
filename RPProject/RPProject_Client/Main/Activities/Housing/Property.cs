using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace client.Main.Activities.Housing
{
    public class Property
    {
        public string Name;
        public Vector3 Posistion;
        public Vector3 GarageSpawnPoint;
        public string Owner;
        public int Price;
        public bool ForSale = false;
    }
}
