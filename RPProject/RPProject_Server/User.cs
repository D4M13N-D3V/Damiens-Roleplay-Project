using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roleplay
{
    public class User
    {
        public int Source;
        public string SteamId;
        public string License;

        public int Cash=2500;
        public int Bank=0;
        public int UntaxedMoney=0;

        public Character[] Characters;
    }
}
