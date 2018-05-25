using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using  CitizenFX.Core.Native;
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
            InventoryProcessing.Instance.AddItemUse("Binoculars", Binoculars);
            InventoryProcessing.Instance.AddItemUse("Bobby-Pins", BobbyPins);
            InventoryProcessing.Instance.AddItemUse("Lockpick", LockPick);
            InventoryProcessing.Instance.AddItemUse("Ciggirates", Ciggirates);
            #endregion
        }

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
            API.TaskStartScenarioInPlace(API.PlayerPedId(), "WORLD_HUMAN_SMOKING", 0,true);
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
            var vehicle = API.GetClosestVehicle(playerPos.X, playerPos.Y, playerPos.Z, 4, 0, 70);
            var random = new Random();
            var rdmInt = random.Next(4);
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            API.TaskPlayAnim(API.PlayerPedId(), "misscarstealfinalecar_5_ig_3", "crouchloop", 8.0f, -1, -1, 1, 1, false, false, false);
            var lockPicking = true;
            async void CancelLockpick()
            {
                while (lockPicking)
                {
                    if (API.IsControlJustPressed(0, (int)Control.PhoneCancel))
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
                Utility.Instance.SendChatMessage("[LOCKPICK]", "Your bobby pin didnt break and you unlock the doors, dropping the entire box of bobby pins!", 255, 0, 0);
                TriggerServerEvent("dropItemByName", "Bobby-Pins");
            }
            else
            {
                Utility.Instance.SendChatMessage("[LOCKPICK]", "You break a bobby pin!", 255, 0, 0);
            }

        }

        public async void LockPick()
        {
            var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
            var vehicle = API.GetClosestVehicle(playerPos.X, playerPos.Y, playerPos.Z, 4, 0, 70);
            var random = new Random();
            var rdmInt = random.Next(3);
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            API.TaskPlayAnim(API.PlayerPedId(), "misscarstealfinalecar_5_ig_3", "crouchloop", 8.0f, -1, -1, 1, 1, false, false, false);
            var lockPicking = true;
            async void CancelLockpick()
            {
                while (lockPicking)
                {
                    if (API.IsControlJustPressed(0, (int)Control.PhoneCancel))
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
                Utility.Instance.SendChatMessage("[LOCKPICK]", "Your lock pick didnt break and you unlock the doors, dropping the entire box of bobby pins!", 255, 0, 0);
            }
            else
            {
                Utility.Instance.SendChatMessage("[LOCKPICK]", "You break a lock pick!", 255, 0, 0);
            }
            TriggerServerEvent("dropItemByName", "Lockpick");

        }
        #endregion

    }
}
