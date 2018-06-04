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

            EventHandlers["saveTattoos"] += new Action<Player, List<dynamic>, List<dynamic>>(SaveTattoos);

            EventHandlers["saveProps"] +=
                new Action<Player, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic> >(SaveProps);

            EventHandlers["saveComponents"] +=
                new Action<Player, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>,
                    List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>>(
                    SaveComponents);
            EventHandlers["saveHeadOverlays"] +=
                new Action<Player, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>,
                    List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>>(
                    SaveHeadOverlays);
            
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

            EventHandlers["RefreshClothes"] +=
                new Action<Player>(RefreshClothes);

            EventHandlers["RequestID"] += new Action<Player, int>(ShowIdRequest);
            EventHandlers["FingerPrintScannerRequest"] += new Action<Player, int>(FingerPrintScannerRequest);
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
            TriggerClientEvent(player, "RequestReset");
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (user != null && user.CurrentCharacter!=null)
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
                                                 "pos='" + JsonConvert.SerializeObject(tmpCharacter.Pos) + "' WHERE firstname='" + tmpCharacter.FirstName + "' AND lastname='" + tmpCharacter.LastName+"' AND steamid = '"+user.SteamId+"';");
                Utility.Instance.Log(" Character saved by " + player.Name + " [ First:" + tmpCharacter.FirstName + ", Last:" + tmpCharacter.LastName + " ]");
                UserManager.Instance.RemoveUserByPlayer(player);
            }
        }

        public void CreateCharacter(Player player, string first, string last, string dateOfBirth, int gender)
        {
            TriggerClientEvent(player, "RequestReset");
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
            tmpCharacter.Money.Bank = 250000;
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
            TriggerClientEvent(player, "RequestReset");
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
            TriggerClientEvent(player, "RequestReset");
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

                    var accessories = new List<int>()
                    {
                        character.Customization.Accessories.Drawable,
                        character.Customization.Accessories.Texture,
                        character.Customization.Accessories.Pallet
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
                    TriggerClientEvent(player, "loadComponents", face, head, hair, eyes, torso, torso2, legs, hands, feet, tasks, textures, accessories);
                    TriggerClientEvent(player, "loadProps", hats, glasses, ears, watches);
                    TriggerClientEvent(player, "loadHeadOverlays", blemishes, beards,eyebrows, ageing,makeup,blush,complexion,sundamage,lipstick,moles,chesthair,bodyblemishes);

                    var inv = new List<ExpandoObject>();

                    foreach (Item item in character.Inventory)
                    {
                        dynamic obj = new ExpandoObject();
                        obj.Id = item.Id;
                        obj.Name = item.Name;
                        obj.Description = item.Description;
                        obj.BuyPrice = item.BuyPrice;
                        obj.SellPrice = item.SellPrice;
                        obj.Weight = item.Weight;
                        obj.Illegal = false;
                        inv.Add(obj);
                    }

                    if (user.CurrentCharacter.JailTime > 0)
                    {
                        TriggerClientEvent("Jail", user.CurrentCharacter.JailTime);
                    }

                    if (user.CurrentCharacter.HospitalTime > 0)
                    {
                        TriggerClientEvent("Hospital", user.CurrentCharacter.HospitalTime);
                    }
                    TriggerClientEvent(player, "RefreshInventoryItems", inv,character.Money.Cash,character.Money.Bank,character.Money.UnTaxed,character.MaximumInventory,character.CurrentInventory);
                    var cols = new List<string>();
                    var tats = new List<string>();

                    foreach (CustomizationDecoration tat in character.Customization.Tattoos)
                    {
                        cols.Add(tat.Collection);
                        tats.Add(tat.Overlay);
                    }

                    TriggerClientEvent(player, "loadTattoos", cols,tats);
                }
            }
        }


        public void RefreshClothes([FromSource] Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var character = user.CurrentCharacter;
            #region Setting up the lists for components
            var face = new List<int>()
                    {
                        character.Customization.Face.Drawable,
                        character.Customization.Face.Texture,
                        character.Customization.Face.Pallet
                    };

            var accessories = new List<int>()
                    {
                        character.Customization.Accessories.Drawable,
                        character.Customization.Accessories.Texture,
                        character.Customization.Accessories.Pallet
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
            
            TriggerClientEvent(player, "loadComponents", face, head, hair, eyes, torso, torso2, legs, hands, feet, tasks, textures, accessories);
            TriggerClientEvent(player, "loadProps", hats, glasses, ears, watches);
            TriggerClientEvent(player, "loadHeadOverlays", blemishes, beards, eyebrows, ageing, makeup, blush, complexion, sundamage, lipstick, moles, chesthair, bodyblemishes);
        }

        public void SaveProps([FromSource]Player source, List<dynamic> hats, List<dynamic> glasses, List<dynamic> ears, List<dynamic> wrists)
        {
            var user = UserManager.Instance.GetUserFromPlayer(source);
            if (user != null)
            {
                var custom = user.CurrentCharacter.Customization;
                custom.Glasses.Drawable = glasses[0];
                custom.Glasses.Texture = glasses[1];

                custom.Hats.Drawable = hats[0];
                custom.Hats.Texture = hats[1];

                custom.Ears.Drawable = ears[0];
                custom.Ears.Texture = ears[1];

                custom.Watches.Drawable = wrists[0];
                custom.Watches.Texture = wrists[1];
            }
        }

        public void SaveComponents([FromSource]Player source, List<dynamic> face, List<dynamic> head, List<dynamic> hair, List<dynamic> eyes, List<dynamic> torso,
            List<dynamic> torso2, List<dynamic> legs, List<dynamic> hands, List<dynamic> feet, List<dynamic> tasks, List<dynamic> textures, List<dynamic> accessories)
        {
            var user = UserManager.Instance.GetUserFromPlayer(source);
            if (user != null)
            {
                var custom = user.CurrentCharacter.Customization;
                custom.Face.Drawable = face[0];
                custom.Face.Texture = face[1];

                custom.Head.Drawable = head[0];
                custom.Head.Texture = head[1];

                custom.Hair.Drawable = hair[0];
                custom.Hair.Texture = hair[1];

                custom.Eyes.Drawable = eyes[0];
                custom.Eyes.Texture = eyes[1];

                custom.Torso.Drawable = torso[0];
                custom.Torso.Texture = torso[1];

                custom.Torso2.Drawable = torso2[0];
                custom.Torso2.Texture = torso2[1];

                custom.Legs.Drawable = legs[0];
                custom.Legs.Texture = legs[1];

                custom.Hands.Drawable = hands[0];
                custom.Hands.Texture = hands[1];

                custom.Feet.Drawable = feet[0];
                custom.Feet.Texture = feet[1];

                custom.Tasks.Drawable = tasks[0];
                custom.Tasks.Texture = tasks[1];

                custom.Textures.Drawable = textures[0];
                custom.Textures.Texture = textures[1];

                custom.Accessories.Drawable = accessories[0];
                custom.Accessories.Texture = accessories[1];
                Utility.Instance.Log(source.Name + " has saved thier characters clothes ( " + user.CurrentCharacter.FirstName + " " + user.CurrentCharacter.LastName + ")");
            }
        }

        public void SaveHeadOverlays([FromSource]Player source, List<dynamic> blemishes, List<dynamic> beards, List<dynamic> eyebrows, List<dynamic> aging, List<dynamic> makeup,
            List<dynamic> blush, List<dynamic> complexion, List<dynamic> sundamage, List<dynamic> lipstick, List<dynamic> moles, List<dynamic> chesthair, List<dynamic> bodyblemishes)
        {
            var user = UserManager.Instance.GetUserFromPlayer(source);
            if (user != null)
            {
                var custom = user.CurrentCharacter.Customization;
                custom.Blemishes.Index = blemishes[1];
                custom.Blemishes.PrimaryColor = blemishes[2];
                custom.Blemishes.PrimaryColor = blemishes[3];

                custom.FacialHair.Index = beards[1];
                custom.FacialHair.PrimaryColor = beards[2];
                custom.FacialHair.PrimaryColor = beards[3];

                custom.Eyebrows.Index = eyebrows[1];
                custom.Eyebrows.PrimaryColor = eyebrows[2];
                custom.Eyebrows.PrimaryColor = eyebrows[3];

                custom.Ageing.Index = aging[1];
                custom.Ageing.PrimaryColor = aging[2];
                custom.Ageing.PrimaryColor = aging[3];

                custom.Makeup.Index = makeup[1];
                custom.Makeup.PrimaryColor = makeup[2];
                custom.Makeup.PrimaryColor = makeup[3];

                custom.Blush.Index = blush[1];
                custom.Blush.PrimaryColor = blush[2];
                custom.Blush.PrimaryColor = blush[3];

                custom.Complexion.Index = complexion[1];
                custom.Complexion.PrimaryColor = complexion[2];
                custom.Complexion.PrimaryColor = complexion[3];

                custom.Complexion.Index = sundamage[1];
                custom.Complexion.PrimaryColor = sundamage[2];
                custom.Complexion.PrimaryColor = sundamage[3];

                custom.Complexion.Index = lipstick[1];
                custom.Complexion.PrimaryColor = lipstick[2];
                custom.Complexion.PrimaryColor = lipstick[3];

                custom.Complexion.Index = moles[1];
                custom.Complexion.PrimaryColor = moles[2];
                custom.Complexion.PrimaryColor = moles[3];

                custom.ChestHair.Index = chesthair[1];
                custom.ChestHair.PrimaryColor = chesthair[2];
                custom.ChestHair.PrimaryColor = chesthair[3];

                custom.BodyBlemishes.Index = bodyblemishes[1];
                custom.BodyBlemishes.PrimaryColor = bodyblemishes[2];
                custom.BodyBlemishes.PrimaryColor = bodyblemishes[3];

                Utility.Instance.Log(source.Name + " has saved thier characters clothes ( " + user.CurrentCharacter.FirstName + " " + user.CurrentCharacter.LastName + ")");
            }
        }

        public void SaveTattoos([FromSource] Player player, List<dynamic> cols, List<dynamic> tattoos)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            user.CurrentCharacter.Customization.Tattoos.Clear();

            for(int i = 0; i<cols.Count; i++)
            {
                Utility.Instance.Log(cols[i]+" "+tattoos[i]);
                user.CurrentCharacter.Customization.Tattoos.Add(new CustomizationDecoration(cols[i], tattoos[i]));
            }
        }

        public void UpdateCurrentPos([FromSource]Player player, float x, float y, float z)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            //Utility.Instance.Log(player.Name+"  posistion for " + user.CurrentCharacter.FirstName+" "+user.CurrentCharacter.LastName+" has been updated!");
            user.CurrentCharacter.Pos = new Vector3(x,y,z);
        }

        public void ShowIdRequest([FromSource] Player player, int targetPlayerId)
        {
            var plyList = new PlayerList();
            var targetPlayer = plyList[targetPlayerId];
            var targetUser = UserManager.Instance.GetUserFromPlayer(targetPlayer);
            var targetCharacter = targetUser.CurrentCharacter;
            string gender = "Male";
            if (targetCharacter.Gender == 1)
            {
                gender = "Female";
            }
            RPCommands.Instance.ActionCommand("Holds up thier identification that reads  :  " +
                                              "\n^1 First Name - ^7 " + targetCharacter.FirstName + "" +
                                              "\n^1 Last Name - ^7 " + targetCharacter.LastName + "" +
                                              "\n^1 Date of Birth  - ^7 " + targetCharacter.DateOfBirth + "" +
                                              "\n^1 Gender  - ^7 " + gender + "" +
                                              "\n^1 Convicted Felon - ^7 INVALID", targetPlayer);
        }

        public void FingerPrintScannerRequest([FromSource] Player player, int targetPlayerId)
        {
            var plyList = new PlayerList();
            var targetPlayer = plyList[targetPlayerId];
            var targetUser = UserManager.Instance.GetUserFromPlayer(targetPlayer);
            var targetCharacter = targetUser.CurrentCharacter;
            string gender = "Male";
            if (targetCharacter.Gender == 1)
            {
                gender = "Female";
            }
            RPCommands.Instance.ActionCommand(" presses "+targetCharacter.FullName+" fingers against the digital finger print scanner and a profile pops up :  " +
                                              "\n^1 First Name - ^7 " + targetCharacter.FirstName + "" +
                                              "\n^1 Last Name - ^7 " + targetCharacter.LastName + "" +
                                              "\n^1 Date of Birth  - ^7 " + targetCharacter.DateOfBirth + "" +
                                              "\n^1 Gender  - ^7 " + gender + "" +
                                              "\n^1 Convicted Felon - ^7 INVALID", targetPlayer);
        }

    }
}






