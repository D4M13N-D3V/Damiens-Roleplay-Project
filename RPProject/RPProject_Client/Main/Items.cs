using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
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
            InventoryProcessing.Instance.AddItemUse("Binoculars", Binoculars);
            InventoryProcessing.Instance.AddItemUse("Bobby-Pins", BobbyPins);
            InventoryProcessing.Instance.AddItemUse("Ciggirates", Ciggirates); 
        }

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

        public async void SlimJim()
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
                Utility.Instance.SendChatMessage("[LOCKPICK]", "Your bobby pin didnt break and you unlock the doors, dropping the entire box of bobby pins!", 255, 0, 0);
            }
            else
            {
                Utility.Instance.SendChatMessage("[LOCKPICK]", "You break a slim jim!", 255, 0, 0);
            }
            TriggerServerEvent("dropItemByName", "Slim-Jim");

        }
    }
}
