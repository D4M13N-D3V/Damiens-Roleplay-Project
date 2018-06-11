using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace client.Main.EmergencyServices
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

            API.DecorRegister("Damage.Vehicle", 0);
            API.DecorRegister("Damage.Petrol", 0);
            API.DecorRegister("Damage.Projectile", 0);
            API.DecorRegister("Damage.Melee.Sharp", 0);
            API.DecorRegister("Damage.Melee.Blunt", 0);
            API.DecorRegister("Damage.Animal", 0);

            API.DecorSetInt(Game.PlayerPed.Handle, "Damage.Vehicle", 3);
            API.DecorSetInt(Game.PlayerPed.Handle, "Damage.Petrol", 3);
            API.DecorSetInt(Game.PlayerPed.Handle, "Damage.Projectile", 3);
            API.DecorSetInt(Game.PlayerPed.Handle, "Damage.Melee.Sharp", 3);
            API.DecorSetInt(Game.PlayerPed.Handle, "Damage.Melee.Blunt", 3);
            API.DecorSetInt(Game.PlayerPed.Handle, "Damage.Animal", 3);
            Instance = this;
            //DamageCheck();
            EventHandlers["InjuryCheckCommand"] += new Action(InjuryCheckCommand);
        }

        private async Task DamageCheck()
        {
            while (Utility.Instance == null)
            {
                await Delay(100);
            }

            while (true)
            {
                    var ped = Game.PlayerPed;
                    if (API.HasEntityBeenDamagedByAnyVehicle(ped.Handle))
                    {
                        AddInjury(ped,"Damage.Vehicle");
                    }

                    if (ped.IsDead)
                    {
                        Entity killer = Entity.FromHandle(API.GetPedSourceOfDeath(ped.Handle));
                        if (killer.Exists())
                        {
                            if ((PedHash) killer.Model.Hash == PedHash.MountainLion)
                            {
                                AddInjury(ped, "Damage.Animal");
                            }
                        }
                    }
                    Enum.GetValues(typeof(WeaponHash)).OfType<WeaponHash>().ToList().ForEach(w =>
                    {
                        if (API.HasPedBeenDamagedByWeapon(ped.Handle, (uint)w, 0))
                        {
                            if (w == WeaponHash.StunGun)
                            {
                                Game.PlayerPed.Ragdoll(15000);
                            }
                            //Log.ToChat($"{w}");
                            switch (API.GetWeaponDamageType((uint)w))
                            {
                                case 2: // Melee
                                    if (_sharpMeleeWeapons.Contains(w))
                                    {
                                        AddInjury(ped, "Damage.Melee.Sharp");
                                    }
                                    else
                                    {
                                        AddInjury(ped, "Damage.Melee.Blunt");
                                    }
                                    break;
                                case 3: // Projectile Weapon
                                    AddInjury(ped, "Damage.Projectile");
                                    break;
                                case 13: // Gasoline?
                                    AddInjury(ped, "Damage.Gas");
                                    break;
                                default:
                                    break;
                            }
                        }
                    });
                    ped.Bones.ClearLastDamaged();
                    API.ClearEntityLastDamageEntity(ped.Handle);
                    ped.ClearLastWeaponDamage();
                await Delay(100);
            }
            
        }

        private void AddInjury(Ped p, string v)
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

        public void ResetInjuries()
        {
            var ped = Game.PlayerPed;
            API.DecorSetInt(ped.Handle, "Damage.Vehicle",0);
            API.DecorSetInt(ped.Handle, "Damage.Petrol",0);
            API.DecorSetInt(ped.Handle, "Damage.Projectile",0);
            API.DecorSetInt(ped.Handle, "Damage.Melee.Sharp",0);
            API.DecorSetInt(ped.Handle, "Damage.Melee.Blunt",0);
            API.DecorSetInt(ped.Handle, "Damage.Animal",0);
        }

        public string GetPedInjuries(int pedHandle)
        {
            var ped = Ped.FromHandle(pedHandle);
            string str = "";
            if (ped.Exists())
            {
                var vehicleInt = API.DecorGetInt(ped.Handle, "Damage.Vehicle");
                var petrolInt = API.DecorGetInt(ped.Handle, "Damage.Petrol");
                var projectileInt = API.DecorGetInt(ped.Handle, "Damage.Projectile");
                var meleeSharpInt = API.DecorGetInt(ped.Handle, "Damage.Melee.Sharp");
                var meleeBluntInt = API.DecorGetInt(ped.Handle, "Damage.Melee.Blunt");
                var animalInt = API.DecorGetInt(ped.Handle, "Damage.Animal");
                str = str + "Vehicle Impact Wound[" + vehicleInt + "]\nBurn Wound[" + petrolInt + "]\nGunshot Wound[" + projectileInt + "]\nBlade Wound[" + meleeSharpInt + "]\nBlunt Force Wound[" + meleeBluntInt + "]\nAnimal Bites[" + animalInt + "]\n";
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
