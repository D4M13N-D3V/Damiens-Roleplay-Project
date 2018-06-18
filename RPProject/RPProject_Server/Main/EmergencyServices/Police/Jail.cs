using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using server.Main.Users;

namespace server.Main.EmergencyServices.Police
{
    /// <summary>
    /// Manaages jail from server side
    /// </summary>
    public class Jail : BaseScript
    {
        /// <summary>
        /// Singleton Instance.
        /// </summary>
        public static Jail Instance;

        public Jail()
        {
            Instance = this;
            EventHandlers["UpdateJailTime"] += new Action<Player, int>(UpdateJailTime);
        }

        /// <summary>
        /// updates the jail time serverside for characer
        /// </summary>
        /// <param name="ply">player triggering</param>
        /// <param name="time">time to set to</param>
        private void UpdateJailTime([FromSource]Player ply, int time)
        {
            //THIS IS VERY INSECURE AND CAN  EASIULY BE MANIPUALTED FIND A BETTER WAY.
            var user = UserManager.Instance.GetUserFromPlayer(ply);
            if (user != null && user.CurrentCharacter != null)
            {
                user.CurrentCharacter.JailTime = time;
            }
        }
    }
}
