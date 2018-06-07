using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using roleplay.Main.Users;

namespace roleplay.Main
{

    public enum FlagTypes { Felon, Aggressive, CopHater, Gang, SuspendedLicense, MentallyUnstable, Probation };

    public class Ticket
    {
        public int CaseNumber;
        public string OfficerName;
        public string SuspectName;
        public string Charges;
        public string FineAmount;
        public Ticket(int casenumber, string officer, string suspect, string charges, string fine)
        {
            CaseNumber = casenumber;
            OfficerName = officer;
            SuspectName = suspect;
            Charges = charges;
            FineAmount = fine;
        }
    }

    public class Arrest
    {
        public int CaseNumber;
        public string OfficerName;
        public string SuspectName;
        public string Charges;
        public string Time;
        public string Fine;
        public Arrest(int casenumber, string officername, string suspectname, string charges, string time, string fine)
        {
            CaseNumber = casenumber;
            OfficerName = officername;
            SuspectName = suspectname;
            Charges = charges;
            Time = time;
            Fine = fine;
        }
    }

    public class Bolo
    {
        public int BoloNumber;
        public string Plate;
        public string Charges;
        public string Evidence;
        public string Description;
        public Bolo(int bolonumber, string plate, string charges, string evidence, string description)
        {
            BoloNumber = bolonumber;
            Plate = plate;
            Charges = charges;
            Evidence = evidence;
            Description = description;
        }
    }

    public class Warrant
    {
        public int WarrantNumber;
        public string Name;
        public string Charges;
        public string Evidence;
        public string Notes;
        public Warrant(int warrantnumbe, string name, string charges, string evidence, string notes)
        {
            WarrantNumber = warrantnumbe;
            Name = name;
            Charges = charges;
            Evidence = evidence;
            Notes = notes;
        }
    }

    public class MDT : BaseScript
    {
        public static MDT Instance;

        public Dictionary<int, Bolo> Bolos = new Dictionary<int, Bolo>();
        public Dictionary<int, Warrant> Warrants = new Dictionary<int, Warrant>();
        public Dictionary<int, Arrest> Arrests = new Dictionary<int, Arrest>();
        public Dictionary<int, Ticket> Tickets = new Dictionary<int, Ticket>();

        public MDT()
        {
            Instance = this;
            LoadInformation();
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
            await LoadArrests();
            await LoadBolos();
            await LoadWarrants();
            await LoadTickets();
        }

        private async Task LoadTickets()
        {
            var data = await DatabaseManager.Instance.StartQueryAsync("SELECT * FROM MDT_Tickets");
            while (data.Read())
            {
                var number = Convert.ToInt32(data["CaseNumber"]);
                var officer = Convert.ToString(data["OfficerName"]);
                var suspect = Convert.ToString(data["SuspectName"]);
                var charges = Convert.ToString(data["Charges"]);
                var amount = Convert.ToString(data["FineAmount"]);
                Tickets.Add(number, new Ticket(number, officer,suspect,charges,amount));
            }
            await DatabaseManager.Instance.EndQueryAsync(data);
            Utility.Instance.Log("Tickets Loaded.");
        }

        private async Task LoadWarrants()
        {
            var data = await  DatabaseManager.Instance.StartQueryAsync("SELECT * FROM MDT_Warrants");
            while (data.Read())
            {
                var number = Convert.ToInt32(data["WarrantNumber"]);
                var name = Convert.ToString(data["Name"]);
                var charges = Convert.ToString(data["Charges"]);
                var evidence = Convert.ToString(data["Evidence"]);
                var notes = Convert.ToString(data["Notes"]);
                Warrants.Add(number, new Warrant(number, name, charges, evidence, notes));
            }
            await DatabaseManager.Instance.EndQueryAsync(data);
            Utility.Instance.Log("Warrants Loaded.");
        }

