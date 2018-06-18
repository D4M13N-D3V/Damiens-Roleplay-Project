using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using server.Main.Users;

namespace server.Main.EmergencyServices.EMS
{

    public class EMS : BaseScript
    {
        /// <summary>
        /// Singleton Instance
        /// </summary>
        public static EMS Instance;

        public EMS()
        {
            Instance = this;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            Paycheck();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            LoadEMS();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            SetupCommands();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.

            EventHandlers["ReviveRequest"] += new Action<Player, int>(ReviveRequest);

        }

        #region Private Variables
        /// <summary>
        /// All the ems that are in the database
        /// </summary>
        public Dictionary<int, EMSMember> LoadedEms = new Dictionary<int, EMSMember>();
        /// <summary>
        /// All the ems that are on duty.
        /// </summary>
        public Dictionary<User, EMSMember> OnDutyEms = new Dictionary<User, EMSMember>();
        /// <summary>
        /// All of the ems ranks configured.
        /// </summary>
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
        /// <summary>
        /// Handles the paycheck
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Eventhandler for when someone attempts to revive someone
        /// </summary>
        /// <param name="ply">the player triggering the revive</param>
        /// <param name="target">The target id of who they are reviving.</param>
        public void ReviveRequest([FromSource] Player ply, int target)
        {
            var plyList = new PlayerList();
            var tgtPly = plyList[target];
            TriggerClientEvent(tgtPly, "Revive");
        }
        /// <summary>
        /// Can the player promote?
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool CanPromote(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var officer = GetEMSObjectByName(user.CurrentCharacter.FullName);
            return Admin.Instance.ActiveAdmins.Contains(player) || officer != null && EmsRanks.ContainsKey(officer.Rank) && EmsRanks[officer.Rank].CanPromote;
        }

        /// <summary>
        /// Adds given player to EMS.
        /// </summary>
        /// <param name="player"></param>
        public void AddEMS(Player player)
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

        /// <summary>
        /// Removes the given player from EMS.
        /// </summary>
        /// <param name="player"></param>
        public void RemoveEMS(Player player)
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

        /// <summary>
        /// Promotes the given player to the given rank.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="rank"></param>
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
                Utility.Instance.SendChatMessageAll("[EMS]", user.CurrentCharacter.FullName + " has been promoted to " + rank + " in the San Andreas Medical Services.", 0, 0, 180);
            }
        }

        /// <summary>
        /// Sets the player on/off duty as ems (True is on false is off )
        /// </summary>
        /// <param name="player"></param>
        /// <param name="onDuty"></param>
        public void SetDuty(Player player, bool onDuty)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (!IsPlayerEMS(player)) { return; }
            if (onDuty && !IsPlayerOnDuty(player))
            {
                var officer = GetEMSObjectByName(user.CurrentCharacter.FullName);
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

        /// <summary>
        /// Loads all the ems from the database.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if the player is EMS
        /// </summary>
        /// <param name="player"></param>
        /// <returns>returns true if yes.</returns>
        public bool IsPlayerEMS(Player player)
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

        /// <summary>
        /// Checks if the player is on duty ems
        /// </summary>
        /// <param name="player"></param>
        /// <returns>true if yes</returns>
        public bool IsPlayerOnDuty(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (OnDutyEms.ContainsKey(user))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a ems object form the loaded ems wit ha character that matches the same name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>the ems object</returns>
        public EMSMember GetEMSObjectByName(string name)
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
            if (CanPromote(user.Source) && !IsPlayerEMS(targetPlayer))
            {
                AddEMS(targetPlayer);
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
            if (CanPromote(user.Source) && IsPlayerEMS(targetPlayer))
            {
                RemoveEMS(targetPlayer);
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
            if (CanPromote(user.Source) && IsPlayerEMS(targetPlayer) && EmsRanks.ContainsKey(rank))
            {
                PromoteCop(targetPlayer, rank);
            }
        }

        public void HospitalPlayerCommand(User user, string[] args)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void UnhospitalPlayerCommand(User user, string[] args)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("hospital", HospitalPlayerCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("unhospital", UnhospitalPlayerCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("addems", AddEMSCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("emsadd", AddEMSCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("remems", RemoveEMSCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("emsrem", RemoveEMSCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("setemsrank", PromoteEMSCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("emsrank", PromoteEMSCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("emspromote", PromoteEMSCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("emsonduty", OnDutyCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("emsoffduty", OffDutyCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }

        #endregion

    }
}
