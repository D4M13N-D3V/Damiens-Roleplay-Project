using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using NativeUI;
using roleplay.Main;

namespace roleplay.Users.Inventory
{
    public class InventoryProcessing : BaseScript
    {
        public static InventoryProcessing Instance;

        public InventoryProcessing()
        {
            Instance = this;
        }

        public void Process(UIMenuItem item, UIMenu menu)
        {
            #region Vehicle Item Handling
            if (menu.ParentItem.Description == "Vehicle Keys")
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
