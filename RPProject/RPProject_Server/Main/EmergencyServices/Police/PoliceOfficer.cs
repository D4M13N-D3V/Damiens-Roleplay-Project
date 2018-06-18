using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.EmergencyServices.Police
{
    /// <summary>
    /// Objet holding info for polcie officer
    /// </summary>
    public class PoliceOfficer
    {
        /// <summary>
        /// Stema id for the player
        /// </summary>
        public string SteamId;

        /// <summary>
        /// name of character htat is ems
        /// </summary>
        public string Name;

        /// <summary>
        /// The rank of the ems char
        /// </summary>
        public string Rank;

        /// <summary>
        /// Badge number
        /// </summary>
        public int Badge;

        public PoliceOfficer(string steamId, string name, string rank, int badge)
        {
            SteamId = steamId;
            Name = name;
            Rank = rank;
            Badge = badge;
        }
    }
}
