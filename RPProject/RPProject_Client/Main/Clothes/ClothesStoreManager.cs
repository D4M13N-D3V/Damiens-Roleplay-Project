using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;

namespace roleplay.Main.Clothes
{
    class ClothesStoreManager : BaseScript
    {
        public static ClothesStoreManager Instance;

        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private int _menuIndex = 0;
        public UIMenu Menu;
        private List<ClothesStore> Stores = new List<ClothesStore>()
        {
            new ClothesStore(1930.519f, 3731.839f, 33.2f, "Clothing Store", ClothesStoreTypes.Clothes),
            new ClothesStore(1693.26f, 4822.27f, 42.5f, "Clothing Store", ClothesStoreTypes.Clothes),
            new ClothesStore(125.83f, -223.16f, 55.2f, "Clothing Store", ClothesStoreTypes.Clothes),
            new ClothesStore(-710.16f, -153.26f, 37.9f, "Clothing Store", ClothesStoreTypes.Clothes),
            new ClothesStore(-821.69f, -1073.90f, 11.9f, "Clothing Store", ClothesStoreTypes.Clothes),
            new ClothesStore(-1192.81f, -768.24f, 17.9f, "Clothing Store", ClothesStoreTypes.Clothes),
            new ClothesStore(4.25f, 6512.88f, 32.2f, "Clothing Store", ClothesStoreTypes.Clothes),
            new ClothesStore(425.471f, -806.164f, 30.1f, "Clothing Store", ClothesStoreTypes.Clothes),
            new ClothesStore(-1101.017f, 2710.264f, 19.5f, "Clothing Store", ClothesStoreTypes.Clothes),
            new ClothesStore(1196.877f, 2709.760f, 38.8f, "Clothing Store", ClothesStoreTypes.Clothes),

            new ClothesStore(242.66821289063f, 362.84359741211f, 105.7381439209f, "Jewlery & Accessories Store", ClothesStoreTypes.Jewlery),
            new ClothesStore(-624.28509521484f, -232.48692321777f, 38.057037353516f, "Jewlery & Accessories Store", ClothesStoreTypes.Jewlery),

            new ClothesStore(-1282.7607421875f,-1117.4642333984f,6.9901118278503f,"Barber Shop", ClothesStoreTypes.Barbor),
            new ClothesStore(-813.72674560547f,-183.9781036377f,37.568935394287f,"Barber Shop", ClothesStoreTypes.Barbor),
            new ClothesStore(-32.760932922363f,-152.15519714355f,57.076499938965f,"Barber Shop", ClothesStoreTypes.Barbor),
            new ClothesStore(1212.5958251953f,-472.69177246094f,66.2080078125f,"Barber Shop", ClothesStoreTypes.Barbor),
            new ClothesStore(136.73826599121f,-1707.6754150391f,29.291622161865f,"Barber Shop", ClothesStoreTypes.Barbor),
            new ClothesStore(1930.9205322266f,3730.9631347656f,32.844425201416f,"Barber Shop", ClothesStoreTypes.Barbor),
            new ClothesStore(-278.35531616211f,6228.6611328125f,31.695512771606f,"Barber Shop", ClothesStoreTypes.Barbor),

            new ClothesStore(1322.9836425781f,-1651.9805908203f,52.275184631348f,"Tattoo Parlor", ClothesStoreTypes.Tattoo),
            new ClothesStore(-1153.7065429688f,-1426.0969238281f,4.9544596672058f,"Tattoo Parlor", ClothesStoreTypes.Tattoo),
            new ClothesStore(323.46041870117f,179.92189025879f,103.58651733398f,"Tattoo Parlor", ClothesStoreTypes.Tattoo),
            new ClothesStore(-3170.5539550781f,1075.5850830078f,20.829183578491f,"Tattoo Parlor", ClothesStoreTypes.Tattoo),
            new ClothesStore(-293.79901123047f,6200.1220703125f,31.487142562866f,"Tattoo Parlor", ClothesStoreTypes.Tattoo),
            new ClothesStore(1863.8935546875f,3748.9865722656f,33.031848907471f,"Tattoo Parlor", ClothesStoreTypes.Tattoo),

            new ClothesStore(-247.47776794434f,6330.83203125f,32.426189422607f,"Plastic Surgeon", ClothesStoreTypes.Plastic),

            new ClothesStore(-1337.3997802734f,-1278.0234375f,4.8725299835205f,"Mask Shop", ClothesStoreTypes.Mask),
    };



