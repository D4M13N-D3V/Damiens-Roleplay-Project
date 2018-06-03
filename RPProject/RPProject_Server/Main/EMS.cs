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
    public enum EMSDepartments
    {
        EMS,
        FIRE,
    }
    public class EMSRank
    {
        public string Name = "None";
        public int Salary = 0;
        public EMSDepartments Department = EMSDepartments.EMS;
        public List<string> AvailableVehicles = new List<string>()
        {

        };
        public bool CanUseHeli = true;
        public bool CanUseSpikeStrips = true;
        public bool CanUseRadar = true;
        public bool CanUseK9 = true;
        public bool CanUseEMS = true;
        public bool CanPromote = false;
        public EMSRank(string name, int salary, EMSDepartments department, List<string> availableVehicles, bool canUseHeli, bool canPromote)
        {
            Department = department;
            Name = name;
            Salary = salary;
            AvailableVehicles = availableVehicles;
            CanPromote = canPromote;
            CanUseHeli = canUseHeli;
        }
    }
    public class EMSMember
    {
        public string SteamId;
        public string Name;
        public string Rank;
        public int Badge;

        public EMSMember(string steamId, string name, string rank, int badge)
        {
            SteamId = steamId;
            Name = name;
            Rank = rank;
            Badge = badge;
        }
    }

    public class EMS : BaseScript
    {
        public static EMS Instance;

        public EMS()
        {
            Instance = this;
            Paycheck();
            SetupCommands();
        }

        #region Private Variables
        private Dictionary<int, EMSMember> _loadedEMS = new Dictionary<int, EMSMember>();
        private Dictionary<User, EMSMember> _onDutyEMS = new Dictionary<User, EMSMember>();
        private Dictionary<string, EMSRank> _emsRanks = new Dictionary<string, EMSRank>()
        {
            ["Cadet"] = new EMSRank( // Rnak Name
                "Cadet", // Rank Name 
                1000, // Rnak Salary
                EMSDepartments.EMS, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                {
                    "ambulance"
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
                foreach (var user in _onDutyEMS.Keys)
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
            return Admin.Instance.ActiveAdmins.Contains(player) || officer != null && _emsRanks.ContainsKey(officer.Rank) && _emsRanks[officer.Rank].CanPromote;
        }

        public void AddCop(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);

            var newid = 0;
            if (!_loadedEMS.Any())
            {
                newid = 1;
            }
            else
            {
                newid = _loadedEMS.Last().Key + 1;
            }
            var officer = new EMSMember(user.SteamId, user.CurrentCharacter.FullName, "Cadet", newid);
            _loadedEMS.Add(officer.Badge, officer);

            DatabaseManager.Instance.Execute("INSERT INTO EMS (badge,emsinfo) VALUES(" + officer.Badge + ",'" + JsonConvert.SerializeObject(officer) + "');");
        }

        public void RemoveCop(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var chara = user.CurrentCharacter;

            var keyToRemove = -1;
            foreach (var officer in _loadedEMS)
            {
                if (officer.Value.Name == chara.FullName)
                {
                    keyToRemove = officer.Key;
                    break;
                }
            }
            _loadedEMS.Remove(keyToRemove);
            DatabaseManager.Instance.Execute("DELETE FROM EMS WHERE badge = " + keyToRemove + ";");
        }

        public void PromoteCop(Player player, string rank)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);

            var chara = user.CurrentCharacter;

            if (_emsRanks.ContainsKey(rank))
            {
                var officerKey = GetLoadedEMSKeyByName(chara.FullName);
                _loadedEMS[officerKey].Rank = rank;

                if (IsPlayerOnDuty(player))
                {
                    _onDutyEMS[user].Rank = rank;
                }
                DatabaseManager.Instance.Execute("UPDATE EMS SET emsinfo='" + JsonConvert.SerializeObject(_loadedEMS[officerKey]) + "' WHERE badge=" + officerKey + ";");
            }
        }

        public void SetDuty(Player player, bool onDuty)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (!IsPlayerCop(player)) { return; }
            if (onDuty && !IsPlayerOnDuty(player))
            {
                var officer = GetOfficerObjectByName(user.CurrentCharacter.FullName);
                _onDutyEMS.Add(user, officer);
                TriggerClientEvent(player, "EMS:SetOnDuty", Convert.ToString(_emsRanks[officer.Rank].Department));
                TriggerClientEvent(player, "UpdateEMSCars", _emsRanks[officer.Rank].AvailableVehicles);
            }
            else if (!onDuty && IsPlayerOnDuty(player))
            {
                _onDutyEMS.Remove(user);
                TriggerClientEvent(player, "EMS:SetOffDuty");
            }
            else
            {
                return;
            }
            TriggerClientEvent("EMS:RefreshOnDutyOfficers", _onDutyEMS.Count);
        }

        public async void LoadEMS()
        {
            while (DatabaseManager.Instance == null)
            {
                await Delay(100);
            }
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM EMS");
            while (data.Read())
            {
                var officer = JsonConvert.DeserializeObject<EMSMember>(Convert.ToString(data["emsinfo"]));
                _loadedEMS.Add(officer.Badge, officer);
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
            foreach (var officer in _loadedEMS)
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
            if (_onDutyEMS.ContainsKey(user))
            {
                return true;
            }

            return false;
        }

        public EMSMember GetOfficerObjectByName(string name)
        {
            foreach (var officer in _loadedEMS)
            {
                if (officer.Value.Name == name)
                {
                    return officer.Value;
                }
            }
            return null;
        }

        public int GetLoadedEMSKeyByName(string name)
        {
            foreach (var officer in _loadedEMS.Keys)
            {
                if (_loadedEMS[officer].Name == name)
                {
                    return officer;
                }
            }
            return -1;
        }
        #endregion

        #region Commands

        public void AddEMSCommand(User user, string[] args)
        {
            if (args.Length < 2) { Utility.Instance.SendChatMessage(user.Source, "[EMS]", "Invalid parameter count.", 0, 255, 0); return; }
            var plyList = new PlayerList();
            var targetPlayer = plyList[Convert.ToInt32(args[1])];
            if (targetPlayer == null) { Utility.Instance.SendChatMessage(user.Source, "[EMS]", "Invalid player provided.", 0, 255, 0); return; }
            if (CanPromote(user.Source) && !IsPlayerCop(targetPlayer))
            {
                AddCop(targetPlayer);
            }
        }

        public void RemoveEMSCommand(User user, string[] args)
        {
            if (args.Length < 2) { Utility.Instance.SendChatMessage(user.Source, "[EMS]", "Invalid parameter count.", 0, 255, 0); return; }
            var plyList = new PlayerList();
            var targetPlayer = plyList[Convert.ToInt32(args[1])];
            if (targetPlayer == null) { Utility.Instance.SendChatMessage(user.Source, "[EMS]", "Invalid player provided.", 0, 255, 0); return; }
            if (CanPromote(user.Source) && IsPlayerCop(targetPlayer))
            {
                RemoveCop(targetPlayer);
            }
        }

        public void PromoteEMSCommand(User user, string[] args)
        {
            if (args.Length < 3) { Utility.Instance.SendChatMessage(user.Source, "[EMS]", "Invalid parameter count.", 0, 255, 0); return; }
            var plyList = new PlayerList();
            var targetPlayer = plyList[Convert.ToInt32(args[1])];
            args[1] = null;
            args[0] = null;
            var rank = String.Join(" ", args);
            rank = rank.Remove(0, 2);
            if (targetPlayer == null) { Utility.Instance.SendChatMessage(user.Source, "[EMS]", "Invalid player provided.", 0, 255, 0); return; }
            if (CanPromote(user.Source) && IsPlayerCop(targetPlayer) && _emsRanks.ContainsKey(rank))
            {
                PromoteCop(targetPlayer, rank);
            }
        }

        public void HospitalPlayerCommand(User user, string[] args)
        {
            if (IsPlayerOnDuty(user.Source) || Admin.Instance.ActiveAdmins.Contains(user.Source))
            {
                if (args.Length < 3)
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Hospital]", "Invalid amount of parameters", 255, 0, 0);
                    return;
                }
                var plyList = new PlayerList();
                var targetPlayer = plyList[Convert.ToInt32(args[1])];
                if (targetPlayer == null) { Utility.Instance.SendChatMessage(user.Source, "[Hospital]", "Invalid player provided.", 0, 0, 255); return; }
                UserManager.Instance.GetUserFromPlayer(targetPlayer).CurrentCharacter.JailTime = Convert.ToInt32(args[2]) * 60;
                TriggerClientEvent(targetPlayer, "Hospital", Convert.ToInt32(args[2]) * 60);
            }
        }

        public void UnhospitalPlayerCommand(User user, string[] args)
        {
            if (IsPlayerOnDuty(user.Source) || Admin.Instance.ActiveAdmins.Contains(user.Source))
            {
                if (args.Length < 2)
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Hospital]", "Invalid amount of parameters", 255, 0, 0);
                    return;
                }

                var plyList = new PlayerList();
                var targetPlayer = plyList[Convert.ToInt32(args[1])];



                if (targetPlayer == null)
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Hospital]", "Invalid player provided.", 0, 0, 255);
                    return;
                }
                TriggerClientEvent(targetPlayer, "Unhospital");
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

        public async void SetupCommands()
        {
            await Delay(500);
            while (CommandManager.Instance == null)
            {
                await Delay(0);
            }
            CommandManager.Instance.AddCommand("hospital", HospitalPlayerCommand);
            CommandManager.Instance.AddCommand("unhospital", UnhospitalPlayerCommand);
            CommandManager.Instance.AddCommand("addems", AddEMSCommand);
            CommandManager.Instance.AddCommand("emsadd", AddEMSCommand);
            CommandManager.Instance.AddCommand("remems", RemoveEMSCommand);
            CommandManager.Instance.AddCommand("emsrem", RemoveEMSCommand);
            CommandManager.Instance.AddCommand("setemsrank", PromoteEMSCommand);
            CommandManager.Instance.AddCommand("emsrank", PromoteEMSCommand);
            CommandManager.Instance.AddCommand("emspromote", PromoteEMSCommand);
            CommandManager.Instance.AddCommand("emsonduty", OnDutyCommand);
            CommandManager.Instance.AddCommand("emsoffduty", OffDutyCommand);
        }

        #endregion

    }

    public class Hospital : BaseScript
    {
        public static Hospital Instance;

        public Hospital()
        {
            Instance = this;
            EventHandlers["UpdateHospitalTime"] += new Action<Player, int>(UpdateHospitalTime);
        }

        private void UpdateHospitalTime([FromSource]Player ply, int time)
        {
            //THIS IS VERY INSECURE AND CAN  EASIULY BE MANIPUALTED FIND A BETTER WAY.
            var user = UserManager.Instance.GetUserFromPlayer(ply);
            user.CurrentCharacter.HospitalTime = time;
        }
    }

}
