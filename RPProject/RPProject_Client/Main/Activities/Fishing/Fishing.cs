using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;

namespace client.Main.Activities.Fishing
{

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

        private bool _canFishAgain = true;

        public Fishing()
        {
            Instance = this;
            SetupBlips();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            FishingSpotCheck();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            DrawMarkers();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                            StartFishing();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                            _canFishAgain = false;
                        }
                        else if(!_currentlyFishing && !_canFishAgain)
                        {
                            Utility.Instance.SendChatMessage("[Fishing]","You need to wait a few more seconds before trying to fish again.",255,255,0);
                        }
                    }
                }
            };
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            GetPlayerPosEverySecond();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
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
                foreach (var pos in _fishSpots)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(new Vector3(pos.X, pos.Y, pos.Z), _playerPos) < 30)
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
                foreach (FishSpot zone in _fishSpots)
                {
                    var distance = API.Vdist(zone.X,zone.Y,zone.Z, _playerPos.X, _playerPos.Y, _playerPos.Z);
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
            _canFishAgain = true;
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

#pragma warning disable CS0219 // The variable 'isNearSpot' is assigned but its value is never used
                var isNearSpot = false;
#pragma warning restore CS0219 // The variable 'isNearSpot' is assigned but its value is never used
                foreach (var var in _fishSpots)
                {   
                    if (!var.IsShop)
                    {
                        if (Utility.Instance.GetDistanceBetweenVector3s(Game.PlayerPed.Position,
                            new Vector3(var.X, var.Y, var.Z))>8)
                        {
                            StopFishing();
                            Game.PlayerPed.Task.ClearAll();
                            return;
                        }
                    }
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

            _canFishAgain = true;
        }
    }
}
