using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using server.Main.Items;
using server.Main.Users;

namespace server.Main.EmergencyServices.Police
{
    public class Police : BaseScript
    {
        public static Police Instance;

        public Police()
        {
            Instance = this;
            SetupEvents();
            LoadCops();
            Paycheck();
            SetupCommands();

            //Setup the events that can be called in this class.
            EventHandlers["911Call"] += new Action<Player, string, string>(Call911);
            EventHandlers["311Call"] += new Action<Player, string, string>(Call311);
            EventHandlers["ConfiscateWeapons"] += new Action<Player, int>(ConfiscateWeapons);
            EventHandlers["ConfiscateItems"] += new Action<Player, int>(ConfiscateItems);

            EventHandlers["playerDropped"] += new Action<Player>(RemoveFromCopsOnLeave);
        }

        /// <summary>
        /// Removes the cop when from on duty ocops when they disconnect.
        /// </summary>
        /// <param name="player"></param>
        private void RemoveFromCopsOnLeave([FromSource]Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (IsPlayerOnDuty(player))
            {
                OnDutyOfficers.Remove(user);
                TriggerClientEvent("Police:RefreshOnDutyOfficers", OnDutyOfficers.Count);
            }
        }

        #region Private Variables
        /// <summary>
        /// All of the information for every officer in the PD that is loaded in the very beggining of the script
        /// </summary>
        public Dictionary<int, PoliceOfficer> LoadedOfficers = new Dictionary<int, PoliceOfficer>();

        /// <summary>
        /// All of the on duty officers currently.
        /// </summary>
        public Dictionary<User, PoliceOfficer> OnDutyOfficers = new Dictionary<User, PoliceOfficer>();

        /// <summary>
        /// Police ranks are configured here.
        /// </summary>
        public Dictionary<string, PoliceRank> PoliceRanks = new Dictionary<string, PoliceRank>()
        {
            ["Cadet"] = new PoliceRank( // Rnak Name
                 "Cadet", // Rank Name 
                 1500, // Rnak Salary
                 LEODepartments.SASP, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                 new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                 {
                    "bx4blue",
                 },
                 false, // Can Use Air1
                 false  // Can Promote
             ),
            ["Trooper"] = new PoliceRank( // Rnak Name
                 "Trooper", // Rank Name 
                 2000, // Rnak Salary
                 LEODepartments.SASP, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                 new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                 {
                    "bx1blue",
                    "bx3blue",
                 },
                 false, // Can Use Air1
                 false  // Can Promote
             ),
            ["Senior Trooper"] = new PoliceRank( // Rnak Name
                 "Senior Trooper", // Rank Name 
                 3000, // Rnak Salary
                 LEODepartments.SASP, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                 new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                 {
                    "bx1blue",
                    "bx3blue",
                    "sherrif2blue",
                 },
                 false, // Can Use Air1
                 false  // Can Promote
             ),
            ["Corporal"] = new PoliceRank( // Rnak Name
                 "Corporal", // Rank Name 
                 3500, // Rnak Salary
                 LEODepartments.SASP, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                 new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                 {
                    "bx1blue",
                    "bx3blue",
                    "bx4blue",
                    "sherrif2blue",
                 },
                 false, // Can Use Air1
                 false  // Can Promote
             ),
            ["Sergeant"] = new PoliceRank( // Rnak Name
                 "Sergeant", // Rank Name 
                 4000, // Rnak Salary
                 LEODepartments.SASP, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                 new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                 {
                    "bx1blue",
                    "bx2blue",
                    "bx3blue",
                    "bx4blue",
                    "sherrif2blue",
                    "police6blue"
                 },
                 true, // Can Use Air1
                 false  // Can Promote
             ),
            ["Pilot"] = new PoliceRank( // Rnak Name
                 "State Pilot", // Rank Name 
                 4000, // Rnak Salary
                 LEODepartments.SASP, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                 new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                 {
                    "bx1blue",
                    "bx2blue",
                    "bx3blue",
                    "bx4blue",
                    "sherrif2blue",
                 },
                 true, // Can Use Air1
                 false  // Can Promote
             ),
            ["Lieutenant"] = new PoliceRank( // Rnak Name
                 "Lieutenant", // Rank Name 
                 4500, // Rnak Salary
                 LEODepartments.SASP, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                 new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                 {
                    "bx1blue",
                    "bx2blue",
                    "bx3blue",
                    "bx4blue",
                    "sherrif2blue",
                    "police6blue"
                 },
                 true, // Can Use Air1
                 true  // Can Promote
             ),
            ["Major"] = new PoliceRank( // Rnak Name
                 "Major", // Rank Name 
                 5000, // Rnak Salary
                 LEODepartments.SASP, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                 new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                 {
                    "bx1blue",
                    "bx2blue",
                    "bx3blue",
                    "bx4blue",
                    "sherrif2blue",
                    "police6blue"
                 },
                 true, // Can Use Air1
                 true  // Can Promote
             ),
            ["Colonel"] = new PoliceRank( // Rnak Name
                 "Colonel", // Rank Name 
                 5500, // Rnak Salary
                 LEODepartments.SASP, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                 new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                 {
                    "bx1blue",
                    "bx2blue",
                    "bx3blue",
                    "bx4blue",
                    "sherrif2blue",
                    "police6blue"
                 },
                 true, // Can Use Air1
                 true  // Can Promote
             ),
            ["Commissioner"] = new PoliceRank( // Rnak Name
                 "Commissioner", // Rank Name 
                 10000, // Rnak Salary
                 LEODepartments.SASP, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                 new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                 {
                    "bx1blue",
                    "bx2blue",
                    "bx3blue",
                    "bx4blue",
                    "sherrif2blue",
                    "police6blue"
                 },
                 true, // Can Use Air1
                 true  // Can Promote
             ),
        };
        #endregion


