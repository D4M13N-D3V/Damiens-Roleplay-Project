using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using roleplay.Main.Police;
using roleplay.Users.Inventory;

namespace roleplay.Main
{
    public class Items : BaseScript
    {
        public Items()
        {
            SetupItems();
        }

        private async void SetupItems()
        {
            while (InventoryProcessing.Instance == null)
            {
                await Delay(0);
            }

            #region Drinks

            InventoryProcessing.Instance.AddItemUse("Monster", Monster);
            InventoryProcessing.Instance.AddItemUse("Mtn-Dew-Kickstart", MtnDewKickstart);
            InventoryProcessing.Instance.AddItemUse("Mtn-Dew", MtnDew);
            InventoryProcessing.Instance.AddItemUse("Lemonade", Lemonade);
            InventoryProcessing.Instance.AddItemUse("Pink-Lemonade", PinkLemonade);
            InventoryProcessing.Instance.AddItemUse("Coke", Coke);
            InventoryProcessing.Instance.AddItemUse("Pepsi", Pepsi);
            InventoryProcessing.Instance.AddItemUse("Sprite", Sprite);
            InventoryProcessing.Instance.AddItemUse("Juice", Juice);
            InventoryProcessing.Instance.AddItemUse("Water", Water);

            #endregion

            #region Food

            InventoryProcessing.Instance.AddItemUse("SlimJim", SlimJim);
            InventoryProcessing.Instance.AddItemUse("BeefJerky", BeefJerky);
            InventoryProcessing.Instance.AddItemUse("PorkRinds", PorkRinds);
            InventoryProcessing.Instance.AddItemUse("Cheetos", Cheetos);
            InventoryProcessing.Instance.AddItemUse("Doritos", Doritos);
            InventoryProcessing.Instance.AddItemUse("Pistachios", Pistachios);
            InventoryProcessing.Instance.AddItemUse("Doughnut", Doughnut);
            InventoryProcessing.Instance.AddItemUse("GummyBears", GummyBears);
            InventoryProcessing.Instance.AddItemUse("IceCream", IceCream);
            InventoryProcessing.Instance.AddItemUse("SlimJim", SlimJim);
            InventoryProcessing.Instance.AddItemUse("Chocolate-Bar", ChocolateBar);

            #endregion

            #region Counter Items

            InventoryProcessing.Instance.AddItemUse("Bobby-Pins", BobbyPins);
            InventoryProcessing.Instance.AddItemUse("Lockpick", LockPick);
            InventoryProcessing.Instance.AddItemUse("Ciggirates", Ciggirates);
            InventoryProcessing.Instance.AddItemUse("Bandages", Bandage);
            InventoryProcessing.Instance.AddItemUse("Pain Killers", PainKillers);
            #endregion

            #region Binocualrs
            InventoryProcessing.Instance.AddItemUse("Binoculars", Binoculars);
            InventoryProcessing.Instance.AddItemUse("Binoculars(P)", Binoculars);
            InventoryProcessing.Instance.AddItemUse("Bandages(EMS)", Binoculars);
            #endregion

            InventoryProcessing.Instance.AddItemUse("Scuba Gear(EMS)", ScubaEMS);
            InventoryProcessing.Instance.AddItemUse("Bandages(EMS)", BandageEMS);
            InventoryProcessing.Instance.AddItemUse("Medical Supplies(EMS)", MedicalSuppliesEMS);
            InventoryProcessing.Instance.AddItemUse("Pain Killers(EMS)", PainKillersEMS);
            InventoryProcessing.Instance.AddItemUse("First Aid Kit(EMS)", FirstAidKitEMS);

            InventoryProcessing.Instance.AddItemUse("Scuba Gear(P)", ScubaP);
            InventoryProcessing.Instance.AddItemUse("Bandages(P)", BandageP);
            InventoryProcessing.Instance.AddItemUse("Pain Killers(P)", PainKillersP);
            InventoryProcessing.Instance.AddItemUse("Medical Supplies(P)", MedicalSuppliesP);
            InventoryProcessing.Instance.AddItemUse("Pain Killers(P)", PainKillersP);
            InventoryProcessing.Instance.AddItemUse("First Aid Kit(P)", FirstAidKitP);
        }

