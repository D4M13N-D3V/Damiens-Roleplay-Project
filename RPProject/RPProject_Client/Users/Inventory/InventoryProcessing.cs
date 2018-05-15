using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using NativeUI;

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
            if (menu.ParentItem.Description == "Vehicle Keys")
            {
                var nameSplit = menu.ParentItem.Text.Split('-');
                nameSplit[1] = nameSplit[1].Remove(8, 4);
                TriggerServerEvent("PullCarRequest",nameSplit[1]);
            }
        }

    }
}
