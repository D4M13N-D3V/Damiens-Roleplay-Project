using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Main.Activities.Fishing
{

    public class FishSpot
    {
        public float X = 0;
        public float Y = 0;
        public float Z = 0;
        public bool IsShop = false;

        public FishSpot(float x, float y, float z, bool isShop)
        {
            IsShop = isShop;
            X = x;
            Y = y;
            Z = z;
        }

    }
}
