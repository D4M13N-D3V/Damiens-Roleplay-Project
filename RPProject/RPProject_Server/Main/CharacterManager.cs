using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using roleplay.Main.Users.CharacterClasses;
using roleplay.Main.Users.Customization;
using roleplay.Main.Users;
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


        public Character CreateCharacter(Player player,string first, string last, int height, string dateOfBirth)
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

                tmpCharacter.PhoneNumber = "";
                var rnd = new Random();
                for (int i = 0; i < 10; i++)
                {
                    tmpCharacter.PhoneNumber = tmpCharacter.PhoneNumber + System.Convert.ToString(rnd.Next(0, 9));
                }

                User user = UserManager.Instance.GetUserFromPlayer(player);
                user.Characters.Add(tmpCharacter);
                DatabaseManager.Instance.Execute("INSERT INTO CHARACTERS (steamid,firstname,lastname,dateofbirth,phonenumber,cash,bank,untaxed,inventory,customization,jailtime,hospitaltime) " +
                                                 "VALUES('"+player.Identifiers["steam"]+"','"+tmpCharacter.FirstName+"','"+tmpCharacter.LastName+"','"+tmpCharacter.DateOfBirth+"','"+tmpCharacter.PhoneNumber+"'," +
                                                 "2500,0,0,'"+JsonConvert.SerializeObject(tmpCharacter.Inventory.Items)+"','"+JsonConvert.SerializeObject(tmpCharacter.Customization)+"',0,0)");
            }
            DatabaseManager.Instance.EndScalar();
            return null;
        }

    }
}