        private async Task LoadBolos()
        {
            var data = await DatabaseManager.Instance.StartQueryAsync("SELECT * FROM MDT_Bolos");
            while (data.Read())
            {
                var number = Convert.ToInt32(data["BoloNumber"]);
                var plate = Convert.ToString(data["Plate"]);
                var charges = Convert.ToString(data["Charges"]);
                var evidence = Convert.ToString(data["Evidence"]);
                var desc = Convert.ToString(data["Description"]);
                Bolos.Add(number, new Bolo(number, plate, charges, evidence, desc));
            }
            await DatabaseManager.Instance.EndQueryAsync(data);
            Utility.Instance.Log("Bolos Loaded.");
        }

        private async Task LoadArrests()
        {
            var data = await DatabaseManager.Instance.StartQueryAsync("SELECT * FROM MDT_Arrests");
            while (data.Read())
            {
                var casenumber = Convert.ToInt32(data["CaseNumber"]);
                var officername = Convert.ToString(data["OfficerName"]);
                var suspectname = Convert.ToString(data["SuspectName"]);
                var charges = Convert.ToString(data["Charges"]);
                var time = Convert.ToString(data["Time"]);
                var fine = Convert.ToString(data["Fine"]);
                Arrests.Add(casenumber, new Arrest(casenumber, officername,suspectname,charges,time,fine));
            }
            await DatabaseManager.Instance.EndQueryAsync(data);
            Utility.Instance.Log("Arrests Loaded.");
        }

        private void CivlianSearch([FromSource] Player player, string firstname, string lastname)
        {
            firstname = firstname.ToLower();
            lastname = lastname.ToLower();
            var message = "^2NCIC Database Has Returned The Following Results\n";
            if (firstname != "" && lastname != "")
            {
                var fullname = firstname + " " + lastname;

                message = message + "^3Criminal Convictions\n";
                foreach (var arrest in Arrests.Values.Where(i => i.SuspectName.ToLower().Equals(fullname)))
                {
                    message = message + "^8CASE#:^7" + arrest.CaseNumber + "^3///^8ARRESTING_OFFICER:^7" +
                              arrest.OfficerName + "^3///^8SUSPECT_NAME:^7" + arrest.SuspectName + "^3///^8CHARGES:^7" +
                              arrest.Charges + "^3///^8TIME:^7" + arrest.Time + "^3///^8FINE:^7" + arrest.Fine + "\n";
                }

                message = message + "^3Non-Criminal Charges\n";
                foreach (var ticket in Tickets.Values.Where(i => i.SuspectName.ToLower().Equals(fullname)))
                {
                    message = message + "^8CASE#:^7" + ticket.CaseNumber + "^3///^8ARRESTING_OFFICER:^7" +
                              ticket.OfficerName + "^3///^8SUSPECT_NAME:^7" + ticket.SuspectName + "^3///^8CHARGES:^7" +
                              ticket.Charges + "^3///^8FINE:^7" + ticket.FineAmount + "\n";
                }
                message = message + "^3Outstanding Warrants\n";
                foreach (var warrant in Warrants.Values.Where(i =>
                    i.Name.ToLower().Equals(fullname)))
                {
                    message = message + "^8WARRANT#:^7" + warrant.WarrantNumber + "^3///^8NAME:^7" + warrant.Name +
                              "^3///^8CHARGES:^7" + warrant.Charges + "^3///^8EVIDENCE:^7" + warrant.Evidence +
                              "^3///^8NOTES" + warrant.Notes + "\n";
                }
            }
            else if (firstname != "" && lastname == "" || firstname == "" && lastname != "")
            {

                message = message + "^3Criminal Convictions\n";
                foreach (var arrest in Arrests.Values.Where(i => i.SuspectName.ToLower().Contains(firstname) || i.SuspectName.ToLower().Contains(lastname)))
                {
                    message = message + "^8CASE#:^7" + arrest.CaseNumber + "^3///^8ARRESTING_OFFICER:^7" +
                              arrest.OfficerName + "^3///^8SUSPECT_NAME:^7" + arrest.SuspectName + "^3///^8CHARGES:^7" +
                              arrest.Charges + "^3///^8TIME:^7" + arrest.Time + "^3///^8FINE:^7" + arrest.Fine + "\n";
                }

                message = message + "^3Non-Criminal Charges\n";
                foreach (var ticket in Tickets.Values.Where(i => i.SuspectName.ToLower().Contains(firstname) || i.SuspectName.ToLower().Contains(lastname)))
                {
                    message = message + "^8CASE#:^7" + ticket.CaseNumber + "^3///^8ARRESTING_OFFICER:^7" +
                              ticket.OfficerName + "^3///^8SUSPECT_NAME:^7" + ticket.SuspectName + "^3///^8CHARGES:^7" +
                              ticket.Charges + "^3///^8FINE:^7" + ticket.FineAmount + "\n";
                }

                message = message + "^3Outstanding Warrants\n";
                foreach (var warrant in Warrants.Values.Where(i =>
                    i.Name.ToLower().Contains(firstname) || i.Name.ToLower().Contains(lastname)))
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
                              "^3///^8EVIDENCE:^7" + warrant.Evidence + "^3///^8NOTES:^7"+warrant.Notes+"\n";
                }
                Utility.Instance.SendChatMessage(player, "[NCIC]", message, 0, 0, 185);
            }
            else
            {
                Utility.Instance.SendChatMessage(player, "[NCIC]", "Invalid parameters supplied, you have to give a model.", 0, 0, 185);
            }
        }

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

