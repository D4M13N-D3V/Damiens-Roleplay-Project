using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Main.EmergencyServices.MDT
{
    public class ChargesInfo
    {
        public string Charges;
        public int TotalFine;
        public int TotalTime;

        public ChargesInfo(string charges, int fine, int time)
        {
            Charges = charges;
            TotalFine = fine;
            TotalTime = time;
        }
    }
}
