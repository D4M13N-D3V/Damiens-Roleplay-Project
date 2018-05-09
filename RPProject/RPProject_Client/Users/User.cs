using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay.Users
{
    public class User : BaseScript
    {
        public static User Instance;

        public User()
        {
            Instance = this;
        }

        public void StartUpdatingPosistion()
        {
            Tick += new Func<Task>(async delegate
            {
                var pos = API.GetEntityCoords(API.PlayerPedId(), true);
                await Delay(5000);
                Utility.Instance.Log("Postion Has Been Sent To Server To Update!");
                TriggerServerEvent("updateCurrentPos", pos.X, pos.Y, pos.Z);
            });
        }

    }
}
