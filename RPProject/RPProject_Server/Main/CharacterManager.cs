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

            EventHandlers["updateCurrentPos"] +=
                new Action<Player, float,float,float>(UpdateCurrentPos);

            EventHandlers["playerDropped"] +=
                new Action<Player>(SavePlayerRequest);
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
            DeleteCharacter(source, first, last);
        }

        private void SavePlayerRequest([FromSource] Player source)
        {
            SavePlayer(source);
        }

        public void SavePlayer(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (user != null)
            {

                var tmpCharacter = user.CurrentCharacter;

                DatabaseManager.Instance.Execute("UPDATE CHARACTERS SET " +
                                                 "cash=" + tmpCharacter.Money.Cash + "," +
                                                 "bank=" + tmpCharacter.Money.Bank + "," +
                                                 "untaxed=" + tmpCharacter.Money.UnTaxed + "," +
                                                 "inventory='" + JsonConvert.SerializeObject(tmpCharacter.Inventory) + "'," +
                                                 "customization='" + JsonConvert.SerializeObject(tmpCharacter.Customization) + "'," +
                                                 "jailtime=" + tmpCharacter.JailTime + "," +
                                                 "hospitaltime=" + tmpCharacter.HospitalTime + "," +
                                                 "pos='" + JsonConvert.SerializeObject(tmpCharacter.Pos) + "'");
                Utility.Instance.Log(" Character saved by " + player.Name + " [ First:" + tmpCharacter.FirstName + ", Last:" + tmpCharacter.LastName + " ]");
                UserManager.Instance.RemoveUserByPlayer(player);
            }
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
            tmpCharacter.Customization.model = "mp_m_freemode_01";
            if (gender == 1)
            {
                tmpCharacter.Customization.model = "mp_f_freemode_01";
            }
            tmpCharacter.Gender = gender;
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
                    user.CurrentCharacter = character;
                    
                    #region Setting up the lists for components
                    var face = new List<int>()
                    {
                        character.Customization.Face.Drawable,
                        character.Customization.Face.Texture,
                        character.Customization.Face.Pallet
                    };

                    var head = new List<int>()
                    {
                        character.Customization.Head.Drawable,
                        character.Customization.Head.Texture,
                        character.Customization.Head.Pallet
                    };

                    var hair = new List<int>()
                    {
                        character.Customization.Hair.Drawable,
                        character.Customization.Hair.Texture,
                        character.Customization.Hair.Pallet
                    };

                    var eyes = new List<int>()
                    {
                        character.Customization.Eyes.Drawable,
                        character.Customization.Eyes.Texture,
                        character.Customization.Eyes.Pallet
                    };

                    var torso = new List<int>()
                    {
                        character.Customization.Torso.Drawable,
                        character.Customization.Torso.Texture,
                        character.Customization.Torso.Pallet
                    };

                    var torso2 = new List<int>()
                    {
                        character.Customization.Torso2.Drawable,
                        character.Customization.Torso2.Texture,
                        character.Customization.Torso2.Pallet
                    };

                    var legs = new List<int>()
                    {
                        character.Customization.Legs.Drawable,
                        character.Customization.Legs.Texture,
                        character.Customization.Legs.Pallet
                    };

                    var hands = new List<int>()
                    {
                        character.Customization.Hands.Drawable,
                        character.Customization.Hands.Texture,
                        character.Customization.Hands.Pallet
                    };

                    var feet = new List<int>()
                    {
                        character.Customization.Feet.Drawable,
                        character.Customization.Feet.Texture,
                        character.Customization.Feet.Pallet
                    };

                    var tasks = new List<int>()
                    {
                        character.Customization.Tasks.Drawable,
                        character.Customization.Tasks.Texture,
                        character.Customization.Tasks.Pallet
                    };

                    var textures = new List<int>()
                    {
                        character.Customization.Textures.Drawable,
                        character.Customization.Textures.Texture,
                        character.Customization.Textures.Pallet
                    };
                    #endregion

                    #region Settings up the lists for props.
                    var hats = new List<int>()
                    {
                        character.Customization.Hats.Drawable,
                        character.Customization.Hats.Texture,
                    };
                    var glasses = new List<int>()
                    {
                        character.Customization.Glasses.Drawable,
                        character.Customization.Glasses.Texture,
                    };
                    var ears = new List<int>()
                    {
                        character.Customization.Ears.Drawable,
                        character.Customization.Ears.Texture,
                    };
                    var watches = new List<int>()
                    {
                        character.Customization.Watches.Drawable,
                        character.Customization.Watches.Texture,
                    };
                    #endregion

                    #region Settings up the lists for props.
                    var blemishes = new List<int>()
                    {
                        character.Customization.Blemishes.Id,
                        character.Customization.Blemishes.Index,
                        character.Customization.Blemishes.PrimaryColor,
                        character.Customization.Blemishes.SecondaryColor,
                        character.Customization.Blemishes.ColorType,
                        character.Customization.Blemishes.Opacity,
                    };

                    var beards = new List<int>()
                    {
                        character.Customization.FacialHair.Id,
                        character.Customization.FacialHair.Index,
                        character.Customization.FacialHair.PrimaryColor,
                        character.Customization.FacialHair.SecondaryColor,
                        character.Customization.FacialHair.ColorType,
                        character.Customization.FacialHair.Opacity,
                    };

                    var eyebrows = new List<int>()
                    {
                        character.Customization.Eyebrows.Id,
                        character.Customization.Eyebrows.Index,
                        character.Customization.Eyebrows.PrimaryColor,
                        character.Customization.Eyebrows.SecondaryColor,
                        character.Customization.Eyebrows.ColorType,
                        character.Customization.Eyebrows.Opacity,
                    };

                    var ageing = new List<int>()
                    {
                        character.Customization.Ageing.Id,
                        character.Customization.Ageing.Index,
                        character.Customization.Ageing.PrimaryColor,
                        character.Customization.Ageing.SecondaryColor,
                        character.Customization.Ageing.ColorType,
                        character.Customization.Ageing.Opacity,
                    };

                    var makeup = new List<int>()
                    {
                        character.Customization.Makeup.Id,
                        character.Customization.Makeup.Index,
                        character.Customization.Makeup.PrimaryColor,
                        character.Customization.Makeup.SecondaryColor,
                        character.Customization.Makeup.ColorType,
                        character.Customization.Makeup.Opacity,
                    };

                    var blush = new List<int>()
                    {
                        character.Customization.Blush.Id,
                        character.Customization.Blush.Index,
                        character.Customization.Blush.PrimaryColor,
                        character.Customization.Blush.SecondaryColor,
                        character.Customization.Blush.ColorType,
                        character.Customization.Blush.Opacity,
                    };

                    var complexion = new List<int>()
                    {
                        character.Customization.Complexion.Id,
                        character.Customization.Complexion.Index,
                        character.Customization.Complexion.PrimaryColor,
                        character.Customization.Complexion.SecondaryColor,
                        character.Customization.Complexion.ColorType,
                        character.Customization.Complexion.Opacity,
                    };

                    var sundamage = new List<int>()
                    {
                        character.Customization.SunDamage.Id,
                        character.Customization.SunDamage.Index,
                        character.Customization.SunDamage.PrimaryColor,
                        character.Customization.SunDamage.SecondaryColor,
                        character.Customization.SunDamage.ColorType,
                        character.Customization.SunDamage.Opacity,
                    };

                    var lipstick = new List<int>()
                    {
                        character.Customization.Lipstick.Id,
                        character.Customization.Lipstick.Index,
                        character.Customization.Lipstick.PrimaryColor,
                        character.Customization.Lipstick.SecondaryColor,
                        character.Customization.Lipstick.ColorType,
                        character.Customization.Lipstick.Opacity,
                    };

                    var moles = new List<int>()
                    {
                        character.Customization.Moles.Id,
                        character.Customization.Moles.Index,
                        character.Customization.Moles.PrimaryColor,
                        character.Customization.Moles.SecondaryColor,
                        character.Customization.Moles.ColorType,
                        character.Customization.Moles.Opacity,
                    };

                    var chesthair = new List<int>()
                    {
                        character.Customization.ChestHair.Id,
                        character.Customization.ChestHair.Index,
                        character.Customization.ChestHair.PrimaryColor,
                        character.Customization.ChestHair.SecondaryColor,
                        character.Customization.ChestHair.ColorType,
                        character.Customization.ChestHair.Opacity,
                    };

                    var bodyblemishes = new List<int>()
                    {
                        character.Customization.BodyBlemishes.Id,
                        character.Customization.BodyBlemishes.Index,
                        character.Customization.BodyBlemishes.PrimaryColor,
                        character.Customization.BodyBlemishes.SecondaryColor,
                        character.Customization.BodyBlemishes.ColorType,
                        character.Customization.BodyBlemishes.Opacity,
                    };

                    #endregion

                    TriggerClientEvent(player,"characterSelected", character.Pos[0], character.Pos[1], character.Pos[2], character.Customization.model );
                    TriggerClientEvent(player, "loadComponents", face, head, hair, eyes, torso, torso2, legs, hands, feet, tasks, textures);
                    TriggerClientEvent(player, "loadProps", hats, glasses, ears, watches);
                    TriggerClientEvent(player, "loadHeadOverlays", blemishes, beards,eyebrows, ageing,makeup,blush,complexion,sundamage,lipstick,moles,chesthair,bodyblemishes);

                }
            }
        }

        public void UpdateCurrentPos([FromSource]Player player, float x, float y, float z)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            Utility.Instance.Log(player.Name+"  posistion for " + user.CurrentCharacter.FirstName+" "+user.CurrentCharacter.LastName+" has been updated!");
            user.CurrentCharacter.Pos = new Vector3(x,y,z);
        }
    }
}
