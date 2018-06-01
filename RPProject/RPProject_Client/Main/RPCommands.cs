using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay.Main
{
    public class RPCommands : BaseScript
    {
        public static RPCommands Instance;

        public RPCommands()
        {
            Instance = this;
            EventHandlers["ActionCommand"] += new Action<int, string, string>(ActionCommand);
            EventHandlers["LoocCommand"] += new Action<int, string, string>(LoocCommand);
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
                Utility.Instance.SendChatMessage("^9LOOC |"+player, "^7^_(("+message+"))", 255, 150, 150);
            }
        }
    }
}
