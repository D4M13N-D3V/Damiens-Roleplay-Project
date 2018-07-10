using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client.Main.Items;
using CitizenFX.Core;

namespace client.Main.EmergencyServices.Police
{

    public class PoliceGear : BaseStore
    {
        public static PoliceGear Instance;
        public PoliceGear() : base("Police Station", "Pick up your police gear here.", 60, 29,
            new List<Vector3>()
            {
                new Vector3(452.09893798828f, -979.99584960938f, 30.689596176147f),
                new Vector3(457.956909179688f, -992.72314453125f, 30.9f),
                new Vector3(-449.90f, 6016.22f, 31.72f),
                new Vector3(1857.01f, 3689.50f, 34.27f),
                new Vector3(-1113.65f, -849.21f, 13.8f),
                new Vector3(-561.28f, -132.60f, 38.04f),
                new Vector3(118.89972686768f, -731.19207763672f, 242.1519317627f),
                new Vector3(-1491.2781982422f,4979.2631835938f,63.420253753662f)
            },
            new Dictionary<string, int>()
            {
                ["Bandages(P)"] = 0,
                ["Riot Shield(P)"] = 0,
                ["Body Armor(P)"] = 0,
                ["Flares(P)"] = 0,
                ["Flashlight"] = 25,
                ["Binoculars(P)"] = 0,
                ["Scuba Gear(P)"] = 0,
                ["Medical Supplies(P)"] = 0,
                ["Pain Killers(P)"] = 0,
                ["First Aid Kit(P)"] = 0,
                ["Handcuffs(P)"] = 0,
                ["Hobblecuffs(P)"] = 0,
                ["Tazer(P)"] = 0,
                ["Nighstick(P)"] = 0,
                ["Binoculars(P)"] = 0,
                ["Police Lock Tool(P)"] = 0,
                ["Combat Pistol"] = 2500,
                ["Pump Shotgun"] = 1200,
                ["Carbine Rifle(P)"] = 0,
                ["Fingerprint Scanner(P)"] = 0,
                ["GSR Kit(P)"] = 0,
                ["Spike Strips(P)"] = 0,
                ["Pistol Ammo"] = 10,
                ["Shotgun Ammo"] = 10,
                ["Rifle Ammo"] = 10,
                ["Fishing License"] = 0,
                ["Boar Hunting License"] = 0,
                ["Deer Hunting License"] = 0,
                ["Mountain Lion Hunting License"] = 0,
                ["Rabbit Hunting License"] = 0,
                ["Diving License"] = 0,
                ["Mining License"] = 0,
                ["Woodcutting License"] = 0,
                ["Boating License"] = 0,
                ["Trail Pass"] = 0,
            })
        {
            Instance = this;
            MenuRestricted = true;
        }
    }
}
