using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

                var ply = plyList[id];
                if (ply == null) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
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
                Player ply = plyList[id];
                if (ply == null) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
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
                Player ply = plyList[id];
                if (ply == null) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
                if (ply != null)
                {
                    var targetUser = UserManager.Instance.GetUserFromPlayer(ply);
                    if (targetUser != null)
                    {
                        DatabaseManager.Instance.Execute("UPDATE USERS SET perms="+args[2]+" WHERE steam='"+targetUser.SteamId+"';");
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

        private async Task RefreshBans()
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

            if (args.Length >= 2)
            {
                if (user != null && args[1] != null)
                {
                    var plyList = new PlayerList();
                    var id = 0;
                    if (!Int32.TryParse(args[1], out id))
                    {
                        Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid parameters", 255, 0, 0);
                        return;
                    }
                    Player ply = plyList[id];
                    if (ply == null) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
                    if (ply != null)
                    {
                        var targetUser = UserManager.Instance.GetUserFromPlayer(ply);
                        if (targetUser != null)
                        {
                            Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Grabbing information for " + ply.Name + "'s current character.", 255, 0, 0);
                            Utility.Instance.SendChatMessage(user.Source, "[Admin]", "First Name : " + targetUser.CurrentCharacter.FirstName + ", Last Name : " + targetUser.CurrentCharacter.LastName + ", DoB : " + targetUser.CurrentCharacter.DateOfBirth, 255, 0, 0);
                            Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Cash : " + targetUser.CurrentCharacter.Money.Cash + ", Bank : " + targetUser.CurrentCharacter.Money.Bank + ", Untaxed " + targetUser.CurrentCharacter.Money.UnTaxed, 255, 0, 0);
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
                Player ply = plyList[id];
                if (ply == null) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
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
                Player ply = plyList[id];
                if (ply == null) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
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
            if (user.Permissions < 1 && !Police.Instance.IsPlayerCop(user.Source))
            {
                Utility.Instance.SendChatMessage(user.Source, "[ADMIN]", "Invalid permissions for this command!!", 255, 0, 0);
                return;
            }
            TriggerClientEvent(user.Source,"DeleteVehicle");
        }


        private void CreateKeyCommand(User user, string[] args)
        {
            if (user.Permissions < 4) { Utility.Instance.SendChatMessage(user.Source, "[Admin]", "Invalid permissions for this command!!", 255, 0, 0); return; }
            if (args.Length < 2){ Utility.Instance.SendChatMessage(user.Source, "[ADMIN]", "Not enough parameters.", 255, 0, 0); return; }
            var car = args[1];
            VehicleManager.Instance.CreateVehicleKey(car,car,user.Source);
        }

        private async Task SetupCommands()
        {
            while (CommandManager.Instance == null)
            {
                await Delay(0);
            }
            await CommandManager.Instance.AddCommand("perms", ShowPerms);
            await CommandManager.Instance.AddCommand("kick", KickPlayer);
            await CommandManager.Instance.AddCommand("ban", BanPlayer);
            await CommandManager.Instance.AddCommand("unban", UnbanPlayer);
            await CommandManager.Instance.AddCommand("info", GrabPlayerInfo);
            await CommandManager.Instance.AddCommand("setperms", UpdatePerms);
            await CommandManager.Instance.AddCommand("spawn", SpawnCar);
            //await CommandManager.Instance.AddCommand("goto", GotoPlayer);
            //await CommandManager.Instance.AddCommand("bring", BringPlayer);
            //await CommandManager.Instance.AddCommand("pos", SavePos);
            await CommandManager.Instance.AddCommand("dv", DeleteVehicle);
            await CommandManager.Instance.AddCommand("createkey", CreateKeyCommand);
            await CommandManager.Instance.AddCommand("restart", async (user, strings) =>
            {
                if (user.Permissions < 5)
                {
                    Utility.Instance.SendChatMessage(user.Source, "[ADMIN]", "Invalid permissions for this command!!", 255, 0, 0);
                    return;
                }
                Utility.Instance.SendChatMessageAll("[System]", "SERVER RESTARTING IN 10 MINUTES", 255, 0, 0);
                await Delay(60000);
                Utility.Instance.SendChatMessageAll("[System]", "SERVER RESTARTING IN 9 MINUTES", 255, 0, 0);
                await Delay(60000);
                Utility.Instance.SendChatMessageAll("[System]", "SERVER RESTARTING IN 8 MINUTES", 255, 0, 0);
                await Delay(60000);
                Utility.Instance.SendChatMessageAll("[System]", "SERVER RESTARTING IN 7 MINUTES", 255, 0, 0);
                await Delay(60000);
                Utility.Instance.SendChatMessageAll("[System]", "SERVER RESTARTING IN 6 MINUTES", 255, 0, 0);
                await Delay(60000);
                Utility.Instance.SendChatMessageAll("[System]", "SERVER RESTARTING IN 5 MINUTES", 255, 0, 0);
                await Delay(60000);
                Utility.Instance.SendChatMessageAll("[System]", "SERVER RESTARTING IN 4 MINUTES", 255, 0, 0);
                await Delay(60000);
                Utility.Instance.SendChatMessageAll("[System]", "SERVER RESTARTING IN 3 MINUTES", 255, 0, 0);
                await Delay(60000);
                Utility.Instance.SendChatMessageAll("[System]", "SERVER RESTARTING IN 2 MINUTES", 255, 0, 0);
                await Delay(60000);
                Utility.Instance.SendChatMessageAll("[System]", "SERVER RESTARTING IN 1 MINUTES", 255, 0, 0);
                await Delay(10000);
                Utility.Instance.SendChatMessageAll("[System]", "SERVER RESTARTING IN 50 SECONDS", 255, 0, 0);
                await Delay(10000);
                Utility.Instance.SendChatMessageAll("[System]", "SERVER RESTARTING IN 40 SECONDS", 255, 0, 0);
                await Delay(10000);
                Utility.Instance.SendChatMessageAll("[System]", "SERVER RESTARTING IN 30 SECONDS", 255, 0, 0);
                await Delay(10000);
                Utility.Instance.SendChatMessageAll("[System]", "SERVER RESTARTING IN 20 SECONDS", 255, 0, 0);
                await Delay(10000);
                Utility.Instance.SendChatMessageAll("[System]", "SERVER RESTARTING IN 10 SECONDS", 255, 0, 0);
                await Delay(10000);
                foreach (Player ply in new PlayerList())
                {
                    ply.Drop("Palace farted and the server broke. GG. (SERVER RESTARTING)");
                }
                await Delay(10000);

                Debug.WriteLine("ALL PALYERS GONE YOUR GOOD TO GO!");
                Debug.WriteLine("ALL PALYERS GONE YOUR GOOD TO GO!");
                Debug.WriteLine("ALL PALYERS GONE YOUR GOOD TO GO!");
                Debug.WriteLine("ALL PALYERS GONE YOUR GOOD TO GO!");
                Debug.WriteLine("ALL PALYERS GONE YOUR GOOD TO GO!");
                Debug.WriteLine("ALL PALYERS GONE YOUR GOOD TO GO!");
                Debug.WriteLine("ALL PALYERS GONE YOUR GOOD TO GO!");
                Debug.WriteLine("ALL PALYERS GONE YOUR GOOD TO GO!");
                Debug.WriteLine("ALL PALYERS GONE YOUR GOOD TO GO!");
            });
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
