using CitizenFX.Core;
using server.Main.Users;
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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            SetupCommands();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("hotwire", HotwireCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("me", ActionCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("tweet", TweetCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("t", TweetCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("advert", AdvertCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("tor", TorCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("ooc", OocCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("o", OocCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("g", OocCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("looc", LoocCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("report", SupportCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("help", HelpCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("emshelp", EMSHelpCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("cophelp", PoliceHelpCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            #endregion

            #region Vehicle Commands
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("engine", (user, strings) =>
            {
                TriggerClientEvent(user.Source,"ToggleEngine");
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("hood", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "ToggleHood");
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("trunk", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "ToggleTrunk");
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("lock", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "ToggleLock");
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("windowsdown", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "WindowsDown");
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("windowsup", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "WindowsUp");
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            #endregion

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("text", TextCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("911", EmergencyCallCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("311", NonEmergencyCallCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("repair", (user, strings) =>
            {
                if (user.CurrentCharacter.Money.Cash >= 1000)
                {
                    MoneyManager.Instance.RemoveMoney(user.Source, MoneyTypes.Cash, 1000);
                    TriggerClientEvent(user.Source, "RepairCar");
                }
                else if (user.CurrentCharacter.Money.Bank >= 1000)
                {
                    MoneyManager.Instance.RemoveMoney(user.Source, MoneyTypes.Bank, 1000);
                    TriggerClientEvent(user.Source, "RepairCar");
                }
                else
                {
                    Utility.Instance.SendChatMessage(user.Source,"[Repair]","Not enough money costs 1000$",255,255,0);
                }
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("selldrugs", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "StartSellingDrugs");
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("injuries", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "InjuryCheckCommand");
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("checkinjuries", (user, strings) =>
            {
                TriggerClientEvent(user.Source, "InjuryCheckCommand");
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("clean", CleanCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("roll", DiceRollCommand);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            CommandManager.Instance.AddCommand("999", TowCallCommand);
            CommandManager.Instance.AddCommand("888", TaxiCallCommand);
        }

        private void TowCallCommand(User user, string[] args)
        {
            TriggerClientEvent("TowBroadcast", user.CurrentCharacter.Pos.X, user.CurrentCharacter.Pos.Y, user.CurrentCharacter.Pos.Z);
        }

        private void TaxiCallCommand(User user, string[] args)
        {
            TriggerClientEvent("TaxiBroadcast", user.CurrentCharacter.Pos.X, user.CurrentCharacter.Pos.Y, user.CurrentCharacter.Pos.Z);
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
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/999 Calls a tow truck driver to your location.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/888 Calls a taxi driver to your location.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/clean | Cleans the car nearby.", 255, 0, 0);
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
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/marine | Spawns police boat..", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/air1 | Spawns police maverick.", 255, 0, 0);
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
                if (user.CurrentCharacter.Money.Cash >= 5000)
                {
                    MoneyManager.Instance.RemoveMoney(user.Source, MoneyTypes.Cash, 5000);
                    var name = args[1];
                    args[0] = null;
                    args[1] = null;
                    var message = string.Join(" ", args);
                    Utility.Instance.SendChatMessageAll("^9TOR | @" + name + " ", "^7" + message, 255, 255, 255);
                }
                else
                {
                    Utility.Instance.SendChatMessage(user.Source, "[Repair]", "Not enough money costs 1000$", 255, 255, 0);
                }
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
                    if (targetUser != null)
                    {
                        Utility.Instance.SendChatMessage(targetUser.Source, "[Texting]", "^2FROM(^3" + user.CurrentCharacter.PhoneNumber + "^2) ^7" + message, 255, 255, 0);
                        Utility.Instance.SendChatMessage(user.Source, "[Texting]", "^1TO(^3" + targetUser.CurrentCharacter.PhoneNumber + "^2) ^7" + message, 255, 255, 0);
                        TriggerClientEvent(targetUser.Source, "AlertSound2");
                    }
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

        private void EmergencyCallCommand(User user, string[] args)
        {
            if (args.Length < 2) { Utility.Instance.SendChatMessage(user.Source, "[911]", "Invalid amount of parameters provided.", 255, 255, 0); return; }
            args[0] = null;
            var message = string.Join(" ", args);
            TriggerClientEvent(user.Source, "911CallClient", message);
            ActionCommand("Takes thier phone out and calls 911.",user.Source);
            TriggerClientEvent(user.Source, "PhoneLookAt");
        }


        private void NonEmergencyCallCommand(User user, string[] args)
        {
            if (args.Length < 2) { Utility.Instance.SendChatMessage(user.Source, "[911]", "Invalid amount of parameters provided.", 255, 255, 0); return; }
            args[0] = null;
            var message = string.Join(" ", args);
            ActionCommand("Takes thier phone out and calls 311.", user.Source);
            TriggerClientEvent(user.Source, "311CallClient", message);
            TriggerClientEvent(user.Source, "PhoneLookAt");
        }


        private static void DiceRollCommand(User user, string[] args)
        {
            var random = new Random();
            var rdmInt1 = random.Next(1, 7);
            var rdmInt2 = random.Next(1, 7);
            ActionCommand(user.Source, "Extends his arm throwing the dice. Dice 1 shows a " + rdmInt1 + " and dice 2 shows a " + rdmInt2 + ")");
        }

        private static void CleanCommand(User user, string[] args)
        {
            TriggerClientEvent(user.Source,"CleanCarByHand");
        }

    }
}
