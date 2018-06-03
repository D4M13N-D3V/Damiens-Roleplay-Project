using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using roleplay.Main.Clothes;
using roleplay.Users.Inventory;

namespace roleplay.Main
{
    public class Weapons : BaseScript
    {
        public static Weapons Instance;

        public Weapons()
        {
            Instance = this;
            AmmoCalculations();
        }

        private readonly Dictionary<string,int> _melee = new Dictionary<string, int>()
        {
            ["Tazer(P)"] = API.GetHashKey("WEAPON_STUNGUN"),
            ["Nighstick(P)"] = API.GetHashKey("WEAPON_NIGHTSTICK")
        };

        private readonly Dictionary<string, int> _pistols = new Dictionary<string, int>()
        {
            ["SNS Pistol"] = API.GetHashKey("WEAPON_SNSPISTOL"),
            ["Pistol .50"] = API.GetHashKey("WEAPON_PISTOL50"),
            ["Pistol"] = API.GetHashKey("WEAPON_PISTOL"),
            ["Combat Pistol"] = API.GetHashKey("WEAPON_COMBATPISTOL"),
            ["Heavy Pistol"] = API.GetHashKey("WEAPON_HEAVYPISTOL"),
            ["Single Action Revolver"] = API.GetHashKey("WEAPON_REVOLVER"),
            ["Double Action Revolver"] = API.GetHashKey("WT_REV_DA"),
        };

        private readonly Dictionary<string, int> _shotguns = new Dictionary<string, int>()
        {
            ["Pump Shotgun"] = API.GetHashKey("WEAPON_PUMPSHOTGUN"),
            ["Hunting Rifle"] = API.GetHashKey("WEAPON_MUSKET"),
        };

        private readonly Dictionary<string, List<string>> _ammos = new Dictionary<string, List<string>>()
        {
            ["Shotgun Ammo"] = new List<string>() { "Pump Shotgun", "Hunting Rifle", "Pump Shotgun(P)" },
            ["Pistol Ammo"] = new List<string>()
            {
                "SNS Pistol", "Pistol .50", "Pistol", "Combat Pistol",
                "Heavy Pistol","Single Action Revolver","Double Action Revolver", "Combat Pistol(P)"
            },
            ["Rifle Ammo"] = new List<string>()
            {
                "Carbine Rifle(P)"
            },
        };


        private readonly Dictionary<string, int> _policePistols = new Dictionary<string, int>()
        {
            ["Combat Pistol(P)"] = API.GetHashKey("WEAPON_COMBATPISTOL")
        };

        private readonly Dictionary<string, int> _policeShotguns = new Dictionary<string, int>()
        {
            ["Pump Shotgun(P)"] = API.GetHashKey("WEAPON_PUMPSHOTGUN")
        };

        private readonly Dictionary<string, int> _policeRifles = new Dictionary<string, int>()
        {
            ["Carbine Rifle(P)"] = API.GetHashKey("WEAPON_CARBINERIFLE"),
        };


        private bool refreshingWeapons = false;

