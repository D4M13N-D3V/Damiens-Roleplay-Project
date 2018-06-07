using System;
using System.Collections.Generic;
using roleplay.Main.Users;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;

namespace roleplay.Main
{
    public class CommandManager : BaseScript
    {
        public static CommandManager Instance = null;

        private Dictionary<string, Action<User,string[]>> Commands = new Dictionary<string, Action<User,string[]>>();

        public CommandManager()
        {
            Instance = this;

            EventHandlers["chatMessage"] += new Action<int, string, string>(ProcessCommand);
        }

        private void ProcessCommand(int pid, string name, string message)
        {
            API.CancelEvent();
            var user = UserManager.Instance.GetUserFromPlayer(name);
            var arguments = message.Split(' ');
            arguments[0] = arguments[0].Remove(0, 1);
            if (Commands.ContainsKey(arguments[0]))
            {
                Commands[arguments[0]](user,arguments);
            }
            else
            {
                Utility.Instance.SendChatMessage(user.Source,"[COMMAND SYSTEM]","Invalid command. Try again. Do /help , /emshelp, /cophelp to see the commands!",255,0,0);
            }
        }

        public async Task AddCommand( string command , Action<User, string[]> callback)
        {
            while (Utility.Instance == null)
            {
                await Delay(0);
            }
            Utility.Instance.Log("A command was added to the command system. ["+command+"]");
            Commands.Add(command, callback);
        }

    }
}
