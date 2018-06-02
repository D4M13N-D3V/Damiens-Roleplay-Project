using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using NativeUI;
using roleplay.Main.Vehicles;

namespace roleplay.Users.Inventory
{
    public class InventoryProcessing : BaseScript
    {
        public static InventoryProcessing Instance;

        public Dictionary<string,Action> ItemUses = new Dictionary<string, Action>();

        public InventoryProcessing()
        {
            Instance = this;
        }

        public void AddItemUse(string itemname, Action cb)
        {
            ItemUses[itemname] = cb;
        }

        public void Process(UIMenuItem item, UIMenu menu)
        {
            var itemname = menu.ParentItem.Text.Split('.')[0];
            if (ItemUses.Keys.Contains(itemname))
            {
                ItemUses[itemname]();
            }
            #region Vehicle Item Handling
            else if (menu.ParentItem.Description == "Vehicle Keys")
            {
                var nameSplit = menu.ParentItem.Text.Split('-');
                nameSplit[1] = nameSplit[1].Remove(8, 4);
                if (VehicleManager.Instance.isNearGarage)
                {
                    TriggerServerEvent("PullCarRequest", nameSplit[1]);
                }
                else
                {
                    Utility.Instance.SendChatMessage("[VEHICLE MANAGER]","You are not near a garage!",0,150,40);
                }
            }
            #endregion
        }

    }
}
