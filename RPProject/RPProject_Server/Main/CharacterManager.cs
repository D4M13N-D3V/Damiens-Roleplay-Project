using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using roleplay.Main.Users.CharacterClasses;
using roleplay.Main.Users.Customization;

namespace roleplay.Main
{
    public class CharacterManager : BaseScript
    {
        private static CharacterManager instance;

        public CharacterManager()
        {

        }

        public static CharacterManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CharacterManager();
                }
                return instance;
            }
        }


        public Character CreateCharacter(int playerId,string first, string last, int height, string dateOfBirth)
        {
            var data = DatabaseManager.Instance.StartScalar("SELECT * FROM CHARACTERS WHERE firstname = '"+first+"' AND lastname = '"+last+"'");
            if (data == null)
            {
                var tmpCharacter = new Character();;
                tmpCharacter.FirstName = first;
                tmpCharacter.LastName = last;
                tmpCharacter.Height = height;
                tmpCharacter.DateOfBirth = dateOfBirth;
                tmpCharacter.MaximumInventory = 150;
                tmpCharacter.CurrentInventory = 0;
                tmpCharacter.Inventory = new CharacterInventory();
                tmpCharacter.Inventory.Character = tmpCharacter;
                tmpCharacter.Money.Character = tmpCharacter;
                tmpCharacter.Customization = new CharacterCustomization();
            }
            DatabaseManager.Instance.EndScalar();
            return null;
        }

    }
}
