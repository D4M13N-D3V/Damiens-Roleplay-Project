using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace client.Main.Users
{
    public class User : BaseScript
    {
        public static User Instance;

        public string CharName;

        public User()
        {
            Instance = this;
        }

        public void StartUpdatingPosistion()
        {
            EventHandlers["SetClientCharacterName"] += new Action<dynamic>(SetName);
            Tick += new Func<Task>(async delegate
            {
                var pos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
                await Delay(5000);
                Utility.Instance.Log("Postion Has Been Sent To Server To Update!");
                TriggerServerEvent("updateCurrentPos", pos.X, pos.Y, pos.Z);
            });
        }

        private void SetName(dynamic o)
        {
            Debug.WriteLine("#@$@$#@#@$#@$#@$@#"+o);
            CharName = o;
        }
    }
}
