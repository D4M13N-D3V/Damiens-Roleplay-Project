using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay.Main
{
    public class AntiWeaponFarming : BaseScript
    {
        public AntiWeaponFarming()
        {
            DeleteWeapons();
        }

        public async void DeleteWeapons()
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