        #region Drinks

        private void Monster()
        {
            FoodManager.Instance.FeedPlayer(true, -10);
            FoodManager.Instance.FeedPlayer(false, 10);
            TriggerServerEvent("dropItemByName", "Monster");
        }

        private void MtnDewKickstart()
        {
            FoodManager.Instance.FeedPlayer(true, -10);
            FoodManager.Instance.FeedPlayer(false, 10);
            TriggerServerEvent("dropItemByName", "Mtn-Dew-Kickstart");
        }

        private void MtnDew()
        {
            FoodManager.Instance.FeedPlayer(true, -10);
            FoodManager.Instance.FeedPlayer(false, 10);
            TriggerServerEvent("dropItemByName", "Mtn-Dew");
        }

        private void Lemonade()
        {
            FoodManager.Instance.FeedPlayer(true, 5);
            FoodManager.Instance.FeedPlayer(false, 25);
            TriggerServerEvent("dropItemByName", "Lemonade");
        }

        private void PinkLemonade()
        {
            FoodManager.Instance.FeedPlayer(true, 5);
            FoodManager.Instance.FeedPlayer(false, 15);
            TriggerServerEvent("dropItemByName", "Pink-Lemonade");
        }

        private void Coke()
        {
            FoodManager.Instance.FeedPlayer(true, -10);
            FoodManager.Instance.FeedPlayer(false, 10);
            TriggerServerEvent("dropItemByName", "Coke");
        }

        private void Pepsi()
        {
            FoodManager.Instance.FeedPlayer(true, -10);
            FoodManager.Instance.FeedPlayer(false, 10);
            TriggerServerEvent("dropItemByName", "Pepsi");
        }

        private void Sprite()
        {
            FoodManager.Instance.FeedPlayer(true, -10);
            FoodManager.Instance.FeedPlayer(false, -10);
            TriggerServerEvent("dropItemByName", "Sprite");
        }

        private void Juice()
        {
            FoodManager.Instance.FeedPlayer(true, 10);
            FoodManager.Instance.FeedPlayer(false, 30);
            TriggerServerEvent("dropItemByName", "Juice");
        }

        private void Water()
        {
            FoodManager.Instance.FeedPlayer(true, 0);
            FoodManager.Instance.FeedPlayer(false, 50);
            TriggerServerEvent("dropItemByName", "Water");
        }

        #endregion

        #region Food


        private void SlimJim()
        {
            FoodManager.Instance.FeedPlayer(true, 10);
            FoodManager.Instance.FeedPlayer(false, -2);
            TriggerServerEvent("dropItemByName", "SlimJim");
        }


        private void BeefJerky()
        {
            FoodManager.Instance.FeedPlayer(true, 10);
            FoodManager.Instance.FeedPlayer(false, -2);
            TriggerServerEvent("dropItemByName", "BeefJerky");
        }


        private void PorkRinds()
        {
            FoodManager.Instance.FeedPlayer(true, 15);
            FoodManager.Instance.FeedPlayer(false, -8);
            TriggerServerEvent("dropItemByName", "PorkRinds");
        }


        private void Cheetos()
        {
            FoodManager.Instance.FeedPlayer(true, 20);
            FoodManager.Instance.FeedPlayer(false, -5);
            TriggerServerEvent("dropItemByName", "Cheetos");
        }


        private void Doritos()
        {
            FoodManager.Instance.FeedPlayer(true, 25);
            FoodManager.Instance.FeedPlayer(false, -10);
            TriggerServerEvent("dropItemByName", "Doritos");
        }


        private void Pistachios()
        {
            FoodManager.Instance.FeedPlayer(true, 25);
            FoodManager.Instance.FeedPlayer(false, -15);
            TriggerServerEvent("dropItemByName", "Pistachios");
        }


        private void Doughnut()
        {
            FoodManager.Instance.FeedPlayer(true, 18);
            FoodManager.Instance.FeedPlayer(false, -8);
            TriggerServerEvent("dropItemByName", "Doughnut");
        }


