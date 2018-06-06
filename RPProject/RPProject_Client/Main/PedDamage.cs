using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay.Main
{
    class PedDamage : BaseScript
    {
        public static PedDamage Instance;

        private List<WeaponHash> _sharpMeleeWeapons = new List<WeaponHash>()
        {
            WeaponHash.BattleAxe,
            WeaponHash.Dagger,
            WeaponHash.Hatchet,
            WeaponHash.SwitchBlade,
            WeaponHash.Knife
        };

        public PedDamage()
        {
            API.DecorRegister("Damage.Vehicle", 3);
            API.DecorRegister("Damage.Petrol", 3);
            API.DecorRegister("Damage.Projectile", 3);
            API.DecorRegister("Damage.Melee.Sharp", 3);
            API.DecorRegister("Damage.Melee.Blunt", 3);
            API.DecorRegister("Damage.Animal", 3);
            Instance = this;
            DamageCheck();
            EventHandlers["InjuryCheckCommand"] += new Action(InjuryCheckCommand);
        }

        private async void DamageCheck()
        {
            while (Utility.Instance == null)
            {
                await Delay(100);
            }

            while (true)
            {
                var plyList = new PlayerList();
                foreach (var ply in plyList)
                {
                    var ped = ply.Character;
                    if (API.HasEntityBeenDamagedByAnyVehicle(ped.Handle))
                    {
                        IncrementPedFlag(ped,"Damage.Vehicle");
                    }

                    if (ped.IsDead)
                    {
                        Entity killer = Entity.FromHandle(API.GetPedSourceOfDeath(ped.Handle));
                        if (killer.Exists())
                        {
                            if ((PedHash) killer.Model.Hash == PedHash.MountainLion)
                            {
                                IncrementPedFlag(ped, "Damage.Animal");
                            }
                        }
                    }
                    Enum.GetValues(typeof(WeaponHash)).OfType<WeaponHash>().ToList().ForEach(w =>
                    {
                        if (API.HasPedBeenDamagedByWeapon(ped.Handle, (uint)w, 0))
                        {
                            //Log.ToChat($"{w}");
                            switch (API.GetWeaponDamageType((uint)w))
                            {
                                case 2: // Melee
                                    if (_sharpMeleeWeapons.Contains(w))
                                    {
                                        IncrementPedFlag(ped, "Damage.Melee.Sharp");
                                    }
                                    else
                                    {
                                        IncrementPedFlag(ped, "Damage.Melee.Blunt");
                                    }
                                    break;
                                case 3: // Projectile Weapon
                                    IncrementPedFlag(ped, "Damage.Projectile");
                                    break;
                                case 13: // Gasoline?
                                    IncrementPedFlag(ped, "Damage.Gas");
                                    break;
                                default:
                                    break;
                            }
                        }
                    });
                    ped.Bones.ClearLastDamaged();
                    API.ClearEntityLastDamageEntity(ped.Handle);
                    ped.ClearLastWeaponDamage();

                }
                await Delay(1000);
            }
        }

        private void IncrementPedFlag(Ped p, string v)
        {
            try
            {
                int value = 0;
                //Log.ToChat($"Incrementing flag '{v}'");
                
                if (API.DecorExistOn(p.Handle, v))
                {
                    //Log.ToChat($"Decor exists!");
                    value = API.DecorGetInt(p.Handle, v);
                }

                value++;
                //Log.ToChat($"New value {value}");
                API.DecorSetInt(p.Handle, v, value);
                Debug.WriteLine(Convert.ToString(value));
            }
            catch
            {

            }
        }

        public string GetPedInjuries(int pedHandle)
        {
            var ped = Ped.FromHandle(pedHandle);
            string str = "";
            if (ped.Exists())
            {
                var petrolInt = API.DecorGetInt(ped.Handle, "Damage.Petrol");
                var projectileInt = API.DecorGetInt(ped.Handle, "Damage.Projectile");
                var meleeSharpInt = API.DecorGetInt(ped.Handle, "Damage.Melee.Sharp");
                var meleeBluntInt = API.DecorGetInt(ped.Handle, "Damage.Melee.Blunt");
                var animalInt = API.DecorGetInt(ped.Handle, "Damage.Animal");
                if (petrolInt > 0)
                {
                    str = str + "Burn Wound[" + petrolInt + "]\n";
                }
                if (projectileInt > 0)
                {
                    str = str + "Gunshot Wound[" + projectileInt + "]\n";
                }
                if (meleeSharpInt > 0)
                {
                    str = str + "Blade Wound[" + meleeSharpInt + "]\n";
                }
                if (meleeBluntInt > 0)
                {
                    str = str + "Animal Bites[" + animalInt + "]\n";
                }
            }   
            return str;
        }

        public void InjuryCheckCommand()
        {
            var ped = Utility.Instance.ClosestPed;
            if (ped.Exists())
            {
                TriggerServerEvent("ActionCommandFromClient", "Checks for wounds and finds :\n"+GetPedInjuries(ped.Handle));
            }
        }

    }
}
