using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.EmergencyServices.Police
{
    /// <summary>
    /// Police departments
    /// </summary>
    public enum LEODepartments
    {
        SASP,
        USMS,
    }

    /// <summary>
    /// Police rank object
    /// </summary>
    public class PoliceRank
    {
        /// <summary>
        /// name of rank
        /// </summary>
        public string Name = "None";
        /// <summary>
        /// salary of rank
        /// </summary>
        public int Salary = 0;
        /// <summary>
        /// Department of rank
        /// </summary>
        public LEODepartments Department = LEODepartments.SASP;
        /// <summary>
        /// string of hashes o vehicles the player can drive
        /// </summary>
        public List<string> AvailableVehicles = new List<string>()
        {

        };
        /// <summary>
        /// can rank use heli
        /// </summary>
        public bool CanUseHeli = true;
        /// <summary>
        /// can rank promote
        /// </summary>
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
