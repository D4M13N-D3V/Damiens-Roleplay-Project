using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.EmergencyServices.Dispatch.MDT
{

    /// <summary>
    /// A ticket object.
    /// </summary>
    public class Ticket
    {
        /// <summary>
        /// The case number for the ticket.
        /// </summary>
        public int CaseNumber;
        /// <summary>
        /// The name of the issueing officer
        /// </summary>
        public string OfficerName;
        /// <summary>
        /// The name of the suspect
        /// </summary>
        public string SuspectName;
        /// <summary>
        /// The charges being fined for.
        /// </summary>
        public string Charges;
        /// <summary>
        /// The amount the fine is for.
        /// </summary>
        public string FineAmount;
        public Ticket(int casenumber, string officer, string suspect, string charges, string fine)
        {
            CaseNumber = casenumber;
            OfficerName = officer;
            SuspectName = suspect;
            Charges = charges;
            FineAmount = fine;
        }
    }
}
