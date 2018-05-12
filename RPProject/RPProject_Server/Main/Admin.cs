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
            if (user.Permissions < BanPermissionLevel)
            {
                TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "Invalid permissions for this command!");
                return;
            }
            if (args[1]!= null && args[2] != null)
            {
                var plyList = new PlayerList();
                var ply = plyList[Convert.ToInt32(args[1])];
                if (ply != null)
                {
                    var targetUser = UserManager.Instance.GetUserFromPlayer(ply);
                    if (targetUser != null)
                    {
                        DatabaseManager.Instance.Execute("INSERT INTO BANS (steamid) VALUES('" + targetUser.SteamId + "')");
                        API.DropPlayer(ply.Handle, "You have been banned for " + args[2] + ", appeal at http//pirp.site/");
                        RefreshBans();
                        TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "You have sucessfully banned " + ply.Name + "!");
                    }
                    else
                    {
                        DatabaseManager.Instance.Execute("INSERT INTO BANS (steamid) VALUES('" + args[1] + "')");
                        RefreshBans();
                        TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "SteamID has been added to the bans list!");
                    }
                }
                else
                {
                    DatabaseManager.Instance.Execute("INSERT INTO BANS (steamid) VALUES('" + args[1] + "')");
                    RefreshBans();
                    TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "SteamID has been added to the bans list!");
                }
            }
            else
            {
                TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "Invalid argument amount, you need to supply a steamid, and an reason!");
            }
            var player = API.GetPlayerFromIndex(Convert.ToInt16(args[1]));
        }

        private void UnbanPlayer(User user, string[] args)
        {
            if (user.Permissions < BanPermissionLevel)
            {
                TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "Invalid permissions for this command!");
                return;
            }
            if (args[1] != null)
            {
                DatabaseManager.Instance.Execute("DELETE FROM BANS WHERE steamid='"+args[1]+"'");
                TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "SteamID has been removed from the bans list!");
            }
            else
            {
                TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "Invalid argument amount, you need to supply a steamid!");
            }
        }

        private void KickPlayer(User user, string[] args)
        {
            if (user.Permissions < KickPermissionLevel)
            {
                TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "Invalid permissions for this command!");
                return;
            }
            if (args[1] != null && args[2] != null)
            {
                var plyList = new PlayerList();
                var ply = plyList[Convert.ToInt32(args[1])];
                if (ply != null)
                {
                    TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "You have sucessfully kicked " + ply.Name + " from the server!");
                    API.DropPlayer(ply.Handle, "You have been kicked for : "+args[2]);
                }
            }
            else
            {
                TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "Invalid amount of paremeters provided. Argument 1 needs to be the player ID, argument 2 needs to be the reason!");
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
            Utility.Instance.Log(" Bans have been refreshed.");
        }

        private void ShowPerms( User user, string[] args)
        {
            if (user != null )
            {
                TriggerClientEvent(user.Source,"chatMessage", "ADMIN", new []{255,0,0} ,"Permission Level Is "+user.Permissions);
            }
        }


        private void GrabPlayerInfo(User user, string[] args)
        {
            if (user.Permissions < InformationPullPermissionLevel)
            {
                TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "Invalid permissions for this command!");
                return;
            }
            if (user != null && args[1] != null)
            {
                var plyList = new PlayerList();
                var ply = plyList[Convert.ToInt32(args[1])];
                if (ply != null)
                {
                    var targetUser = UserManager.Instance.GetUserFromPlayer(ply);
                    if (targetUser != null)
                    {
                        TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "Grabbing information for "+ply.Name+"'s current character.");
                        TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "First Name : " + user.CurrentCharacter.FirstName + ", Last Name : " + user.CurrentCharacter.LastName + ", DoB : " + user.CurrentCharacter.DateOfBirth);
                        TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "Cash : "+user.CurrentCharacter.Money.Cash+", Bank : "+user.CurrentCharacter.Money.Bank+", Untaxed "+user.CurrentCharacter.Money.UnTaxed);
                    }
                    else
                    {
                        TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "Invalid Player.");
                    }
                }
                else
                {
                    TriggerClientEvent(user.Source, "chatMessage", "ADMIN", new[] { 255, 0, 0 }, "Invalid Player ID.");
                }
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
            CommandManager.Instance.AddCommand("info", GrabPlayerInfo);
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