        private async void AmmoCalculations()
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
                            TriggerServerEvent("dropItem", "Shotgun Ammo", 1);
                        }
                    }
                    else if (_pistols.ContainsValue((int)curWeapon))
                    {
                        if (InventoryUI.Instance.HasItem("Pistol Ammo") > Game.PlayerPed.Weapons.Current.Ammo)
                        {
                            TriggerServerEvent("dropItem", "Pistol Ammo", 1);
                        }
                    }
                    else if (_policeShotguns.ContainsValue((int)curWeapon))
                    {
                        if (InventoryUI.Instance.HasItem("Shotgun Ammo") > Game.PlayerPed.Weapons.Current.Ammo)
                        {
                            TriggerServerEvent("dropItem", "Shotgun Ammo", 1);
                        }
                    }
                    else if (_policePistols.ContainsValue((int)curWeapon))
                    {
                        if (InventoryUI.Instance.HasItem("Pistol Ammo") > Game.PlayerPed.Weapons.Current.Ammo)
                        {
                            TriggerServerEvent("dropItem", "Pistol Ammo", 1);
                        }
                    }
                    else if (_policeRifles.ContainsValue((int)curWeapon))
                    {
                        if (InventoryUI.Instance.HasItem("Rifle Ammo") > Game.PlayerPed.Weapons.Current.Ammo)
                        {
                            TriggerServerEvent("dropItem", "Rifle Ammo", 1);
                        }
                    }
                    #endregion
                }
                await Delay(100);
            }
        }   

        public async void RefreshWeapons()
        {
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
                    Game.PlayerPed.Weapons.Give((WeaponHash)melee.Value, 1000, false, false);
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
                            if (!API.HasPedGotWeapon(Game.PlayerPed.Handle, (uint)_shotguns[weapon], false))
                            {
                                API.GiveWeaponToPed(Game.PlayerPed.Handle, (uint)_shotguns[weapon], 0, false, false);
                            }

                            if (API.GetPedAmmoByType(Game.PlayerPed.Handle, API.GetPedAmmoTypeFromWeapon(Game.PlayerPed.Handle, (uint)_shotguns[weapon])) != ammoCount)
                            {
                                API.SetPedAmmoByType(Game.PlayerPed.Handle, API.GetPedAmmoTypeFromWeapon(Game.PlayerPed.Handle, (uint)_shotguns[weapon]),ammoCount);
                            }
                        }
                        if (_pistols.ContainsKey(weapon))
                        {
                            if (!API.HasPedGotWeapon(Game.PlayerPed.Handle,(uint)_pistols[weapon],false))
                            {
                                API.GiveWeaponToPed(Game.PlayerPed.Handle, (uint)_pistols[weapon], 0, false, false);
                            }

                            if (API.GetPedAmmoByType(Game.PlayerPed.Handle, API.GetPedAmmoTypeFromWeapon(Game.PlayerPed.Handle, (uint)_pistols[weapon])) != ammoCount)
                            {
                                API.SetPedAmmoByType(Game.PlayerPed.Handle, API.GetPedAmmoTypeFromWeapon(Game.PlayerPed.Handle, (uint)_pistols[weapon]), ammoCount);
                            }
                        }
                        if (_policeRifles.ContainsKey(weapon))
                        {
                            if (!API.HasPedGotWeapon(Game.PlayerPed.Handle, (uint)_policeRifles[weapon], false))
                            {
                                API.GiveWeaponToPed(Game.PlayerPed.Handle, (uint)_policeRifles[weapon], 0, false, false);
                            }

                            if (API.GetPedAmmoByType(Game.PlayerPed.Handle, API.GetPedAmmoTypeFromWeapon(Game.PlayerPed.Handle, (uint)_policeRifles[weapon])) != ammoCount)
                            {
                                API.SetPedAmmoByType(Game.PlayerPed.Handle, API.GetPedAmmoTypeFromWeapon(Game.PlayerPed.Handle, (uint)_policeRifles[weapon]), ammoCount);
                            }
                        }
                        if (_policeShotguns.ContainsKey(weapon))
                        {
                            if (!API.HasPedGotWeapon(Game.PlayerPed.Handle, (uint)_policeShotguns[weapon], false))
                            {
                                API.GiveWeaponToPed(Game.PlayerPed.Handle, (uint)_policeShotguns[weapon], 0, false, false);
                            }

                            if (API.GetPedAmmoByType(Game.PlayerPed.Handle, API.GetPedAmmoTypeFromWeapon(Game.PlayerPed.Handle, (uint)_policeShotguns[weapon])) != ammoCount)
                            {
                                API.SetPedAmmoByType(Game.PlayerPed.Handle, API.GetPedAmmoTypeFromWeapon(Game.PlayerPed.Handle, (uint)_policeShotguns[weapon]), ammoCount);
                            }
                        }
                        if (_policePistols.ContainsKey(weapon))
                        {
                            if (!API.HasPedGotWeapon(Game.PlayerPed.Handle, (uint)_policePistols[weapon], false))
                            {
                                API.GiveWeaponToPed(Game.PlayerPed.Handle, (uint)_policePistols[weapon], 0, false, false);
                            }

                            if (API.GetPedAmmoByType(Game.PlayerPed.Handle, API.GetPedAmmoTypeFromWeapon(Game.PlayerPed.Handle, (uint)_policePistols[weapon])) != ammoCount)
                            {
                                API.SetPedAmmoByType(Game.PlayerPed.Handle, API.GetPedAmmoTypeFromWeapon(Game.PlayerPed.Handle, (uint)_policePistols[weapon]), ammoCount);
                            }
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
                        if (_policeRifles.ContainsKey(weapon))
                        {
                            Game.PlayerPed.Weapons.Remove((WeaponHash)_policeRifles[weapon]);
                        }
                        if (_policeShotguns.ContainsKey(weapon))
                        {
                            Game.PlayerPed.Weapons.Remove((WeaponHash)_policeShotguns[weapon]);
                        }
                        if (_policePistols.ContainsKey(weapon))
                        {
                            Game.PlayerPed.Weapons.Remove((WeaponHash)_policePistols[weapon]);
                        }
                    }
                }
            }

            refreshingWeapons = false;
        }
    }
}
