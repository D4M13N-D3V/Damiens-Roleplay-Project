using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.Activities.Trucking
{
    public class TruckingDestination
    {
        public string Name;
        public float X;
        public float Y;
        public float Z;
        public int Payout;

        public TruckingDestination(string name, float x, float y, float z, int payout)
        {
            Name = name;
            X = x;
            Y = y;
            Z = z;
            Payout = payout;
        }

    }
}
