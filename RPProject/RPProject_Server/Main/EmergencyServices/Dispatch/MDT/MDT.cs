using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using server.Main.Users;
using server.Main.Vehicles;

namespace server.Main.EmergencyServices.Dispatch.MDT
{

    public class MDT : BaseScript
    {
        /// <summary>
        /// Singleton Instance
        /// </summary>
        public static MDT Instance;

        /// <summary>
        /// All loaded bolos
        /// </summary>
        public Dictionary<int, Bolo> Bolos = new Dictionary<int, Bolo>();

        /// <summary>
        /// All loaded warrrants.
        /// </summary>
        public Dictionary<int, Warrant> Warrants = new Dictionary<int, Warrant>();

        /// <summary>
        /// All loaded arrests
        /// </summary>
        public Dictionary<int, Arrest> Arrests = new Dictionary<int, Arrest>();

        /// <summary>
        /// All loaded tickets.
        /// </summary>
        public Dictionary<int, Ticket> Tickets = new Dictionary<int, Ticket>();

        public MDT()
        {
            Instance = this;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            LoadInformation();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            EventHandlers["ArrestPaperwork"] += new Action<Player, int, int, int, string>(ArrestPaperwork);
            EventHandlers["VehicleSearchByOwner"] += new Action<Player, string, string>(VehicleSearchByOwner);
            EventHandlers["VehicleSearchByPlate"] += new Action<Player, string>(VehicleSearchByPlate);
            EventHandlers["VehicleSearchByModel"] += new Action<Player, string>(VehicleSearchByModel);
            EventHandlers["WarrantPaperwork"] += new Action<Player, string, string, string, string>(WarrantPaperwork);
            EventHandlers["WarrantRemovalPaperwork"] += new Action<Player, int>(WarrantRemovelPaperwork);
            EventHandlers["BoloPaperwork"] += new Action<Player, string, string, string, string>(BoloPaperwork);
            EventHandlers["BoloRemovalPaperwork"] += new Action<Player, int>(BoloRemovelPaperwork);
            EventHandlers["BoloSearch"] += new Action<Player, string>(BoloSearch);
            EventHandlers["WarrantSearch"] += new Action<Player, string>(WarrantSearch);
            EventHandlers["CivlianSearch"] += new Action<Player, string, string>(CivlianSearch);
            EventHandlers["TicketPaperwork"] += new Action<Player, int, string, int>(TicketPaperwork);
            EventHandlers["BankStatementRequest"] += new Action<Player, int>(BankStatementRequest);
        }
        /// <summary>
        /// All loaded character info regarding the MDT.
        /// </summary>
        public List<MDTCharacterInfo> MDTInfo = new List<MDTCharacterInfo>();

        /// <summary>
        /// Loads lal the character info required for the mdt and stores it in a object and adds them to the list.
        /// </summary>
        private void LoadCharacterInfoForMDT()
        {
            var data = DatabaseManager.Instance.StartQuery(
                "SELECT flags,firstname,lastname,dateofbirth,gender FROM CHARACTERS;").Result;
            while (data.Read())
            {
                var flags = JsonConvert.DeserializeObject<Dictionary<FlagTypes, bool>>(Convert.ToString(data["flags"]));
                var first = Convert.ToString(data["firstname"]);
                var last = Convert.ToString(data["lastname"]);
                var dob = Convert.ToString(data["dateofbirth"]);
                var gender = Convert.ToInt32(data["gender"]);
                var genderstr = gender == 0 ? "Male" : "Female";
                var chara = new MDTCharacterInfo(flags, first, last, dob, genderstr);
                MDTInfo.Add(chara);
            }
            DatabaseManager.Instance.EndQuery(data);
        }

        /// <summary>
        /// CAlls all the methods to load the various information.
        /// </summary>
        /// <returns></returns>
        private async Task LoadInformation()
        {
            while (DatabaseManager.Instance == null)
            {
                await Delay(0);
            }
            while (Utility.Instance == null)
            {
                await Delay(0);
            }
            LoadArrests();
            LoadBolos();
            LoadWarrants();
            LoadTickets();
            LoadCharacterInfoForMDT();
        }

        /// <summary>
        /// Loads all the tickets from the database.
        /// </summary>
        private void LoadTickets()
        {
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM MDT_Tickets").Result;
            while (data.Read())
            {
                var number = Convert.ToInt32(data["CaseNumber"]);
                var officer = Convert.ToString(data["OfficerName"]);
                var suspect = Convert.ToString(data["SuspectName"]);
                var charges = Convert.ToString(data["Charges"]);
                var amount = Convert.ToString(data["FineAmount"]);
                Tickets.Add(number, new Ticket(number, officer, suspect, charges, amount));
            }
            DatabaseManager.Instance.EndQuery(data);
            Utility.Instance.Log("Tickets Loaded.");
        }