        private ClothesStore _currentStore;

        public ClothesStoreManager()
        {
            Instance = this;
            SetupBlips();
            DrawMarkers();
            Tick += new Func<Task>(async delegate
            {
                if (ClothesManager.Instance.modelSet)
                {
                    _menuOpen = false;
                    _currentStore = null;
                    foreach (ClothesStore store in Stores)
                    {
                        var distance = API.Vdist(store.X, store.Y, store.Z, _playerPos.X, _playerPos.Y, _playerPos.Z);
                        if (distance < 5)
                        {
                            _currentStore = store;
                            _menuOpen = true;
                        }
                    }

                    if (_menuOpen && !_menuCreated)
                    {
                        _menuCreated = true;
                        switch (_currentStore.Type)
                        {
                            case ClothesStoreTypes.Clothes:
                                Menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(InteractionMenu.Instance._interactionMenu, "Clothing Store", "Open the clothing store and choose your clothing.", new PointF(5, Screen.Height / 2));
                                //var test10 = new ComponentUI(menu, "10", 9, ComponentTypes.Tasks);//Vests
                                //var test11 = new ComponentUI(menu, "11", 10, ComponentTypes.Textures);// Decals
                                var Hats = new PropUI(Menu, "Hats", 0, PropTypes.Hats);
                                var Gloves = new ComponentUI(Menu, "Gloves", 3, ComponentTypes.Torso); //Arms/Gloves
                                var Overshirts = new ComponentUI(Menu, "Overshirts", 11, ComponentTypes.Torso2);//Overshirt
                                var Pants = new ComponentUI(Menu, "Pants", 4, ComponentTypes.Legs);// Pants
                                var Backpack = new ComponentUI(Menu, "Parachutes,Backpacks", 5, ComponentTypes.Hands);// Parachute/Backpack
                                var Shoes = new ComponentUI(Menu, "Shoes", 6, ComponentTypes.Feet); // Shoes
                                var UnderShirt = new ComponentUI(Menu, "Undershirt", 8, ComponentTypes.Acessories);// Parachute/Backpack
                                InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                                break;
                            case ClothesStoreTypes.Jewlery:
                                Menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(InteractionMenu.Instance._interactionMenu, "Jewlery & Acessories Store", "Open the jewlery store and choose your acessories.", new PointF(5, Screen.Height / 2));
                                var Necklaces = new ComponentUI(Menu, "Necklaces,Ties,Chains", 7, ComponentTypes.Eyes); // Neck
                                var Glasses = new PropUI(Menu, "Glasses", 1, PropTypes.Glasses);
                                var EarRings = new PropUI(Menu, "Ear Rings/Ear Pieces", 2, PropTypes.Ears);
                                InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                                break;
                            case ClothesStoreTypes.Barbor:
                                Menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(InteractionMenu.Instance._interactionMenu, "Barbor Shop", "Open the barbor shop to change your hair.", new PointF(5, Screen.Height / 2));
                                var Hair = new ComponentUI(Menu, "Hair", 2, ComponentTypes.Hair); // Hair
                                var Beards = new HeadOverlayUI(Menu, "Beards", 1, HeadOverlayTypes.Beards);// Parachute/Backpack
                                var Eyebrows = new HeadOverlayUI(Menu, "Eyebrows", 2, HeadOverlayTypes.Eyebrows);// Parachute/Backpack
                                var Chesthair = new HeadOverlayUI(Menu, "Chesthair", 10, HeadOverlayTypes.Chesthair);// Parachute/Backpack
                                break;
                            case ClothesStoreTypes.Plastic:
                                Menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(InteractionMenu.Instance._interactionMenu, "Plastic Surgeon", "Open the plastic surgeons officer to change your body.", new PointF(5, Screen.Height / 2));
                                var Models = new ModelMenu(Menu, "Models");
                                var Face = new ComponentUI(Menu, "Face", 0, ComponentTypes.Face); // fACE
                                var Aging = new HeadOverlayUI(Menu, "Aging", 3, HeadOverlayTypes.Aging);// Parachute/Backpack
                                var Makeup = new HeadOverlayUI(Menu, "Makeup", 4, HeadOverlayTypes.Makeup);// Parachute/Backpack
                                var Blush = new HeadOverlayUI(Menu, "Blush", 5, HeadOverlayTypes.Blush);// Parachute/Backpack
                                var Complexion = new HeadOverlayUI(Menu, "Complexion", 6, HeadOverlayTypes.Complexion);// Parachute/Backpack
                                var Sundamage = new HeadOverlayUI(Menu, "Sundamage", 7, HeadOverlayTypes.Sundamage);// Parachute/Backpack
                                var Lipstick = new HeadOverlayUI(Menu, "Lipstick", 8, HeadOverlayTypes.Lipstick);// Parachute/Backpack
                                var Moles = new HeadOverlayUI(Menu, "Moles", 9, HeadOverlayTypes.Moles);// Parachute/Backpack
                                var Blemishes = new HeadOverlayUI(Menu, "Blemishes", 0, HeadOverlayTypes.Blemishes);// Parachute/Backpack
                                var Bodyblemishes = new HeadOverlayUI(Menu, "Bodyblemishes", 11, HeadOverlayTypes.BodyBlemishes);// Parachute/Backpack
                                break;
                            case ClothesStoreTypes.Mask:
                                Menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(InteractionMenu.Instance._interactionMenu, "Mask Store", "Open the mask store to buy a mask.", new PointF(5, Screen.Height / 2));
                                var Head = new ComponentUI(Menu, "Masks", 1, ComponentTypes.Head); // mASK
                                break;
                            case ClothesStoreTypes.Tattoo:
                                var Tattoos = new TatooUI(InteractionMenu.Instance._interactionMenu, "Tattoos"); // mASK
                                break;
                        }
                        InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                        _menuIndex = InteractionMenu.Instance._interactionMenu.MenuItems.Count-1;
                    }
                    else if (!_menuOpen && _menuCreated)
                    {
                        _menuCreated = false;
                        InteractionMenu.Instance._interactionMenu.RemoveItemAt(_menuIndex);
                        InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                    }
                }

            });

            GetPlayerPosEverySecond();
        }