        /// <summary>
        /// Event to handle the calling of 9111
        /// </summary>
        /// <param name="player">The playe from the source that is calling</param>
        /// <param name="message">The message of the 911 call.</param>
        /// <param name="location">A string consisting of the two streetnames and the zone that the player is in.</param>
        private void Call911([FromSource]Player player, string message, string location)
        {
            foreach (var officer in OnDutyOfficers.Keys)
            {
                Utility.Instance.SendChatMessage(officer.Source, "[Dispatch]", "911 (" + location + ") " + message, 0, 0, 150);
            }
        }

        /// <summary>
        /// Event to handle the calling of 311
        /// </summary>
        /// <param name="player">The player that is calling that is gotten from the source string</param>
        /// <param name="message">The message of the 311 call</param>
        /// <param name="location"></param>
        private void Call311([FromSource]Player player, string message, string location)
        {
            foreach (var officer in OnDutyOfficers.Keys)
            {
                Utility.Instance.SendChatMessage(officer.Source, "[Dispatch]", "311 (" + location + ") " + message, 175, 175, 0);
            }
        }


        #region Paycheck
        /// <summary>
        /// A task that handles the reoccuring paycheck of every 10 minutes.
        /// </summary>
        /// <returns></returns>
        private async Task Paycheck()
        {
            while (true)
            {
                await Delay(600000);
                foreach (var user in OnDutyOfficers.Keys)
                {
                    if (user != null && user.Source != null)
                    {
                        MoneyManager.Instance.AddMoney(user.Source, MoneyTypes.Bank, PoliceRanks[OnDutyOfficers[user].Rank].Salary);
                    }
                    else
                    {
                        OnDutyOfficers.Remove(user);
                    }
                }
            }
        }

        #endregion

        #region Functioanlity Methods

        /// <summary>
        /// This function checks if the given player can promote within the police department.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <returns>A bool that is true if yes, false if not.</returns>
        public bool CanPromote(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var officer = GetOfficerObjectByName(user.CurrentCharacter.FullName);
            return Admin.Instance.ActiveAdmins.Contains(player) || officer != null && PoliceRanks.ContainsKey(officer.Rank) && PoliceRanks[officer.Rank].CanPromote;
        }

        /// <summary>
        /// Adds a given player to the police force by inserting them nito the database and the currently loaded officers.
        /// </summary>
        /// <param name="player">Player to add.</param>
        public void AddCop(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);

            var newid = 0;
            if (!LoadedOfficers.Any())
            {
                newid = 1;
            }
            else
            {
                newid = LoadedOfficers.Last().Key + 1;
            }
            var officer = new PoliceOfficer(user.SteamId, user.CurrentCharacter.FullName, "Cadet", newid);
            LoadedOfficers.Add(officer.Badge, officer);
            MoneyManager.Instance.AddMoney(player, MoneyTypes.Bank, 10000);
            DatabaseManager.Instance.Execute("INSERT INTO POLICE (badge,officerinfo) VALUES(" + officer.Badge + ",'" + JsonConvert.SerializeObject(officer) + "');");
            Utility.Instance.SendChatMessageAll("[Police]", user.CurrentCharacter.FullName + " has been hired into the SASP.", 0, 0, 180);
        }