        /// <summary>
        /// Loads all the warrants from the database.
        /// </summary>
        private void LoadWarrants()
        {
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM MDT_Warrants").Result;
            while (data.Read())
            {
                var number = Convert.ToInt32(data["WarrantNumber"]);
                var name = Convert.ToString(data["Name"]);
                var charges = Convert.ToString(data["Charges"]);
                var evidence = Convert.ToString(data["Evidence"]);
                var notes = Convert.ToString(data["Notes"]);
                Warrants.Add(number, new Warrant(number, name, charges, evidence, notes));
            }
            DatabaseManager.Instance.EndQuery(data);
            Utility.Instance.Log("Warrants Loaded.");
        }

        /// <summary>
        /// Loads all the bolos fromt he database.
        /// </summary>
        private void LoadBolos()
        {
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM MDT_Bolos").Result;
            while (data.Read())
            {
                var number = Convert.ToInt32(data["BoloNumber"]);
                var plate = Convert.ToString(data["Plate"]);
                var charges = Convert.ToString(data["Charges"]);
                var evidence = Convert.ToString(data["Evidence"]);
                var desc = Convert.ToString(data["Description"]);
                Bolos.Add(number, new Bolo(number, plate, charges, evidence, desc));
            }
            DatabaseManager.Instance.EndQuery(data);
            Utility.Instance.Log("Bolos Loaded.");
        }

        /// <summary>
        /// Loads all the arrests from the database.
        /// </summary>
        private void LoadArrests()
        {
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM MDT_Arrests").Result;
            while (data.Read())
            {
                var casenumber = Convert.ToInt32(data["CaseNumber"]);
                var officername = Convert.ToString(data["OfficerName"]);
                var suspectname = Convert.ToString(data["SuspectName"]);
                var charges = Convert.ToString(data["Charges"]);
                var time = Convert.ToString(data["Time"]);
                var fine = Convert.ToString(data["Fine"]);
                Arrests.Add(casenumber, new Arrest(casenumber, officername, suspectname, charges, time, fine));
            }
            DatabaseManager.Instance.EndQuery(data);
            Utility.Instance.Log("Arrests Loaded.");
        }

