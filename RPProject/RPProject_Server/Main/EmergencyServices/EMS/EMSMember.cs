using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.EmergencyServices.EMS
{
    /// <summary>
    /// A member of ems
    /// </summary>
    public class EMSMember
    {
        /// <summary>
        /// The steam id
        /// </summary>
        public string SteamId;

        /// <summary>
        /// The name of the character that is ems
        /// </summary>
        public string Name;

        /// <summary>
        /// The rank
        /// </summary>
        public string Rank;

        /// <summary>
        /// The badge number
        /// </summary>
        public int Badge;

        public EMSMember(string steamId, string name, string rank, int badge)
        {
            SteamId = steamId;
            Name = name;
            Rank = rank;
            Badge = badge;
        }
    }
}
