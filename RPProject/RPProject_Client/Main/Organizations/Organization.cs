using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Main.Organizations
{
    public class Organization
    {
        public string Name;
        public bool CanInvite;
        public bool CanPromote;
        public bool CanDeposit;
        public bool CanWithdrawl;
        public int Bank;
        public List<dynamic> Members;

        public Organization(string name, bool invite, bool promote, bool deposit, bool withdrawl, List<dynamic> members, int bank)
        {
            Name = name;
            CanInvite = invite;
            CanPromote = promote;
            CanDeposit = deposit;
            CanWithdrawl = withdrawl;
            Members = members;
            Bank = bank;
        }
    }
}
