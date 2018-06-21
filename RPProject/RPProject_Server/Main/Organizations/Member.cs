using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using server.Main.Users.CharacterClasses;

namespace server.Main.Organizations
{
    public class Member
    {
        public string Character;
        public bool CanInvite;
        public bool CanPromote;
        public bool CanDeposit;
        public bool CanWithdrawl;

        public Member(string character, bool invite, bool canpromote ,bool deposit, bool withdrawl)
        {
            Character = character;
            CanInvite = invite;
            CanDeposit = deposit;
            CanWithdrawl = withdrawl;
            CanPromote = canpromote;
        }
    }
}
