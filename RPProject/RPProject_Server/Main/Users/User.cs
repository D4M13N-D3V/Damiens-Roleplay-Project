using System.Collections.Generic;
using CitizenFX.Core;
using Newtonsoft.Json;
using roleplay.Main.Users.CharacterClasses;
using roleplay.Main.Users.Customization;

namespace roleplay.Main.Users
{
    public class User
    {
        public Player Source;
        public string SteamId;
        public string License;
        public int Permissions;
        public bool Whitelisted;


        public List<Character> Characters;

        public Character CurrentCharacter;

        public void LoadCharacters()
        {
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM CHARACTERS WHERE steamid ='"+Source.Identifiers["steam"]+"'");
            while (data.Read())
            {
                var tmpCharacter = new Character();
                tmpCharacter.User = this;
                tmpCharacter.Customization = JsonConvert.DeserializeObject<CharacterCustomization>(System.Convert.ToString(data["customization"]));
                tmpCharacter.DateOfBirth = System.Convert.ToString(data["dateofbirth"]);
                tmpCharacter.FirstName = System.Convert.ToString(data["firstname"]);
                tmpCharacter.LastName = System.Convert.ToString(data["lastname"]);
                tmpCharacter.PhoneNumber = System.Convert.ToString(data["phonenumber"]);
                tmpCharacter.JailTime = System.Convert.ToInt32(data["jailtime"]);
                tmpCharacter.HospitalTime = System.Convert.ToInt32(data["hospitaltime"]);
                tmpCharacter.Money = new CharacterMoney();
                tmpCharacter.Money.Cash = System.Convert.ToInt32(data["cash"]);
                tmpCharacter.Money.Bank = System.Convert.ToInt32(data["bank"]);
                tmpCharacter.Money.UnTaxed = System.Convert.ToInt32(data["untaxed"]);
                Characters.Add(tmpCharacter);
            }   
            Utility.Instance.Log("Characters Have Been Loaded For "+Source.Name);
            DatabaseManager.Instance.EndQuery(data);
        }

    }
}
