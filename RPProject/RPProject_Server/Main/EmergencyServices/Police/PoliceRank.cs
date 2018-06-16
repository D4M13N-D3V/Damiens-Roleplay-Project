using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.EmergencyServices.Police
{
    public enum LEODepartments
    {
        SASP,
        USMS,
    }
    public class PoliceRank
    {
        public string Name = "None";
        public int Salary = 0;
        public LEODepartments Department = LEODepartments.SASP;
        public List<string> AvailableVehicles = new List<string>()
        {

        };
        public bool CanUseHeli = true;
        public bool CanUseSpikeStrips = true;
        public bool CanUseRadar = true;
        public bool CanUseK9 = true;
        public bool CanUseEMS = true;
        public bool CanPromote = false;
        public PoliceRank(string name, int salary, LEODepartments department, List<string> availableVehicles, bool canUseHeli, bool canPromote)
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
