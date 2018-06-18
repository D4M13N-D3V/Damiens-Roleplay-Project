using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.EmergencyServices.Dispatch.MDT
{

    /// <summary>
    /// A bolo object.
    /// </summary>
    public class Bolo
    {
        /// <summary>
        /// The bolo number.
        /// </summary>
        public int BoloNumber;
        /// <summary>
        /// The plate number for the bolo
        /// </summary>
        public string Plate;
        /// <summary>
        /// The charges that should 
        /// </summary>
        public string Charges;
        /// <summary>
        /// The evidence to back up the charges
        /// </summary>
        public string Evidence;
        /// <summary>
        /// Teh description of whath appened and the vehicle.
        /// </summary>
        public string Description;
        public Bolo(int bolonumber, string plate, string charges, string evidence, string description)
        {
            BoloNumber = bolonumber;
            Plate = plate;
            Charges = charges;
            Evidence = evidence;
            Description = description;
        }
    }
}
