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


        public List<Character> Characters = new List<Character>();

        public Character CurrentCharacter;

        public void LoadCharacters()
        {
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM CHARACTERS WHERE steamid ='"+Source.Identifiers["steam"]+"'");
            while (data.Read())
            {
                var tmpCharacter = new Character();
                tmpCharacter.Customization = JsonConvert.DeserializeObject<CharacterCustomization>(System.Convert.ToString(data["customization"]));
                tmpCharacter.DateOfBirth = System.Convert.ToString(data["dateofbirth"]);
                tmpCharacter.FirstName = System.Convert.ToString(data["firstname"]);
                tmpCharacter.LastName = System.Convert.ToString(data["lastname"]);
                tmpCharacter.PhoneNumber = System.Convert.ToString(data["phonenumber"]);
                tmpCharacter.JailTime = System.Convert.ToInt32(data["jailtime"]);
                tmpCharacter.Gender = System.Convert.ToInt32(data["gender"]);
                tmpCharacter.HospitalTime = System.Convert.ToInt32(data["hospitaltime"]);
                tmpCharacter.Money = new CharacterMoney();
                tmpCharacter.Money.Cash = System.Convert.ToInt32(data["cash"]);
                tmpCharacter.Money.Bank = System.Convert.ToInt32(data["bank"]);
                tmpCharacter.Money.UnTaxed = System.Convert.ToInt32(data["untaxed"]);
                tmpCharacter.MDTFlags =
                    JsonConvert.DeserializeObject<Dictionary<FlagTypes, bool>>(System.Convert.ToString(data["flags"]));
                tmpCharacter.Pos = JsonConvert.DeserializeObject<Vector3>(System.Convert.ToString(data["pos"]));
                tmpCharacter.Inventory = JsonConvert.DeserializeObject<List<Item>>(System.Convert.ToString((data["inventory"])));
                tmpCharacter.MaximumInventory = 250;
                var Items = tmpCharacter.Inventory;
                var totalWeight = 0;
                foreach (Item item in Items)
                {
                    totalWeight = totalWeight + item.Weight;
                }
                tmpCharacter.CurrentInventory = totalWeight;
                Characters.Add(tmpCharacter);
            }   
            CurrentCharacter = new Character();
            Utility.Instance.Log("Characters Have Been Loaded For "+Source.Name);
            DatabaseManager.Instance.EndQuery(data);
            CharacterManager.Instance.RefreshCharacters(this);
        }

    }
}
