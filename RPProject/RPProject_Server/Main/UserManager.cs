using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using roleplay;
using roleplay.Main.Users;
using roleplay.Main.Users.CharacterClasses;
using roleplay.Main.Users.Customization;

namespace roleplay.Main
{
    public class UserManager : BaseScript
    {
        private static UserManager instance;
        public UserManager()
        {
            SetupEvents();
        }
        public static UserManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserManager();
                }
                return instance;
            }
        }

        public List<User> ActiveUsers = new List<User>();

        private void SetupEvents()
        {
            EventHandlers["roleplay:setFirstSpawn"] += new Action<Player>(Spawned);
        }

        //Handles all the code for first spawn.
        private List<Player> PlayersFirstSpawned = new List<Player>();
        private void Spawned([FromSource] Player source)
        {
            TriggerEvent("roleplay:firstspawn");
            LoadUser(source);
        }

        private void LoadUser(Player player)
        {
            if (player != null)
            {
                var steamid = player.Identifiers["steam"];
                var license = player.Identifiers["license"];
                var tmpUser = new User();
                var data = DatabaseManager.Instance.StartQuery("SELECT * FROM USERS WHERE steam = '" + steamid + "'");

                while (data.Read())
                {
                    if (data["perms"] != null)
                    {
                        tmpUser.Source = player;
                        tmpUser.License = license;
                        tmpUser.SteamId = steamid;
                        tmpUser.Permissions = Convert.ToInt32(data["perms"]);
                        Utility.Instance.Log("Loaded Player [ "+player.Name+" ]");
                        DatabaseManager.Instance.EndQuery(data);
                        tmpUser.LoadCharacters();
                        return;
                    }
                }

                DatabaseManager.Instance.EndQuery(data);

                tmpUser.Source = player;
                tmpUser.License = license;
                tmpUser.SteamId = steamid;
                tmpUser.Permissions = 0;
                Utility.Instance.Log("Player Did Not Exist, Created New User [ " + player.Name + " ]");
                DatabaseManager.Instance.Execute(
                    "INSERT INTO USERS (steam,license,perms,whitelisted,banned) VALUES('" + steamid + "','" + license +
                    "',0,0,0);");
                DatabaseManager.Instance.EndQuery(data);
                return;

            }
        }

        public User GetUserFromPlayer(Player player)
        {
            foreach (User user in ActiveUsers)
            {
                if (user.Source == player)
                {
                    return user;
                }
            }
            return null;
        }


    }
}
