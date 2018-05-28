using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using roleplay.Main.Users;
using roleplay.Main.Users.CharacterClasses;

namespace roleplay.Main
{
    /// <summary>
    /// LSPD - Los Santos Police Department
    /// BCSO - Blain County Sheriffs Office
    /// LSCSO - Los Santos County Sheriffs Office
    /// SASP - San Andreas State Police
    /// SAHP - San Andreas Highway Patrol
    /// SAAO - San Andreas Air Operations
    /// </summary>
    public enum LEODepartments
    {
        LSPD,
        BCSO,
        LSCSO,
        SASP,
        SAHP,
        SAAO,
        USMS,
        FBI,
        DEA
    }
    public class PoliceRank
    {
        public string Name = "None";
        public int Salary = 0;
        public LEODepartments Department = LEODepartments.LSPD;
        public List<string> WeaponLoadout = new List<string>()
        {

        };
        public List<string> AvailableVehicles = new List<string>()
        {

        };
        public bool CanUseHeli = true;
        public bool CanUseSpikeStrips = true;
        public bool CanUseRadar = true;
        public bool CanUseK9 = true;
        public bool CanUseEMS = true;
        public bool CanPromote = false;
        public PoliceRank(string name, int salary, LEODepartments department, List<string> weaponLoadout, List<string> availableVehicles, bool canUseSpikeStrips, bool canUseRadar, bool canUseK9, bool canUseEMS, bool canUseHeli, bool canPromote)
        {
            Department = department;
            Name = name;
            Salary = salary;
            WeaponLoadout = weaponLoadout;
            AvailableVehicles = availableVehicles;
            CanPromote = canPromote;
            CanUseSpikeStrips = canUseSpikeStrips;
            CanUseRadar = canUseRadar;
            CanUseK9 = canUseK9;
            CanUseEMS = canUseEMS;
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
                LEODepartments.LSPD, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                new List<string>() // Rank Weapon Loadout https://wiki.fivem.net/wiki/Weapons
                {

                },new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                {

                },
                false, // Can Use Spikestrips
                false, // Can Use Radar
                false, // Can Use K9
                false, // Can Use EMS Abilities
                false, // Can Use Air1
                false  // Can Promote
                ),
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
                    MoneyManager.Instance.AddMoney(user.Source,MoneyTypes.Bank, _policeRanks[_onDutyOfficers[user].Rank].Salary);
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
            DatabaseManager.Instance.StartQuery("DELETE FROM POLICE WHERE badge = " + keyToRemove + ";");
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
                DatabaseManager.Instance.Execute("UPDATE POLICE SET policeinfo='"+JsonConvert.SerializeObject(_loadedOfficers[officerKey])+"' WHERE badge="+officerKey+";");
            }
        }

        public void SetDuty(Player player, bool onDuty)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (onDuty && !_onDutyOfficers.ContainsKey(user))
            {
                var officer = GetOfficerObjectByName(user.CurrentCharacter.FullName);
                _onDutyOfficers.Add(user,officer);
                TriggerClientEvent(player,"Police:SetOffDuty");
            }
            else if(!onDuty && _onDutyOfficers.ContainsKey(user))
            {
                _onDutyOfficers.Remove(user);
                TriggerClientEvent(player, "Police:SetOnDuty");
            }
            else
            {
                return;
            }
            TriggerClientEvent("Police:RefreshOnDutyOfficers", _onDutyOfficers);
        }

        public void LoadCops()
        {   
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM POLICE");
            while (data.Read())
            {
                var officer = JsonConvert.DeserializeObject<PoliceOfficer>(Convert.ToString(data["officerinfo"]));
                _loadedOfficers.Add(officer.Badge,officer);
            }
            DatabaseManager.Instance.EndQuery(data);
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
            foreach (var officer in _loadedOfficers)
            {
                if (officer.Value.Name == name)
                {
                    return officer.Key;
                }
            }
            return -1;
        }
        #endregion

        #region Commands

        public void AddCopCommand(User user, string[] args)
        {
            if (args.Length < 2) { Utility.Instance.SendChatMessage(user.Source,"[POLICE]","Invalid parameter count.",0,0,255); return; }
            var plyList = new PlayerList();
            var targetPlayer = plyList[Convert.ToInt32(args[1])];
            if (targetPlayer==null) { Utility.Instance.SendChatMessage(user.Source, "[POLICE]", "Invalid player provided.", 0, 0, 255); return; }
            if (CanPromote(user.Source) && !IsPlayerCop(targetPlayer))
            {
                AddCop(targetPlayer);
            }
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
        }

        #endregion

    }
}
