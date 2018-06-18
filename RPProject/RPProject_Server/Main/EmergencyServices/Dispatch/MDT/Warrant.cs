using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.EmergencyServices.Dispatch.MDT
{

    /// <summary>
    ///  A warrant object.
    /// </summary>
    public class Warrant
    {
        /// <summary>
        /// The warrant number
        /// </summary>
        public int WarrantNumber;
        /// <summary>
        /// The name of the person the warrant is for.
        /// </summary>
        public string Name;
        /// <summary>
        /// The charges that the warrant is for
        /// </summary>
        public string Charges;
        /// <summary>
        /// The eveidence for the charges
        /// </summary>
        public string Evidence;
        /// <summary>
        /// Any other notes.
        /// </summary>
        public string Notes;
        public Warrant(int warrantnumbe, string name, string charges, string evidence, string notes)
        {
            WarrantNumber = warrantnumbe;
            Name = name;
            Charges = charges;
            Evidence = evidence;
            Notes = notes;
        }
    }
}
