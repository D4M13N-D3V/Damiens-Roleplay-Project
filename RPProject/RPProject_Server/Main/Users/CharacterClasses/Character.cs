using roleplay.Main.Users.CharacterClasses;
using roleplay.Main.Users.Customization;
using roleplay.Main.Vehicles;
using System.Collections.Generic;
using CitizenFX.Core;

namespace roleplay.Main.Users.CharacterClasses
{
    public class Character
    {
        public string FirstName = "John";
        public string LastName = "Doe";
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
