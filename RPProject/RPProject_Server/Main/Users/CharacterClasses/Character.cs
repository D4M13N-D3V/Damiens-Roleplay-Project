using roleplay.Main.Users.CharacterClasses;
using roleplay.Main.Users.Customization;
using roleplay.Main.Vehicles;

namespace roleplay.Main.Users.CharacterClasses
{
    public class Character
    {
        public User User;
        public string FirstName = "John";
        public string LastName = "Doe";
        public int Height = 100;
        public string DateOfBirth = "00/00/0000";

        public string PhoneNumber = "9999999999";

        public int JailTime = 0;
        public int HospitalTime = 0;

        public int MaximumInventory = 200;
        public int CurrentInventory = 0;

        public CharacterInventory Inventory;
        public CharacterMoney Money;
        public CharacterCustomization Customization;
    }
}