        private void GummyBears()
        {
            FoodManager.Instance.FeedPlayer(true, 30);
            FoodManager.Instance.FeedPlayer(false, -2);
            TriggerServerEvent("dropItemByName", "GummyBears");
        }


        private void IceCream()
        {
            FoodManager.Instance.FeedPlayer(true, 10);
            FoodManager.Instance.FeedPlayer(false, 10);
            TriggerServerEvent("dropItemByName", "IceCream");
        }


        private void ChocolateBar()
        {
            FoodManager.Instance.FeedPlayer(true, 10);
            FoodManager.Instance.FeedPlayer(false, -20);
            TriggerServerEvent("dropItemByName", "Chocolate-Bar");
        }

        #endregion

        #region Counter Items

        public void Binoculars()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            TriggerEvent("binoculars:Activate");
        }

        public void Ciggirates()
        {
            var hasCig = true;
            API.TaskStartScenarioInPlace(API.PlayerPedId(), "WORLD_HUMAN_SMOKING", 0, true);

            async void CancelCig()
            {
                while (hasCig)
                {
                    if (API.IsControlJustPressed(0, (int) Control.PhoneCancel))
                    {
                        hasCig = false;
                        API.ClearPedTasks(API.PlayerPedId());
                    }

                    await Delay(0);
                }
            }

            CancelCig();
        }

        public async void BobbyPins()
        {
            var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
            var vehicle = Utility.Instance.ClosestVehicle.Handle;
            var random = new Random();
            var rdmInt = random.Next(4);
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            Game.PlayerPed.Task.PlayAnimation("misscarstealfinalecar_5_ig_3", "crouchloop");
            var lockPicking = true;

            if (Utility.Instance.GetDistanceBetweenVector3s(playerPos, API.GetEntityCoords(vehicle, false)) > 5)
            {
                return;
            }

            async void CancelLockpick()
            {
                while (lockPicking)
                {
                    if (API.IsControlJustPressed(0, (int) Control.PhoneCancel))
                    {
                        lockPicking = false;
                        API.ClearPedTasks(API.PlayerPedId());
                    }

                    await Delay(0);
                }
            }

            CancelLockpick();
            await Delay(15000);
            lockPicking = false;
            API.ClearPedTasks(API.PlayerPedId());
            if (rdmInt == 3)
            {
                API.SetVehicleDoorsLocked(vehicle, 0);
                Utility.Instance.SendChatMessage("[Lockpick]",
                    "Your bobby pin didnt break and you unlock the doors, dropping the entire box of bobby pins!", 255,
                    0, 0);
                TriggerServerEvent("dropItemByName", "Bobby-Pins");
            }
            else
            {
                Utility.Instance.SendChatMessage("[Lockpick]", "You break a bobby pin!", 255, 0, 0);
            }

        }

        public async void LockPick()
        {
            var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
            var vehicle = Utility.Instance.ClosestVehicle.Handle;
            var random = new Random();
            var rdmInt = random.Next(3);
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            Game.PlayerPed.Task.PlayAnimation("misscarstealfinalecar_5_ig_3", "crouchloop");
            var lockPicking = true;
            if (Utility.Instance.GetDistanceBetweenVector3s(playerPos, API.GetEntityCoords(vehicle, false)) > 5)
            {
                return;
            }

            async void CancelLockpick()
            {
                while (lockPicking)
                {
                    if (API.IsControlJustPressed(0, (int) Control.PhoneCancel))
                    {
                        lockPicking = false;
                        API.ClearPedTasks(API.PlayerPedId());
                    }

                    await Delay(0);
                }
            }

            CancelLockpick();
            await Delay(15000);
            lockPicking = false;
            API.ClearPedTasks(API.PlayerPedId());
            if (rdmInt == 2)
            {
                API.SetVehicleDoorsLocked(vehicle, 0);
                Utility.Instance.SendChatMessage("[Lockpick]",
                    "Your lock pick didnt break and you unlock the doors, dropping the entire box of bobby pins!", 255,
                    0, 0);
            }
            else
            {
                Utility.Instance.SendChatMessage("[Lockpick]", "You break a lock pick!", 255, 0, 0);
            }

            TriggerServerEvent("dropItem", "Lockpick");

        }

