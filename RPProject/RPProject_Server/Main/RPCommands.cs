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

            CommandManager.Instance.AddCommand("me", ActionCommand);
            CommandManager.Instance.AddCommand("tweet", TweetCommand);
            CommandManager.Instance.AddCommand("tor", TorCommand);
            CommandManager.Instance.AddCommand("ooc", OocCommand);
            CommandManager.Instance.AddCommand("looc", LoocCommand);
            CommandManager.Instance.AddCommand("report", SupportCommand);
        }

        private void ActionCommand(User user, string[] args)
        {
            var name = user.CurrentCharacter.FirstName + " " + user.CurrentCharacter.LastName;
            var message = "";
            for (int i = 1; i < args.Length-1; i++)
            {
                message = message+args[i];
            }
            TriggerClientEvent("ActionCommand",user.Source.Handle,name,message);
        }

        private void TweetCommand(User user, string[] args)
        {
            var name = user.CurrentCharacter.FirstName + " " + user.CurrentCharacter.LastName;
            args[0] = null;
            var message = string.Concat(args);
            Utility.Instance.SendChatMessageAll("[TWITTER] "+name, message,0,0,200);
        }

        private void TorCommand(User user, string[] args)
        {
            var name = args[1];
            args[0] = null;
            var message = string.Concat(args);
            for (int i = 2; i < args.Length - 1; i++)
            {
                message = message + args[i] + " ";
            }
            Utility.Instance.SendChatMessageAll("[TOR] " + name, message, 150, 150, 150);
        }

        private void OocCommand(User user, string[] args)
        {
            var name = user.Source.Name;
            args[0] = null;
            var message = string.Concat(args);
            for (int i = 1; i < args.Length - 1; i++)
            {
                message = message + args[i] + " ";
            }
            Utility.Instance.SendChatMessageAll("[OOC] " + name+"|"+user.Source.Handle, message, 255, 255, 255);
        }

        private void LoocCommand(User user, string[] args)
        {
            var name = user.Source.Name;
            args[0] = null;
            var message = string.Concat(args);
            for (int i = 1; i < args.Length - 1; i++)
            {
                message = message + args[i] + " ";
            }
            TriggerClientEvent("LoocCommand", user.Source.Handle, name, message);
        }

        private void SupportCommand(User user, string[] args)
        {
            var name = user.Source.Name;
            args[0] = null;
            var message = string.Concat(args);
            for (int i = 1; i < args.Length - 1; i++)
            {
                message = message + args[i] + " ";
            }

            Utility.Instance.SendChatMessage(user.Source, "[REPORT] " + name + "|" + user.Source.Handle, message, 255, 0, 0);
            foreach (Player admin in Admin.Instance.ActiveAdmins)
            {
                Utility.Instance.SendChatMessage(admin,"[REPORT] " + name+ "|"+user.Source.Handle, message, 255, 0, 0);
            }
        }
    }
}
