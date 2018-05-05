using roleplay.Main.Users.CharacterClasses;
using roleplay.Main.Users.Customization;
using roleplay.Main.Vehicles;

namespace roleplay.Main.Users.CharacterClasses
{
    public class Character
    {
        public string FirstName = "John";
        public string LastName = "Doe";
        public int Height = 100;
        public string DateOfBirth = "00/00/0000";

        public int MaximumInventory = 200;
        public CharacterInventory Inventory;
        public CharacterMoney Money;
        public CharacterCustomization Customization;
        public Vehicle[] Vehicles;
    }
}
