using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using roleplay.Main.Users;

namespace roleplay.Main
{
    public class UserManager:BaseScript
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
            if (PlayersFirstSpawned.Find(x => x==source) == null)
            {
                PlayersFirstSpawned.Add(source);
                TriggerEvent("roleplay:firstspawn");
                LoadUser(source);
            }
        }

        private void LoadUser(Player player)
        {
            if (player != null)
            {
                var steamid = player.Identifiers["steam"];
                var license = player.Identifiers["license"];

                var data = DatabaseManager.Instance.StartQuery("SELECT * FROM USERS WHERE steam = '"+steamid+"'");
                var tmpUser = new User();
                if (data.HasRows)
                {
                    tmpUser.Source = player;
                    tmpUser.License = license;
                    tmpUser.SteamId = steamid;
                    tmpUser.Permissions = Convert.ToInt32(data["perms"]);
                }
                else
                {
                    tmpUser.Source = player;
                    tmpUser.License = license;
                    tmpUser.SteamId = steamid;
                    tmpUser.Permissions = 0;
                    DatabaseManager.Instance.Execute("INSERT INTO USERS (steam,license,perms,whitelisted,banned) VALUES('"+steamid+"','"+license+"',0,0,0");
                }
                DatabaseManager.Instance.EndQuery(data);

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
    