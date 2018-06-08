using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace roleplay.Main
{
    public class DispatchSystem : BaseScript
    {
        public static DispatchSystem Instance;
        private int _currentCallNumber = 0;

        public DispatchSystem()
        {
            Instance = this;
            EventHandlers["911CallServer"] += new Action<Player, string, string, string, string>(EmergencyCall);
            EventHandlers["911CallServerAnonymous"] += new Action<Player, string, string, string, string>(EmergencyCallAnonymous);
            EventHandlers["311CallServer"] += new Action<Player, string, string, string, string>(NonEmergencyCall);
        }

        private void EmergencyCall([FromSource]Player player, string message, string street1, string street2, string zone)
        {
            Utility.Instance.SendChatMessage(player, "[911]", "^2SENT ^1(^3" + street1 + "," + street2 + " in " + zone + "^2)^7" + message, 255, 0, 0);
            _currentCallNumber++;
            foreach (var ply in new PlayerList())
            {
                if (Police.Instance.IsPlayerOnDuty(ply))
                {
                    Utility.Instance.SendChatMessage(ply, "[911] " + _currentCallNumber + " ", "^1(^3" + street1 + "," + street2 + " in " + zone + "^1)^7" + message, 255, 0, 0);
                    TriggerClientEvent(ply, "AlertSound");
                    TriggerClientEvent(ply, "AlertBlip", _currentCallNumber, UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Pos.X, UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Pos.Y, UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Pos.Z);
                }
                else if (EMS.Instance.IsPlayerOnDuty(ply))
                {
                    Utility.Instance.SendChatMessage(ply, "[911] " + _currentCallNumber + " ", "^1(^3" + street1 + "," + street2 + " in " + zone + "^1)^7" + message, 255, 0, 0);
                    TriggerClientEvent(ply, "AlertSound");
                }
            }
        }

        private void EmergencyCallAnonymous([FromSource]Player player, string message, string street1, string street2, string zone)
        {
            _currentCallNumber++;
            foreach (var ply in new PlayerList())
            {
                if (Police.Instance.IsPlayerOnDuty(ply))
                {
                    Utility.Instance.SendChatMessage(ply, "[911] " + _currentCallNumber + " ", "^1(^3" + street1 + "," + street2 + " in " + zone + "^1)^7" + message, 255, 0, 0);
                    TriggerClientEvent(ply, "AlertSound");
                    TriggerClientEvent(ply, "AlertBlip", _currentCallNumber, UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Pos.X, UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Pos.Y, UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Pos.Z);
                }
                else if (EMS.Instance.IsPlayerOnDuty(ply))
                {
                    Utility.Instance.SendChatMessage(ply, "[911] " + _currentCallNumber + " ", "^1(^3" + street1 + "," + street2 + " in " + zone + "^1)^7" + message, 255, 0, 0);
                    TriggerClientEvent(ply, "AlertSound");
                }
            }
        }

        private void NonEmergencyCall([FromSource]Player player, string message, string street1, string street2, string zone)
        {
            Utility.Instance.SendChatMessage(player, "[311] "+_currentCallNumber+" ", "^2SENT ^1(^3" + street1 + "," + street2 + " in " + zone + "^2)^7" + message, 255, 255, 0);
            foreach (var ply in new PlayerList())
            {
                _currentCallNumber++;
                if (Police.Instance.IsPlayerOnDuty(ply))
                {
                    Utility.Instance.SendChatMessage(ply, "[311] " + _currentCallNumber + " ", "^1(^3" + street1 + "," + street2 + " in " + zone + "^1)^7" + message, 255, 255, 0);
                    TriggerClientEvent(ply, "AlertSound");
                }
                else if (EMS.Instance.IsPlayerOnDuty(ply))
                {
                    Utility.Instance.SendChatMessage(ply, "[311] " + _currentCallNumber + " ", "^1(^3" + street1 + "," + street2 + " in " + zone + "^1)^7" + message, 255, 255, 0);
                    TriggerClientEvent(ply, "AlertSound");
                }
            }
        }
    }
}
