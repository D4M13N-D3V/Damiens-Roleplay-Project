using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
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
        private const int CarSpawnPermissionLevel = 3;
        private const int PermissionSettingPermissionLevel = 3;
        private const int BanPermissionLevel = 2;
        private const int InformationPullPermissionLevel = 2;
        private const int KickPermissionLevel = 1;
        private const int GotoPermissionLevel = 2;
        private const int BringPermissionLevel = 2;
        private const int SpectatePermissionLevel = 1;
        #endregion


        public List<Player> ActiveAdmins = new List<Player>();
        private List<string> BannedPlayerSteamIds = new List<string>();

        private void AddActiveAdmin(Player player)
        {
            ActiveAdmins.Add(player);
        }

        private void RemoveActiveAdmin(Player player)
        {
            ActiveAdmins.Remove(player);
        }

        private void BanPlayer(User user, string[] args)
        {
            if (user.Permissions < BanPermissionLevel)
            {
                Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid permissions for this command!!", 255, 0, 0);
                return;
            }
            if (args.Length >= 3)
            {
                var plyList = new PlayerList();
                var id = 0;
                if (!Int32.TryParse(args[1], out id))
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid parameters", 255, 0, 0);
                    return;
                }


                var targetPlayerList = plyList.Where(x => Convert.ToInt32(x.Handle) == id).ToList();
                if (!targetPlayerList.Any()) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
                var ply = targetPlayerList[0];
                if (ply != null)
                {
                    var targetUser = UserManager.Instance.GetUserFromPlayer(ply);
                    if (targetUser != null)
                    {
                        DatabaseManager.Instance.Execute("INSERT INTO BANS (steamid) VALUES('" + targetUser.SteamId + "')");
                        API.DropPlayer(ply.Handle, "You have been banned for " + args[2] + ", appeal at http//pirp.site/");
                        RefreshBans();
                        Utility.Instance.SendChatMessage(ply, "[Admin]", "You have sucessfully banned " + ply.Name + "!",255,0,0);
                    }
                    else
                    {
                        DatabaseManager.Instance.Execute("INSERT INTO BANS (steamid) VALUES('" + args[1] + "')");
                        RefreshBans();
                        Utility.Instance.SendChatMessage(ply, "[Admin]", "SteamID has been added to the bans list!", 255, 0, 0);
                    }
                }
                else
                {
                    DatabaseManager.Instance.Execute("INSERT INTO BANS (steamid) VALUES('" + args[1] + "')");
                    RefreshBans();
                    Utility.Instance.SendChatMessage(ply, "[Admin]", "SteamID has been added to the bans list!", 255, 0, 0);
                }
            }
            else
            {
                Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid argument amount, you need to supply a steamid/playerid, and an reason!", 255, 0, 0);
            }
            var player = API.GetPlayerFromIndex(Convert.ToInt16(args[1]));
        }

        private void UnbanPlayer(User user, string[] args)
        {
            if (user.Permissions < BanPermissionLevel)
            {
                Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid permissions for this command!!", 255, 0, 0);
                return;
            }
            if (args.Length >= 2)
            {
                DatabaseManager.Instance.Execute("DELETE FROM BANS WHERE steamid='"+args[1]+"'");
                Utility.Instance.SendChatMessage(user.Source, "[Admin]", "SteamID has been removed from the bans list!", 255, 0, 0);
            }
            else
            {
                Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid argument amount, you need to supply a steamid!", 255, 0, 0);
            }
        }

        private void KickPlayer(User user, string[] args)
        {
            if (user.Permissions < KickPermissionLevel)
            {
                Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid permissions for this command!!", 255, 0, 0);
                return;
            }
            if (args.Length >= 3)
            {
                var plyList = new PlayerList();
                var id = 0;
                if (!Int32.TryParse(args[1], out id))
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid parameters", 255, 0, 0);
                    return;
                }
                var targetPlayerList = plyList.Where(x => Convert.ToInt32(x.Handle) == id).ToList();
                if (!targetPlayerList.Any()) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
                var ply = targetPlayerList[0];
                if (ply != null)
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Admin]", "You have sucessfully kicked " + ply.Name + " from the server!", 255, 0, 0);
                    API.DropPlayer(ply.Handle, "You have been kicked for : "+args[2]);
                }
            }
            else
            {
                Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid amount of paremeters provided. Argument 1 needs to be the player ID, argument 2 needs to be the reason!", 255, 0, 0);
            }
            var player = API.GetPlayerFromIndex(Convert.ToInt16(args[1]));
        }

        private void UpdatePerms(User user, string[] args)
        {
            if (user.Permissions < PermissionSettingPermissionLevel)
            {
                Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid permissions for this command!!", 255, 0, 0);
                return;
            }
            if (args.Length >= 3)
            {
                var plyList = new PlayerList();
                var id = 0;
                if (!Int32.TryParse(args[1], out id))
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid parameters", 255, 0, 0);
                    return;
                }
                var targetPlayerList = plyList.Where(x => Convert.ToInt32(x.Handle) == id).ToList();
                if (!targetPlayerList.Any()) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
                var ply = targetPlayerList[0];
                if (ply != null)
                {
                    var targetUser = UserManager.Instance.GetUserFromPlayer(ply);
                    if (targetUser != null)
                    {
                        DatabaseManager.Instance.Execute("UPDATE SET perms="+args[2]+" WHERE steamid='"+targetUser.SteamId+"';");
                        targetUser.Permissions = Convert.ToInt32(args[2]);
                        Utility.Instance.SendChatMessage(user.Source, "[Admin]", "You have sucessfully set permissions for  " + ply.Name + " to " + args[2] + "!", 255, 0, 0);
                        Utility.Instance.SendChatMessage(targetUser.Source, "[Admin]", "Youer permissions have been set to " + args[2] + "!", 255, 0, 0);
                    }
                    else
                    {
                        Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid user!", 255, 0, 0);
                    }
                }
                else
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid player id!", 255, 0, 0);
                }
            }
            else
            {
                Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid argument amount, you need to supply a player ID and permissions level!", 255, 0, 0);
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
                Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Permission Level Is " + user.Permissions, 255, 0, 0);
            }
        }


        private void GrabPlayerInfo(User user, string[] args)
        {
            if (user.Permissions < InformationPullPermissionLevel)
            {
                Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid permissions for this command!!", 255, 0, 0);
                return;
            }
            if (user != null && args[1] != null)
            {
                var plyList = new PlayerList();
                var id = 0;
                if (!Int32.TryParse(args[1], out id))
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid parameters", 255, 0, 0);
                    return;
                }
                var targetPlayerList = plyList.Where(x => Convert.ToInt32(x.Handle) == id).ToList();
                if (!targetPlayerList.Any()) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
                var ply = targetPlayerList[0];
                if (ply != null)
                {
                    var targetUser = UserManager.Instance.GetUserFromPlayer(ply);
                    if (targetUser != null)
                    {
                        Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Grabbing information for " + ply.Name + "'s current character.", 255, 0, 0);
                        Utility.Instance.SendChatMessage(user.Source, "[Admin]", "First Name : " + user.CurrentCharacter.FirstName + ", Last Name : " + user.CurrentCharacter.LastName + ", DoB : " + user.CurrentCharacter.DateOfBirth, 255, 0, 0);
                        Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Cash : " + user.CurrentCharacter.Money.Cash + ", Bank : " + user.CurrentCharacter.Money.Bank + ", Untaxed " + user.CurrentCharacter.Money.UnTaxed, 255, 0, 0);
                    }
                    else
                    {
                        Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid Player!", 255, 0, 0);
                    }
                }
                else
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid Player ID!", 255, 0, 0);
                }
            }
        }

        private void SpawnCar(User user, string[] args)
        {
            if (user.Permissions < CarSpawnPermissionLevel)
            {
                Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid permissions for this command!!", 255, 0, 0);
                return;
            }
            if (args.Length >= 2)
            {
                TriggerClientEvent(user.Source,"AdminSpawnCar",args[1]);
            }
        }

        private void BringPlayer(User user, string[] args)
        {
            if (user.Permissions < BringPermissionLevel)
            {
                Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid permissions for this command!!", 255, 0, 0);
                return;
            }

            if (args.Length >= 2)
            {
                var plyList = new PlayerList();
                var id = 0;
                if (!Int32.TryParse(args[1], out id))
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid parameters", 255, 0, 0);
                    return;
                }
                var targetPlayerList = plyList.Where(x => Convert.ToInt32(x.Handle) == id).ToList();
                if (!targetPlayerList.Any()) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
                var ply = targetPlayerList[0];
                TriggerClientEvent("TeleportToPlayer", ply,user.Source.Handle);
                Utility.Instance.SendChatMessageAll("[Admin]", user.Source.Name + " has teleported to " + ply.Name, 255, 0, 0);
            }
        }

        private void GotoPlayer(User user, string[] args)
        {
            if (user.Permissions < GotoPermissionLevel)
            {
                Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid permissions for this command!!", 255, 0, 0);
                return;
            }

            if (args.Length >= 2)
            {
                var plyList = new PlayerList();
                var id = 0;
                if (!Int32.TryParse(args[1], out id))
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid parameters", 255, 0, 0);
                    return;
                }
                var targetPlayerList = plyList.Where(x => Convert.ToInt32(x.Handle) == id).ToList();
                if (!targetPlayerList.Any()) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
                var ply = targetPlayerList[0];
                TriggerClientEvent("TeleportToPlayer",user.Source, ply.Handle);
                Utility.Instance.SendChatMessageAll("[Admin]", user.Source.Name + " has teleported to " + ply.Name, 255, 0, 0);
            }
        }

        private void SavePos(User user, string[] args)
        {
            if (user.Permissions < 1)
            {
                Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid permissions for this command!!", 255, 0, 0);
                return;
            }

            var posFile = API.LoadResourceFile(API.GetCurrentResourceName(), "posistions");
            if (posFile == null)
            {
                var posistions = new List<Vector3>()
                {
                    new Vector3(user.CurrentCharacter.Pos.X, user.CurrentCharacter.Pos.Y, user.CurrentCharacter.Pos.Z)
                };
                API.SaveResourceFile(API.GetCurrentResourceName(), "posistions", JsonConvert.SerializeObject(posistions), -1);
            }
            else
            {
                List<Vector3> posistions = JsonConvert.DeserializeObject<List<Vector3>>(posFile);
                posistions.Add(new Vector3(user.CurrentCharacter.Pos.X, user.CurrentCharacter.Pos.Y, user.CurrentCharacter.Pos.Z));
                API.SaveResourceFile(API.GetCurrentResourceName(), "posistions", JsonConvert.SerializeObject(posistions), -1);
            }

        }

        private void DeleteVehicle(User user, string[] args)
        {
            if (user.Permissions < 1)
            {
                Utility.Instance.SendChatMessage(user.Source, "[ADMIN]", "Invalid permissions for this command!!", 255, 0, 0);
                return;
            }
            TriggerClientEvent(user.Source,"DeleteVehicle");
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
            CommandManager.Instance.AddCommand("setperms", UpdatePerms);
            CommandManager.Instance.AddCommand("spawn", SpawnCar);
            CommandManager.Instance.AddCommand("goto", GotoPlayer);
            CommandManager.Instance.AddCommand("bring", BringPlayer);
            CommandManager.Instance.AddCommand("pos", SavePos);
            CommandManager.Instance.AddCommand("dv", DeleteVehicle);
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
