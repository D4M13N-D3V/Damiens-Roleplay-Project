using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roleplay
{
    public class Character
    {
        public string FirstName = "John";
        public string LastName = "Doe";
        public int Height = 100;
        public string DateOfBirth = "00/00/0000";

        public int Cash = 2500;
        public int Bank = 0;
        public int UntaxedMoney = 0;

        public Vehicle[] Vehicles;
        
        public Character()
        {
            
        }
    }
}
