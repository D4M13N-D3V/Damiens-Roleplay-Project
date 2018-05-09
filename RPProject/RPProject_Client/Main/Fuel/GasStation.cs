using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roleplay.Main.Fuel
{
    public class GasStation
    {
        public GasStation(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;   
        }

        public float X = 0;
        public float Y = 0;
        public float Z = 0;
        public int Coffer = 0;
    }
}