        /// <summary>
        /// Removes a given cop from the PD. Removing them from the Db and loaded cops. Also puts them off duty.
        /// </summary>
        /// <param name="player">target player</param>
        public void RemoveCop(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var chara = user.CurrentCharacter;

            var keyToRemove = -1;
            foreach (var officer in LoadedOfficers)
            {
                if (officer.Value.Name == chara.FullName)
                {
                    keyToRemove = officer.Key;
                    break;
                }
            }
            LoadedOfficers.Remove(keyToRemove);
            SetDuty(player,false);
            DatabaseManager.Instance.Execute("DELETE FROM POLICE WHERE badge = " + keyToRemove + ";");
            Utility.Instance.SendChatMessageAll("[Police]", user.CurrentCharacter.FullName + " has been fired from the SASP.", 0, 0, 180);
        }

        /// <summary>
        /// Sets the cops rank to a certain rank via string, and then sets off duty then back on.
        /// </summary>
        /// <param name="player">target player</param>
        /// <param name="rank">string of rank in the dictionary</param>
        public void PromoteCop(Player player, string rank)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);

            var chara = user.CurrentCharacter;

            if (PoliceRanks.ContainsKey(rank))
            {
                var officerKey = GetLoadedOfficerKeyByName(chara.FullName);
                LoadedOfficers[officerKey].Rank = rank;

                if (IsPlayerOnDuty(player))
                {
                    OnDutyOfficers[user].Rank = rank;
                }
                SetDuty(player, false);
                SetDuty(player, true);
                DatabaseManager.Instance.Execute("UPDATE POLICE SET officerinfo='" + JsonConvert.SerializeObject(LoadedOfficers[officerKey]) + "' WHERE badge=" + officerKey + ";");
                Utility.Instance.SendChatMessageAll("[Police]", user.CurrentCharacter.FullName + " has been promoted to " + rank + " into the SASP.", 0, 0, 180);
            }
        }

        /// <summary>
        /// Set the player onduty or offduty as a cop.
        /// </summary>
        /// <param name="player">target player</param>
        /// <param name="onDuty">true=onduty true=off duty</param>
        public void SetDuty(Player player, bool onDuty)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (!IsPlayerCop(player)) { return; }
            if (onDuty && !IsPlayerOnDuty(player))
            {
                var officer = GetOfficerObjectByName(user.CurrentCharacter.FullName);
                OnDutyOfficers.Add(user, officer);
                TriggerClientEvent(player, "Police:SetOnDuty", Convert.ToString(PoliceRanks[officer.Rank].Department));
                TriggerClientEvent(player, "UpdatePoliceCars", PoliceRanks[officer.Rank].AvailableVehicles);
            }
            else if (!onDuty && IsPlayerOnDuty(player))
            {
                OnDutyOfficers.Remove(user);
                TriggerClientEvent(player, "Police:SetOffDuty");
            }
            else
            {
                return;
            }
            TriggerClientEvent("Police:RefreshOnDutyOfficers", OnDutyOfficers.Count);
        }

        /// <summary>
        /// Loads alkl the information about alll the cops from the database.
        /// </summary>
        /// <returns></returns>
        public async Task LoadCops()
        {
            while (DatabaseManager.Instance == null)
            {
                await Delay(100);
            }
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM POLICE ORDER BY badge ASC").Result;
            while (data.Read())
            {
                var officer = JsonConvert.DeserializeObject<PoliceOfficer>(Convert.ToString(data["officerinfo"]));
                LoadedOfficers.Add(officer.Badge, officer);
            }
            DatabaseManager.Instance.EndQuery(data);
            while (Utility.Instance == null)
            {
                await Delay(100);
            }
            Utility.Instance.Log("Police Loaded");
        }

        /// <summary>
        /// checks if the player is a cop.
        /// </summary>
        /// <param name="player">Target Player</param>
        /// <returns>A bool that is true if yes, false if not.</returns>
        public bool IsPlayerCop(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            foreach (var officer in LoadedOfficers)
            {
                if (officer.Value.Name == user.CurrentCharacter.FullName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// /Check if the given player is a on duty cop.
        /// </summary>
        /// <param name="player">target player</param>
        /// <returns>A bool that is true if yes, false if not.</returns>
        public bool IsPlayerOnDuty(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (OnDutyOfficers.ContainsKey(user))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get a officer object by the name of the officer from the loaded officers. ( Not On Duty! )
        /// </summary>
        /// <param name="name">The name to use.</param>
        /// <returns>The PoliceOfficer object that matches the name</returns>
        public PoliceOfficer GetOfficerObjectByName(string name)
        {
            foreach (var officer in LoadedOfficers)
            {
                if (officer.Value.Name == name)
                {
                    return officer.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the index/badge of a loaded officer by tyhier name from the loaded officers list.
        /// </summary>
        /// <param name="name">The name of the officer.</param>
        /// <returns></returns>
        public int GetLoadedOfficerKeyByName(string name)
        {
            foreach (var officer in LoadedOfficers.Keys)
            {
                if (LoadedOfficers[officer].Name == name)
                {
                    return officer;
                }
            }
            return -1;
        }
        #endregion

        #region Functionality Events

        private void SetupEvents()
        {
            EventHandlers["Police:SearchPlayer"] += new Action<Player, int>(SearchPlayer);
        }

        /// <summary>
        /// Searches the player.
        /// </summary>
        /// <param name="player">The player searching</param>
        /// <param name="targetPlayerId">The target player server id</param>
        private void SearchPlayer([FromSource] Player player, int targetPlayerId)
        {
#pragma warning disable CS0472 // The result of the expression is always 'false' since a value of type 'int' is never equal to 'null' of type 'int?'
            if (targetPlayerId == null) { return; }
#pragma warning restore CS0472 // The result of the expression is always 'false' since a value of type 'int' is never equal to 'null' of type 'int?'
            var list = new PlayerList();
            var targetPlayer = list[targetPlayerId];
            if (targetPlayer == null) { return; }
            var targetUser = UserManager.Instance.GetUserFromPlayer(targetPlayer);
            var chatString = "";

            var quantitys = new Dictionary<int, int>();

            var inventory = targetUser.CurrentCharacter.Inventory;

            foreach (Item item in inventory)
            {
                if (quantitys.ContainsKey(item.Id))
                {
                    quantitys[item.Id] = quantitys[item.Id] + 1;
                }
                else
                {
                    quantitys.Add(item.Id, 1);
                }
            }

            foreach (var itemID in quantitys.Keys)
            {
                var itemName = inventory.Find(x => x.Id == itemID).Name;
                var itemWeight = inventory.Find(x => x.Id == itemID).Weight;
                chatString = chatString + "" + itemName + "(" + itemWeight * quantitys[itemID] + "kg)[" + quantitys[itemID] + "],  ";
            }
            RPCommands.Instance.ActionCommand("Pats down the subject thoroughly, and searches pockets,bags, anywhere else that the person may be hiding something on them and finds " + chatString, player);
        }

        #endregion  

        #region Commands

        public void RiotShieldCommand(User user, string[] args)
        {
            if (IsPlayerOnDuty(user.Source))
            {
                TriggerClientEvent(user.Source, "RiotShield");
            }
        }

        public void AddCopCommand(User user, string[] args)
        {
            if (args.Length < 2) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid parameter count.", 0, 0, 255); return; }
            var plyList = new PlayerList();
            var id = 0;
            if (!Int32.TryParse(args[1], out id))
            {
                Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid parameters", 255, 0, 0);
                return;
            }
            Player targetPlayer = plyList[id];
            if (targetPlayer == null) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
            if (CanPromote(user.Source) && !IsPlayerCop(targetPlayer))
            {
                AddCop(targetPlayer);
            }
        }

        public void RemoveCopCommand(User user, string[] args)
        {
            if (args.Length < 2) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid parameter count.", 0, 0, 255); return; }
            var plyList = new PlayerList();
            var id = 0;
            if (!Int32.TryParse(args[1], out id))
            {
                Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid parameters", 255, 0, 0);
                return;
            }
            Player targetPlayer = plyList[id];
            if (targetPlayer == null) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
            if (CanPromote(user.Source) && IsPlayerCop(targetPlayer))
            {
                RemoveCop(targetPlayer);
            }
        }

        public void PromoteCopCommand(User user, string[] args)
        {
            if (args.Length < 3) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid parameter count.", 0, 0, 255); return; }
            var plyList = new PlayerList();
            var id = 0;
            if (!Int32.TryParse(args[1], out id))
            {
                Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid parameters", 255, 0, 0);
                return;
            }
            args[1] = null;
            args[0] = null;
            var rank = String.Join(" ", args);
            rank = rank.Remove(0, 2);

            Player targetPlayer = plyList[id];
            if (targetPlayer == null) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
            if (CanPromote(user.Source) && IsPlayerCop(targetPlayer) && PoliceRanks.ContainsKey(rank))
            {
                PromoteCop(targetPlayer, rank);
            }
        }

        public void JailPlayerCommand(User user, string[] args)
        {
            if (IsPlayerOnDuty(user.Source) || Admin.Instance.ActiveAdmins.Contains(user.Source))
            {
                if (args.Length < 3)
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Jail]", "Invalid amount of parameters", 255, 0, 0);
                    return;
                }
                var plyList = new PlayerList();
                var id = 0;
                if (!Int32.TryParse(args[1], out id))
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid parameters", 255, 0, 0);
                    return;
                }
                Player targetPlayer = plyList[id];
                if (targetPlayer == null) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }

                if (!Int32.TryParse(args[2], out var amoutn))
                {
                    return;
                }
                UserManager.Instance.GetUserFromPlayer(targetPlayer).CurrentCharacter.JailTime = amoutn * 60;
                TriggerClientEvent(targetPlayer, "Jail", amoutn * 60);
            }
        }

        public void UnjailPlayerCommand(User user, string[] args)
        {
            if (IsPlayerOnDuty(user.Source) || Admin.Instance.ActiveAdmins.Contains(user.Source))
            {
                if (args.Length < 2)
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Jail]", "Invalid amount of parameters", 255, 0, 0);
                    return;
                }

                var plyList = new PlayerList();
                var id = 0;
                if (!Int32.TryParse(args[1], out id))
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Jail]", "Invalid parameters", 255, 0, 0);
                    return;
                }
                Player targetPlayer = plyList[id];
                if (targetPlayer == null) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
                TriggerClientEvent(targetPlayer, "Unjail");
                var tgtUser = UserManager.Instance.GetUserFromPlayer(targetPlayer);
                tgtUser.CurrentCharacter.JailTime = 0;
            }
        }

        public void FinePlayerCommand(User user, string[] args)
        {
            if (IsPlayerOnDuty(user.Source) || Admin.Instance.ActiveAdmins.Contains(user.Source))
            {
                if (args.Length < 3)
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Jail]", "Invalid amount of parameters", 255, 0, 0);
                    return;
                }

                if (!Int32.TryParse(args[2], out var amoutn))
                {
                    return;
                }
                var plyList = new PlayerList();
                var id = 0;
                if (!Int32.TryParse(args[1], out id))
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Jail]", "Invalid parameters", 255, 0, 0);
                    return;
                }

                Player targetPlayer = plyList[id];
                if (targetPlayer == null) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
                var targetUser = UserManager.Instance.GetUserFromPlayer(targetPlayer);
                if (MoneyManager.Instance.GetMoney(targetPlayer, MoneyTypes.Cash) >= amoutn)
                {
                    MoneyManager.Instance.RemoveMoney(targetPlayer, MoneyTypes.Cash, amoutn);
                    Utility.Instance.SendChatMessage(user.Source, "[Fines]", targetUser.CurrentCharacter.FullName + " has paid their fine with cash", 255, 0, 0);
                }
                else if (MoneyManager.Instance.GetMoney(targetPlayer, MoneyTypes.Bank) >= amoutn)
                {
                    MoneyManager.Instance.RemoveMoney(targetPlayer, MoneyTypes.Bank, amoutn);
                    Utility.Instance.SendChatMessage(user.Source, "[Fines]", targetUser.CurrentCharacter.FullName + " has paid their fine with bank balance", 255, 0, 0);
                }
                else
                {
                    var newAmount = amoutn - MoneyManager.Instance.GetMoney(targetPlayer, MoneyTypes.Bank);
                    Utility.Instance.SendChatMessage(user.Source, "[Fines]", targetUser.CurrentCharacter.FullName + " has paid as much as they can of their fine with bank balance. ($" + newAmount + " left)", 255, 0, 0);
                }
            }
        }

        public void OnDutyCommand(User user, string[] args)
        {
            SetDuty(user.Source, true);
        }

        public void OffDutyCommand(User user, string[] args)
        {
            SetDuty(user.Source, false);
        }
        public void ConfiscateItems([FromSource]Player player, int target)
        {
            if (IsPlayerOnDuty(player) || Admin.Instance.ActiveAdmins.Contains(player))
            {

                var plyList = new PlayerList();
                Player targetPlayer = plyList[target];
                if (targetPlayer == null) { Utility.Instance.SendChatMessage(player, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
                Utility.Instance.SendChatMessage(player, "[Confiscate]", "You have confiscated all illegal items " +
                                                                         "from " + UserManager.Instance.GetUserFromPlayer(targetPlayer).CurrentCharacter.FullName, 255, 0, 0);

                Utility.Instance.SendChatMessage(targetPlayer, "[Confiscate]", "All of your illegal items have been confiscated " +
                                                                               "by " + UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.FullName, 255, 0, 0);
                InventoryManager.Instance.ConfiscateItems(targetPlayer);
            }
        }

        public void ConfiscateWeapons([FromSource]Player player, int target)
        {
            if (IsPlayerOnDuty(player) || Admin.Instance.ActiveAdmins.Contains(player))
            {

                var plyList = new PlayerList();
                Player targetPlayer = plyList[target];
                if (targetPlayer == null) { Utility.Instance.SendChatMessage(player, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
                Utility.Instance.SendChatMessage(player, "[Confiscate]", "You have confiscated all weapons " +
                                                                         "from " + UserManager.Instance.GetUserFromPlayer(targetPlayer).CurrentCharacter.FullName, 255, 0, 0);

                Utility.Instance.SendChatMessage(targetPlayer, "[Confiscate]", "All of your weapons have been confiscated " +
                                                                               "by " + UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.FullName, 255, 0, 0);
                InventoryManager.Instance.ConfiscateWeapons(targetPlayer);
            }
        }

        private static void PanicButtonCommand(User user, string[] args)
        {
            if (Police.Instance.IsPlayerOnDuty(user.Source))
            {
                TriggerClientEvent(user.Source, "911CallClient", "OFFICER IN DISTRESS");
                TriggerClientEvent(user.Source, "911CallClient", "OFFICER IN DISTRESS");
                TriggerClientEvent(user.Source, "911CallClient", "OFFICER IN DISTRESS");
                TriggerClientEvent(user.Source, "911CallClient", "OFFICER IN DISTRESS");
                TriggerClientEvent(user.Source, "911CallClient", "OFFICER IN DISTRESS");
                TriggerClientEvent(user.Source, "911CallClient", "OFFICER IN DISTRESS");
                TriggerClientEvent(user.Source, "911CallClient", "OFFICER IN DISTRESS");
            }
        }

        private static void Air1Command(User user, string[] args)
        {
            if (Police.Instance.IsPlayerOnDuty(user.Source) || EMS.EMS.Instance.IsPlayerOnDuty(user.Source))
            {
                TriggerClientEvent(user.Source, "Police:AirUnit");
            }
        }

        private static void MarineCommand(User user, string[] args)
        {
            if (Police.Instance.IsPlayerOnDuty(user.Source) || EMS.EMS.Instance.IsPlayerOnDuty(user.Source))
            {
                TriggerClientEvent(user.Source, "Police:MarineUnit");
            }
        }

        public async Task SetupCommands()
        {
            await Delay(500);
            while (CommandManager.Instance == null)
            {
                await Delay(0);
            }
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("jail", JailPlayerCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("shield", RiotShieldCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("fine", FinePlayerCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("unjail", UnjailPlayerCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("addcop", AddCopCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("copadd", AddCopCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("remcop", RemoveCopCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("coprem", RemoveCopCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("setcoprank", PromoteCopCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("coprank", PromoteCopCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("coppromote", PromoteCopCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("policeonduty", OnDutyCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("policeoffduty", OffDutyCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("coponduty", OnDutyCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("copoffduty", OffDutyCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("panic", PanicButtonCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("air1", Air1Command);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("marine", MarineCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }

        #endregion

    }
}
