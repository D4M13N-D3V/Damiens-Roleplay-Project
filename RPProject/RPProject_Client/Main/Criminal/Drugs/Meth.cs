using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace client.Main.Criminal.Drugs
{
    public class Meth : Drug
    {
        public Meth() : base(
            new Vector3(1395.281f, 3608.274f, 38.9419022f),
            new Vector3(60.799774169922f, 3718.8859863281f, 39.746185302734f),
            DrugTypes.Meth)
        {

        }
    }
}
