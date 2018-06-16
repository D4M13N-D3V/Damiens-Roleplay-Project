using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.EmergencyServices.Police
{
    public class PoliceOfficer
    {
        public string SteamId;
        public string Name;
        public string Rank;
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