        /// <summary>
        /// Event handler for attemping to search for a person.
        /// </summary>
        /// <param name="player">Person searching that triggerd event</param>
        /// <param name="firstname">First name of who your looking for</param>
        /// <param name="lastname">Last name of who your looking for.</param>
        private void CivlianSearch([FromSource] Player player, string firstname, string lastname)
        {

            firstname = new string(firstname.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-' || c == '_').ToArray());
            lastname = new string(lastname.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-' || c == '_').ToArray());
            firstname = firstname.ToLower();
            lastname = lastname.ToLower();
            var message = "^2NCIC Database Has Returned The Following Results\n";
            if (firstname != "" && lastname != "")
            {
                var fullname = firstname + " " + lastname;

                var isFirstConviction = true;
                foreach (var arrest in Arrests.Values.Where(i => i.SuspectName.ToLower().Equals(fullname)))
                {
                    if(isFirstConviction) { message = message + "^3Criminal Convictions\n"; isFirstConviction = false; }
                    message = message + "^8CASE#:^7" + arrest.CaseNumber + "^3///^8ARRESTING_OFFICER:^7" +
                              arrest.OfficerName + "^3///^8SUSPECT_NAME:^7" + arrest.SuspectName + "^3///^8CHARGES:^7" +
                              arrest.Charges + "^3///^8TIME:^7" + arrest.Time + "^3///^8FINE:^7" + arrest.Fine + "\n";
                }

                var isFirstTicket = true;
                foreach (var ticket in Tickets.Values.Where(i => i.SuspectName.ToLower().Equals(fullname)))
                {
                    if (isFirstTicket) { message = message + "^3Non-Criminal Convictions\n"; isFirstTicket = false; }
                    message = message + "^8CASE#:^7" + ticket.CaseNumber + "^3///^8ARRESTING_OFFICER:^7" +
                              ticket.OfficerName + "^3///^8SUSPECT_NAME:^7" + ticket.SuspectName + "^3///^8CHARGES:^7" +
                              ticket.Charges + "^3///^8FINE:^7" + ticket.FineAmount + "\n";
                }
                var isFirstWarrant = true;
                foreach (var warrant in Warrants.Values.Where(i =>
                    i.Name.ToLower().Equals(fullname)))
                {
                    if (isFirstWarrant) { message = message + "^3Outstanding Warrants\n"; isFirstWarrant = false; }
                    message = message + "^8WARRANT#:^7" + warrant.WarrantNumber + "^3///^8NAME:^7" + warrant.Name +
                              "^3///^8CHARGES:^7" + warrant.Charges + "^3///^8EVIDENCE:^7" + warrant.Evidence +
                              "^3///^8NOTES" + warrant.Notes + "\n";
                }

                if (isFirstWarrant && isFirstConviction && isFirstTicket)
                {
                    message=message+"^1RESULTS CAME BACK WITH NOTHING!";
                }
            }
            else if (firstname == "" && lastname != "")
            {
                var isFirstName = true;
                foreach (var chara in MDTInfo)
                {
                    if (chara.Last.ToLower().Contains(lastname.ToLower()))
                    {
                        if (isFirstName) { message = message + "^3Matching Names\n"; isFirstName = false; }
                        message = message + "^8FIRST:^7" + chara.First + "^3///^8LAST:^7" + chara.Last + "^3//^8DOB:^7" + chara.DOB + "^3///^8GENDER:^7" + chara.Gender + "\n";
                    }
                }

                var isFirstConviction = true;
                foreach (var arrest in Arrests.Values.Where(i => i.SuspectName.ToLower().Contains(lastname)))
                {
                    if (isFirstConviction) { message = message + "^3Criminal Convictions\n"; isFirstConviction = false; }
                    message = message + "^8CASE#:^7" + arrest.CaseNumber + "^3///^8ARRESTING_OFFICER:^7" +
                              arrest.OfficerName + "^3///^8SUSPECT_NAME:^7" + arrest.SuspectName + "^3///^8CHARGES:^7" +
                              arrest.Charges + "^3///^8TIME:^7" + arrest.Time + "^3///^8FINE:^7" + arrest.Fine + "\n";
                }

                var isFirstTicket = true;
                foreach (var ticket in Tickets.Values.Where(i => i.SuspectName.ToLower().Contains(lastname)))
                {
                    if (isFirstTicket) { message = message + "^3Non-Criminal Convictions\n"; isFirstTicket = false; }
                    message = message + "^8CASE#:^7" + ticket.CaseNumber + "^3///^8ARRESTING_OFFICER:^7" +
                              ticket.OfficerName + "^3///^8SUSPECT_NAME:^7" + ticket.SuspectName + "^3///^8CHARGES:^7" +
                              ticket.Charges + "^3///^8FINE:^7" + ticket.FineAmount + "\n";
                }

                var isFirstWarrant = true;
                foreach (var warrant in Warrants.Values.Where(i =>
                    i.Name.ToLower().Contains(lastname)))
                {
                    if (isFirstWarrant) { message = message + "^3Outstanding Warrants\n"; isFirstWarrant = false; }
                    message = message + "^8WARRANT#:^7" + warrant.WarrantNumber + "^3///^8NAME:^7" + warrant.Name +
                              "^3///^8CHARGES:^7" + warrant.Charges + "^3///^8EVIDENCE:^7" + warrant.Evidence +
                              "^3///^8NOTES:^7" + warrant.Notes + "\n";
                }

                if (isFirstWarrant && isFirstConviction && isFirstTicket)
                {
                    message = message + "^1RESULTS CAME BACK WITH NOTHING!";
                }

            }
            else if (firstname != "" && lastname == "")
            {

                var isFirstName = true;
                foreach (var chara in MDTInfo)
                {
                    if (chara.First.ToLower().Contains(firstname.ToLower()))
                    {
                        if (isFirstName) { message = message + "^3Matching Names\n"; isFirstName = false; }
                        message = message + "^8FIRST:^7" + chara.First + "^3///^8LAST:^7" + chara.Last + "^3//^8DOB:^7" + chara.DOB + "^3///^8GENDER:^7" + chara.Gender + "\n";
                    }
                }

                var isFirstConviction = true;
                foreach (var arrest in Arrests.Values.Where(i => i.SuspectName.ToLower().Contains(firstname)))
                {
                    if (isFirstConviction) { message = message + "^3Criminal Convictions\n"; isFirstConviction = false; }
                    message = message + "^8CASE#:^7" + arrest.CaseNumber + "^3///^8ARRESTING_OFFICER:^7" +
                              arrest.OfficerName + "^3///^8SUSPECT_NAME:^7" + arrest.SuspectName + "^3///^8CHARGES:^7" +
                              arrest.Charges + "^3///^8TIME:^7" + arrest.Time + "^3///^8FINE:^7" + arrest.Fine + "\n";
                }

                var isFirstTicket = true;
                foreach (var ticket in Tickets.Values.Where(i => i.SuspectName.ToLower().Contains(firstname)))
                {
                    if (isFirstTicket) { message = message + "^3Non-Criminal Convictions\n"; isFirstTicket = false; }
                    message = message + "^8CASE#:^7" + ticket.CaseNumber + "^3///^8ARRESTING_OFFICER:^7" +
                              ticket.OfficerName + "^3///^8SUSPECT_NAME:^7" + ticket.SuspectName + "^3///^8CHARGES:^7" +
                              ticket.Charges + "^3///^8FINE:^7" + ticket.FineAmount + "\n";
                }

                var isFirstWarrant = true;
                foreach (var warrant in Warrants.Values.Where(i =>
                    i.Name.ToLower().Contains(firstname)))
                {
                    if (isFirstWarrant) { message = message + "^3Outstanding Warrants\n"; isFirstWarrant = false; }
                    message = message + "^8WARRANT#:^7" + warrant.WarrantNumber + "^3///^8NAME:^7" + warrant.Name +
                              "^3///^8CHARGES:^7" + warrant.Charges + "^3///^8EVIDENCE:^7" + warrant.Evidence +
                              "^3///^8NOTES:^7" + warrant.Notes + "\n";
                }

                if (isFirstWarrant && isFirstConviction && isFirstTicket)
                {
                    message = message + "^1RESULTS CAME BACK WITH NOTHING!";
                }


                message = message + "^3Matching Names\n";
                foreach (var chara in MDTInfo)
                {
                    if (chara.First.ToLower().Contains(firstname.ToLower()))
                    {
                        message = message + "^8FIRST:^7" + chara.First + "^3///^8LAST:^7" + chara.Last + "^3//^8DOB:^7" + chara.DOB + "^3///^8GENDER:^7" + chara.Gender + "\n";
                    }
                }

                message = message + "^3Criminal Convictions\n";
                foreach (var arrest in Arrests.Values.Where(i => i.SuspectName.ToLower().Contains(firstname)))
                {
                    message = message + "^8CASE#:^7" + arrest.CaseNumber + "^3///^8ARRESTING_OFFICER:^7" +
                              arrest.OfficerName + "^3///^8SUSPECT_NAME:^7" + arrest.SuspectName + "^3///^8CHARGES:^7" +
                              arrest.Charges + "^3///^8TIME:^7" + arrest.Time + "^3///^8FINE:^7" + arrest.Fine + "\n";
                }

                message = message + "^3Non-Criminal Charges\n";
                foreach (var ticket in Tickets.Values.Where(i => i.SuspectName.ToLower().Contains(firstname)))
                {
                    message = message + "^8CASE#:^7" + ticket.CaseNumber + "^3///^8ARRESTING_OFFICER:^7" +
                              ticket.OfficerName + "^3///^8SUSPECT_NAME:^7" + ticket.SuspectName + "^3///^8CHARGES:^7" +
                              ticket.Charges + "^3///^8FINE:^7" + ticket.FineAmount + "\n";
                }

                message = message + "^3Outstanding Warrants\n";
                foreach (var warrant in Warrants.Values.Where(i =>
                    i.Name.ToLower().Contains(firstname)))
                {
                    message = message + "^8WARRANT#:^7" + warrant.WarrantNumber + "^3///^8NAME:^7" + warrant.Name +
                              "^3///^8CHARGES:^7" + warrant.Charges + "^3///^8EVIDENCE:^7" + warrant.Evidence +
                              "^3///^8NOTES" + warrant.Notes + "\n";
                }

            }
            else if (firstname == "" && lastname == "")
            {
                Utility.Instance.SendChatMessage(player, "[NCIC]", "You must provide a firstname or lastname, or at least partial first or last names!", 0, 0, 185);
                return;
            }
            Utility.Instance.SendChatMessage(player, "[NCIC]", message, 0, 0, 185);
        }

        /// <summary>
        /// Event handler for searhcing warrants for name
        /// </summary>
        /// <param name="player">Player who triggered</param>
        /// <param name="name">The name on warrant your searching for.</param>
        private void WarrantSearch([FromSource]Player player, string name)
        {
            name = name.ToLower();
            var message = "^3NCIC Database Has Returned The Following Results\n";
            if (name != "")
            {
                foreach (var warrant in Warrants.Values.Where(i =>
                    i.Name.ToLower().Contains(name)))
                {
                    message = message + "^8WARRANT#:^7" + warrant.WarrantNumber + "^8NAME:^7" + warrant.Name + "^3///^8CHARGES:^7" + warrant.Charges +
                              "^3///^8EVIDENCE:^7" + warrant.Evidence + "^3///^8NOTES:^7" + warrant.Notes + "\n";
                }
                Utility.Instance.SendChatMessage(player, "[NCIC]", message, 0, 0, 185);
            }
            else
            {
                Utility.Instance.SendChatMessage(player, "[NCIC]", "Invalid parameters supplied, you have to give a model.", 0, 0, 185);
            }
        }

        /// <summary>
        /// Event handler for searching bolos by plate
        /// </summary>
        /// <param name="player">Players earching</param>
        /// <param name="plate">the plate of the evhicle your looking for.</param>
        private void BoloSearch([FromSource]Player player, string plate)
        {
            plate = plate.ToLower();
            var message = "^3NCIC Database Has Returned The Following Results\n";
            if (plate != "")
            {
                foreach (var warrant in Bolos.Values.Where(i =>
                    i.Plate.ToLower().Contains(plate)))
                {
                    message = message + "^8BOLO#:^7" + warrant.BoloNumber + "^8PLATE:^7" + warrant.Plate + "^3///^8CHARGES:^7" + warrant.Charges +
                              "^3///^8EVIDENCE:^7" + warrant.Evidence + "^3///^8DESC:^7" + warrant.Description + "\n";
                }
                Utility.Instance.SendChatMessage(player, "[NCIC]", message, 0, 0, 185);
            }
            else
            {
                Utility.Instance.SendChatMessage(player, "[NCIC]", "Invalid parameters supplied, you have to give a model.", 0, 0, 185);
            }
        }

        /// <summary>
        /// Event handler for filing paperwork for warrnats
        /// </summary>
        /// <param name="player">The person triggering</param>
        /// <param name="name">name on warrant</param>
        /// <param name="charges">charges on warrant</param>
        /// <param name="evidence">evidence on warrant</param>
        /// <param name="notes">notes for warrant</param>
        private void WarrantPaperwork([FromSource]Player player, string name, string charges, string evidence, string notes)
        {
            name = new string(name.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-' || c == '_').ToArray());
            charges = new string(charges.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-' || c == '_').ToArray());
            evidence = new string(evidence.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-' || c == '_').ToArray());
            notes = new string(notes.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-' || c == '_').ToArray());
            if (!Police.Police.Instance.IsPlayerOnDuty(player)) { Utility.Instance.SendChatMessage(player, "[Warrants]", "Have to be a cop to do this.", 0, 0, 185); return; }
            if (name == "") { Utility.Instance.SendChatMessage(player, "[Warrants]", "Invalid name.", 0, 0, 185); return; }
            if (charges == "") { Utility.Instance.SendChatMessage(player, "[Warrants]", "Invalid charges.", 0, 0, 185); return; }
            if (evidence == "") { Utility.Instance.SendChatMessage(player, "[Warrants]", "Invalid evidence.", 0, 0, 185); return; }
            if (notes == "") { Utility.Instance.SendChatMessage(player, "[Warrants]", "Invalid notes.", 0, 0, 185); return; }
            Utility.Instance.SendChatMessage(player, "[Warrants]", "Your warrant for " + name + " (" + charges + ") has been submitted.", 0, 0, 185);
            DatabaseManager.Instance.Execute("INSERT INTO MDT_Warrants (Name,Charges,Evidence,Notes) VALUES('" + name + "','" + charges + "','" + evidence + "','" + notes + "');");
            if (Warrants.Any())
            {
                Warrants.Add(Warrants.Keys.Last() + 1, new Warrant(Warrants.Keys.Last() + 1, name, charges, evidence, notes));
            }
            else
            {
                Warrants.Add(1, new Warrant(1, name, charges, evidence, notes));
            }
        }

        /// <summary>
        /// Event handler for removing warrants
        /// </summary>
        /// <param name="player">Player who triggered</param>
        /// <param name="id">id of warrant to remove.</param>
        private void WarrantRemovelPaperwork([FromSource]Player player, int id)
        {
            if (!Police.Police.Instance.IsPlayerOnDuty(player)) { Utility.Instance.SendChatMessage(player, "[Warrants]", "Have to be a cop to do this.", 0, 0, 185); return; }
            if (Warrants.ContainsKey(id))
            {
                DatabaseManager.Instance.Execute("DELETE FROM MDT_Warrants WHERE WarrantNumber=" + id + ";");
                Utility.Instance.SendChatMessage(player, "[Warrants]", "Warrant #" + id + " has been removed from the database.", 0, 0, 185);
                Warrants.Remove(id);
            }
            else
            {
                Utility.Instance.SendChatMessage(player, "[Warrants]", "Warrant does not exist.", 0, 0, 185);
            }
        }

        /// <summary>
        /// Paperwork for bolos
        /// </summary>
        /// <param name="player">player who triggered</param>
        /// <param name="plate">plateo n bolo</param>
        /// <param name="charges">carges on bolo</param>
        /// <param name="evidence">evidence on bolo</param>
        /// <param name="desc">any other details needed</param>
        private void BoloPaperwork([FromSource]Player player, string plate, string charges, string evidence, string desc)
        {
            plate = new string(plate.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-' || c == '_').ToArray());
            charges = new string(charges.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-' || c == '_').ToArray());
            evidence = new string(evidence.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-' || c == '_').ToArray());
            desc = new string(desc.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-' || c == '_').ToArray());
            if (!Police.Police.Instance.IsPlayerOnDuty(player)) { Utility.Instance.SendChatMessage(player, "[Warrants]", "Have to be a cop to do this.", 0, 0, 185); return; }
            if (plate == "") { Utility.Instance.SendChatMessage(player, "[Bolos]", "Invalid plate.", 0, 0, 185); return; }
            if (charges == "") { Utility.Instance.SendChatMessage(player, "[Bolos]", "Invalid charges.", 0, 0, 185); return; }
            if (evidence == "") { Utility.Instance.SendChatMessage(player, "[Bolos]", "Invalid evidence.", 0, 0, 185); return; }
            if (desc == "") { Utility.Instance.SendChatMessage(player, "[Bolos]", "Invalid description.", 0, 0, 185); return; }
            Utility.Instance.SendChatMessage(player, "[Bolos]", "Your BOLO for " + plate + " (" + charges + ") has been submitted.", 0, 0, 185);
            DatabaseManager.Instance.Execute("INSERT INTO MDT_Bolos (Plate,Charges,Evidence,Description) VALUES('" + plate + "','" + charges + "','" + evidence + "','" + desc + "');");
            if (Bolos.Any())
            {
                Bolos.Add(Bolos.Keys.Last() + 1, new Bolo(Bolos.Keys.Last() + 1, plate, charges, evidence, desc));
            }
            else
            {
                Bolos.Add(1, new Bolo(1, plate, charges, evidence, desc));
            }
        }
        
        /// <summary>
        /// Event handler for handling bolo removal
        /// </summary>
        /// <param name="player">player triggering</param>
        /// <param name="id">Bolo id to remove</param>
        private void BoloRemovelPaperwork([FromSource]Player player, int id)
        {
            if (!Police.Police.Instance.IsPlayerOnDuty(player)) { Utility.Instance.SendChatMessage(player, "[Bolos]", "Have to be a cop to do this.", 0, 0, 185); return; }
            if (Bolos.ContainsKey(id))
            {
                DatabaseManager.Instance.Execute("DELETE FROM MDT_Bolos WHERE BoloNumber=" + id + ";");
                Utility.Instance.SendChatMessage(player, "[Bolos]", "Bolo #" + id + " has been removed from the database.", 0, 0, 185);
                Bolos.Remove(id);
            }
            else
            {
                Utility.Instance.SendChatMessage(player, "[Bolos]", "Bolo does not exist.", 0, 0, 185);
            }
        }

        /// <summary>
        /// Event handler for filing arrest paperwork.
        /// </summary>
        /// <param name="player">player filing out paperwork</param>
        /// <param name="tgtId">the target player id</param>
        /// <param name="jailTime">the amount of jaitleim to give</param>
        /// <param name="fineAmount">the amount to fine the player</param>
        /// <param name="charges">the charges to give the palyer.</param>
        private void ArrestPaperwork([FromSource] Player player, int tgtId, int jailTime, int fineAmount,
            string charges)
        {
            var plyList = new PlayerList();
            var tgtPly = plyList[tgtId];
            if (!Police.Police.Instance.IsPlayerOnDuty(player)) { Utility.Instance.SendChatMessage(player, "[Booking]", "Have to be a cop to do this.", 0, 0, 185); return; }
            if (tgtPly == null) { Utility.Instance.SendChatMessage(player, "[Booking]", "Invalid player ID provided.", 0, 0, 185); return; }
            if (jailTime <= 0) { Utility.Instance.SendChatMessage(player, "[Booking]", "Invalid Jailtime.", 0, 0, 185); return; }
            if (fineAmount <= 0) { Utility.Instance.SendChatMessage(player, "[Booking]", "Invalid Fine Amount.", 0, 0, 185); return; }
            var tgtUser = UserManager.Instance.GetUserFromPlayer(tgtPly);
            var user = UserManager.Instance.GetUserFromPlayer(player);
            DatabaseManager.Instance.Execute("INSERT INTO MDT_Arrests (OfficerName,SuspectName,Charges,Time,Fine) VALUES('" + user.CurrentCharacter.FullName + "','" + tgtUser.CurrentCharacter.FullName + "','" + charges + "','" + jailTime + "','" + fineAmount + "');");
            var officername = user.CurrentCharacter.FullName;
            var suspectname = tgtUser.CurrentCharacter.FullName;
            if (Arrests.Any())
            {
                Arrests.Add(Arrests.Keys.Last() + 1, new Arrest(Arrests.Keys.Last() + 1, officername, suspectname, charges, Convert.ToString(jailTime), Convert.ToString(fineAmount)));
            }
            else
            {
                Arrests.Add(1, new Arrest(1, officername, suspectname, charges, Convert.ToString(jailTime), Convert.ToString(fineAmount)));
            }

            UserManager.Instance.GetUserFromPlayer(tgtPly).CurrentCharacter.JailTime = Convert.ToInt32(jailTime) * 60;
            TriggerClientEvent(tgtPly, "Jail", Convert.ToInt32(jailTime) * 60);

            if (MoneyManager.Instance.GetMoney(tgtPly, MoneyTypes.Cash) >= fineAmount)
            {
                MoneyManager.Instance.RemoveMoney(tgtPly, MoneyTypes.Cash, fineAmount);
                Utility.Instance.SendChatMessage(user.Source, "[Fines]", tgtUser.CurrentCharacter.FullName + " has paid their fine with cash", 255, 0, 0);
            }
            else if (MoneyManager.Instance.GetMoney(tgtPly, MoneyTypes.Bank) >= fineAmount)
            {
                MoneyManager.Instance.RemoveMoney(tgtPly, MoneyTypes.Bank, fineAmount);
                Utility.Instance.SendChatMessage(user.Source, "[Fines]", tgtUser.CurrentCharacter.FullName + " has paid their fine with bank balance", 255, 0, 0);
            }
            else
            {
                var newAmount = fineAmount - MoneyManager.Instance.GetMoney(tgtPly, MoneyTypes.Bank);
                Utility.Instance.SendChatMessage(user.Source, "[Fines]", tgtUser.CurrentCharacter.FullName + " has paid as much as they can of their fine with bank balance. ($" + newAmount + " left)", 255, 0, 0);
            }
        }

        /// <summary>
        /// Filing paperwork for ticket event handler
        /// </summary>
        /// <param name="player">player filing paperwork</param>
        /// <param name="tgtId">Target id to charge</param>
        /// <param name="charges">charges to file in paperwork</param>
        /// <param name="fine">the total of the fine to give</param>
        private void TicketPaperwork([FromSource] Player player, int tgtId, string charges, int fine)
        {
            var plyList = new PlayerList();
            var tgtPly = plyList[tgtId];
            if (!Police.Police.Instance.IsPlayerOnDuty(player)) { Utility.Instance.SendChatMessage(player, "[Tickets]", "Have to be a cop to do this.", 0, 0, 185); return; }
            if (tgtPly == null) { Utility.Instance.SendChatMessage(player, "[Tickets]", "Invalid player ID provided.", 0, 0, 185); return; }
            if (fine <= 0) { Utility.Instance.SendChatMessage(player, "[Tickets]", "Invalid Fine Amount.", 0, 0, 185); return; }
            var tgtUser = UserManager.Instance.GetUserFromPlayer(tgtPly);
            var user = UserManager.Instance.GetUserFromPlayer(player);
            DatabaseManager.Instance.Execute("INSERT INTO MDT_Tickets (OfficerName,SuspectName,Charges,FineAmount) VALUES('" + user.CurrentCharacter.FullName + "','" + tgtUser.CurrentCharacter.FullName + "','" + charges + "','" + fine + "');");
            var officername = user.CurrentCharacter.FullName;
            var suspectname = tgtUser.CurrentCharacter.FullName;
            if (Tickets.Any())
            {
                Tickets.Add(Tickets.Keys.Last() + 1, new Ticket(Tickets.Keys.Last() + 1, officername, suspectname, charges, Convert.ToString(fine)));
            }
            else
            {
                Tickets.Add(1, new Ticket(1, officername, suspectname, charges, Convert.ToString(fine)));
            }


            if (MoneyManager.Instance.GetMoney(tgtPly, MoneyTypes.Cash) >= fine)
            {
                MoneyManager.Instance.RemoveMoney(tgtPly, MoneyTypes.Cash, fine);
                Utility.Instance.SendChatMessage(user.Source, "[Fines]", tgtUser.CurrentCharacter.FullName + " has paid their fine with cash", 255, 0, 0);
            }
            else if (MoneyManager.Instance.GetMoney(tgtPly, MoneyTypes.Bank) >= fine)
            {
                MoneyManager.Instance.RemoveMoney(tgtPly, MoneyTypes.Bank, fine);
                Utility.Instance.SendChatMessage(user.Source, "[Fines]", tgtUser.CurrentCharacter.FullName + " has paid their fine with bank balance", 255, 0, 0);
            }
            else
            {
                var newAmount = fine - MoneyManager.Instance.GetMoney(tgtPly, MoneyTypes.Bank);
                Utility.Instance.SendChatMessage(user.Source, "[Fines]", tgtUser.CurrentCharacter.FullName + " has paid as much as they can of their fine with bank balance. ($" + newAmount + " left)", 255, 0, 0);
            }
        }

        /// <summary>
        /// Event handler for searching for vehicle by model
        /// </summary>
        /// <param name="player">playesrs earching</param>
        /// <param name="model">model to look for.</param>
        private void VehicleSearchByModel([FromSource] Player player, string model)
        {
            Debug.WriteLine(model);
            var message = "^3DMV Database Has Returned The Following Results\n";
            if (model != "")
            {
                foreach (var vehicle in VehicleManager.Instance.LoadedVehicles.Values.Where(i =>
                    i.Model.Contains(model)))
                {
                    message = message + "^8MODEL:^7" + vehicle.Model + "^3///^8PLATE:^7" + vehicle.Plate +
                              "^3///^8OWNER:^7" + vehicle.RegisteredOwner + "\n";
                }
                Utility.Instance.SendChatMessage(player, "[DMV Records]", message, 0, 0, 185);
            }
            else
            {
                Utility.Instance.SendChatMessage(player, "[DMV Records]", "Invalid parameters supplied, you have to give a model.", 0, 0, 185);
            }
        }

        /// <summary>
        /// Event hanldler for searching for vehicle by plate
        /// </summary>
        /// <param name="player">Player searching</param>
        /// <param name="plate">palte to search for</param>
        private void VehicleSearchByPlate([FromSource] Player player, string plate)
        {
            var message = "^3DMV Database Has Returned The Following Results\n";
            if (plate != "")
            {
                foreach (var vehicle in VehicleManager.Instance.LoadedVehicles.Values.Where(i =>
                         i.Plate.Contains(plate)))
                {
                    message = message + "^8MODEL:^7" + vehicle.Model + "^3///^8PLATE:^7" + vehicle.Plate +
                              "^3///^8OWNER:^7" + vehicle.RegisteredOwner + "\n";
                }
                Utility.Instance.SendChatMessage(player, "[DMV Records]", message, 0, 0, 185);
            }
            else
            {
                Utility.Instance.SendChatMessage(player, "[DMV Records]", "Invalid parameters supplied, you have to give a model.", 0, 0, 185);
            }
        }

        /// <summary>
        /// Event hanlder for seraching for a vehicle by the owner
        /// </summary>
        /// <param name="player">palyer searching</param>
        /// <param name="first">firstn ame to look for</param>
        /// <param name="last">last name to look for.</param>
        private void VehicleSearchByOwner([FromSource] Player player, string first, string last)
        {
            first = first.ToLower();
            last = last.ToLower();
            var message = "^3DMV Database Has Returned The Following Results\n";
            if (first != "" && last != "")
            {
                message = VehicleManager.Instance.LoadedVehicles.Values.Where(i => i.RegisteredOwner.ToLower() == first.ToLower() + " " + last.ToLower()).Aggregate(message, (current, vehicle) => current + "^8MODEL:^7" + vehicle.Model + "^3///^8PLATE:^7" + vehicle.Plate + "^3///^8OWNER:^7" + vehicle.RegisteredOwner + "\n");
            }
            else if (first != "" && last == "")
            {
                foreach (var vehicle in VehicleManager.Instance.LoadedVehicles.Values.Where(i =>
                    i.RegisteredOwner.Split(' ')[0].ToLower().Contains(first)))
                {
                    message = message + "^8MODEL:^7" + vehicle.Model + "^3///^8PLATE:^7" + vehicle.Plate + "^3///^8OWNER:^7" + vehicle.RegisteredOwner + "\n";
                }
            }
            else if (first == "" && last != "")
            {
                Debug.WriteLine(last);
                foreach (var vehicle in VehicleManager.Instance.LoadedVehicles.Values.Where(i =>
                    i.RegisteredOwner.Split(' ')[1].ToLower().Contains(last)))
                {
                    message = message + "^8MODEL:^7" + vehicle.Model + "^3///^8PLATE:^7" + vehicle.Plate + "^3///^8OWNER:^7" + vehicle.RegisteredOwner + "\n";
                }
            }
            else if (first == "" && last == "")
            {
                Utility.Instance.SendChatMessage(player, "[DMV Records]", "Invalid parameters supplied, you have to give a first or last name", 0, 0, 185);
                return;
            }
            Utility.Instance.SendChatMessage(player, "[DMV Records]", message, 0, 0, 185);
        }

        /// <summary>
        /// Event handler for getting a players bank statement
        /// </summary>
        /// <param name="player">player triggering</param>
        /// <param name="tgtId">The id of the player to check.</param>
        private void BankStatementRequest([FromSource] Player player, int tgtId)
        {
            var plyList = new PlayerList();
            var tgtPly = plyList[tgtId];
            //if (!Police.Instance.IsPlayerOnDuty(player)) { Utility.Instance.SendChatMessage(player, "[Bank Statement Warrant]", "Have to be a cop to do this.", 0, 0, 185); return; }
            if (tgtPly == null) { Utility.Instance.SendChatMessage(player, "[Bank Statement Warrant]", "Invalid player ID provided.", 0, 0, 185); return; }
            var tgtUser = UserManager.Instance.GetUserFromPlayer(tgtPly);
            if (tgtUser == null) { Utility.Instance.SendChatMessage(player, "[Bank Statement Warrant]", "Invalid player ID provided.", 0, 0, 185); return; }
            Utility.Instance.SendChatMessage(player, "[Bank Statement Warrant]", "^8" + tgtUser.CurrentCharacter.FullName + " Bank Balance :^2" + tgtUser.CurrentCharacter.Money.Bank, 0, 0, 185);

        }
    }
}
