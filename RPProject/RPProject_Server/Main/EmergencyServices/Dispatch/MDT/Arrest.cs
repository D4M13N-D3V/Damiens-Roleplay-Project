using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.EmergencyServices.Dispatch.MDT
{

    /// <summary>
    /// A arrect object.
    /// </summary>
    public class Arrest
    {
        /// <summary>
        /// The case number for the arrest.
        /// </summary>
        public int CaseNumber;
        /// <summary>
        /// The name of the arresting officer.
        /// </summary>
        public string OfficerName;
        /// <summary>
        /// The name of the suspect
        /// </summary>
        public string SuspectName;
        /// <summary>
        /// List of charges issued.
        /// </summary>
        public string Charges;
        /// <summary>
        /// The amount of time issued.
        /// </summary>
        public string Time;
        /// <summary>
        /// The size of fine issued.
        /// </summary>
        public string Fine;
        public Arrest(int casenumber, string officername, string suspectname, string charges, string time, string fine)
        {
            CaseNumber = casenumber;
            OfficerName = officername;
            SuspectName = suspectname;
            Charges = charges;
            Time = time;
            Fine = fine;
        }
    }
}
