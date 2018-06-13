using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client.Main.Items;
using CitizenFX.Core;

namespace client.Main.EmergencyServices.EMS
{
    public class EMSGear : BaseStore
    {
        public static EMSGear Instance;
        public EMSGear() : base("Hospital", "Pick up your EMS gear here.", 61, 24,
            new List<Vector3>()
            {
                new Vector3(1155.26f,-1520.82f,34.9f),
                new Vector3(295.411f,-1446.88f,30.5f),
                new Vector3(361.145f,-584.414f,29.2f),
                new Vector3(-449.639f,-340.586f,34.8f),
                new Vector3(-874.467f,-307.896f,39.8f),
                new Vector3(-677.135f,310.275f,83.5f),
                new Vector3(1839.39f,3672.78f,34.6f),
                new Vector3(-242.968f,6326.29f,32.8f)
            },
            new Dictionary<string, int>()
            {
                ["Bandages(EMS)"] = 0,
                ["Binoculars(EMS)"] = 0,
                ["Scuba Gear(EMS)"] = 0,
                ["Medical Supplies(EMS)"] = 0,
                ["Pain Killers(EMS)"] = 0,
                ["First Aid Kit(EMS)"] = 0
            })
        {
            Instance = this;
            MenuRestricted = true;
        }
    }
}
