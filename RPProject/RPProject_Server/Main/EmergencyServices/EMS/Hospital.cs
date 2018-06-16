using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using server.Main.Users;

namespace server.Main.EmergencyServices.EMS
{
    public class Hospital : BaseScript
    {
        public static Hospital Instance;

        public Hospital()
        {
            Instance = this;
            EventHandlers["UpdateHospitalTime"] += new Action<Player, int>(UpdateHospitalTime);
        }

        private void UpdateHospitalTime([FromSource]Player ply, int time)
        {
            //THIS IS VERY INSECURE AND CAN  EASIULY BE MANIPUALTED FIND A BETTER WAY.
            var user = UserManager.Instance.GetUserFromPlayer(ply);
            user.CurrentCharacter.HospitalTime = time;
        }
    }
}
