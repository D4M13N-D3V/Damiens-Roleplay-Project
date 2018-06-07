using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using roleplay.Main.Users;

namespace roleplay.Main
{
    public class UserManager : BaseScript
    {
        public static UserManager Instance;
        public UserManager()
        {
            Instance = this;
            SetupEvents();
        }

        private  List<User> _activeUsers = new List<User>();

        private void SetupEvents()
        {
            EventHandlers["roleplay:setFirstSpawn"] += new Action<Player>(Spawned);
        }

        //Handles all the code for first spawn.
        private List<Player> _playersFirstSpawned = new List<Player>();
        private async void Spawned([FromSource] Player source)
        {
            TriggerEvent("roleplay:firstspawn");
            await LoadUser(source);
        }

        private async Task LoadUser(Player player)
        {
            if (player != null)
            {
                var steamid = player.Identifiers["steam"];
                var license = player.Identifiers["license"];
                var tmpUser = new User();
                var data = await DatabaseManager.Instance.StartQueryAsync("SELECT * FROM USERS WHERE steam = '" + steamid + "'");

                while (data.Read())
                {
                    if (data["perms"] != null)
                    {
                        tmpUser.Source = player;
                        tmpUser.License = license;
                        tmpUser.SteamId = steamid;
                        tmpUser.Permissions = Convert.ToInt32(data["perms"]);
                        await DatabaseManager.Instance.EndQueryAsync(data);
                        _activeUsers.Add(tmpUser);
                        if (tmpUser.Permissions > 0)
                        {
                            Admin.Instance.ActiveAdmins.Add(player);
                        }
                        Utility.Instance.Log("Loaded Player [ "+player.Name+" ]");
                        await tmpUser.LoadCharacters();
                        return;
                    }
                }

                await DatabaseManager.Instance.EndQueryAsync(data);

                tmpUser.Source = player;
                tmpUser.License = license;
                tmpUser.SteamId = steamid;
                tmpUser.Permissions = 0;
                _activeUsers.Add(tmpUser);
                Utility.Instance.Log("Player Did Not Exist, Created New User [ " + player.Name + " ]");
                await DatabaseManager.Instance.ExecuteAsync(
                    "INSERT INTO USERS (steam,license,perms,whitelisted,banned) VALUES('" + steamid + "','" + license +
                    "',0,0,0);");
                return;

            }
        }

        public User GetUserFromPlayer(Player player)
        {
            foreach (User user in _activeUsers)
            {
                if (user.Source.Name == player.Name)
                {
                    return user;
                }
            }
            return null;
        }
        public User GetUserFromPlayer(string name)
        {
            foreach (User user in _activeUsers)
            {
                if (user.Source.Name == name)
                {
                    return user;
                }
            }
            return null;
        }

        public void RemoveUserByPlayer(Player player)
        {
            foreach (User user in _activeUsers)
            {
                if (user.Source.Name == player.Name)
                {
                    _activeUsers.Remove(user);
                    return;
                }
            }
        }

    }
}
