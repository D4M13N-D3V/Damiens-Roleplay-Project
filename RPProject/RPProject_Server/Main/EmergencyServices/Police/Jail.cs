using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using server.Main.Users;

namespace server.Main.EmergencyServices.Police
{

    public class Jail : BaseScript
    {
        public static Jail Instance;

        public Jail()
        {
            Instance = this;
            EventHandlers["UpdateJailTime"] += new Action<Player, int>(UpdateJailTime);
        }

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
