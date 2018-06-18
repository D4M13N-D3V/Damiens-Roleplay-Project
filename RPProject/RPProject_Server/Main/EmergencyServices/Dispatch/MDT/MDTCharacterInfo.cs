using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.EmergencyServices.Dispatch.MDT
{
    /// <summary>
    /// Flag types for crminals
    /// </summary>
    public enum FlagTypes { Felon, Aggressive, CopHater, Gang, SuspendedLicense, MentallyUnstable, Probation };

    /// <summary>
    /// The MDT info for every character.
    /// </summary>
    public class MDTCharacterInfo
    {
        /// <summary>
        /// All the flags this character has.
        /// </summary>
        public Dictionary<FlagTypes, bool> Flags;
        /// <summary>
        /// First name of the character
        /// </summary>
        public string First;
        /// <summary>
        /// Last name of the character
        /// </summary>
        public string Last;
        /// <summary>
        /// Date of birth of the character
        /// </summary>
        public string DOB;
        /// <summary>
        /// the gender of the character
        /// </summary>
        public string Gender;

        public MDTCharacterInfo(Dictionary<FlagTypes, bool> flags, string first, string last, string dob, string gender)
        {
            Flags = flags;
            First = first;
            Last = last;
            DOB = dob;
            Gender = gender;
        }
    }
}
