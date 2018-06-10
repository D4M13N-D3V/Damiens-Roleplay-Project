using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;

namespace roleplay.Main.Activities
{

    public class FishSpot
    {
        public float X = 0;
        public float Y = 0;
        public float Z = 0;
        public bool IsShop = false;

        public FishSpot(float x, float y, float z, bool isShop)
        {
            IsShop = isShop;
            X = x;
            Y = y;
            Z = z;
        }

    }

    public class Fishing : BaseScript
    {

        private List<FishSpot> _fishSpots = new List<FishSpot>()
        {
            new FishSpot(-1845.090f,-1197.110f,19.186f,true),
            new FishSpot(-1614.893f,5260.193f,3.974f,false),
        };

        public static Fishing Instance;
        private const int DistanceCanFish = 20;
        private const int FishingRate = 8000; 
        private bool _currentlyFishing = false;

        private bool _buttonOpen = false;
        private bool _buttonCreated = false;
        private int _buttonIndex = 0;
        private bool _buttonIsShop = false;
        public UIMenuItem Button;

        private Entity _fishingPole;

        private bool _canFishAgain = true;

        public Fishing()
        {
            Instance = this;
            SetupBlips();
            FishingSpotCheck();
            DrawMarkers();
            InteractionMenu.Instance._interactionMenu.OnItemSelect += async(sender, item, index) =>
            {
                if (item == Button)
                {
                    if (_buttonIsShop)
                    {
                        TriggerServerEvent("SellAllFish");
                    }
                    else
                    {
                        if (_currentlyFishing)
                        {
                            StopFishing();
                            await Delay(FishingRate + 1000);
                            _canFishAgain = true;
                        }
                        else if(!_currentlyFishing && _canFishAgain)
                        {
                            StartFishing();
                            _canFishAgain = false;
                        }
                        else if(!_currentlyFishing && !_canFishAgain)
                        {
                            Utility.Instance.SendChatMessage("[Fishing]","You need to wait a few more seconds before trying to fish again.",255,255,0);
                        }
                    }
                }
            };
        }

        private async Task DrawMarkers()
        {
            while (true)
            {
                foreach (var pos in _fishSpots)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(new Vector3(pos.X, pos.Y, pos.Z), Game.PlayerPed.Position) < 30)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, new Vector3(pos.X, pos.Y, pos.Z) - new Vector3(0, 0, 0.8f), Vector3.Zero, Vector3.Zero, Vector3.One, Color.FromArgb(175, 255, 255, 0));
                    }
                }
                await Delay(0);
            }
        }

        private async Task FishingSpotCheck()
        {
            while (true)
            {
                _buttonOpen = false;
                var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
                foreach (FishSpot zone in _fishSpots)
                {
                    var distance = API.Vdist(zone.X,zone.Y,zone.Z,playerPos.X,playerPos.Y,playerPos.Z);
                    if (distance < 8)
                    {
                        _buttonOpen = true;
                        _buttonIsShop = zone.IsShop;
                    }
                }

                if (_buttonOpen && !_buttonCreated)
                {
                    _buttonCreated = true;
                    if (_buttonIsShop)
                    {
                        Button = new UIMenuItem("Sell all fish!", "Sell all of the fish in your inventory!");
                        InteractionMenu.Instance._interactionMenu.AddItem(Button);
                        InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                        _buttonIndex = InteractionMenu.Instance._interactionMenu.MenuItems.Count - 1;
                    }
                    else
                    {
                        Button = new UIMenuItem("Start/Stop fishing!", "Start/Stop fishing for..fish? Duh..?");
                        InteractionMenu.Instance._interactionMenu.AddItem(Button);
                        InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                        _buttonIndex = InteractionMenu.Instance._interactionMenu.MenuItems.Count - 1;
                    }
                }
                else if (!_buttonOpen && _buttonCreated)
                {
                    _buttonCreated = false;
                    InteractionMenu.Instance._interactionMenu.RemoveItemAt(_buttonIndex);
                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                    StopFishing();
                }

                await Delay(1000);
            }
        }

        private void SetupBlips()
        {
            foreach (FishSpot store in _fishSpots)
            {
                var blip = API.AddBlipForCoord(store.X, store.Y, store.Z);
                if (store.IsShop)
                {
                    API.SetBlipSprite(blip, 356);
                }
                else
                {
                    API.SetBlipSprite(blip, 68);
                }
                API.SetBlipColour(blip, 3);
                API.SetBlipAsShortRange(blip, true);
                API.BeginTextCommandSetBlipName("STRING");
                if (store.IsShop)
                {
                    API.AddTextComponentString("Fishing Store");
                }
                else
                {
                    API.AddTextComponentString("Fishing Zone");
                }
                API.EndTextCommandSetBlipName(blip);
            }
        }

        private void StopFishing()
        {
            _currentlyFishing = false;
            API.ClearPedTasks(Game.PlayerPed.Handle);
        }

        private async Task StartFishing()
        {
            _currentlyFishing = true;
            API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "WORLD_HUMAN_STAND_FISHING",0,false);
            while (_currentlyFishing)
            {

                if (!API.IsPedUsingScenario(Game.PlayerPed.Handle, "WORLD_HUMAN_STAND_FISHING"))
                {
                    API.TaskStartScenarioInPlace(Game.PlayerPed.Handle, "WORLD_HUMAN_STAND_FISHING", 0, false);
                }

                var isNearSpot = false;
                foreach (var var in _fishSpots)
                {   
                    if (!var.IsShop)
                    {
                        if (Utility.Instance.GetDistanceBetweenVector3s(Game.PlayerPed.Position,
                            new Vector3(var.X, var.Y, var.Z))<8)
                        {
                            isNearSpot = true;
                        }
                    }
                }

                if (!isNearSpot)
                {
                    _currentlyFishing = false;
                    StopFishing();
                    return;
                }
                await Delay(FishingRate);
                Random rdm = new Random();
                var rng = rdm.Next(1, 20);
                if (rng > 19)
                {
                    Utility.Instance.SendChatMessage("FISHING", "You caught a bass!", 0, 150, 150);
                    TriggerServerEvent("GetFish","Bass");
                }
                else if (rng > 17)
                {
                    Utility.Instance.SendChatMessage("FISHING", "You caught a catfish!", 0, 150, 150);
                    TriggerServerEvent("GetFish", "Catfish");
                }
                else if (rng > 14)
                {
                    Utility.Instance.SendChatMessage("FISHING", "You caught a flounder!", 0, 150, 150);
                    TriggerServerEvent("GetFish", "Flounder");
                }
                else if (rng > 8)
                {
                    Utility.Instance.SendChatMessage("FISHING", "You caught a salmon!", 0, 150, 150);
                    TriggerServerEvent("GetFish", "Salmon");
                }
                else if(rng>4)
                {
                    Utility.Instance.SendChatMessage("FISHING", "You caught a trout!", 0, 150, 150);
                    TriggerServerEvent("GetFish", "Trout");
                }
                else
                {
                    Utility.Instance.SendChatMessage("FISHING","You didnt catch a fish!",0,150,150);
                }
            }
        }
    }
}
