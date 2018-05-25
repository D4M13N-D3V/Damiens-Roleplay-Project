using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
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
        public User User;
        public string Rank;
        public int Badge;
        public bool OnDuty;

        public PoliceOfficer( string rank, int badge, bool onDuty)
        {
            Rank = rank;
            Badge = badge;
            OnDuty = onDuty;
        }
    }

    public class Police : BaseScript
    {
        public static Police Instance;

        public Police()
        {
            Instance = this;
            SetupCommands();
        }

        /// <summary>
        /// This is a list that the players are the keys and their ranks are the values.
        /// </summary>
        private Dictionary<string, PoliceOfficer> _officers = new Dictionary<string, PoliceOfficer>();

        private Dictionary<User,PoliceOfficer> _activeOfficers = new Dictionary<User, PoliceOfficer>();

        private Dictionary<string,PoliceRank> _ranks = new Dictionary<string,PoliceRank>()
        {
            ["Marshal"] = new PoliceRank( 
                "Marshal", // Name
                1000, // Salary
                LEODepartments.USMS, // Department
                new List<string>() { "WEAPON_NIGHTSTICK" }, // Weapons
                new List<string>() { "Police2" }, // Vehicles
                true,   //Can Use Spike Strips
                true,   //Can Use Radar
                true,   //Can Use K9
                true,   //Can Use EMS
                true,   //Can Use Helicopter
                true), //Can promote
        };

        private void AddOfficer(User user, string[] args)
        {
            // Check if they have provided a player at all.
            if (args[1] == null) { Utility.Instance.SendChatMessage(user.Source, "[POLICE]", "Did not provide a player ID.", 0, 0, 150); return; }

            //Make sure a rank was provided.
            if (args.Length>=3) { Utility.Instance.SendChatMessage(user.Source, "[POLICE]", "Did not provide a rank.", 0, 0, 150); return; }

            var plyList = new PlayerList();
            var targetPly = plyList[Convert.ToInt32(args[1])];
            args[1] = null;
            args[0] = null;
            var rank = string.Join(" ",args);

            //Check if the rank exists
            if (!_ranks.ContainsKey(rank)) { Utility.Instance.SendChatMessage(user.Source, "[POLICE]", "Rank does not exist.", 0, 0, 150); return; }

            //Check if they have provided a valid player.
            if (targetPly == null) { Utility.Instance.SendChatMessage(user.Source, "[POLICE]", "Invalid player provided.",0,0,150); return; }
            var targetUser = UserManager.Instance.GetUserFromPlayer(targetPly);
            var targetCharacter = targetUser.CurrentCharacter;
            
            //Check if the rank can promote.
            if (!_ranks[_officers[user.CurrentCharacter.FullName].Rank].CanPromote && !Admin.Instance.ActiveAdmins.Contains(user.Source)){ Utility.Instance.SendChatMessage(user.Source,"[POLICE]","You do not have the correct rank to promote people!",0,0,150); return; }

            DatabaseManager.Instance.Execute("INSERT INTO POLICE VALUES(name,rank) ('"+targetCharacter.FirstName+" "+targetCharacter.LastName+"','"+rank+"')");
            var data = DatabaseManager.Instance.StartQuery("SELECT badge WHERE name='" + targetCharacter.FirstName + " " + targetCharacter.LastName + "'");
            while (data.Read())
            {
                _officers[user.CurrentCharacter.FullName] = new PoliceOfficer( rank, Convert.ToInt32(data["badge"]),false);
                break;
            }
            DatabaseManager.Instance.EndQuery(data);
            Utility.Instance.SendChatMessageAll("[POLICE]", targetUser.CurrentCharacter.FirstName + " " + targetUser.CurrentCharacter.LastName + " has been hired as law enforcment on Pineapple Island.", 0, 0, 150);
        }

        private void RemoveOfficer(User user, string[] args)
        {
            if (args[1] == null) { Utility.Instance.SendChatMessage(user.Source, "[POLICE]", "Did not provide a player ID.", 0, 0, 150); return; }
            var plyList = new PlayerList();
            var targetPly = plyList[Convert.ToInt32(args[1])];
            if (targetPly == null) { Utility.Instance.SendChatMessage(user.Source, "[POLICE]", "Invalid player provided.", 0, 0, 150); return; }
            var targetUsr = UserManager.Instance.GetUserFromPlayer(targetPly);

            //Check if the rank can promote.
            if (!_ranks[_officers[user.CurrentCharacter.FullName].Rank].CanPromote && !Admin.Instance.ActiveAdmins.Contains(user.Source)) { Utility.Instance.SendChatMessage(user.Source, "[POLICE]", "You do not have the correct rank to promote people!", 0, 0, 150); return; }
            if (!_officers.ContainsKey(user.CurrentCharacter.FullName)) { Utility.Instance.SendChatMessage(user.Source, "[POLICE]", "Invalid user provided, they are not in the police.",0,0,150); return; }
            DatabaseManager.Instance.Execute("DELETE FROM POLICE WHERE name = '"+targetUsr.CurrentCharacter.FirstName+" "+targetUsr.CurrentCharacter.LastName+"';");
            Utility.Instance.SendChatMessageAll("[POLICE]", targetUsr.CurrentCharacter.FirstName + " " + targetUsr.CurrentCharacter.LastName + " has been fired from law enforcment on Pineapple Island.", 0, 0, 150);
            // Code to set the person off duty.
        }

        private void SetOfficerRank(User user, string[] args)
        {
            if (args[1] == null) { Utility.Instance.SendChatMessage(user.Source, "[POLICE]", "Did not provide a player ID.", 0, 0, 150); return; }
            var plyList = new PlayerList();
            var targetPly = plyList[Convert.ToInt32(args[1])];
            if (targetPly == null) { Utility.Instance.SendChatMessage(user.Source, "[POLICE]", "Invalid player provided.", 0, 0, 150); return; }
            var targetUsr = UserManager.Instance.GetUserFromPlayer(targetPly);

            //Make sure a rank was provided.
            if (args.Length >= 3) { Utility.Instance.SendChatMessage(user.Source, "[POLICE]", "Did not provide a rank.", 0, 0, 150); return; }

            args[1] = null;
            args[0] = null;
            var rank = string.Join(" ", args);

            //Check if the rank can promote.    
            if (!_ranks[_officers[user.CurrentCharacter.FullName].Rank].CanPromote && !Admin.Instance.ActiveAdmins.Contains(user.Source)) { Utility.Instance.SendChatMessage(user.Source, "[POLICE]", "You do not have the correct rank to promote people!", 0, 0, 150); return; }
            if (!_officers.ContainsKey(user.CurrentCharacter.FullName)) { Utility.Instance.SendChatMessage(user.Source, "[POLICE]", "Invalid user provided, they are not in the police.", 0, 0, 150); return; }
            if (!_ranks.ContainsKey(rank)) { Utility.Instance.SendChatMessage(user.Source, "[POLICE]", "Invalid rank provided.", 0, 0, 150); return; }
            DatabaseManager.Instance.Execute("UPDATE POLICE SET rank='"+rank+ "' WHERE name = '" + targetUsr.CurrentCharacter.FirstName + " " + targetUsr.CurrentCharacter.LastName + "';");
            _officers[user.CurrentCharacter.FullName].Rank = rank;
            Utility.Instance.SendChatMessageAll("[POLICE]", targetUsr.CurrentCharacter.FirstName + " " + targetUsr.CurrentCharacter.LastName + " has been promoted to "+rank+" as law enforcment on Pineapple Island.",0,0,150);
        }


        public bool IsCharacterCop(Character character)
        {
            if (_officers.ContainsKey(character.FullName))
            {
                return true;
            }
            return false;
        }

        public void GoOnDuty([FromSource] Player ply)
        {
            var user = UserManager.Instance.GetUserFromPlayer(ply);
            var character = user.CurrentCharacter;
            if (_officers.ContainsKey(character.FullName))
            {

            }
        }


        private async void SetupCommands()
        {
            while (CommandManager.Instance == null)
            {
                await Delay(0);
            }
            /*
            CommandManager.Instance.AddCommand("copadd", AddOfficer);
            CommandManager.Instance.AddCommand("addcop", AddOfficer);
            CommandManager.Instance.AddCommand("remcop", RemoveOfficer);
            CommandManager.Instance.AddCommand("removecop", RemoveOfficer);
            CommandManager.Instance.AddCommand("coprem", RemoveOfficer);
            CommandManager.Instance.AddCommand("copremove", RemoveOfficer);
            CommandManager.Instance.AddCommand("setpolicerank", SetOfficerRank);
            CommandManager.Instance.AddCommand("setcoprank", SetOfficerRank);
            */
        }

    }
}
