using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace client.Main
{
    public class RPCommands : BaseScript
    {
        public static RPCommands Instance;

        public RPCommands()
        {
            Instance = this;
            EventHandlers["ActionCommand"] += new Action<int, string, string>(ActionCommand);
            EventHandlers["LoocCommand"] += new Action<int, string, string>(LoocCommand);
            EventHandlers["CleanCarByHand"] += new Action(Target);
        }

        private async void Target()
        {
            if (Game.PlayerPed.IsInVehicle())
            {
                return;
            }
            API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "WORLD_HUMAN_MAID_CLEAN",0,true);
            await Delay(5000);
            Game.PlayerPed.Task.ClearAll();
            Utility.Instance.ClosestVehicle.DirtLevel = 0;
        }

        private void ActionCommand(int player, string name, string message)
        {
            var nearbyPlayers = new List<int>();
            Utility.Instance.GetPlayersInRadius(API.GetPlayerFromServerId(player), 15, out nearbyPlayers);
            if (nearbyPlayers.Contains(API.PlayerId()) || API.PlayerId()==API.GetPlayerFromServerId(player))
            {
                Utility.Instance.SendChatMessage("^4" + name, "^3"+message, 255, 0, 255);
            }
        }

        private void LoocCommand(int player, string name, string message)
        {
            var nearbyPlayers = new List<int>();
            Utility.Instance.GetPlayersInRadius(API.GetPlayerFromServerId(player), 15, out nearbyPlayers);
            if (nearbyPlayers.Contains(API.PlayerId()) || API.PlayerId() == API.GetPlayerFromServerId(player))
            {
                Utility.Instance.SendChatMessage("^9LOOC | "+name+" |"+player, "^7^_(("+message+"))", 255, 150, 150);
            }
        }
    }
}
