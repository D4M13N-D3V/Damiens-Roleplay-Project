using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using roleplay.Main.Users;

namespace roleplay.Main
{
    public class Admin : BaseScript
    {
        public static Admin Instance;
        public Admin()
        {
            Instance = this;
            RefreshBans();
            EventHandlers["playerConnecting"] += new Action<Player, string, CallbackDelegate, IDictionary<string, object>>(CheckBan);
            SetupCommands();
        }

        #region Config
        private const int GiveUntaxedPermissionLevel = 3;
        private const int GiveBankPermissionLevel = 3;
        private const int GiveMoneyPermissionLevel = 3;
        private const int GiveItemPermissionLevel = 3;
        private const int PermissionSettingPermissionLevel = 3;
        private const int BanPermissionLevel = 2;
        private const int InformationPullPermissionLevel = 2;
        private const int KickPermissionLevel = 1;
        private const int SpectatePermissionLevel = 1;
        private const int GetUserFromCharacterNamePermissionLevel = 1;
        #endregion


        private List<Player> _activeAdmins = new List<Player>();
        private List<string> BannedPlayerSteamIds = new List<string>();

        private void AddActiveAdmin(Player player)
        {
            _activeAdmins.Add(player);
        }

        private void RemoveActiveAdmin(Player player)
        {
            _activeAdmins.Remove(player);
        }

        private void BanPlayer(User user, string[] args)
        {
            if (user.Permissions < BanPermissionLevel) { /*Code here to send chat message saying invalid permission level */ return; }
            if (args[1]!= null && args[2] != null)
            {
                var plyList = new PlayerList();
                var ply = plyList[Convert.ToInt32(args[1])];
                var targetUser = UserManager.Instance.GetUserFromPlayer(ply);
                if (targetUser != null)
                {
                    DatabaseManager.Instance.Execute("INSERT INTO BANS (steamid) VALUES('"+targetUser.SteamId+"')");
                    API.DropPlayer(ply.Handle, "You have been banned for "+args[2]+", appeal at http//pirp.site/");
                    RefreshBans();
                    //Chat Message Saying That The Player Has Been Banned.
                }
                else
                {
                    DatabaseManager.Instance.Execute("INSERT INTO BANS (steamid) VALUES('" + args[1] + "')");
                    RefreshBans();
                    //Chate message saying that the player has been banned by his steam id.
                }
            }
            else
            {
                //Chat Message Saying Invalid Parameters
            }
            var player = API.GetPlayerFromIndex(Convert.ToInt16(args[1]));
        }

        private void UnbanPlayer(User user, string[] args)
        {
            if (user.Permissions < BanPermissionLevel) { /*Code here to send chat message saying invalid permission level */ return; }
            if (args[1] != null)
            {
                DatabaseManager.Instance.Execute("DELETE FROM BANS WHERE steamid='"+args[1]+"'");
                //Chat message saying steam id has been removed fromt he bans list.
            }
            else
            {
                //Chat message saying invalid paremeters.
            }
        }

        private void KickPlayer(User user, string[] args)
        {
            if (user.Permissions < KickPermissionLevel) { /*Code here to send chat message saying invalid permission level */ return; }
            if (args[1] != null && args[2] != null)
            {
                var plyList = new PlayerList();
                var ply = plyList[Convert.ToInt32(args[1])];
                if (ply != null)
                {
                    Debug.WriteLine(args[1] + " " + args[2]);
                    API.DropPlayer(ply.Handle, "You have been kicked : "+args[2]);
                    //Chat Message Saying That The Player Has Been Kicked.
                }
            }
            else
            {
                //Chat Message Saying Invalid Parameters
            }
            var player = API.GetPlayerFromIndex(Convert.ToInt16(args[1]));
        }

        private async void RefreshBans()
        {
            BannedPlayerSteamIds.Clear();

            while (DatabaseManager.Instance == null)
            {
                await Delay(0);
            }

            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM BANS");
            while (data.Read())
            {
                BannedPlayerSteamIds.Add(Convert.ToString(data["steamid"]));
                Debug.WriteLine(Convert.ToString(data["steamid"]));
            }
            DatabaseManager.Instance.EndQuery(data);
        }

        private void ShowPerms( User user, string[] args)
        {
            if (user != null )
            {
                TriggerClientEvent("chatMessage", user.Source, "ADMIN", new []{255,0,0} ,"Permission Level Is "+user.Permissions);
            }
        }

        private async void SetupCommands()
        {
            while (CommandManager.Instance == null)
            {
                await Delay(0);
            }
            CommandManager.Instance.AddCommand("perms", ShowPerms);
            CommandManager.Instance.AddCommand("kick", KickPlayer);
            CommandManager.Instance.AddCommand("ban", BanPlayer);
            CommandManager.Instance.AddCommand("unban", UnbanPlayer);
        }

        public void CheckBan([FromSource]Player player, string playerName, CallbackDelegate kickCallback, IDictionary<string, object> deferrals)
        {
            if(BannedPlayerSteamIds.Find(x => x == player.Identifiers["steam"]) != null)
            {
                kickCallback("You are banned! Appeal at http://pirp.site/");
                API.CancelEvent();
                return;
            }
        }

    }
}
