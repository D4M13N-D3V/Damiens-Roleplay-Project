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
        public static CharacterManager Instance;

        public CharacterManager()
        {
            Instance = this;

            EventHandlers["characterCreationRequest"] +=
                new Action<Player, string, string, string>(NewCharacterRequest);

            EventHandlers["selectCharacterRequest"] +=
                new Action<Player, int>(SelectCharacterRequest);
        }

        private void NewCharacterRequest([FromSource] Player source, string first, string last, string dateOfBirth)
        {
            CreateCharacter(source, first, last, dateOfBirth);
        }

        private void SelectCharacterRequest([FromSource] Player source, int characterId)
        {

        }

        public void CreateCharacter(Player player, string first, string last, string dateOfBirth)
        {
            var charactarData = DatabaseManager.Instance.StartQuery("SELECT id FROM CHARACTERS WHERE firstname = '" + first + "' AND lastname = '" + last + "'");
            while (charactarData.Read())
            {
                Utility.Instance.Log(player.Name + " tried to create a character with the same name as another existing character. Was invalid name, character was not created.");
                DatabaseManager.Instance.EndQuery(charactarData);
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
            tmpCharacter.Money = new CharacterMoney();

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
            

            DatabaseManager.Instance.Execute("INSERT INTO CHARACTERS (steamid,firstname,lastname,dateofbirth,phonenumber,cash,bank,untaxed,inventory,customization,jailtime,hospitaltime) VALUES(" +
                                             "'" + player.Identifiers["steam"] + "'," +
                                             "'" + tmpCharacter.FirstName + "'," +
                                             "'" + tmpCharacter.LastName + "'," +
                                             "'" + tmpCharacter.DateOfBirth + "'," +
                                             "'" + tmpCharacter.PhoneNumber + "'," +
                                             "" + tmpCharacter.Money.Cash + "," +
                                             "" + tmpCharacter.Money.Bank + "," +
                                             "" + tmpCharacter.Money.UnTaxed + "," +
                                             "'" + JsonConvert.SerializeObject(tmpCharacter.Inventory) + "'," +
                                             "'" + JsonConvert.SerializeObject(tmpCharacter.Customization) + "'," +
                                             "0," +
                                             "0" +
                                             ");"); 
            Utility.Instance.Log(" Character created by " + player.Name + " [ First:" + first + ", Last:" + last + " ]");
            User user = UserManager.Instance.GetUserFromPlayer(player);
            user.Characters.Add(tmpCharacter);
            RefreshCharacters(user);
        }

        public void RefreshCharacters(User user)
        {
            List<string> firstNames = new List<string>();
            List<string> lastNames = new List<string>();
            List<string> dateOfBirths = new List<string>();

            foreach (Character character in user.Characters)
            {
                firstNames.Add(character.FirstName);
                lastNames.Add(character.LastName);
                dateOfBirths.Add(character.DateOfBirth);
            }
            TriggerClientEvent(user.Source, "refreshCharacters", firstNames,lastNames,dateOfBirths);
        }

        public void RefreshCharacters(Player player)
        {
            User user = UserManager.Instance.GetUserFromPlayer(player);
            List<string> firstNames = new List<string>();
            List<string> lastNames = new List<string>();
            List<string> dateOfBirths = new List<string>();

            foreach (Character character in user.Characters)
            {
                firstNames.Add(character.FirstName);
                lastNames.Add(character.LastName);  
                dateOfBirths.Add(character.DateOfBirth);
            }
            TriggerClientEvent(user.Source, "refreshCharacters", firstNames, lastNames, dateOfBirths);
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
        public void SelectCharacter(User player, string first, string last)
        {
            ;
            foreach (Character character in player.Characters)
            {
                if (character.FirstName == first && character.LastName == last)
                {

                }
            }
        }
    }
}
