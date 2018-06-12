using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using server.Main.Users;

namespace server.Main.EmergencyServices
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
            LoadEMS();
            SetupCommands();

            EventHandlers["ReviveRequest"] += new Action<Player, int>(ReviveRequest);

        }

        #region Private Variables
        public Dictionary<int, EMSMember> LoadedEms = new Dictionary<int, EMSMember>();
        public Dictionary<User, EMSMember> OnDutyEms = new Dictionary<User, EMSMember>();
        public Dictionary<string, EMSRank> EmsRanks = new Dictionary<string, EMSRank>()
        {
            ["Volunteer"] = new EMSRank( // Rnak Name
                "Volunteer", // Rank Name 
                2500, // Rnak Salary
                EMSDepartments.EMS, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                {
                    "ambulance",
                    "ambulance2",
                },
                false, // Can Use Air1
                false  // Can Promote
            ),
            ["Recruit"] = new EMSRank( // Rnak Name
                "Recruit", // Rank Name 
                2750, // Rnak Salary
                EMSDepartments.EMS, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                {
                    "ambulance",
                    "ambulance2",
                },
                false, // Can Use Air1
                false  // Can Promote
            ),
            ["Paramedic"] = new EMSRank( // Rnak Name
                "Paramedic", // Rank Name 
                3000, // Rnak Salary
                EMSDepartments.EMS, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                {
                    "ambulance",
                    "ambulance2",
                    "emsvan",
                },
                true, // Can Use Air1
                false  // Can Promote
            ),
            ["Advanced Paramedic"] = new EMSRank( // Rnak Name
                "Advanced Paramedic", // Rank Name 
                3250, // Rnak Salary
                EMSDepartments.EMS, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                {
                    "ambulance",
                    "ambulance2",
                    "emsvan",
                    "ems1",
                },
                true, // Can Use Air1
                false  // Can Promote
            ),
            ["Lieutenant"] = new EMSRank( // Rnak Name
                "Lieutenant", // Rank Name 
                3500, // Rnak Salary
                EMSDepartments.EMS, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                {
                    "ambulance",
                    "ambulance2",
                    "emsvan",
                    "ems1",
                    "ems2",
                    "ems4",
                },
                true, // Can Use Air1
                false  // Can Promote
            ),
            ["Assistant Captain"] = new EMSRank( // Rnak Name
                "Assistant Captain", // Rank Name 
                4000, // Rnak Salary
                EMSDepartments.EMS, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                {
                    "ambulance",
                    "ambulance2",
                    "emsvan",
                    "ems1",
                    "ems2",
                    "ems4",
                },
                true, // Can Use Air1
                false  // Can Promote
            ),
            ["Captain"] = new EMSRank( // Rnak Name
                "Captain", // Rank Name 
                5000, // Rnak Salary
                EMSDepartments.EMS, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                {
                    "ambulance",
                    "ambulance2",
                    "emsvan",
                    "ems1",
                    "ems2",
                    "ems4",
                },
                true, // Can Use Air1
                false  // Can Promote
            ),
            ["Assistant Superintendent"] = new EMSRank( // Rnak Name
                "Assistant Superintendent", // Rank Name 
                6000, // Rnak Salary
                EMSDepartments.EMS, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                {
                    "ambulance",
                    "ambulance2",
                    "emsvan",
                    "ems1",
                    "ems2",
                    "ems4",
                    "ems6",
                },
                true, // Can Use Air1
                false  // Can Promote
            ),
            ["Superintendent"] = new EMSRank( // Rnak Name
                "Superintendent", // Rank Name 
                7000, // Rnak Salary
                EMSDepartments.EMS, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                {
                    "ambulance",
                    "ambulance2",
                    "emsvan",
                    "ems1",
                    "ems2",
                    "ems4",
                    "ems6",
                },
                true, // Can Use Air1
                true  // Can Promote
            ),
            ["Superintendent Chief"] = new EMSRank( // Rnak Name
                "Superintendent Chief", // Rank Name 
                9500, // Rnak Salary
                EMSDepartments.EMS, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                {
                    "ambulance",
                    "ambulance2",
                    "emsvan",
                    "ems1",
                    "ems2",
                    "ems4",
                    "ems6",
                    "lguard"
                },
                true, // Can Use Air1
                true  // Can Promote
            ),
            ["Chief"] = new EMSRank( // Rnak Name
                "Chief", // Rank Name 
                10000, // Rnak Salary
                EMSDepartments.EMS, // Rank Departments ( LSPD,BCSO,LSCSO,SASP,SAHP,SAAO,USMS,FBI,DEA )
                new List<string>() // Rank Vehicle Selection https://wiki.gtanet.work/index.php?title=Vehicle_Models
                {
                    "ambulance",
                    "ambulance2",
                    "emsvan",
                    "ems1",
                    "ems2",
                    "ems3",
                    "ems4",
                    "ems5",
                    "ems6",
                    "lguard"
                },
                true, // Can Use Air1
                true  // Can Promote
            ),
        };
        #endregion

        #region Paycheck

        private async Task Paycheck()
        {
            while (true)
            {
                await Delay(600000);
                foreach (var user in OnDutyEms.Keys)
                {
                    if (user != null && user.Source != null)
                    {
                        MoneyManager.Instance.AddMoney(user.Source, MoneyTypes.Bank, EmsRanks[OnDutyEms[user].Rank].Salary);
                    }
                    else
                    {
                        OnDutyEms.Remove(user);
                    }
                }
            }
        }

        #endregion

        #region Functioanlity Methods

        public void ReviveRequest([FromSource] Player ply, int target)
        {
            var plyList = new PlayerList();
            var tgtPly = plyList[target];
            TriggerClientEvent(tgtPly,"Revive");
        }

        public bool CanPromote(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var officer = GetOfficerObjectByName(user.CurrentCharacter.FullName);
            return Admin.Instance.ActiveAdmins.Contains(player) || officer != null && EmsRanks.ContainsKey(officer.Rank) && EmsRanks[officer.Rank].CanPromote;
        }

        public void AddCop(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);

            var newid = 0;
            if (!LoadedEms.Any())
            {
                newid = 1;
            }
            else
            {
                newid = LoadedEms.Last().Key + 1;
            }
            var officer = new EMSMember(user.SteamId, user.CurrentCharacter.FullName, "Volunteer", newid);
            LoadedEms.Add(officer.Badge, officer);

            DatabaseManager.Instance.Execute("INSERT INTO EMS (badge,emsinfo) VALUES(" + officer.Badge + ",'" + JsonConvert.SerializeObject(officer) + "');");
            Utility.Instance.SendChatMessageAll("[EMS]", user.CurrentCharacter.FullName + " has been hired into the San Andreas Medical Services.", 0, 0, 180);
        }

        public void RemoveCop(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var chara = user.CurrentCharacter;

            var keyToRemove = -1;
            foreach (var officer in LoadedEms)
            {
                if (officer.Value.Name == chara.FullName)
                {
                    keyToRemove = officer.Key;
                    break;
                }
            }
            LoadedEms.Remove(keyToRemove);
            DatabaseManager.Instance.Execute("DELETE FROM EMS WHERE badge = " + keyToRemove + ";");
            Utility.Instance.SendChatMessageAll("[EMS]", user.CurrentCharacter.FullName + " has been fired from the San Andreas Medical Services.", 0, 0, 180);
        }

        public void PromoteCop(Player player, string rank)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);

            var chara = user.CurrentCharacter;

            if (EmsRanks.ContainsKey(rank))
            {
                var officerKey = GetLoadedEMSKeyByName(chara.FullName);
                LoadedEms[officerKey].Rank = rank;

                if (IsPlayerOnDuty(player))
                {
                    OnDutyEms[user].Rank = rank;
                }
                DatabaseManager.Instance.Execute("UPDATE EMS SET emsinfo='" + JsonConvert.SerializeObject(LoadedEms[officerKey]) + "' WHERE badge=" + officerKey + ";");
                Utility.Instance.SendChatMessageAll("[EMS]", user.CurrentCharacter.FullName + " has been promoted to "+rank+" in the San Andreas Medical Services.", 0, 0, 180);
            }
        }

        public void SetDuty(Player player, bool onDuty)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (!IsPlayerCop(player)) { return; }
            if (onDuty && !IsPlayerOnDuty(player))
            {
                var officer = GetOfficerObjectByName(user.CurrentCharacter.FullName);
                OnDutyEms.Add(user, officer);
                TriggerClientEvent(player, "EMS:SetOnDuty", Convert.ToString(EmsRanks[officer.Rank].Department));
                TriggerClientEvent(player, "UpdateEMSCars", EmsRanks[officer.Rank].AvailableVehicles);
            }
            else if (!onDuty && IsPlayerOnDuty(player))
            {
                OnDutyEms.Remove(user);
                TriggerClientEvent(player, "EMS:SetOffDuty");
            }
            else
            {
                return;
            }
            TriggerClientEvent("EMS:RefreshOnDutyOfficers", OnDutyEms.Count);
        }

        public async Task LoadEMS()
        {
            while (DatabaseManager.Instance == null && DatabaseManager.Instance.Connection.State == ConnectionState.Open)
            {
                await Delay(100);
            }
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM EMS").Result;
            while (data.Read())
            {
                var officer = JsonConvert.DeserializeObject<EMSMember>(Convert.ToString(data["emsinfo"]));
                LoadedEms.Add(officer.Badge, officer);
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
            foreach (var officer in LoadedEms)
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
            if (OnDutyEms.ContainsKey(user))
            {
                return true;
            }

            return false;
        }

        public EMSMember GetOfficerObjectByName(string name)
        {
            foreach (var officer in LoadedEms)
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
            foreach (var officer in LoadedEms.Keys)
            {
                if (LoadedEms[officer].Name == name)
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
            var id = 0;
            if (!Int32.TryParse(args[1], out id))
            {
                    Utility.Instance.SendChatMessage(user.Source, "[EMS]", "Invalid parameters", 255, 0, 0);
                    return;
            }
            PlayerList plyList = new PlayerList();
            Player targetPlayer = plyList[id];
            if (targetPlayer == null) { Utility.Instance.SendChatMessage(user.Source, "[Hospital]", "Invalid player provided.", 0, 0, 255); return; }
            if (CanPromote(user.Source) && !IsPlayerCop(targetPlayer))
            {
                AddCop(targetPlayer);
            }
        }

        public void RemoveEMSCommand(User user, string[] args)
        {
            if (args.Length < 2) { Utility.Instance.SendChatMessage(user.Source, "[EMS]", "Invalid parameter count.", 0, 255, 0); return; }
            var id = 0;
            if (!Int32.TryParse(args[1], out id))
            {
                Utility.Instance.SendChatMessage(user.Source, "[EMS]", "Invalid parameters", 255, 0, 0);
                return;
            }
            PlayerList plyList = new PlayerList();
            Player targetPlayer = plyList[id];
            if (targetPlayer == null) { Utility.Instance.SendChatMessage(user.Source, "[Hospital]", "Invalid player provided.", 0, 0, 255); return; }
            if (CanPromote(user.Source) && IsPlayerCop(targetPlayer))
            {
                RemoveCop(targetPlayer);
            }
        }

        public void PromoteEMSCommand(User user, string[] args)
        {
            if (args.Length < 3) { Utility.Instance.SendChatMessage(user.Source, "[EMS]", "Invalid parameter count.", 0, 255, 0); return; }
            var id = 0;
            if (!Int32.TryParse(args[1], out id))
            {
                Utility.Instance.SendChatMessage(user.Source, "[EMS]", "Invalid parameters", 255, 0, 0);
                return;
            }
            PlayerList plyList = new PlayerList();
            Player targetPlayer = plyList[id];
            if (targetPlayer == null) { Utility.Instance.SendChatMessage(user.Source, "[Hospital]", "Invalid player provided.", 0, 0, 255); return; }
            args[1] = null;
            args[0] = null;
            var rank = String.Join(" ", args);
            rank = rank.Remove(0, 2);
            if (CanPromote(user.Source) && IsPlayerCop(targetPlayer) && EmsRanks.ContainsKey(rank))
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
                var id = 0;
                if (!Int32.TryParse(args[1], out id))
                {
                    Utility.Instance.SendChatMessage(user.Source, "[EMS]", "Invalid parameters", 255, 0, 0);
                    return;
                }
                PlayerList plyList = new PlayerList();
                Player targetPlayer = plyList[id];
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
               
                var id = 0;
                if (!Int32.TryParse(args[1], out id))
                {
                    Utility.Instance.SendChatMessage(user.Source, "[EMS]", "Invalid parameters", 255, 0, 0);
                    return;
                }
                PlayerList plyList = new PlayerList();
                Player targetPlayer = plyList[id];
                if (targetPlayer == null) { Utility.Instance.SendChatMessage(user.Source, "[Hospital]", "Invalid player provided.", 0, 0, 255); return; }
                TriggerClientEvent(targetPlayer, "Unhospital");
                var tgtUser = UserManager.Instance.GetUserFromPlayer(targetPlayer);
                tgtUser.CurrentCharacter.JailTime = 0;
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

        public async Task SetupCommands()
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
