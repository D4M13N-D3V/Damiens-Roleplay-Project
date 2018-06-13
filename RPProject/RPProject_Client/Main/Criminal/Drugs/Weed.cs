using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace client.Main.Criminal.Drugs
{
    public class Weed : Drug
    {
        public Weed() : base(
            new Vector3(2218.37646f, 5612.836f, 54.69646f),
            new Vector3(-1173.23523f, -1573.29883f, 4.51461172f),
            DrugTypes.Weed)
        {

        }
    }
}
