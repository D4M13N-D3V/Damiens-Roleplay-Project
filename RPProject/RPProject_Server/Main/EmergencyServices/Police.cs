using CitizenFX.Core;
using Newtonsoft.Json;
using server.Main.Items;
using server.Main.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Main.EmergencyServices
{
    public enum LEODepartments
    {
        SASP,
        USMS,
    }
    public class PoliceRank
    {
        public string Name = "None";
        public int Salary = 0;
        public LEODepartments Department = LEODepartments.SASP;
        public List<string> AvailableVehicles = new List<string>()
        {

        };
        public bool CanUseHeli = true;
        public bool CanUseSpikeStrips = true;
        public bool CanUseRadar = true;
        public bool CanUseK9 = true;
        public bool CanUseEMS = true;
        public bool CanPromote = false;
        public PoliceRank(string name, int salary, LEODepartments department, List<string> availableVehicles,bool canUseHeli, bool canPromote)
        {
            Department = department;
            Name = name;
            Salary = salary;
            AvailableVehicles = availableVehicles;
            CanPromote = canPromote;
            CanUseHeli = canUseHeli;
        }
    }
    public class PoliceOfficer
    {
        public string SteamId;
        public string Name;
        public string Rank;
        public int Badge;

        public PoliceOfficer( string steamId, string name, string rank, int badge)
        {
            SteamId = steamId;
            Name = name;
            Rank = rank;
            Badge = badge;
        }
    }

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
            EventHandlers["911Call"] += new Action<Player, string, string>(Call911);
            EventHandlers["311Call"] += new Action<Player, string, string>(Call311);
        }
        #region Private Variables
        public Dictionary<int,PoliceOfficer> LoadedOfficers = new Dictionary<int, PoliceOfficer>();
        public Dictionary<User,PoliceOfficer> OnDutyOfficers = new Dictionary<User, PoliceOfficer>();
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


        private void Call911([FromSource]Player player, string message, string location)
        {
            foreach (var officer in OnDutyOfficers.Keys)
            {
                Utility.Instance.SendChatMessage(officer.Source, "[Dispatch]", "911 (" + location + ") " + message, 0, 0, 150);
            }
        }

        private void Call311([FromSource]Player player, string message, string location)
        {
            foreach (var officer in OnDutyOfficers.Keys)
            {
                Utility.Instance.SendChatMessage(officer.Source, "[Dispatch]", "311 (" + location + ") " + message, 175,175, 0);
            }
        }


        #region Paycheck

        private async Task Paycheck()
        {
            while (true)
            {
                await Delay(600000);
                foreach (var user in OnDutyOfficers.Keys)
                {
                    if (user!=null && user.Source != null)
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

        public bool CanPromote(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var officer = GetOfficerObjectByName(user.CurrentCharacter.FullName);
            return Admin.Instance.ActiveAdmins.Contains(player) || officer!=null && PoliceRanks.ContainsKey(officer.Rank) && PoliceRanks[officer.Rank].CanPromote;
        }

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
            var officer = new PoliceOfficer(user.SteamId,user.CurrentCharacter.FullName,"Cadet",newid);
            LoadedOfficers.Add(officer.Badge,officer);
            MoneyManager.Instance.AddMoney(player,MoneyTypes.Bank,10000);
            DatabaseManager.Instance.Execute("INSERT INTO POLICE (badge,officerinfo) VALUES(" + officer.Badge+",'"+JsonConvert.SerializeObject(officer)+"');");
            Utility.Instance.SendChatMessageAll("[Police]",user.CurrentCharacter.FullName+" has been hired into the SASP.",0,0,180);
        }

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
            DatabaseManager.Instance.Execute("DELETE FROM POLICE WHERE badge = " + keyToRemove + ";");
            Utility.Instance.SendChatMessageAll("[Police]", user.CurrentCharacter.FullName + " has been fired from the SASP.", 0, 0, 180);
        }

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
                DatabaseManager.Instance.Execute("UPDATE POLICE SET officerinfo='"+JsonConvert.SerializeObject(LoadedOfficers[officerKey])+"' WHERE badge="+officerKey+";");
                Utility.Instance.SendChatMessageAll("[Police]", user.CurrentCharacter.FullName + " has been promoted to "+rank+" into the SASP.", 0, 0, 180);
            }
        }

        public void SetDuty(Player player, bool onDuty)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (!IsPlayerCop(player)){  return; }
            if (onDuty && !IsPlayerOnDuty(player))
            {
                var officer = GetOfficerObjectByName(user.CurrentCharacter.FullName);
                OnDutyOfficers.Add(user,officer);
                TriggerClientEvent(player, "Police:SetOnDuty",Convert.ToString(PoliceRanks[officer.Rank].Department));
                TriggerClientEvent(player, "UpdatePoliceCars", PoliceRanks[officer.Rank].AvailableVehicles);
            }
            else if(!onDuty && IsPlayerOnDuty(player))
            {
                OnDutyOfficers.Remove(user);
                TriggerClientEvent(player,"Police:SetOffDuty");
            }
            else
            {
                return;
            }
            TriggerClientEvent("Police:RefreshOnDutyOfficers", OnDutyOfficers.Count);
        }

        public async Task LoadCops()
        {   
            while (DatabaseManager.Instance==null)
            {
                await Delay(100);
            }
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM POLICE ORDER BY badge ASC").Result;
            while (data.Read())
            {
                var officer = JsonConvert.DeserializeObject<PoliceOfficer>(Convert.ToString(data["officerinfo"]));
                LoadedOfficers.Add(officer.Badge,officer);
            }
            DatabaseManager.Instance.EndQuery(data);
            while (Utility.Instance == null)
            {
                await Delay(100);
            }
            Utility.Instance.Log("Police Loaded");
        }

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

        public bool IsPlayerOnDuty(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (OnDutyOfficers.ContainsKey(user))
            {
                return true;
            }

            return false;
        }

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

        private void SearchPlayer([FromSource] Player player, int targetPlayerId)
        {
            if (targetPlayerId == null) { return; }
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
                chatString = chatString + "" + itemName + "(" + itemWeight * quantitys[itemID] + "kg)["+quantitys[itemID]+"],  ";
            }
            RPCommands.Instance.ActionCommand("Pats down the subject thoroughly, and searches pockets,bags, anywhere else that the person may be hiding something on them and finds "+chatString,player);
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
            var rank = String.Join(" ",args);
            rank = rank.Remove(0, 2);

            Player targetPlayer = plyList[id];
            if (targetPlayer == null) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
            if (CanPromote(user.Source) && IsPlayerCop(targetPlayer) && PoliceRanks.ContainsKey(rank))
            {
                PromoteCop(targetPlayer,rank);
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
                    return; }
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
                if (MoneyManager.Instance.GetMoney(targetPlayer,MoneyTypes.Cash) >= amoutn)
                {
                    MoneyManager.Instance.RemoveMoney(targetPlayer, MoneyTypes.Cash, amoutn);
                    Utility.Instance.SendChatMessage(user.Source,"[Fines]",targetUser.CurrentCharacter.FullName+" has paid their fine with cash",255,0,0);
                }
                else if (MoneyManager.Instance.GetMoney(targetPlayer, MoneyTypes.Bank) >= amoutn)
                {
                    MoneyManager.Instance.RemoveMoney(targetPlayer,MoneyTypes.Bank, amoutn);
                    Utility.Instance.SendChatMessage(user.Source, "[Fines]", targetUser.CurrentCharacter.FullName + " has paid their fine with bank balance", 255, 0, 0);
                }
                else
                {
                    var newAmount = amoutn - MoneyManager.Instance.GetMoney(targetPlayer, MoneyTypes.Bank);
                    Utility.Instance.SendChatMessage(user.Source, "[Fines]", targetUser.CurrentCharacter.FullName + " has paid as much as they can of their fine with bank balance. ($"+ newAmount + " left)", 255, 0, 0);
                }
            }
        }

        public void OnDutyCommand(User user, string[] args)
        {
            SetDuty(user.Source,true);
        }

        public void OffDutyCommand(User user, string[] args)
        {
            SetDuty(user.Source, false);
        }

        public void ConfiscateCommand(User user, string[] args)
        {

            try
            {

                if (IsPlayerOnDuty(user.Source) || Admin.Instance.ActiveAdmins.Contains(user.Source))
                {
                    if (args.Length < 2)
                    {
                        Utility.Instance.SendChatMessage(user.Source, "[Confiscate]", "Invalid amount of parameters", 255, 0, 0);
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

                    Utility.Instance.SendChatMessage(user.Source, "[Confiscate]", "You have confiscated all illegal items " +
                                                                                  "from " + UserManager.Instance.GetUserFromPlayer(targetPlayer).CurrentCharacter.FullName, 255, 0, 0);

                    Utility.Instance.SendChatMessage(targetPlayer, "[Confiscate]", "All of your illegal items have been confiscated " +
                                                                                   "by " + UserManager.Instance.GetUserFromPlayer(user.Source).CurrentCharacter.FullName, 255, 0, 0);
                    InventoryManager.Instance.ConfiscateItems(targetPlayer);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void ConfiscateWeapons(User user, string[] args)
        {
            try
            {
                if (IsPlayerOnDuty(user.Source) || Admin.Instance.ActiveAdmins.Contains(user.Source))
                {
                    if (args.Length < 2)
                    {
                        Utility.Instance.SendChatMessage(user.Source, "[Confiscate]", "Invalid amount of parameters", 255, 0, 0);
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
                    Utility.Instance.SendChatMessage(user.Source, "[Confiscate]", "You have confiscated all weapons " +
                                                                                  "from " + UserManager.Instance.GetUserFromPlayer(targetPlayer).CurrentCharacter.FullName, 255, 0, 0);

                    Utility.Instance.SendChatMessage(targetPlayer, "[Confiscate]", "All of your weapons have been confiscated " +
                                                                                   "by " + UserManager.Instance.GetUserFromPlayer(user.Source).CurrentCharacter.FullName, 255, 0, 0);
                    InventoryManager.Instance.ConfiscateItems(targetPlayer);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void PanicButtonCommand(User user, string[] args)
        {
            if (Police.Instance.IsPlayerOnDuty(user.Source))
            {
                TriggerClientEvent(user.Source,"911CallClient", "OFFICER IN DISTRESS");
                TriggerClientEvent(user.Source, "911CallClient", "OFFICER IN DISTRESS");
                TriggerClientEvent(user.Source, "911CallClient", "OFFICER IN DISTRESS");
                TriggerClientEvent(user.Source, "911CallClient", "OFFICER IN DISTRESS");
                TriggerClientEvent(user.Source, "911CallClient", "OFFICER IN DISTRESS");
                TriggerClientEvent(user.Source, "911CallClient", "OFFICER IN DISTRESS");
                TriggerClientEvent(user.Source, "911CallClient", "OFFICER IN DISTRESS");
            }
        }

        public async Task SetupCommands()
        {
            await Delay(500);
            while (CommandManager.Instance == null)
            {
                await Delay(0);
            }
            CommandManager.Instance.AddCommand("jail", JailPlayerCommand);
            CommandManager.Instance.AddCommand("shield", RiotShieldCommand);
            CommandManager.Instance.AddCommand("fine", FinePlayerCommand);
            CommandManager.Instance.AddCommand("unjail", UnjailPlayerCommand);
            CommandManager.Instance.AddCommand("addcop", AddCopCommand);
            CommandManager.Instance.AddCommand("copadd", AddCopCommand);
            CommandManager.Instance.AddCommand("remcop", RemoveCopCommand);
            CommandManager.Instance.AddCommand("coprem", RemoveCopCommand);
            CommandManager.Instance.AddCommand("setcoprank", PromoteCopCommand);
            CommandManager.Instance.AddCommand("coprank", PromoteCopCommand);
            CommandManager.Instance.AddCommand("coppromote", PromoteCopCommand);
            CommandManager.Instance.AddCommand("policeonduty", OnDutyCommand);
            CommandManager.Instance.AddCommand("policeoffduty", OffDutyCommand);
            CommandManager.Instance.AddCommand("coponduty", OnDutyCommand);
            CommandManager.Instance.AddCommand("copoffduty", OffDutyCommand);
            CommandManager.Instance.AddCommand("confiscate", ConfiscateCommand);
            CommandManager.Instance.AddCommand("confiscateweapons", ConfiscateWeapons);
            CommandManager.Instance.AddCommand("panic", PanicButtonCommand); 
        }

        #endregion

    }

    public class Jail : BaseScript
    {
        public static Jail Instance;

        public Jail()
        {
            Instance = this;
            EventHandlers["UpdateJailTime"] += new Action<Player,int>(UpdateJailTime);
        }

        private void UpdateJailTime([FromSource]Player ply, int time)
        {
            //THIS IS VERY INSECURE AND CAN  EASIULY BE MANIPUALTED FIND A BETTER WAY.
            var user = UserManager.Instance.GetUserFromPlayer(ply);
            if (user != null && user.CurrentCharacter!=null)
            {
                user.CurrentCharacter.JailTime = time;
            }
        }
    }

}