        #endregion

        public void ScubaP()
        {
            if (Game.PlayerPed.Model == API.GetHashKey("mp_m_freemode_01"))
            {
                API.SetPedComponentVariation(Game.PlayerPed.Handle, 8, 123, 0, 0);
            }
            else
            {
                API.SetPedComponentVariation(Game.PlayerPed.Handle, 8, 153, 0, 0);
            }
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
        }
        public void ScubaEMS()
        {
            if (Game.PlayerPed.Model == API.GetHashKey("mp_m_freemode_01"))
            {
                API.SetPedComponentVariation(Game.PlayerPed.Handle, 8, 123, 0, 0);
            }
            else
            {
                API.SetPedComponentVariation(Game.PlayerPed.Handle, 8, 153, 0, 0);
            }
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
        }

        public void BandageP()
        {
            Game.PlayerPed.Health = Game.PlayerPed.Health + Game.PlayerPed.MaxHealth / 4;
            TriggerServerEvent("dropItem", "Bandages(P)", 1);
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
        }
        public void BandageEMS()
        {
            Game.PlayerPed.Health = Game.PlayerPed.Health + Game.PlayerPed.MaxHealth / 4;
            PedDamage.Instance.ResetInjuries();
            TriggerServerEvent("dropItem", "Bandages(EMS)",1);
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
        }
        public void Bandage()
        {
            Game.PlayerPed.Health = Game.PlayerPed.Health + Game.PlayerPed.MaxHealth / 4;
            PedDamage.Instance.ResetInjuries();
            TriggerServerEvent("dropItem", "Bandages", 1);
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
        }

        public void FirstAidKitP()
        {
            Game.PlayerPed.Health = Game.PlayerPed.Health + Game.PlayerPed.MaxHealth;
            PedDamage.Instance.ResetInjuries();
            TriggerServerEvent("dropItem", "First Aid Kit(EMS)", 1);
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
        }
        public void FirstAidKitEMS()
        {
            Game.PlayerPed.Health = Game.PlayerPed.Health + Game.PlayerPed.MaxHealth;
            PedDamage.Instance.ResetInjuries();
            TriggerServerEvent("dropItem", "First Aid Kit(EMS)", 1);
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
        }

        public void PainKillersP()
        {
            API.ResetPedMovementClipset(Game.PlayerPed.Handle, 1);
            EMS.Instance.NeedsPills = true;
            TriggerServerEvent("dropItem", "Pain Killers(P)", 1);
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
        }
        public void PainKillers()
        {
            API.ResetPedMovementClipset(Game.PlayerPed.Handle, 1);
            EMS.Instance.NeedsPills = true;
            TriggerServerEvent("dropItem", "Pain Killers", 1);
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
        }
        public void PainKillersEMS()
        {
            API.ResetPedMovementClipset(Game.PlayerPed.Handle, 1);
            EMS.Instance.NeedsPills = true;
            TriggerServerEvent("dropItem", "Pain Killers(EMS)", 1);
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
        }

        public async void MedicalSuppliesP()
        {
            API.TaskStartScenarioInPlace(Game.PlayerPed.Handle,"CODE_HUMAN_MEDIC_TEND_TO_DEAD",0,true);
            await Delay(5000);
            Game.PlayerPed.Task.ClearAll();
            Utility.Instance.GetClosestPlayer(out var output);
            if (output.Dist < 5)
            {
                InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                TriggerServerEvent("ReviveRequest", API.GetPlayerServerId(output.Pid));
            }
        }
        public async void MedicalSuppliesEMS()
        {
            API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "CODE_HUMAN_MEDIC_TEND_TO_DEAD", 0, true);
            await Delay(5000);
            Game.PlayerPed.Task.ClearAll();
            Utility.Instance.GetClosestPlayer(out var output);
            if (output.Dist < 5)
            {
                InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                TriggerServerEvent("ReviveRequest", API.GetPlayerServerId(output.Pid));
            }
        }
    }
}
