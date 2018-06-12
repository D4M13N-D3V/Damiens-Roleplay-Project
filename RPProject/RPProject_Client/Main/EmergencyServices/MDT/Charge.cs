using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Main.EmergencyServices.MDT
{
    public enum ChargeTypes
    {
        Felony,
        Misdemeanors,
        Traffic
    };

    public class Charge
    {
        public string Title;
        public ChargeTypes Type;
        public int Fine;
        public int Time;
        public Charge(string title, ChargeTypes type,  int fine, int time)
        {
            Title = title;
            Type = type;
            Fine = fine;
            Time = time;
        }
    }
}
