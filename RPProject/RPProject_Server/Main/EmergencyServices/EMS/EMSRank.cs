using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.EmergencyServices.EMS
{
    public enum EMSDepartments
    {
        EMS,
        FIRE,
    }
    public class EMSRank
    {
        public string Name = "None";
        public int Salary = 0;
        public EMSDepartments Department = EMSDepartments.EMS;
        public List<string> AvailableVehicles = new List<string>()
        {

        };
        public bool CanUseHeli = true;
        public bool CanUseSpikeStrips = true;
        public bool CanUseRadar = true;
        public bool CanUseK9 = true;
        public bool CanUseEMS = true;
        public bool CanPromote = false;
        public EMSRank(string name, int salary, EMSDepartments department, List<string> availableVehicles, bool canUseHeli, bool canPromote)
        {
            Department = department;
            Name = name;
            Salary = salary;
            AvailableVehicles = availableVehicles;
            CanPromote = canPromote;
            CanUseHeli = canUseHeli;
        }
    }
}
