using System;
using System.Dynamic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using CitizenFX.Core.Native;
namespace roleplay
{
    public class User : BaseScript
    {
        public User()
        {
            EventHandlers["playerSpawned"] += new Action<ExpandoObject>(onSpawn);
        }

        private void onSpawn(ExpandoObject pos)
        {
            Debug.WriteLine();
            TriggerServerEvent("roleplay:spawned");
        }
                
    }
}
    