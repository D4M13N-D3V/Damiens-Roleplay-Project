using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.EmergencyServices.EMS
{
    /// <summary>
    /// The deaprtments within EMS
    /// </summary>
    public enum EMSDepartments
    {
        EMS,
        FIRE,
    }

    /// <summary>
    /// Object holding information for a EMS rank
    /// </summary>
    public class EMSRank
    {
        /// <summary>
        /// The name of the rank
        /// </summary>
        public string Name = "None";
        /// <summary>
        /// The salarey for the rank.
        /// </summary>
        public int Salary = 0;
        /// <summary>
        /// The ranks department
        /// </summary>
        public EMSDepartments Department = EMSDepartments.EMS;
        /// <summary>
        /// The hashes as strings of vehicles that they can drive/pull out from garage.
        /// </summary>
        public List<string> AvailableVehicles = new List<string>()
        {

        };
        /// <summary>
        /// Can the rank use the helicopter
        /// </summary>
        public bool CanUseHeli = true;
        /// <summary>
        /// Cane the rank promote other ems members.
        /// </summary>
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
