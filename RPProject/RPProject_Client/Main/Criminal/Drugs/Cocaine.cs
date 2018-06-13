using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace client.Main.Criminal.Drugs
{
    public class Cocaine : Drug
    {
        public Cocaine() : base(
            new Vector3(-2032.3114f, -1038.63586f, 5.882404f),
            new Vector3(-1502.6539306641f, 137.2939453125f, 55.653125762939f),
            DrugTypes.Cocaine)
        {

        }
    }
}
