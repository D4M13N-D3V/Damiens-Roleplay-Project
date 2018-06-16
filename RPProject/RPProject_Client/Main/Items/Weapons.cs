using CitizenFX.Core;
using CitizenFX.Core.Native;
using client.Main.Clothes;
using client.Main.Users.Inventory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace client.Main.Items
{
    public class Weapons : BaseScript
    {
        public static Weapons Instance;

        public Weapons()
        {
            Instance = this;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            AmmoCalculations();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }

        private readonly Dictionary<string, int> _melee = new Dictionary<string, int>()
        {
            ["Tazer(P)"] = API.GetHashKey("WEAPON_STUNGUN"),
            ["Nighstick(P)"] = API.GetHashKey("WEAPON_NIGHTSTICK"),
            ["Knife"] = API.GetHashKey("WEAPON_KNIFE"),
            ["Hammer"] = API.GetHashKey("WEAPON_HAMMER"),
            ["Bat"] = API.GetHashKey("WEAPON_BAT"),
            ["Fireaxe"] = API.GetHashKey("WEAPON_GOLFCLUB"),
            ["Crowbar"] = API.GetHashKey("WEAPON_CROWBAR"),
            ["Bottle"] = API.GetHashKey("WEAPON_BOTTLE"),
            ["Dagger"] = API.GetHashKey("WEAPON_DAGGER"),
            ["Hatchet"] = API.GetHashKey("WEAPON_HATCHET"),
            ["Machete"] = API.GetHashKey("WEAPON_MACHETE"),
            ["Pool Cue"] = API.GetHashKey("WEAPON_POOLCUE"),
            ["Wrench"] = API.GetHashKey("WEAPON_WRENCH"),
            ["Switchblade"] = API.GetHashKey("WEAPON_SWITCHBLADE"),
            ["Brass Knuckles"] = API.GetHashKey("WEAPON_KNUCKLE"),
            ["Flashlight"] = API.GetHashKey("WEAPON_FLASHLIGHT"),
            ["Flares(P)"] = API.GetHashKey("WEAPON_FLARE"),
        };


        private readonly Dictionary<string, int> _pistols = new Dictionary<string, int>()
        {
            ["SNS Pistol"] = API.GetHashKey("WEAPON_SNSPISTOL"),
            ["Pistol .50"] = API.GetHashKey("WEAPON_PISTOL50"),
            ["Pistol"] = API.GetHashKey("WEAPON_PISTOL"),
            ["Combat Pistol"] = API.GetHashKey("WEAPON_COMBATPISTOL"),
            ["Heavy Pistol"] = API.GetHashKey("WEAPON_HEAVYPISTOL"),
            ["Single Action Revolver"] = API.GetHashKey("WEAPON_REVOLVER"),
            ["Double Action Revolver"] = API.GetHashKey("WEAPON_DOUBLEACTION"),
        };

        private readonly Dictionary<string, int> _shotguns = new Dictionary<string, int>()
        {
            ["Pump Shotgun"] = API.GetHashKey("WEAPON_PUMPSHOTGUN"),
            ["Hunting Rifle"] = API.GetHashKey("WEAPON_MUSKET"),
        };
        private readonly Dictionary<string, int> _rifles = new Dictionary<string, int>()
        {
            ["Carbine Rifle(P)"] = API.GetHashKey("WEAPON_CARBINERIFLE")
        };

        private readonly Dictionary<string, List<string>> _ammos = new Dictionary<string, List<string>>()
        {
            ["Shotgun Ammo"] = new List<string>() { "Pump Shotgun", "Hunting Rifle" },
            ["Rifle Ammo"] = new List<string>() { "Carbine Rifle(P)" },
            ["Pistol Ammo"] = new List<string>()
            {
                "SNS Pistol", "Pistol .50", "Pistol", "Combat Pistol",
                "Heavy Pistol","Single Action Revolver","Double Action Revolver"
            }
        };

        private bool refreshingWeapons = false;

        private async Task AmmoCalculations()
        {
            while (true)
            {
                if (!refreshingWeapons)
                {
                    #region ammo item calulations
                    var curWeapon = Game.PlayerPed.Weapons.Current.Hash;
                    if (_shotguns.ContainsValue((int)curWeapon))
                    {
                        if (InventoryUI.Instance.HasItem("Shotgun Ammo") > Game.PlayerPed.Weapons.Current.Ammo)
                        {
                            TriggerServerEvent("dropItemByName", "Shotgun Ammo");
                        }
                    }
                    else if (_pistols.ContainsValue((int)curWeapon))
                    {
                        if (InventoryUI.Instance.HasItem("Pistol Ammo") > Game.PlayerPed.Weapons.Current.Ammo)
                        {
                            TriggerServerEvent("dropItemByName", "Pistol Ammo");
                        }
                    }
                    #endregion
                }
                await Delay(100);
            }
        }

        public async Task RefreshWeapons()
        {
            WeaponHash oldWeapon = Game.PlayerPed.Weapons.Current;
            refreshingWeapons = true;
            while (!ClothesManager.Instance.modelSet)
            {
                await Delay(1000);
            }
            foreach (var melee in _melee)
            {
                var count = InventoryUI.Instance.HasItem(melee.Key);
                if (count > 0)
                {
                    Game.PlayerPed.Weapons.Give((WeaponHash)melee.Value, 20, false, false);
                }
                else
                {
                    Game.PlayerPed.Weapons.Remove((WeaponHash)melee.Value);
                }
            }
            foreach (var ammo in _ammos.Keys)
            {
                var ammoCount = InventoryUI.Instance.HasItem(ammo);
                foreach (var weapon in _ammos[ammo])
                {
                    if (InventoryUI.Instance.HasItem(weapon) > 0)
                    {
                        if (_shotguns.ContainsKey(weapon))
                        {
                            Game.PlayerPed.Weapons.Give((WeaponHash)_shotguns[weapon], 0, true, true);
                            Game.PlayerPed.Weapons.Current.Ammo = ammoCount;
                        }
                        if (_pistols.ContainsKey(weapon))
                        {
                            Debug.WriteLine(Convert.ToString(ammoCount));
                            Game.PlayerPed.Weapons.Give((WeaponHash)_pistols[weapon], 0, true, true);
                            Game.PlayerPed.Weapons.Current.Ammo = ammoCount;
                        }
                        if (_rifles.ContainsKey(weapon))
                        {
                            Debug.WriteLine(Convert.ToString(ammoCount));
                            Game.PlayerPed.Weapons.Give((WeaponHash)_rifles[weapon], 0, true, true);
                            Game.PlayerPed.Weapons.Current.Ammo = ammoCount;
                        }
                    }
                    else
                    {
                        if (_shotguns.ContainsKey(weapon))
                        {
                            Game.PlayerPed.Weapons.Remove((WeaponHash)_shotguns[weapon]);
                        }
                        if (_pistols.ContainsKey(weapon))
                        {
                            Game.PlayerPed.Weapons.Remove((WeaponHash)_pistols[weapon]);
                        }
                        if (_rifles.ContainsKey(weapon))
                        {
                            Game.PlayerPed.Weapons.Remove((WeaponHash)_rifles[weapon]);
                        }
                    }
                }
            }
            if (Game.PlayerPed.Weapons.HasWeapon(oldWeapon))
            {
                Game.PlayerPed.Weapons.Select(oldWeapon);
            }
            else
            {
                Game.PlayerPed.Weapons.Select(WeaponHash.Unarmed);
            }
            refreshingWeapons = false;
        }
    }
}