        private void WarrantPaperwork([FromSource]Player player, string name, string charges, string evidence, string notes)
        {
            if (!Police.Instance.IsPlayerOnDuty(player)) { Utility.Instance.SendChatMessage(player, "[Warrants]", "Have to be a cop to do this.", 0, 0, 185); return; }
            if (name == "") { Utility.Instance.SendChatMessage(player, "[Warrants]", "Invalid name.", 0, 0, 185); return; }
            if (charges == "") { Utility.Instance.SendChatMessage(player, "[Warrants]", "Invalid charges.", 0, 0, 185); return; }
            if (evidence == "") { Utility.Instance.SendChatMessage(player, "[Warrants]", "Invalid evidence.", 0, 0, 185); return; }
            if (notes == "") { Utility.Instance.SendChatMessage(player, "[Warrants]", "Invalid notes.", 0, 0, 185); return; }
            Utility.Instance.SendChatMessage(player, "[Warrants]", "Your warrant for " + name + " (" + charges + ") has been submitted.", 0, 0, 185);
            DatabaseManager.Instance.ExecuteAsync("INSERT INTO MDT_Warrants (Name,Charges,Evidence,Notes) VALUES('" + name + "','" + charges + "','" + evidence + "','" + notes + "');");
            if (Warrants.Any())
            {
                Warrants.Add(Warrants.Keys.Last()+1, new Warrant(Warrants.Keys.Last()+1,name, charges, evidence, notes));
            }
            else
            {
                Warrants.Add(1, new Warrant(1, name, charges, evidence, notes));
            }
        }

        private void WarrantRemovelPaperwork([FromSource]Player player, int id)
        {
            if (!Police.Instance.IsPlayerOnDuty(player)) { Utility.Instance.SendChatMessage(player, "[Warrants]", "Have to be a cop to do this.", 0, 0, 185); return; }
            if (Warrants.ContainsKey(id))
            {
                DatabaseManager.Instance.ExecuteAsync("DELETE FROM MDT_Warrants WHERE WarrantNumber=" + id + ";");
                Utility.Instance.SendChatMessage(player, "[Warrants]", "Warrant #" + id + " has been removed from the database.", 0, 0, 185);
                Warrants.Remove(id);
            }
            else
            {
                Utility.Instance.SendChatMessage(player, "[Warrants]", "Warrant does not exist.", 0, 0, 185);
            }
        }

