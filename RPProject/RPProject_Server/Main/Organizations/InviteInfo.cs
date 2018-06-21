using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace server.Main.Organizations
{
    public class InviteInfo
    {
        public string Organization;
        public string Inviter;
        public string Recipient;

        public InviteInfo(string org, string inviter, string recipient)
        {
            Organization = org;
            Inviter = inviter;
            Recipient = recipient;
        }
    }
}
