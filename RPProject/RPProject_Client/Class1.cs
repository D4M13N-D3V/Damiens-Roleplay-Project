using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using CitizenFX.Core.Native;
namespace roleplay
{
    public class Class1 : BaseScript
    {
        public Class1()
        {
            Tick += OnTick;
        }

        public async Task OnTick()
        {
            // Show notification when Z was pressed
            if (Game.IsControlPressed(0, Control.MultiplayerInfo))
            {
               
            }
        }
    }
}
