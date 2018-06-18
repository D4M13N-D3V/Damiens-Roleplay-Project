using System;
using CitizenFX.Core;
using server.Main.Users;
using server.Main.EmergencyServices.EMS;
namespace server.Main.EmergencyServices
{
    /// <summary>
    /// The dispatch manager
    /// </summary>
    public class DispatchSystem : BaseScript
    {
        /// <summary>
        /// The singleton instance
        /// </summary>
        public static DispatchSystem Instance;

        /// <summary>
        /// The current amount of calls since restart
        /// </summary>
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
            foreach (var user in Police.Police.Instance.OnDutyOfficers.Keys)
            {
                if (user.Source != null)
                {
                    TriggerClientEvent(user.Source, "RemoveBlip",i);
                }
            }

            foreach (var user in EMS.EMS.Instance.OnDutyEms.Keys)   
            {
                if (user.Source != null)
                {
                    TriggerClientEvent(user.Source, "RemoveBlip", i);
                }
            }
        }

        /// <summary>
        /// 911 Call event handler
        /// </summary>
        /// <param name="player">The player who called it</param>
        /// <param name="message">The message</param>
        /// <param name="street1">The first street</param>
        /// <param name="street2">The sEcond street</param>
        /// <param name="zone">The zone that the player is in.</param>
        private void EmergencyCall([FromSource]Player player, string message, string street1, string street2, string zone)
        {
            Utility.Instance.SendChatMessage(player, "[911]", "^2SENT ^1(^3" + street1 + "," + street2 + " in " + zone + "^2)^7" + message, 255, 0, 0);
            _currentCallNumber++;
            var pos = UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Pos;
            foreach (var user in Police.Police.Instance.OnDutyOfficers.Keys)
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
                    Police.Police.Instance.OnDutyOfficers.Remove(user);
                }
            }
            foreach (var user in EMS.EMS.Instance.OnDutyEms.Keys)
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
                    EMS.EMS.Instance.OnDutyEms.Remove(user);
                }
            }
        }
        /// <summary>
        /// The anonymous 911 call usually for locals
        /// </summary>
        /// <param name="player">The player who called it</param>
        /// <param name="message">The message</param>
        /// <param name="street1">The first street</param>
        /// <param name="street2">The sEcond street</param>
        /// <param name="zone">The zone that the player is in.</param>
        private void EmergencyCallAnonymous([FromSource]Player player, string message, string street1, string street2, string zone)
        {
            try
            {
                _currentCallNumber++;
                var pos = UserManager.Instance.GetUserFromPlayer(player).CurrentCharacter.Pos;
                foreach (var user in Police.Police.Instance.OnDutyOfficers.Keys)
                {
                    if (user.Source != null)
                    {
                        var ply = user.Source; Utility.Instance.SendChatMessage(ply, "[911] " + _currentCallNumber + " ", "^1(^3" + street1 + "," + street2 + " in " + zone + "^1)^7" + message, 255, 0, 0);
                        TriggerClientEvent(ply, "AlertSound");
                        TriggerClientEvent(ply, "AlertBlip", _currentCallNumber, pos.X, pos.Y, pos.Z);
                    }
                    else
                    {
                        Police.Police.Instance.OnDutyOfficers.Remove(user);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// The 311 call.
        /// </summary>
        /// <param name="player">The player who called it</param>
        /// <param name="message">The message</param>
        /// <param name="street1">The first street</param>
        /// <param name="street2">The sEcond street</param>
        /// <param name="zone">The zone that the player is in.</param>
        private void NonEmergencyCall([FromSource]Player player, string message, string street1, string street2, string zone)
        {
            Utility.Instance.SendChatMessage(player, "[311] "+_currentCallNumber+" ", "^2SENT ^1(^3" + street1 + "," + street2 + " in " + zone + "^2)^7" + message, 255, 255, 0);
            foreach (var user in Police.Police.Instance.OnDutyOfficers.Keys)
            {
                if (user.Source != null)
                {
                    var ply = user.Source;
                    Utility.Instance.SendChatMessage(ply, "[311] " + _currentCallNumber + " ", "^1(^3" + street1 + "," + street2 + " in " + zone + "^1)^7" + message, 255, 255, 0);
                    TriggerClientEvent(ply, "AlertSound");
                }
                else
                {
                    Police.Police.Instance.OnDutyOfficers.Remove(user);
                }
            }
            foreach (var user in EMS.EMS.Instance.OnDutyEms.Keys)
            {
                if (user.Source != null)
                {
                    var ply = user.Source;
                    Utility.Instance.SendChatMessage(ply, "[311] " + _currentCallNumber + " ", "^1(^3" + street1 + "," + street2 + " in " + zone + "^1)^7" + message, 255, 255, 0);
                    TriggerClientEvent(ply, "AlertSound");
                }
                else
                {
                    EMS.EMS.Instance.OnDutyEms.Remove(user);
                }
            }
        }
    }
}
