using System.Collections.Generic;
using CitizenFX.Core;
using roleplay.Main.Users.CharacterClasses;
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
    }
}
