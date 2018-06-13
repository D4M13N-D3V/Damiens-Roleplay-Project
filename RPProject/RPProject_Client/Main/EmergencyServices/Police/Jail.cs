using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client.Main.Items;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace client.Main.EmergencyServices.Police
{
    public class Jail : BaseScript
    {
        public static Jail Instance;

        public bool InJail = false;
        public int TimeLeft = 0;

        private readonly Vector3 _inPos = new Vector3(1710.7552490234f, 2672.4741210938f, 45.564888000488f);
        private readonly Vector3 _outPos = new Vector3(1878.4635f, 2606.99438f, 45.6720123f);

        public Jail()
        {
            Instance = this;
            EventHandlers["Jail"] += new Action<dynamic>(JailFunc);
            EventHandlers["Unjail"] += new Action(UnjailFunc);
        }

        private void JailFunc(dynamic timeDynamic)
        {
            Game.PlayerPed.Weapons.RemoveAll();
            Game.PlayerPed.IsInvincible = true;
            Game.PlayerPed.CanSwitchWeapons = false;

            API.SetEntityCoords(Game.PlayerPed.Handle, _inPos.X, _inPos.Y, _inPos.Z, false, false, false, false);
            TimeLeft = timeDynamic;
            InJail = true;
            Loop();
            Draw();
        }

        private async Task Draw()
        {
            while (InJail)
            {
                Utility.Instance.DrawTxt(0.5f, 0.05f, 0, 200, 1, "Time Left : " + TimeLeft, 255, 255, 255, 255, true);
                await Delay(0);
            }
        }

        private async Task Loop()
        {
            while (InJail)
            {
                Game.PlayerPed.Weapons.RemoveAll();
                Game.PlayerPed.IsInvincible = true;
                Game.PlayerPed.CanSwitchWeapons = false;
                TimeLeft = TimeLeft - 1;
                var pos = Game.PlayerPed.Position;
                if (API.Vdist(pos.X, pos.Y, pos.Z, _inPos.X, _inPos.Y, _inPos.Z) > 30)
                {
                    API.SetEntityCoords(Game.PlayerPed.Handle, _inPos.X, _inPos.Y, _inPos.Z, false, false, false,
                        false);
                }

                TriggerServerEvent("UpdateJailTime", TimeLeft);
                await Delay(1000);

                if (TimeLeft <= 0)
                {
                    break;
                }
            }

            UnjailFunc();
            return;
        }

        private void UnjailFunc()
        {
            API.SetEntityCoords(Game.PlayerPed.Handle, _outPos.X, _outPos.Y, _outPos.Z, false, false, false, false);
            InJail = false;
            Game.PlayerPed.IsInvincible = false;
            Game.PlayerPed.CanSwitchWeapons = true;
            Weapons.Instance.RefreshWeapons();
        }

    }
}
