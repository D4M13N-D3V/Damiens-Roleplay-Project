using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using client.Users.Inventory;
using client.Main.EmergencyServices;

namespace client.Main.Items
{
    public class Items : BaseScript
    {
        public Items()
        {
            SetupItems();
        }

        private async Task SetupItems()
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

            InventoryProcessing.Instance.AddItemUse("Scuba Gear", Scuba);

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
            InventoryProcessing.Instance.AddItemUse("Body Armor(P)", PoliceBodyArmor);
        }

        #region Drinks

        private void Monster()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, -10);
            FoodManager.Instance.FeedPlayer(false, 10);
            TriggerServerEvent("dropItem", "Monster", 1);
        }

        private void MtnDewKickstart()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, -10);
            FoodManager.Instance.FeedPlayer(false, 10);
            TriggerServerEvent("dropItem", "Mtn-Dew-Kickstart", 1);
        }

        private void MtnDew()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, -10);
            FoodManager.Instance.FeedPlayer(false, 10);
            TriggerServerEvent("dropItem", "Mtn-Dew", 1);
        }

        private void Lemonade()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, 5);
            FoodManager.Instance.FeedPlayer(false, 25);
            TriggerServerEvent("dropItem", "Lemonade", 1);
        }

        private void PinkLemonade()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, 5);
            FoodManager.Instance.FeedPlayer(false, 15);
            TriggerServerEvent("dropItem", "Pink-Lemonade", 1);
        }

        private void Coke()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, -10);
            FoodManager.Instance.FeedPlayer(false, 10);
            TriggerServerEvent("dropItem", "Coke", 1);
        }

        private void Pepsi()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, -10);
            FoodManager.Instance.FeedPlayer(false, 10);
            TriggerServerEvent("dropItem", "Pepsi", 1);
        }

        private void Sprite()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, -10);
            FoodManager.Instance.FeedPlayer(false, -10);
            TriggerServerEvent("dropItem", "Sprite", 1);
        }

        private void Juice()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, 10);
            FoodManager.Instance.FeedPlayer(false, 30);
            TriggerServerEvent("dropItem", "Juice", 1);
        }

        private void Water()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, 0);
            FoodManager.Instance.FeedPlayer(false, 50);
            TriggerServerEvent("dropItem", "Water", 1);
        }

        #endregion

        #region Food


        private void SlimJim()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, 10);
            FoodManager.Instance.FeedPlayer(false, -2);
            TriggerServerEvent("dropItem", "SlimJim", 1);
        }


        private void BeefJerky()
        {
            FoodManager.Instance.FeedPlayer(true, 10);
            FoodManager.Instance.FeedPlayer(false, -2);
            TriggerServerEvent("dropItem", "BeefJerky", 1);
        }


        private void PorkRinds()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, 15);
            FoodManager.Instance.FeedPlayer(false, -8);
            TriggerServerEvent("dropItem", "PorkRinds", 1);
        }


        private void Cheetos()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, 20);
            FoodManager.Instance.FeedPlayer(false, -5);
            TriggerServerEvent("dropItem", "Cheetos", 1);
        }


        private void Doritos()
        {
            FoodManager.Instance.FeedPlayer(true, 25);
            FoodManager.Instance.FeedPlayer(false, -10);
            TriggerServerEvent("dropItem", "Doritos", 1);
        }


        private void Pistachios()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, 25);
            FoodManager.Instance.FeedPlayer(false, -15);
            TriggerServerEvent("dropItem", "Pistachios", 1);
        }


        private void Doughnut()
        {
            FoodManager.Instance.FeedPlayer(true, 18);
            FoodManager.Instance.FeedPlayer(false, -8);
            TriggerServerEvent("dropItem", "Doughnut", 1);
        }


        private void GummyBears()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, 30);
            FoodManager.Instance.FeedPlayer(false, -2);
            TriggerServerEvent("dropItem", "GummyBears", 1);
        }


        private void IceCream()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, 10);
            FoodManager.Instance.FeedPlayer(false, 10);
            TriggerServerEvent("dropItem", "IceCream", 1);
        }


        private void ChocolateBar()
        {
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            FoodManager.Instance.FeedPlayer(true, 10);
            FoodManager.Instance.FeedPlayer(false, -20);
            TriggerServerEvent("dropItem", "Chocolate-Bar",1);
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
            API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "WORLD_HUMAN_SMOKING", 0, true);

            async Task CancelCig()
            {
                while (hasCig)
                {
                    if (API.IsControlJustPressed(0, (int) Control.PhoneCancel))
                    {
                        hasCig = false;
                        API.ClearPedTasks(Game.PlayerPed.Handle);
                    }

                    await Delay(0);
                }
            }

            CancelCig();
        }

        public async void BobbyPins()
        {
            var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
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

            async Task CancelLockpick()
            {
                while (lockPicking)
                {
                    if (API.IsControlJustPressed(0, (int) Control.PhoneCancel))
                    {
                        lockPicking = false;
                        API.ClearPedTasks(Game.PlayerPed.Handle);
                    }

                    await Delay(0);
                }
            }

            CancelLockpick();
            await Delay(15000);
            lockPicking = false;
            API.ClearPedTasks(Game.PlayerPed.Handle);
            if (rdmInt == 3)
            {
                API.SetVehicleDoorsLocked(vehicle, 0);
                Utility.Instance.SendChatMessage("[Lockpick]",
                    "Your bobby pin didnt break and you unlock the doors, dropping the entire box of bobby pins!", 255,
                    0, 0);
                TriggerServerEvent("dropItem", "Bobby-Pins",1);
            }
            else
            {
                Utility.Instance.SendChatMessage("[Lockpick]", "You break a bobby pin!", 255, 0, 0);
            }

        }

        public async void LockPick()
        {
            var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
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

            async Task CancelLockpick()
            {
                while (lockPicking)
                {
                    if (API.IsControlJustPressed(0, (int) Control.PhoneCancel))
                    {
                        lockPicking = false;
                        API.ClearPedTasks(Game.PlayerPed.Handle);
                    }

                    await Delay(0);
                }
            }

            CancelLockpick();
            await Delay(15000);
            lockPicking = false;
            API.ClearPedTasks(Game.PlayerPed.Handle);
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

            TriggerServerEvent("dropItem", "Lockpick", 1);

        }

        #endregion

        public void PoliceBodyArmor()
        {
            Game.PlayerPed.Armor = 100;
        }

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
        public void Scuba()
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
            EMS.Instance.NeedsPills = false;
            TriggerServerEvent("dropItem", "Pain Killers(P)", 1);
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
        }
        public void PainKillers()
        {
            API.ResetPedMovementClipset(Game.PlayerPed.Handle, 1);
            EMS.Instance.NeedsPills = false;
            TriggerServerEvent("dropItem", "Pain Killers", 1);
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
        }
        public void PainKillersEMS()
        {
            API.ResetPedMovementClipset(Game.PlayerPed.Handle, 1);
            EMS.Instance.NeedsPills = false;
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
