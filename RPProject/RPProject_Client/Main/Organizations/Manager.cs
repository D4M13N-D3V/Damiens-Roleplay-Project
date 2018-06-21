using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace client.Main.Organizations
{
    public class Manager : BaseScript
    {
        public static Manager Instance;

        public Manager()
        {
            Instance = this;
        }
    }
}
