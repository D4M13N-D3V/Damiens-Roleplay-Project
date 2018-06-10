using roleplay.Main.Users.CharacterClasses;
using roleplay.Main.Users.Customization;
using roleplay.Main.Vehicles;
using System.Collections.Generic;
using CitizenFX.Core;
using roleplay.Users.Inventory;

namespace roleplay.Main.Users.CharacterClasses
{
    public class Character
    {
        public string FirstName = "John";
        public string LastName = "Doe";

        public Dictionary<FlagTypes,bool> MDTFlags = new Dictionary<FlagTypes, bool>()
        {
            [FlagTypes.Felon] = false,
            [FlagTypes.Gang] = false,
            [FlagTypes.CopHater] = false,
            [FlagTypes.MentallyUnstable] = false,
            [FlagTypes.SuspendedLicense] = false,
            [FlagTypes.Probation]=false
        };

        public string FullName
        {
            get { return FirstName + " " + LastName; }
            set { FullName = value; }
        }

        public string DateOfBirth = "00/00/0000";

        public string PhoneNumber = "9999999999";

        public Vector3 Pos = new Vector3(0,0,0);

        /// <summary>
        /// 0 is male
        /// 1 is female
        /// </summary>
        public int Gender = 0;

        public int JailTime = 0;
        public int HospitalTime = 0;

        public int MaximumInventory = 200;
        public int CurrentInventory = 0;

        public List<Item> Inventory = new List<Item>();
        public CharacterMoney Money;
        public CharacterCustomization Customization;
    }
}
