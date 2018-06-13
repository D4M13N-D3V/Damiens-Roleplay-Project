using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using server.Main.Activities.TreasureHunting;
using server.Main.Items;
using server.Main.Users;

namespace client.Main.Activities.TreasureHunting
{
    public class TreasureHunting : BaseScript
    {
        public static TreasureHunting Instance;

        private Random _random = new Random();

        private List<TreasureLocation> _locations = new List<TreasureLocation>()
        {
            new TreasureLocation(new Vector3(-3353.6511230469f,509.04403686523f,-24.632040023804f), new List<string>(){"Jewlery","Antique Gun","Dinosaur Bone","Sea Monster Bone","Scrap Metal"}),
            new TreasureLocation(new Vector3(-3370.3835449219f,527.53869628906f,-26.632040023804f), new List<string>(){"Jewlery","Antique Gun","Dinosaur Bone","Sea Monster Bone","Scrap Metal"}),
            new TreasureLocation(new Vector3(-3385.6926269531f,517.6806640625f,-26.032039642334f), new List<string>(){"Jewlery","Antique Gun","Dinosaur Bone","Sea Monster Bone","Scrap Metal"}),
            new TreasureLocation(new Vector3(-3421.6557617188f,513.7734375f,-29.232040405273f), new List<string>(){"Jewlery","Antique Gun","Dinosaur Bone","Sea Monster Bone","Scrap Metal"}),
            new TreasureLocation(new Vector3(-3411.0744628906f,494.05322265625f,-31.732040405273f), new List<string>(){"Jewlery","Antique Gun","Dinosaur Bone","Sea Monster Bone","Scrap Metal"}),
            new TreasureLocation(new Vector3(-3381.3666992188f,462.15539550781f,-28.132040023804f), new List<string>(){"Jewlery","Antique Gun","Dinosaur Bone","Sea Monster Bone","Scrap Metal"}),
            new TreasureLocation(new Vector3(-3359.3078613281f,493.94512939453f,-25.132040023804f), new List<string>(){"Jewlery","Antique Gun","Dinosaur Bone","Sea Monster Bone","Scrap Metal"}),
            new TreasureLocation(new Vector3(-3162.4340820313f,2997.1599121094f,-37.681610107422f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3174.2780761719f,2999.13671875f,-38.647926330566f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3177.1328125f,3010.5100097656f,-38.992805480957f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3172.2866210938f,3030.1535644531f,-34.99157333374f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3175.0146484375f,3040.1687011719f,-36.175594329834f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3181.4809570313f,3034.4848632813f,-36.761028289795f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3173.9438476563f,3025.9587402344f,-35.926536560059f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3181.2646484375f,3053.3110351563f,-39.06164932251f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3199.0578613281f,3054.3278808594f,-41.308925628662f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3186.8557128906f,3033.4460449219f,-32.029102325439f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(3097.5026855469f,-251.03153991699f,-5.5886726379395f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3142.525390625f,-260.9123840332f,-20.188673019409f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3149.7543945313f,-265.08602905273f,-13.888672828674f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3145.8234863281f,-270.76745605469f,-11.188673019409f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3146.0908203125f,-273.75552368164f,-8.8886728286743f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3147.2492675781f,-283.47860717773f,-7.8886728286743f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3163.5620117188f,-297.10464477539f,-6.188672542572f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3167.5717773438f,-309.25122070313f,-12.91489315033f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3168.3293457031f,-321.69174194336f,-12.915464401245f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3167.220703125f,-320.30953979492f,-13.140027999878f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3166.7841796875f,-306.86361694336f,-12.858468055725f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3167.3811035156f,-330.27307128906f,-24.881834030151f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3180.5048828125f,-336.29653930664f,-27.882518768311f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3185.2727050781f,-357.76507568359f,-29.902994155884f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3192.4794921875f,-372.09683227539f,-29.766319274902f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3186.0991210938f,-382.15493774414f,-24.865468978882f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3193.4614257813f,-381.41271972656f,-17.47966003418f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3199.787109375f,-390.66363525391f,-22.823389053345f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3202.025390625f,-404.08636474609f,-25.16423034668f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3212.4555664063f,-410.81307983398f,-32.668796539307f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(774.88745117188f,7405.6259765625f,-118.8558807373f), new List<string>(){"Plane Part", "Pearl", "Jewlery", "Hard Drive", "Firearm"}),
            new TreasureLocation(new Vector3(746.65539550781f,7409.609375f,-121.43287658691f), new List<string>(){"Plane Part", "Pearl", "Jewlery", "Hard Drive", "Firearm"}),
            new TreasureLocation(new Vector3(755.68218994141f,7385.625f,-114.78730010986f), new List<string>(){"Plane Part", "Pearl", "Jewlery", "Hard Drive", "Firearm"}),
            new TreasureLocation(new Vector3(801.1748046875f,7361.455078125f,-130.07354736328f), new List<string>(){"Plane Part", "Pearl", "Jewlery", "Hard Drive", "Firearm"}),
            new TreasureLocation(new Vector3(738.86572265625f,7444.8208007813f,-169.24443054199f), new List<string>(){"Plane Part", "Pearl", "Jewlery", "Hard Drive", "Firearm"}),
            new TreasureLocation(new Vector3(-931.36468505859f,6599.671875f,-29.363540649414f), new List<string>(){"Firearm", "Pearl", "Alien Bone","Alien Gun","Alien Ship Part"}),
            new TreasureLocation(new Vector3(-920.67474365234f,6623.7978515625f,-30.169900894165f), new List<string>(){"Firearm", "Pearl", "Alien Bone","Alien Gun","Alien Ship Part"}),
            new TreasureLocation(new Vector3(-925.27722167969f,6644.9301757813f,-29.637880325317f), new List<string>(){"Firearm", "Pearl", "Alien Bone","Alien Gun","Alien Ship Part"}),
            new TreasureLocation(new Vector3(-913.81701660156f,6664.2319335938f,-30.845436096191f), new List<string>(){"Firearm", "Pearl", "Alien Bone","Alien Gun","Alien Ship Part"}),
            new TreasureLocation(new Vector3(-901.16687011719f,6649.1459960938f,-30.046863555908f), new List<string>(){"Firearm", "Pearl", "Alien Bone","Alien Gun","Alien Ship Part"}),
            new TreasureLocation(new Vector3(-854.17895507813f,6649.0415039063f,-21.403295516968f), new List<string>(){"Firearm", "Pearl", "Alien Bone","Alien Gun","Alien Ship Part"}),
            new TreasureLocation(new Vector3(-845.91870117188f,6666.9243164063f,-26.820055007935f), new List<string>(){"Firearm", "Pearl", "Alien Bone","Alien Gun","Alien Ship Part"}),
            new TreasureLocation(new Vector3(-811.60137939453f,6665.45703125f,-13.750008583069f), new List<string>(){"Firearm", "Pearl", "Alien Bone","Alien Gun","Alien Ship Part"}),
            new TreasureLocation(new Vector3(4185.3559570313f,3576.5087890625f,-45.745246887207f), new List<string>(){ "Antique Gun", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearl"}),
            new TreasureLocation(new Vector3(4198.3979492188f,3601.5886230469f,-44.570709228516f), new List<string>(){ "Antique Gun", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearl"}),
            new TreasureLocation(new Vector3(4219.9985351563f,3606.5180664063f,-46.959362030029f), new List<string>(){ "Antique Gun", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearl"}),
            new TreasureLocation(new Vector3(4237.8662109375f,3594.8830566406f,-46.260566711426f), new List<string>(){ "Antique Gun", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearl"}),
            new TreasureLocation(new Vector3(4241.5927734375f,3588.9301757813f,-46.579597473145f), new List<string>(){ "Antique Gun", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearl"}),
            new TreasureLocation(new Vector3(4241.4741210938f,3610.6987304688f,-48.466213226318f), new List<string>(){ "Antique Gun", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearl"}),
            new TreasureLocation(new Vector3(4242.2919921875f,3617.5881347656f,-45.866725921631f), new List<string>(){ "Antique Gun", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearl"}),
            new TreasureLocation(new Vector3(4202.0517578125f,3643.4736328125f,-39.013248443604f), new List<string>(){ "Antique Gun", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearl"}),
            new TreasureLocation(new Vector3(4213.6840820313f,3647.2878417969f,-41.811008453369f), new List<string>(){ "Antique Gun", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearl"}),
        };

        public TreasureHunting()
        {
            Instance = this;
            SetupItems();
            EventHandlers["TreasureHuntingLooted"] += new Action<Player, int>(TreasureWasLooted);
            EventHandlers["SellAllTreasureLoot"] += new Action<Player>(SellLoot);
        }

        private void SellLoot([FromSource]Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var inventory = user.CurrentCharacter.Inventory;
            var allItems = new List<string>();
            foreach (var location in _locations)
            {
                var items = location.PossibleItems;
                foreach (var item in items)
                {
                    allItems.Add(item);
                }
            }
            foreach (var item in inventory)
            {
                if (allItems.Contains(item.Name))
                {
                    InventoryManager.Instance.SellItemByName(item.Name,player);
                }
            }
        }

        private async Task SetupItems()
        {
            while (ItemManager.Instance == null)
            {
                await Delay(0);
            }
            ItemManager.Instance.DynamicCreateItem("Antique Gun", "Found on the bottom of the ocean.", 150, 150, 10, false);
            ItemManager.Instance.DynamicCreateItem("Dinosaur Bone", "Found on the bottom of the ocean.", 150, 150, 10, false);
            ItemManager.Instance.DynamicCreateItem("Sea Monster Bone", "Found on the bottom of the ocean.", 300, 300, 10, false);
            ItemManager.Instance.DynamicCreateItem("Pearl", "Found on the bottom of the ocean.", 150, 150, 10, false);
            ItemManager.Instance.DynamicCreateItem("Scrap Metal", "Found on the bottom of the ocean.", 150, 150, 10, false);
            ItemManager.Instance.DynamicCreateItem("Watch", "Found on the bottom of the ocean.", 230, 230, 10, false);
            ItemManager.Instance.DynamicCreateItem("Jewlery", "Found on the bottom of the ocean.", 230, 230, 10, false);
            ItemManager.Instance.DynamicCreateItem("Boat Part", "Found on the bottom of the ocean.", 150, 150, 10, false);
            ItemManager.Instance.DynamicCreateItem("Game Console", "Found on the bottom of the ocean.", 230, 230, 10, false);
            ItemManager.Instance.DynamicCreateItem("Car Part", "Found on the bottom of the ocean.", 150, 150, 10, false);
            ItemManager.Instance.DynamicCreateItem("Firearm", "Found on the bottom of the ocean.", 150, 150, 10, false);
            ItemManager.Instance.DynamicCreateItem("Boat Part", "Found on the bottom of the ocean.", 230, 230, 10, false);
            ItemManager.Instance.DynamicCreateItem("Sealed Electronics", "Found on the bottom of the ocean.", 150, 150, 10, false);
            ItemManager.Instance.DynamicCreateItem("Hard Drive", "Found on the bottom of the ocean.", 150, 150, 10, false);
            ItemManager.Instance.DynamicCreateItem("Ivory", "Found on the bottom of the ocean.", 230, 230, 5, false);
            ItemManager.Instance.DynamicCreateItem("Plane Part", "Found on the bottom of the ocean.", 150, 150, 10, false);
            ItemManager.Instance.DynamicCreateItem("Firearm", "Found on the bottom of the ocean.", 150, 150, 10, false);
            ItemManager.Instance.DynamicCreateItem("Alien Bone", "Found on the bottom of the ocean.", 230, 230, 10, false);
            ItemManager.Instance.DynamicCreateItem("Alien Gun", "Found on the bottom of the ocean.", 230, 230, 10, false);
            ItemManager.Instance.DynamicCreateItem("Alien Ship Part", "Found on the bottom of the ocean.", 230, 230, 10, false);
            ItemManager.Instance.DynamicCreateItem("Tank Part", "Found on the bottom of the ocean.", 230, 230, 5, false);
            ItemManager.Instance.DynamicCreateItem("Antique War Clothing", "Found on the bottom of the ocean.", 230, 230, 10, false);
            ItemManager.Instance.DynamicCreateItem("Mortar Shell", "Found on the bottom of the ocean.", 230, 230, 5, false);
        }

        private void TreasureWasLooted([FromSource]Player player, int i)
        {
            var location = _locations[i];
            InventoryManager.Instance.AddItem(location.PossibleItems[_random.Next(0, location.PossibleItems.Count)], 1, player);
        }
        
    }
}
