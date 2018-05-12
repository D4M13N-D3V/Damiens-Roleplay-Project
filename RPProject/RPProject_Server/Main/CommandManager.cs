using System;
using System.Collections.Generic;
using roleplay.Main.Users;
using CitizenFX.Core;
using CitizenFX.Core.Native;
namespace roleplay.Main
{
    public class CommandManager : BaseScript
    {
        public static CommandManager Instance;

        private Dictionary<string, Action<User,string[]>> Commands = new Dictionary<string, Action<User,string[]>>();

        public CommandManager()
        {
            Instance = this;

            EventHandlers["chatMessage"] += new Action<int, string, string>(ProcessCommand);
        }

        private void ProcessCommand(int pid, string name, string message)
        {
            API.CancelEvent();
            var player = API.GetPlayerFromIndex(pid);   
            var arguments = message.Split(' ');
            arguments[0] = arguments[0].Remove(0, 1);
            Action<User,string[]> command;
            Commands.TryGetValue(arguments[0],out command);
            if (command != null)
            {
                var user = UserManager.Instance.GetUserFromPlayer(name);
                command(user,arguments);
            }
            else
            {
                Utility.Instance.Log("A command was requested to be ran by "+API.GetPlayerName(player)+", but it is invalid, please make sure that it has been properly added.");
            }
        }

        public void AddCommand( string command , Action<User, string[]> callback)
        {
            Utility.Instance.Log("A command was added to the command system. ["+command+"]");
            Commands.Add(command, callback);
        }

    }
}