        private void BoloPaperwork([FromSource]Player player, string plate, string charges, string evidence, string desc)
        {
            if (!Police.Instance.IsPlayerOnDuty(player)) { Utility.Instance.SendChatMessage(player, "[Warrants]", "Have to be a cop to do this.", 0, 0, 185); return; }
            if (plate == "") { Utility.Instance.SendChatMessage(player, "[Bolos]", "Invalid plate.", 0, 0, 185); return; }
            if (charges == "") { Utility.Instance.SendChatMessage(player, "[Bolos]", "Invalid charges.", 0, 0, 185); return; }
            if (evidence == "") { Utility.Instance.SendChatMessage(player, "[Bolos]", "Invalid evidence.", 0, 0, 185); return; }
            if (desc == "") { Utility.Instance.SendChatMessage(player, "[Bolos]", "Invalid description.", 0, 0, 185); return; }
            Utility.Instance.SendChatMessage(player, "[Bolos]", "Your BOLO for " + plate + " (" + charges + ") has been submitted.", 0, 0, 185);
            DatabaseManager.Instance.ExecuteAsync("INSERT INTO MDT_Bolos (Plate,Charges,Evidence,Description) VALUES('" + plate + "','" + charges + "','" + evidence + "','" + desc + "');");
            if (Bolos.Any())
            {
                Bolos.Add(Bolos.Keys.Last() + 1, new Bolo(Bolos.Keys.Last() + 1, plate, charges, evidence, desc));
            }
            else
            {
                Bolos.Add(1, new Bolo(1, plate, charges, evidence, desc));
            }
        }

        private void BoloRemovelPaperwork([FromSource]Player player, int id)
        {
            if (!Police.Instance.IsPlayerOnDuty(player)) { Utility.Instance.SendChatMessage(player, "[Bolos]", "Have to be a cop to do this.", 0, 0, 185); return; }
            if (Bolos.ContainsKey(id))
            {
                DatabaseManager.Instance.ExecuteAsync("DELETE FROM MDT_Bolos WHERE BoloNumber=" + id + ";");
                Utility.Instance.SendChatMessage(player, "[Bolos]", "Bolo #" + id + " has been removed from the database.", 0, 0, 185);
                Bolos.Remove(id);
            }
            else
            {
                Utility.Instance.SendChatMessage(player, "[Bolos]", "Bolo does not exist.", 0, 0, 185);
            }
        }

