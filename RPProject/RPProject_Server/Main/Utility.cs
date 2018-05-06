using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace roleplay.Main
{
    class Utility:BaseScript
    {
        private static Utility instance;
        public Utility()
        {
        }
        public static Utility Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Utility();
                }
                return instance;
            }
        }

        public void Log(string message)
        {
            Debug.WriteLine("[PINEAPPLE ISLAND ROLEPALY] [DEBUG LOG] "+message);
        }

    }
}
