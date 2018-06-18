using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.Activities.Trucking
{
    /// <summary>
    /// Destination for trucking
    /// </summary>
    public class TruckingDestination
    {
        /// <summary>
        /// Name of the destination
        /// </summary>
        public string Name;
        /// <summary>
        /// X pos
        /// </summary>
        public float X;
        /// <summary>
        /// Y Pos
        /// </summary>
        public float Y;
        /// <summary>
        /// Z Pos
        /// </summary>
        public float Z;
        /// <summary>
        /// How much it pays.
        /// </summary>
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
