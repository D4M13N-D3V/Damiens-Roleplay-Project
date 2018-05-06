using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CitizenFX.Core;
using CitizenFX.Core.Native;
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
            EventHandlers["characterCreationRequest"] +=
                new Action<Player, string, string, string>(NewCharacterRequest);
            
            EventHandlers["selectCharacterRequest"] +=
                new Action<Player,int>(SelectCharacterRequest);
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

        private void NewCharacterRequest([FromSource] Player source, string first, string last, string dateOfBirth)
        {
            CreateCharacter(source, first, last, dateOfBirth);
        }

        private void SelectCharacterRequest([FromSource] Player source, int characterId)
        {

        }

        public void CreateCharacter(Player player,string first, string last, string dateOfBirth)
        {
            var charactarData = DatabaseManager.Instance.StartQuery("SELECT id FROM CHARACTERS WHERE firstname = '"+first+"' AND lastname = '"+last+"'");
            while (charactarData.Read())
            {
                Utility.Instance.Log(player.Name+" tried to create a character with the same name as another existing character. Was invalid name, character was not created.");
                return;
            }
            DatabaseManager.Instance.EndQuery(charactarData);

            var tmpCharacter = new Character(); 
            tmpCharacter.FirstName = first;
            tmpCharacter.LastName = last;
            tmpCharacter.DateOfBirth = dateOfBirth;
            tmpCharacter.MaximumInventory = 150;
            tmpCharacter.CurrentInventory = 0;
            tmpCharacter.Inventory = new List<Item>();
            tmpCharacter.Customization = new CharacterCustomization();

            var phoneTaken = true;
            while (phoneTaken)
            {
                tmpCharacter.PhoneNumber = "";
                var rnd = new Random();
                for (int i = 0; i < 10; i++)
                {
                    tmpCharacter.PhoneNumber = tmpCharacter.PhoneNumber + System.Convert.ToString(rnd.Next(0, 9));
                }
                var phoneData = DatabaseManager.Instance.StartQuery("SELECT id FROM CHARACTERS WHERE phonenumber = '" + tmpCharacter.PhoneNumber + "'");
                phoneTaken = false;
                while (phoneData.Read())
                {
                    phoneTaken = true;
                }
                DatabaseManager.Instance.EndQuery(phoneData);
            }
            Utility.Instance.Log(" Character created by " + player.Name + " [ First:" + first + ", Last:" + last + " ]");
            User user = UserManager.Instance.GetUserFromPlayer(player);
            if (user == null)
            {
                Debug.WriteLine("TEST");
            }
            user.Characters.Add(tmpCharacter);
            SelectCharacter(player, first, last);
        }

        public void SelectCharacter(Player player, string first, string last)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            foreach (Character character in user.Characters)
            {
                if (character.FirstName == first && character.LastName == last)
                {

                }
            }
        }

    }
}
