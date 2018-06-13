using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client.Main.Items;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace client.Main.EmergencyServices.EMS
{
    public class Hospital : BaseScript
    {
        public static Hospital Instance;

        public bool InHospital = false;
        public int TimeLeft = 0;

        private readonly Vector3 _inPos = new Vector3(331.889771f, -585.050354f, 43.3529778f);
        private readonly Vector3 _outPos = new Vector3(308.729767f, -591.7288f, 43.291893f);

        public Hospital()
        {
            Instance = this;
            EventHandlers["Hospital"] += new Action<dynamic>(JailFunc);
            EventHandlers["Unhospital"] += new Action(UnjailFunc);
        }

        private void JailFunc(dynamic timeDynamic)
        {
            Game.PlayerPed.Weapons.RemoveAll();
            Game.PlayerPed.IsInvincible = true;
            Game.PlayerPed.CanSwitchWeapons = false;

            API.SetEntityCoords(Game.PlayerPed.Handle, _inPos.X, _inPos.Y, _inPos.Z, false, false, false, false);
            TimeLeft = timeDynamic;
            InHospital = true;
            Loop();
            Draw();
        }

        private async Task Draw()
        {
            while (InHospital)
            {
                Utility.Instance.DrawTxt(0.5f, 0.05f, 0, 200, 1, "Time Left : " + TimeLeft, 255, 255, 255, 255, true);
                await Delay(0);
            }
        }

        private async Task Loop()
        {
            while (InHospital)
            {
                Game.PlayerPed.Weapons.RemoveAll();
                Game.PlayerPed.IsInvincible = true;
                Game.PlayerPed.CanSwitchWeapons = false;
                TimeLeft = TimeLeft - 1;
                var pos = Game.PlayerPed.Position;
                if (API.Vdist(pos.X, pos.Y, pos.Z, _inPos.X, _inPos.Y, _inPos.Z) > 30)
                {
                    API.SetEntityCoords(Game.PlayerPed.Handle, _inPos.X, _inPos.Y, _inPos.Z, false, false, false, false);
                }
                TriggerServerEvent("UpdateHospitalTime", TimeLeft);
                await Delay(1000);

                if (TimeLeft == 0)
                {
                    break;
                }
            }
            UnjailFunc();
        }
        private void UnjailFunc()
        {
            API.SetEntityCoords(Game.PlayerPed.Handle, _outPos.X, _outPos.Y, _outPos.Z, false, false, false, false);
            InHospital = false;
            Game.PlayerPed.IsInvincible = false;
            Game.PlayerPed.CanSwitchWeapons = true;
            Weapons.Instance.RefreshWeapons();
        }

    }
}
