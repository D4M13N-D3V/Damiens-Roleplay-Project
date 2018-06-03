using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using roleplay.Main.Users;
using roleplay.Main.Users.CharacterClasses;

namespace roleplay.Main
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
        }

        #region Private Variables
        private Dictionary<int,PoliceOfficer> _loadedOfficers = new Dictionary<int, PoliceOfficer>();
        private Dictionary<User,PoliceOfficer> _onDutyOfficers = new Dictionary<User, PoliceOfficer>();
        private Dictionary<string,PoliceRank> _policeRanks = new Dictionary<string, PoliceRank>()
        {
            ["Cadet"] = new PoliceRank( // Rnak Name
                "Cadet", // Rank Name 
                1000, // Rnak Salary
                LEODepartments.SASP, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                {
                    "bx1blue",
                    "bx2blue",
                    "bx3blue",
                    "bx4blue",
                    "sheriff2blue",
                    "police6blue"
                },
                false, // Can Use Air1
                false  // Can Promote
            )
        };
        #endregion

        #region Paycheck

        private async void Paycheck()
        {
            while (true)
            {
                await Delay(600000);
                foreach (var user in _onDutyOfficers.Keys)
                {
                    //MoneyManager.Instance.AddMoney(user.Source,MoneyTypes.Bank, _policeRanks[_onDutyOfficers[user].Rank].Salary);
                }
            }
        }
        
        #endregion

        #region Functioanlity Methods

        public bool CanPromote(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var officer = GetOfficerObjectByName(user.CurrentCharacter.FullName);
            return Admin.Instance.ActiveAdmins.Contains(player) || officer!=null && _policeRanks.ContainsKey(officer.Rank) && _policeRanks[officer.Rank].CanPromote;
        }

        public void AddCop(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);

            var newid = 0;
            if (!_loadedOfficers.Any())
            {
                newid = 1;
            }
            else
            {
                newid = _loadedOfficers.Last().Key + 1;
            }
            var officer = new PoliceOfficer(user.SteamId,user.CurrentCharacter.FullName,"Cadet",newid);
            _loadedOfficers.Add(officer.Badge,officer);

            DatabaseManager.Instance.Execute("INSERT INTO POLICE (badge,officerinfo) VALUES(" + officer.Badge+",'"+JsonConvert.SerializeObject(officer)+"');");
        }

        public void RemoveCop(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var chara = user.CurrentCharacter;

            var keyToRemove = -1;
            foreach (var officer in _loadedOfficers)
            {
                if (officer.Value.Name == chara.FullName)
                {
                    keyToRemove = officer.Key;
                    break;
                }
            }
            _loadedOfficers.Remove(keyToRemove);
            DatabaseManager.Instance.Execute("DELETE FROM POLICE WHERE badge = " + keyToRemove + ";");
        }

        public void PromoteCop(Player player, string rank)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);

            var chara = user.CurrentCharacter;
            
            if (_policeRanks.ContainsKey(rank))
            {
                var officerKey = GetLoadedOfficerKeyByName(chara.FullName);
                _loadedOfficers[officerKey].Rank = rank;

                if (IsPlayerOnDuty(player))
                {
                    _onDutyOfficers[user].Rank = rank;
                }
                DatabaseManager.Instance.Execute("UPDATE POLICE SET officerinfo='"+JsonConvert.SerializeObject(_loadedOfficers[officerKey])+"' WHERE badge="+officerKey+";");
            }
        }

        public void SetDuty(Player player, bool onDuty)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (!IsPlayerCop(player)){  return; }
            if (onDuty && !IsPlayerOnDuty(player))
            {
                var officer = GetOfficerObjectByName(user.CurrentCharacter.FullName);
                _onDutyOfficers.Add(user,officer);
                TriggerClientEvent(player, "Police:SetOnDuty",Convert.ToString(_policeRanks[officer.Rank].Department));
                TriggerClientEvent(player, "UpdatePoliceCars", _policeRanks[officer.Rank].AvailableVehicles);
            }
            else if(!onDuty && IsPlayerOnDuty(player))
            {
                _onDutyOfficers.Remove(user);
                TriggerClientEvent(player,"Police:SetOffDuty");
            }
            else
            {
                return;
            }
            TriggerClientEvent("Police:RefreshOnDutyOfficers", _onDutyOfficers.Count);
        }

        public async void LoadCops()
        {   
            while (DatabaseManager.Instance==null)
            {
                await Delay(100);
            }
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM POLICE");
            while (data.Read())
            {
                var officer = JsonConvert.DeserializeObject<PoliceOfficer>(Convert.ToString(data["officerinfo"]));
                _loadedOfficers.Add(officer.Badge,officer);
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
            foreach (var officer in _loadedOfficers)
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
            if (_onDutyOfficers.ContainsKey(user))
            {
                return true;
            }

            return false;
        }

        public PoliceOfficer GetOfficerObjectByName(string name)
        {
            foreach (var officer in _loadedOfficers)
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
            foreach (var officer in _loadedOfficers.Keys)
            {
                if (_loadedOfficers[officer].Name == name)
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
            var list = new PlayerList();
            var targetPlayer = list[targetPlayerId];
            var targetUser = UserManager.Instance.GetUserFromPlayer(targetPlayer);
            var chatString = "Items Found : ";

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
        }

        #endregion  

        #region Commands

        public void AddCopCommand(User user, string[] args)
        {
            if (args.Length < 2) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid parameter count.", 0, 0, 255); return; }
            var plyList = new PlayerList();
            var targetPlayer = plyList[Convert.ToInt32(args[1])];
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
            var targetPlayer = plyList[Convert.ToInt32(args[1])];
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
            var targetPlayer = plyList[Convert.ToInt32(args[1])];
            args[1] = null;
            args[0] = null;
            var rank = String.Join(" ",args);
            rank = rank.Remove(0, 2);
            if (targetPlayer == null) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
            if (CanPromote(user.Source) && IsPlayerCop(targetPlayer) && _policeRanks.ContainsKey(rank))
            {
                PromoteCop(targetPlayer,rank);
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

        public async void SetupCommands()
        {
            await Delay(500);
            while (CommandManager.Instance == null)
            {
                await Delay(0);
            }
            CommandManager.Instance.AddCommand("addcop", AddCopCommand);
            CommandManager.Instance.AddCommand("copadd", AddCopCommand);
            CommandManager.Instance.AddCommand("remcop", RemoveCopCommand);
            CommandManager.Instance.AddCommand("coprem", RemoveCopCommand);
            CommandManager.Instance.AddCommand("setcoprank", PromoteCopCommand);
            CommandManager.Instance.AddCommand("coprank", PromoteCopCommand);
            CommandManager.Instance.AddCommand("coppromote", PromoteCopCommand);
            CommandManager.Instance.AddCommand("policeonduty", OnDutyCommand);
            CommandManager.Instance.AddCommand("policeoffduty", OffDutyCommand);
        }

        #endregion

    }
}
