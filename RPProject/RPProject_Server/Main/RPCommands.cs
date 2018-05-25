using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using roleplay.Main.Users;

namespace roleplay.Main
{
    public class RPCommands:BaseScript
    {
        public static RPCommands Instance;

        public RPCommands()
        {
            Instance = this;
            SetupCommands();
            EventHandlers["ActionCommandFromClient"] += new Action<Player,string>(ActionCommand);
        }

        private async void SetupCommands()
        {
            while (CommandManager.Instance == null)
            {
                await Delay(0);
            }
            while (Utility.Instance == null)
            {
                await Delay(0);
            }

            CommandManager.Instance.AddCommand("hotwire", HotwireCommand);
            CommandManager.Instance.AddCommand("me", ActionCommand);
            CommandManager.Instance.AddCommand("tweet", TweetCommand);
            CommandManager.Instance.AddCommand("tor", TorCommand);
            CommandManager.Instance.AddCommand("ooc", OocCommand);
            CommandManager.Instance.AddCommand("looc", LoocCommand);
            CommandManager.Instance.AddCommand("report", SupportCommand);
            CommandManager.Instance.AddCommand("help", HelpCommand); 
        }

        private void HelpCommand(User user, string[] args)
        {
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "-------------HELP-------------", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "---F1 to open interaction menu---", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "--------COMMANDS--------", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/me | To roleplay out actions.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/tweet message | To send out a tweet.", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/tor name message | To send out a anonymous message with a custom name", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/looc message | Local out of character chat", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/ooc message | Global out of character chat", 255, 0, 0);
            Utility.Instance.SendChatMessage(user.Source, "[HELP]", "/report message | Send a report to online admins", 255, 0, 0);
        }
        
        private void HotwireCommand(User user, string[] args)
        {
            TriggerClientEvent(user.Source, "HotwireCar");
        }

        private void ActionCommand(User user, string[] args)
        {
            var name = user.CurrentCharacter.FirstName + " " + user.CurrentCharacter.LastName;
            args[0] = null;
            var message = string.Join(" ", args);
            TriggerClientEvent("ActionCommand", user.Source.Handle, name, message);
        }

        private void ActionCommand([FromSource] Player ply, string message)
        {
            var user = UserManager.Instance.GetUserFromPlayer(ply);
            var name = user.CurrentCharacter.FirstName + " " + user.CurrentCharacter.LastName;
            TriggerClientEvent("ActionCommand", user.Source.Handle, name, message);
        }

        private void TweetCommand(User user, string[] args)
        {
            var name = user.CurrentCharacter.FirstName + " " + user.CurrentCharacter.LastName;
            args[0] = null;
            var message = string.Join(" ", args);
            Utility.Instance.SendChatMessageAll("[TWITTER] "+name, message,0,0,200);
        }

        private void TorCommand(User user, string[] args)
        {
            var name = args[1];
            args[0] = null;
            args[1] = null;
            var message = string.Join(" ", args);
            Utility.Instance.SendChatMessageAll("[TOR] " + name, message, 150, 150, 150);
        }

        private void OocCommand(User user, string[] args)
        {
            var name = user.Source.Name;
            args[0] = null;
            var message = string.Join(" ", args);
            Utility.Instance.SendChatMessageAll("[OOC] " + name+" | "+user.Source.Handle, message, 255, 120, 120);
        }

        private void LoocCommand(User user, string[] args)
        {
            var name = user.Source.Name;
            args[0] = null;
            var message = string.Join(" ", args);
            TriggerClientEvent("LoocCommand", user.Source.Handle, name, message);
        }

        private void SupportCommand(User user, string[] args)
        {
            var name = user.Source.Name;
            args[0] = null;
            var message = string.Join(" ", args);
            Utility.Instance.SendChatMessage(user.Source, "[REPORT] " + name + " | " + user.Source.Handle, message, 255, 0, 0);
            foreach (Player admin in Admin.Instance.ActiveAdmins)
            {
                Utility.Instance.SendChatMessage(admin,"[REPORT] " + name+ " | "+user.Source.Handle, message, 255, 0, 0);
            }
        }
    }
}
