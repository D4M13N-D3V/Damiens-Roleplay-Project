using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay
{
    public class HUD : BaseScript
    {
        public HUD Instance;
        public HUD()
        {
            Instance = this;
            Tick += new Func<Task>(async delegate
            {
                var pid = API.PlayerPedId();
                if (API.IsPedInAnyVehicle(pid, false))
                {
                    Utility.Instance.DrawRct(0.108f, 0.93f, 0.046f, 0.035f, 0, 0, 0, 120);
                    Utility.Instance.DrawRct(0.061f, 0.93f, 0.046f, 0.035f, 0, 0, 0, 120);
                    Utility.Instance.DrawRct(0.0147f, 0.93f, 0.045f, 0.035f, 0, 0, 0, 120);

                    var veh = API.GetVehiclePedIsIn(pid, false);
                    var kmh = API.GetEntitySpeed(veh) * 3.6f;
                    var mph = API.GetEntitySpeed(veh) * 2.236936f;
                    var plateVeh = API.GetVehicleNumberPlateText(veh);

                    Utility.Instance.DrawTxt(0.61f, 1.42f, 1.0f, 1.0f, 0.64f, "~w~"+Math.Ceiling(kmh), 255, 255, 255, 255);

                    Utility.Instance.DrawTxt(0.631f, 1.434f, 1.0f, 1.0f, 0.4f, "~w~ km/h", 255, 255, 255, 255);

                    Utility.Instance.DrawTxt(0.563f, 1.42f, 1.0f, 1.0f, 0.64f, "~w~"+Math.Ceiling(mph), 255, 255, 255, 255);

                    Utility.Instance.DrawTxt(0.585f, 1.434f, 1.0f, 1.0f, 0.4f, "~w~ mph", 255, 255, 255, 255);

                    Utility.Instance.DrawTxt(0.52f, 1.427f, 1.0f, 1.0f, 0.42f, "~w~"+ plateVeh, 255, 255, 255, 255);
                }
                else
                {

                }
            });
        }

    }
}
