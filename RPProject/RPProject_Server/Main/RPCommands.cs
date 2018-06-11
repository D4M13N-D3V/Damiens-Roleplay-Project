using CitizenFX.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace server.Main
{
    public class RPCommands:BaseScript
    {
        public static RPCommands Instance;

        public RPCommands()
        {
            Instance = this;
            SetupCommands();
            EventHandlers["ActionCommandFromClient"] += new Action<Player,string>(ActionCommand);
            EventHandlers["TextingFromClient"] += new Action<Player, string, string>(TextFunction);
        }

        private async Task SetupCommands()
        {
            while (CommandManager.Instance == null)
            {
                await Delay(0);
            }
            while (Utility.Instance == null)
            {
                await Delay(0);
            }

            #region Chat Commands
            CommandManager.Instance.AddCommand("hotwire", HotwireCommand);
            CommandManager.Instance.AddCommand("me", ActionCommand);
            CommandManager.Instance.AddCommand("tweet", TweetCommand);
            CommandManager.Instance.AddCommand("t", TweetCommand);
            CommandManager.Instance.AddCommand("advert", AdvertCommand);
            CommandManager.Instance.AddCommand("tor", TorCommand);
            CommandManager.Instance.AddCommand("ooc", OocCommand);
            CommandManager.Instance.AddCommand("o", OocCommand);
            CommandManager.Instance.AddCommand("g", OocCommand);
            CommandManager.Instance.AddCommand("looc", LoocCommand);
            CommandManager.Instance.AddCommand("report", SupportCommand);
            CommandManager.Instance.AddCommand("help", HelpCommand);
            CommandManager.Instance.AddCommand("emshelp", EMSHelpCommand);
            CommandManager.Instance.AddCommand("cophelp", PoliceHelpCommand);
            #endregion

            #region Vehicle Commands
            CommandManager.Instance.AddCommand("engine", (user, strings) =>
            {
                TriggerClientEvent(user.Source,"ToggleEngine");
            });
            CommandManager.Instance.AddCommand("hood", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "ToggleHood");
            });
            CommandManager.Instance.AddCommand("trunk", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "ToggleTrunk");
            });
            CommandManager.Instance.AddCommand("lock", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "ToggleLock");
            });
            CommandManager.Instance.AddCommand("windowsdown", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "WindowsDown");
            });
            CommandManager.Instance.AddCommand("windowsup", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "WindowsUp");
            });
            #endregion

            CommandManager.Instance.AddCommand("text", TextCommand);
            
            CommandManager.Instance.AddCommand("911", EmergencyCallCommand);

            CommandManager.Instance.AddCommand("311", NonEmergencyCallCommand);

            CommandManager.Instance.AddCommand("repair", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "RepairCar");
            });

            CommandManager.Instance.AddCommand("selldrugs", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "StartSellingDrugs");
            });

            CommandManager.Instance.AddCommand("injuries", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "InjuryCheckCommand");
            });

            CommandManager.Instance.AddCommand("checkinjuries", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "InjuryCheckCommand");
            });
            
            CommandManager.Instance.AddCommand("roll", DiceRollCommand);
        }
        private static void HelpCommand(User user, string[] args)
        {
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "-------------HELP-------------", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "---F1 to open interaction menu---", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "--------COMMANDS--------", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "Aim and press E tto drag a restrained player or rob a local.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "Aim and press R to put into car / get out when restrained.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "Aim and press 4 to soft cuff.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "K to put belt on and off.", 255, 0, 0);

            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/transfer /givecar id plate | Transfer ownership of a car with given plate that you own to another person with the matching id.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/roll | Rolls a pair of dice.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/insurance plate | Claim insurance on a car you own. This charges you 1/6th of the cost of the car each time. You can wait until server restart or do this.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/911 message | Put it a emergency call to EMS/Police.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/311 message | Put it a non emergency call to EMS/Police.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/text id message | Sends a text to a player with a matching server id.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/text phonenumber message | Sends a text to a player with the matching phone number.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/tweet /t message | To send out a tweet.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/advert message | To send out a advertisment with only your first name.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/tor name message | To send out a anonymous message with a custom name", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/looc message | Local out of character chat", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/ooc /g /o message | Global out of character chat", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/report message | Send a report to online admins", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/hood | Open the vehicle hood. Cane be done inside or out. Vehicle must be unlocked.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/trunk | Opens the trunk of the vehicle, can be done in or out, vehicle must be unlocked.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/lock | Locks the car. Can only do it from the outside if you own the car.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/windowsdown | Rolls the windows down of the car that you are inside of.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/windowsup | Rolls the windows up of the car that you are inside of.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/repair | Repair your vehicle to a drivable state. (Hood must be open)", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/checkinjuries | Check the closest down'd players injuries.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/injuries |  Check the closest down'd players injuries.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/hotwire | Hotwire the vehicle that you are inside of. Can only be used once every 10 minutes.", 255, 0, 0);
        }

        private static void PoliceHelpCommand(User user, string[] args)
        {
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "-------------HELP-------------", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "---F1 to open interaction menu---", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "--------COMMANDS--------", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/panic | Alert all officers that you are in distress discretly.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/coponduty | Put on police uniform and go on duty.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/copoffduty | Take off police uniform and go off duty.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/policeonduty | Put on police uniform and go on duty.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/policeoffduty | Take off police uniform and go off duty.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/shield | Equip your riot shield if you have the item..", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/confiscate | Take all of someones illegal items.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/confiscateweapons | Take all of someones firearms.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/jail id timeInMinutes | Send someone to the jail for x minutes..", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/fine id amount | Fine someone money for thier crimes.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/unjail id | Release someone from the jail.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/addcop id | Add someone to police.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/copadd id | Add someone to police.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/remcop id | Remove someone from police.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/coprem id | Remove someone from police.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/setcoprank id rank | Set someones rank police.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/coppromote id rank | Set someones rank police.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/coprank id rank | Set someones rank police.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "--------Hotkeys--------", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "Left Ctrl + M in a polcioe vehicle for the radar system.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "Aim and press 4 to soft cuff.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "Aim and press 5 to hard cuff.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "Aim and press e to drag while restrained or dead.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "Hold e and press r to search the player.", 255, 0, 0);
        }

        private static void EMSHelpCommand(User user, string[] args)
        {
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "-------------HELP-------------", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "---F1 to open interaction menu---", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "--------COMMANDS--------", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "Aim and press e to drag while restrained or dead.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "Aim and press R to put into car / get out when restrained.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/emsonduty | Put on EMS uniform and go on duty.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/emsoffduty | Take off EMS uniform and go off duty.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/hospital id timeInMinutes | Send someone to the hospital for x minutes..", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/unhospital id | Release someone from the hospital.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/addems id | Add someone to EMS.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/emsadd id | Add someone to EMS.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/remems id | Remove someone from EMS.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/emsrem id | Remove someone from EMS.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/emspromote id rank | Set someones rank in EMS.", 255, 0, 0);
        }


        private static void HotwireCommand(User user, string[] args)
        {
            TriggerClientEvent(user.Source, "HotwireCar");
        }

        private static void ActionCommand(User user, string[] args)
        {
            var name = user.CurrentCharacter.FirstName + " " + user.CurrentCharacter.LastName;
            args[0] = null;
            var message = string.Join(" ", args);
            TriggerClientEvent("ActionCommand", user.Source.Handle, name, message);
        }

        private static void ActionCommand([FromSource] Player ply, string message)
        {
            var user = UserManager.Instance.GetUserFromPlayer(ply);
            var name = user.CurrentCharacter.FirstName + " " + user.CurrentCharacter.LastName;
            TriggerClientEvent("ActionCommand", user.Source.Handle, name, message);
        }

        public void ActionCommand(string message, Player ply)
        {
            var user = UserManager.Instance.GetUserFromPlayer(ply);
            var name = user.CurrentCharacter.FirstName + " " + user.CurrentCharacter.LastName;
            TriggerClientEvent("ActionCommand", user.Source.Handle, name, message);
        }

        private static void TweetCommand(User user, string[] args)
        {
            var name = user.CurrentCharacter.FirstName + "_" + user.CurrentCharacter.LastName;
            args[0] = null;
            var message = string.Join(" ", args);
            Utility.Instance.SendChatMessageAll("^5Twitter | @" + name.ToLower() + " ", "^7" + message, 255, 255, 255);
        }

        private static void AdvertCommand(User user, string[] args)
        {
            args[0] = null;
            var message = string.Join(" ", args);
            Utility.Instance.SendChatMessageAll("^3Craigslist | " + user.CurrentCharacter.FirstName + " ", "^2" + message, 255, 255, 255);
        }

        private static void TorCommand(User user, string[] args)
        {
            if (args.Length >= 3)
            {
                var name = args[1];
                args[0] = null;
                args[1] = null;
                var message = string.Join(" ", args);
                Utility.Instance.SendChatMessageAll("^9TOR | @" + name + " ", "^7" + message, 255, 255, 255);
            }
        }

        private static void OocCommand(User user, string[] args)
        {
            var name = user.Source.Name;
            args[0] = null;
            var message = string.Join(" ", args);
            Utility.Instance.SendChatMessageAll("^6OOC | " + name+" | "+user.Source.Handle + " ", "^7^_(("+message+" )) ", 255, 255, 255);
        }

        private static void LoocCommand(User user, string[] args)
        {
            var name = user.Source.Name;
            args[0] = null;
            var message = string.Join(" ", args);
            TriggerClientEvent("LoocCommand", user.Source.Handle + " ", name, message+" ");
        }

        private static void SupportCommand(User user, string[] args)
        {
            var name = user.Source.Name;
            args[0] = null;
            var message = string.Join(" ", args);
            Utility.Instance.SendChatMessage(user.Source, "^1[REPORT] " + name + " | " + user.Source.Handle+" ", "^*^7" + message, 255, 255, 255);
            foreach (Player admin in Admin.Instance.ActiveAdmins)
            {
                Utility.Instance.SendChatMessage(admin, "^1[REPORT]" + name+ " | "+user.Source.Handle + " ", "^*^7"+message, 255, 255, 255);
            }
        }



        private static void TextCommand(User user, string[] args)
        {
            if (args.Length < 3) { Utility.Instance.SendChatMessage(user.Source, "[Texting]", "Invalid amount of parameters provided.", 255, 255, 0); return; }
            var number = args[1];
            args[1] = null;
            args[0] = null;
            if (!args.Any()) { Utility.Instance.SendChatMessage(user.Source,"[Texting]","No message provided.",255,255,0); return; }
            var message = string.Join(" ",args);
            var tgtUser = UserManager.Instance.GetUserFromPhoneNumber(number);
            if (tgtUser != null)
            {
                Utility.Instance.SendChatMessage(tgtUser.Source, "[Texting]", "^2FROM(^3" + user.CurrentCharacter.PhoneNumber + "^2) ^7" + message, 255, 255, 0);
                Utility.Instance.SendChatMessage(user.Source, "[Texting]", "^1TO(^3" + tgtUser.CurrentCharacter.PhoneNumber + "^2) ^7" + message, 255, 255, 0);
                TriggerClientEvent(tgtUser.Source, "AlertSound2");
            }
            else
            {
                if (Int32.TryParse(number,out var output))
                {
                    var plyList = new PlayerList();
                    Player ply = plyList[output];
                    if (ply == null) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
                    var targetUser = UserManager.Instance.GetUserFromPlayer(ply);
                    Utility.Instance.SendChatMessage(targetUser.Source, "[Texting]", "^2FROM(^3" + user.CurrentCharacter.PhoneNumber + "^2) ^7" + message, 255, 255, 0);
                    Utility.Instance.SendChatMessage(user.Source, "[Texting]", "^1TO(^3" + targetUser.CurrentCharacter.PhoneNumber + "^2) ^7" + message, 255, 255, 0);
                    TriggerClientEvent(targetUser.Source, "AlertSound2");
                }
                else
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Texting]", "Invalid Phone Number", 255, 255, 0);
                }
            }
        }

        public static void TextFunction([FromSource] Player player, string number, string message)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var tgtUser = UserManager.Instance.GetUserFromPhoneNumber(number);
            if (tgtUser != null)
            {
                Utility.Instance.SendChatMessage(tgtUser.Source, "[Texting]", "^2FROM(^3" + user.CurrentCharacter.PhoneNumber + "^2) ^7" + message, 255, 255, 0);
                Utility.Instance.SendChatMessage(user.Source, "[Texting]", "^1TO(^3" + tgtUser.CurrentCharacter.PhoneNumber + "^2) ^7" + message, 255, 255, 0);
                TriggerClientEvent(tgtUser.Source, "AlertSound2");
            }
            else
            {
                if (Int32.TryParse(number, out var output))
                {
                    var plyList = new PlayerList();
                    Player ply = plyList[output];
                    if (ply == null) { Utility.Instance.SendChatMessage(user.Source, "[Police]", "Invalid player provided.", 0, 0, 255); return; }
                    var targetUser = UserManager.Instance.GetUserFromPlayer(ply);
                    Utility.Instance.SendChatMessage(targetUser.Source, "[Texting]", "^2FROM(^3" + user.CurrentCharacter.PhoneNumber + "^2) ^7" + message, 255, 255, 0);
                    Utility.Instance.SendChatMessage(user.Source, "[Texting]", "^1TO(^3" + targetUser.CurrentCharacter.PhoneNumber + "^2) ^7" + message, 255, 255, 0);
                    TriggerClientEvent(targetUser.Source, "AlertSound2");
                }
                else
                {
                    Utility.Instance.SendChatMessage(player, "[Texting]", "Invalid Phone Number", 255, 255, 0);
                }
            }
        }

        private static void EmergencyCallCommand(User user, string[] args)
        {
            if (args.Length < 2) { Utility.Instance.SendChatMessage(user.Source, "[911]", "Invalid amount of parameters provided.", 255, 255, 0); return; }
            args[0] = null;
            var message = string.Join(" ", args);
            TriggerClientEvent(user.Source, "911CallClient", message);
        }


        private static void NonEmergencyCallCommand(User user, string[] args)
        {
            if (args.Length < 2) { Utility.Instance.SendChatMessage(user.Source, "[911]", "Invalid amount of parameters provided.", 255, 255, 0); return; }
            args[0] = null;
            var message = string.Join(" ", args);
            TriggerClientEvent(user.Source, "311CallClient", message);
        }


        private static void DiceRollCommand(User user, string[] args)
        {
            var random = new Random();
            var rdmInt1 = random.Next(1,7);
            var rdmInt2 = random.Next(1,7);
            ActionCommand(user.Source,"Extends his arm throwing the dice. Dice 1 shows a "+rdmInt1+" and dice 2 shows a "+rdmInt2+")");
        }
        
    }
}
