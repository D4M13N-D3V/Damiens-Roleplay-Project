using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;

namespace roleplay.Main.Clothes
{
    class ClothesStoreManager:BaseScript
    {
        public static ClothesStoreManager Instance;

        private bool menuOpen = false;
        private bool menuCreated = false;

        private UIMenu menu;
        private List<ClothesStore> Stores = new List<ClothesStore>()
        {
            new ClothesStore(1930.519f, 3731.839f, 33.2f, "Clothing Store"),
            new ClothesStore(1693.26f, 4822.27f, 42.5f, "Clothing Store"),
            new ClothesStore(125.83f, -223.16f, 55.2f, "Clothing Store"),
            new ClothesStore(-710.16f, -153.26f, 37.9f, "Clothing Store"),
            new ClothesStore(-821.69f, -1073.90f, 11.9f, "Clothing Store"),
            new ClothesStore(-1192.81f, -768.24f, 17.9f, "Clothing Store"),
            new ClothesStore(4.25f, 6512.88f, 32.2f, "Clothing Store"),
            new ClothesStore(425.471f, -806.164f, 30.1f, "Clothing Store"),
            new ClothesStore(-1101.017f, 2710.264f, 19.5f, "Clothing Store"),
            new ClothesStore(1196.877f, 2709.760f, 38.8f, "Clothing Store"),
        };

        public ClothesStoreManager()
        {
            Instance = this;
            SetupBlips();
            Tick += new Func<Task>(async delegate
            {
                if (ClothesManager.Instance.modelSet)
                {
                    menuOpen = false;
                    var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
                    foreach (ClothesStore store in Stores)
                    {
                        var distance = API.Vdist(store.X, store.Y, store.Z, playerPos.X, playerPos.Y, playerPos.Z);
                        if (distance < 5)
                        {
                            menuOpen = true;
                        }
                    }

                    if (menuOpen && !menuCreated)
                    {
                        menuCreated = true;
                        menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenu(InteractionMenu.Instance._interactionMenu, "Clothing Store", "Open the clothing store and choose your clothing.");
                        InteractionMenu.Instance._menus.Add(menu);
                        //var test1 = new ComponentUI(menu, "1", 0, ComponentTypes.Face); // fACE
                        //var test2 = new ComponentUI(menu, "2", 1, ComponentTypes.Head); // mASK
                        //var test3 = new ComponentUI(menu, "3", 2, ComponentTypes.Hair); // Hair
                        var test4 = new ComponentUI(menu, "Necklaces,Ties,Chains", 7, ComponentTypes.Eyes); // Neck
                        var test5 = new ComponentUI(menu, "Gloves", 3, ComponentTypes.Torso); //Arms/Gloves
                        var test6 = new ComponentUI(menu, "Overshirts", 11, ComponentTypes.Torso2);//Overshirt
                        var test7 = new ComponentUI(menu, "Pants", 4, ComponentTypes.Legs);// Pants
                        var test8 = new ComponentUI(menu, "Parachutes,Backpacks", 5, ComponentTypes.Hands);// Parachute/Backpack
                        var test9 = new ComponentUI(menu, "Shoes", 6, ComponentTypes.Feet); // Shoes
                        //var test10 = new ComponentUI(menu, "10", 9, ComponentTypes.Tasks);//Vests
                        //var test11 = new ComponentUI(menu, "11", 10, ComponentTypes.Textures);// Decals
                        var test12 = new ComponentUI(menu, "Undershirt", 8, ComponentTypes.Acessories);// Parachute/Backpack
                        InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                    }
                    else if (!menuOpen && menuCreated)
                    {
                        menuCreated = false;
                        var index = InteractionMenu.Instance._menus.IndexOf(menu);
                        InteractionMenu.Instance._menus.RemoveAt(index);
                        InteractionMenu.Instance._interactionMenu.RemoveItemAt(index);
                        InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                    }
                }

            });

        }

        private void SetupBlips()
        {
            foreach (ClothesStore Store in Stores)
            {
                var blip = API.AddBlipForCoord(Store.X, Store.Y, Store.Z);
                API.SetBlipSprite(blip,73);
                API.SetBlipColour(blip,3);
                API.SetBlipScale(blip,0.7f);
                API.SetBlipAsShortRange(blip,true);
                API.BeginTextCommandSetBlipName("STRING");
                API.AddTextComponentString(Store.Name);
                API.EndTextCommandSetBlipName(blip);
            }
        }
    }
}
