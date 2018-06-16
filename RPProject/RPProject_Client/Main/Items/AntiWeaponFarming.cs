using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace client.Main.Items
{
    public class AntiWeaponFarming : BaseScript
    {
        public AntiWeaponFarming()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            DeleteWeapons();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }

        public async Task DeleteWeapons()
        {
            while (true)
            {
                API.RemoveAllPickupsOfType(0xDF711959);
                API.RemoveAllPickupsOfType(0xF9AFB48F);
                API.RemoveAllPickupsOfType(0xA9355DCD);
               await Delay(0);
            }
        }
    }
}
