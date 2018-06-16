using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.EmergencyServices.EMS
{
    public class EMSMember
    {
        public string SteamId;
        public string Name;
        public string Rank;
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
