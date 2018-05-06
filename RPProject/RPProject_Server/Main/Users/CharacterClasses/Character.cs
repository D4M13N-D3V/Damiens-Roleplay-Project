using roleplay.Main.Users.CharacterClasses;
using roleplay.Main.Users.Customization;
using roleplay.Main.Vehicles;
using System.Collections.Generic;

namespace roleplay.Main.Users.CharacterClasses
{
    public class Character
    {
        public string FirstName = "John";
        public string LastName = "Doe";
        public string DateOfBirth = "00/00/0000";

        public string PhoneNumber = "9999999999";

        public int JailTime = 0;
        public int HospitalTime = 0;

        public int MaximumInventory = 200;
        public int CurrentInventory = 0;

        public List<Item> Inventory = new List<Item>();
        public CharacterMoney Money;
        public CharacterCustomization Customization;
    }
}
