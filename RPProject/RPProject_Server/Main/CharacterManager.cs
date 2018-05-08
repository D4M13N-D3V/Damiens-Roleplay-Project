using System;
using System.Collections.Generic;
using System.Dynamic;
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
                new Action<Player, string, string, string, int>(NewCharacterRequest);

            EventHandlers["selectCharacterRequest"] +=
                new Action<Player, string, string>(SelectCharacterRequest);

            EventHandlers["deleteCharacterRequest"] +=
                new Action<Player, string, string>(DeleteCharacterRequest);
        }

        private void NewCharacterRequest([FromSource] Player source, string first, string last, string dateOfBirth,int gender)
        {
            CreateCharacter(source, first, last, dateOfBirth,gender);
        }

        private void SelectCharacterRequest([FromSource] Player source, string first, string last)
        {
            SelectCharacter(source,first,last);
        }

        private void DeleteCharacterRequest([FromSource] Player source, string first, string last)
        {
            DeleteCharacter(source,first,last);
        }

        public void CreateCharacter(Player player, string first, string last, string dateOfBirth, int gender)
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
            tmpCharacter.Pos = new Vector3( 165.34895324707f, -1037.4916992188f, 29.323148727417f);
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
            

            DatabaseManager.Instance.Execute("INSERT INTO CHARACTERS (steamid,gender,firstname,lastname,dateofbirth,phonenumber,cash,bank,untaxed,inventory,customization,jailtime,hospitaltime,pos) VALUES(" +
                                             "'" + player.Identifiers["steam"] + "'," +
                                             ""+ gender + ","+
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
                                             "0," +
                                             "'"+JsonConvert.SerializeObject(tmpCharacter.Pos)+"');"); 
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
            List<int> genders = new List<int>();

            foreach (Character character in user.Characters)
            {
                firstNames.Add(character.FirstName);
                lastNames.Add(character.LastName);
                dateOfBirths.Add(character.DateOfBirth);
                genders.Add(character.Gender);
            }
            TriggerClientEvent(user.Source, "refreshCharacters", firstNames,lastNames,dateOfBirths,genders);
        }

        public void RefreshCharacters(Player player)
        {
            User user = UserManager.Instance.GetUserFromPlayer(player);
            List<string> firstNames = new List<string>();
            List<string> lastNames = new List<string>();
            List<string> dateOfBirths = new List<string>();
            List<int> genders = new List<int>();

            foreach (Character character in user.Characters)
            {
                firstNames.Add(character.FirstName);
                lastNames.Add(character.LastName);
                dateOfBirths.Add(character.DateOfBirth);
                genders.Add(character.Gender);
            }
            TriggerClientEvent(user.Source, "refreshCharacters", firstNames, lastNames, dateOfBirths, genders);
        }

        public void DeleteCharacter(Player player, string first, string last)
        {
            Debug.WriteLine("SELECT id FROM CHARACTERS WHERE steamid = '" + player.Identifiers["steam"] + "' AND firstname = '" + first + "' AND lastname = '" + last + "'");
            var charactarData = DatabaseManager.Instance.StartQuery("SELECT id FROM CHARACTERS WHERE steamid = '"+player.Identifiers["steam"]+"' AND firstname = '" + first + "' AND lastname = '" + last + "'");
            while (charactarData.Read())
            {
                DatabaseManager.Instance.EndQuery(charactarData);
                DatabaseManager.Instance.Execute("DELETE FROM CHARACTERS  WHERE steamid = '" + player.Identifiers["steam"] + "' AND firstname = '" + first + "' AND lastname = '" + last + "'");
                var user = UserManager.Instance.GetUserFromPlayer(player);
                foreach (var character in user.Characters)
                {
                    if (character.FirstName == first && character.LastName == last)
                    {
                        Utility.Instance.Log(" Character Deleted ( "+first+" "+last+" )");
                        user.Characters.Remove(character);
                        RefreshCharacters(player);
                        return;
                    }
                }
            }
            DatabaseManager.Instance.EndQuery(charactarData);

        }

        public void SelectCharacter(Player player, string first, string last)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            foreach (Character character in user.Characters)
            {
                if (character.FirstName == first && character.LastName == last)
                {
                    
                    TriggerClientEvent(player,"characterSelected", character.Pos[0], character.Pos[1], character.Pos[2]);
                }
            }
        }
    }
}
