using System.Collections.Generic;
using roleplay.Main.Users.CharacterClasses;
namespace roleplay.Main.Users
{
    public class User
    {
        public int Source;
        public string SteamId;
        public string License;

        public List<Character> Characters;
        public Character CurrentCharacter;
    }
}
