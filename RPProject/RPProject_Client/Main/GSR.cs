using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay.Main
{
    class GSR:BaseScript
    {
        public static GSR Instance;

        private readonly List<WeaponHash> BlackList = new List<WeaponHash>()
        {
            WeaponHash.PetrolCan,
            WeaponHash.Snowball,
            WeaponHash.Ball,
            WeaponHash.FlareGun,
            WeaponHash.PetrolCan,
            WeaponHash.Snowball,
            WeaponHash.Ball,
            WeaponHash.FlareGun,
            WeaponHash.Grenade,
            WeaponHash.SmokeGrenade,
            WeaponHash.Flare,
            WeaponHash.BZGas,
            WeaponHash.Firework,
            WeaponHash.Molotov,
            WeaponHash.ProximityMine,
            WeaponHash.StickyBomb,
            WeaponHash.StunGun,
            WeaponHash.PipeBomb,
            WeaponHash.Unarmed,
            WeaponHash.Bat,
        };

        public GSR()
        {
            Instance = this;
            API.DecorRegister("GSR_Active", 2);
            API.DecorSetBool(Game.PlayerPed.Handle, "GSR_Active", false);
            GSRCheck();
        }

        private async void GSRCheck()
        {
            while (true)
            {
                if (Game.PlayerPed.IsShooting)
                {
                    if (API.DecorGetBool(Game.PlayerPed.Handle, "GSR_Active"))
                    {
                        API.DecorSetBool(Game.PlayerPed.Handle, "GSR_Active", false);
                        await Delay(1000);
                        GSRLogic();
                    }
                }
                await Delay(1);
            }
        }

        private async void GSRLogic()
        {
            var i = 0.0f;
            while (i< 1800000 && API.DecorGetBool(Game.PlayerPed.Handle, "GSR_Active"))
            {
                i = i + 0.5f;
                await Delay(500);
            }
            API.DecorSetBool(Game.PlayerPed.Handle, "GSR_Active", false);
        }
    }
}