        private void ArrestPaperwork([FromSource] Player player, int tgtId, int jailTime, int fineAmount,
            string charges)
        {
            var plyList = new PlayerList();
            var tgtPly = plyList[tgtId];
            if (!Police.Instance.IsPlayerOnDuty(player)) { Utility.Instance.SendChatMessage(player, "[Booking]", "Have to be a cop to do this.", 0, 0, 185); return; }
            if (tgtPly == null) { Utility.Instance.SendChatMessage(player, "[Booking]", "Invalid player ID provided.", 0, 0, 185); return; }
            if (jailTime<=0) { Utility.Instance.SendChatMessage(player, "[Booking]", "Invalid Jailtime.", 0, 0, 185); return; }
            if (fineAmount<=0) { Utility.Instance.SendChatMessage(player, "[Booking]", "Invalid Fine Amount.", 0, 0, 185); return; }
            var tgtUser = UserManager.Instance.GetUserFromPlayer(tgtPly);
            var user = UserManager.Instance.GetUserFromPlayer(player);
            DatabaseManager.Instance.ExecuteAsync("INSERT INTO MDT_Arrests (OfficerName,SuspectName,Charges,Time,Fine) VALUES('"+user.CurrentCharacter.FullName+"','"+tgtUser.CurrentCharacter.FullName+"','"+charges+"','"+jailTime+"','"+fineAmount+"');");
            var officername = user.CurrentCharacter.FullName;
            var suspectname = tgtUser.CurrentCharacter.FullName;
            if (Arrests.Any())
            {
                Arrests.Add(Arrests.Keys.Last() + 1, new Arrest(Arrests.Keys.Last() + 1, officername,suspectname,charges,Convert.ToString(jailTime),Convert.ToString(fineAmount)));
            }
            else
            {
                Arrests.Add(1, new Arrest(1,officername, suspectname, charges, Convert.ToString(jailTime), Convert.ToString(fineAmount)));
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

        private void TicketPaperwork([FromSource] Player player,int tgtId, string charges, int fine)
        {
            var plyList = new PlayerList();
            var tgtPly = plyList[tgtId];
            if (!Police.Instance.IsPlayerOnDuty(player)) { Utility.Instance.SendChatMessage(player, "[Tickets]", "Have to be a cop to do this.", 0, 0, 185); return; }
            if (tgtPly == null) { Utility.Instance.SendChatMessage(player, "[Tickets]", "Invalid player ID provided.", 0, 0, 185); return; }
            if (fine <= 0) { Utility.Instance.SendChatMessage(player, "[Tickets]", "Invalid Fine Amount.", 0, 0, 185); return; }
            var tgtUser = UserManager.Instance.GetUserFromPlayer(tgtPly);
            var user = UserManager.Instance.GetUserFromPlayer(player);
            DatabaseManager.Instance.ExecuteAsync("INSERT INTO MDT_Tickets (OfficerName,SuspectName,Charges,FineAmount) VALUES('" + user.CurrentCharacter.FullName + "','" + tgtUser.CurrentCharacter.FullName + "','" + charges + "','" + fine + "');");
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

        private void VehicleSearchByOwner([FromSource] Player player, string first, string last)
        {
            first = first.ToLower();
            last = last.ToLower();
            var message = "^3DMV Database Has Returned The Following Results\n";
            if (first != "" && last != "")
            {
                message = VehicleManager.Instance.LoadedVehicles.Values.Where(i => i.RegisteredOwner.ToLower() == first.ToLower() + " " + last.ToLower()).Aggregate(message, (current, vehicle) => current + "^8MODEL:^7" + vehicle.Model + "^3///^8PLATE:^7" + vehicle.Plate + "^3///^8OWNER:^7" + vehicle.RegisteredOwner + "\n");
            }
            else if (first != "" && last == "" || first == "" && last != "")
            {
                foreach (var vehicle in VehicleManager.Instance.LoadedVehicles.Values.Where(i =>
                    i.RegisteredOwner.Split(' ')[0].ToLower().Contains(first) || i.RegisteredOwner.Split(' ')[1].Contains(last)))
                {
                    message = message + "^8MODEL:^7" + vehicle.Model + "^3///^8PLATE:^7" + vehicle.Plate + "^3///^8OWNER:^7" + vehicle.RegisteredOwner + "\n";
                }
            }
            else if (first == "" && last == "")
            {
                Utility.Instance.SendChatMessage(player,"[DMV Records]","Invalid parameters supplied, you have to give a first or last name",0,0,185);
                return;
            }
            Utility.Instance.SendChatMessage(player, "[DMV Records]", message, 0, 0, 185);
        }

        private void BankStatementRequest([FromSource] Player player, int tgtId)
        {
            var plyList = new PlayerList();
            var tgtPly = plyList[tgtId];
            //if (!Police.Instance.IsPlayerOnDuty(player)) { Utility.Instance.SendChatMessage(player, "[Bank Statement Warrant]", "Have to be a cop to do this.", 0, 0, 185); return; }
            if (tgtPly == null) { Utility.Instance.SendChatMessage(player, "[Bank Statement Warrant]", "Invalid player ID provided.", 0, 0, 185); return; }

            var tgtUser = UserManager.Instance.GetUserFromPlayer(tgtPly);
            Utility.Instance.SendChatMessage(player, "[Bank Statement Warrant]", "^8"+tgtUser.CurrentCharacter.FullName+" Bank Balance :^2"+tgtUser.CurrentCharacter.Money.Bank,0,0,185);

        }
    }
}