        public Vector3 _playerPos;
        private async Task GetPlayerPosEverySecond()
        {
            while (true)
            {
                _playerPos = Game.PlayerPed.Position;
                await Delay(1000);
            }
        }


        private async Task DrawMarkers()
        {
            while (true)
            {
                foreach (var pos in Stores)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(new Vector3(pos.X, pos.Y, pos.Z), _playerPos) < 30)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, new Vector3(pos.X, pos.Y, pos.Z) - new Vector3(0, 0, 0.8f), Vector3.Zero, Vector3.Zero, Vector3.One, Color.FromArgb(175, 255, 255, 0));
                    }
                }
                await Delay(0);
            }
        }

        private void SetupBlips()
        {
            foreach (ClothesStore Store in Stores)
            {
                var blip = API.AddBlipForCoord(Store.X, Store.Y, Store.Z);
                switch (Store.Type)
                {
                    case ClothesStoreTypes.Clothes:
                        API.SetBlipSprite(blip, 73);
                        break;
                    case ClothesStoreTypes.Jewlery:
                        API.SetBlipSprite(blip, 439);
                        break;
                    case ClothesStoreTypes.Barbor:
                        API.SetBlipSprite(blip, 71);
                        break;
                    case ClothesStoreTypes.Tattoo:
                        API.SetBlipSprite(blip, 75);
                        break;
                    case ClothesStoreTypes.Plastic:
                        API.SetBlipSprite(blip, 279);
                        break;
                    case ClothesStoreTypes.Mask:
                        API.SetBlipSprite(blip, 362);
                        break;
                }
                API.SetBlipColour(blip, 2);
                API.SetBlipScale(blip, 0.7f);
                API.SetBlipAsShortRange(blip, true);
                API.BeginTextCommandSetBlipName("STRING");
                API.AddTextComponentString(Store.Name);
                API.EndTextCommandSetBlipName(blip);
            }
        }
    }
}
