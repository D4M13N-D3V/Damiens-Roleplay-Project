using System;
using CitizenFX.Core;
using server.Main.Users;

namespace server.Main.EmergencyServices
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
            EventHandlers["RemoveEmergencyBlipServer"] += new Action<Player, int>(RemoveBlip);
        }

        private void RemoveBlip([FromSource]Player player, int i)
        {
            foreach (var user in Police.Instance.OnDutyOfficers.Keys)
            {
                if (user.Source != null)
                {
                    TriggerClientEvent(user.Source, "RemoveBlip",i);
                }
            }

            foreach (var user in EMS.Instance.OnDutyEms.Keys)   
            {
                if (user.Source != null)
                {
                    TriggerClientEvent(user.Source, "RemoveBlip", i);
                }
            }
        }

        private void EmergencyCall([FromSource]Player player, string message, string street1, string street2, string zone)
        {
            Utility.Instance.SendChatMessage(player, "[911]", "^2SENT ^1(^3" + street1 + "," + street2 + " in " + zone + "^2)^7" + message, 255, 0, 0);
            _currentCallNumber++;
            var pos = UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Pos;
            foreach (var user in Police.Instance.OnDutyOfficers.Keys)
            {
                if (user.Source != null)
                {
                    var ply = user.Source;
                    Utility.Instance.SendChatMessage(ply, "[911] " + _currentCallNumber + " ", "^1(^3" + street1 + "," + street2 + " in " + zone + "^1)^7" + message, 255, 0, 0);
                    TriggerClientEvent(ply, "AlertSound");
                    TriggerClientEvent(ply, "AlertBlip", _currentCallNumber, pos.X, pos.Y, pos.Z);
                }
                else
                {
                    Police.Instance.OnDutyOfficers.Remove(user);
                }
            }
            foreach (var user in EMS.Instance.OnDutyEms.Keys)
            {
                if (user.Source != null)
                {
                    var ply = user.Source;
                    Utility.Instance.SendChatMessage(ply, "[911] " + _currentCallNumber + " ", "^1(^3" + street1 + "," + street2 + " in " + zone + "^1)^7" + message, 255, 0, 0);
                    TriggerClientEvent(ply, "AlertSound");
                    TriggerClientEvent(ply, "AlertBlip", _currentCallNumber, pos.X, pos.Y, pos.Z);
                }
                else
                {
                    EMS.Instance.OnDutyEms.Remove(user);
                }
            }
        }

        private void EmergencyCallAnonymous([FromSource]Player player, string message, string street1, string street2, string zone)
        {
            _currentCallNumber++;
            var pos = UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Pos;
            foreach (var user in Police.Instance.OnDutyOfficers.Keys)
            {
                if (user.Source != null)
                {
                    var ply = user.Source; Utility.Instance.SendChatMessage(ply, "[911] " + _currentCallNumber + " ", "^1(^3" + street1 + "," + street2 + " in " + zone + "^1)^7" + message, 255, 0, 0);
                    TriggerClientEvent(ply, "AlertSound");
                    TriggerClientEvent(ply, "AlertBlip", _currentCallNumber, pos.X, pos.Y, pos.Z);
                }
                else
                {
                    Police.Instance.OnDutyOfficers.Remove(user);
                }
            }
        }

        private void NonEmergencyCall([FromSource]Player player, string message, string street1, string street2, string zone)
        {
            Utility.Instance.SendChatMessage(player, "[311] "+_currentCallNumber+" ", "^2SENT ^1(^3" + street1 + "," + street2 + " in " + zone + "^2)^7" + message, 255, 255, 0);
            foreach (var user in Police.Instance.OnDutyOfficers.Keys)
            {
                if (user.Source != null)
                {
                    var ply = user.Source;
                    Utility.Instance.SendChatMessage(ply, "[311] " + _currentCallNumber + " ", "^1(^3" + street1 + "," + street2 + " in " + zone + "^1)^7" + message, 255, 255, 0);
                    TriggerClientEvent(ply, "AlertSound");
                }
                else
                {
                    Police.Instance.OnDutyOfficers.Remove(user);
                }
            }
            foreach (var user in EMS.Instance.OnDutyEms.Keys)
            {
                if (user.Source != null)
                {
                    var ply = user.Source;
                    Utility.Instance.SendChatMessage(ply, "[311] " + _currentCallNumber + " ", "^1(^3" + street1 + "," + street2 + " in " + zone + "^1)^7" + message, 255, 255, 0);
                    TriggerClientEvent(ply, "AlertSound");
                }
                else
                {
                    EMS.Instance.OnDutyEms.Remove(user);
                }
            }
        }
    }
}
