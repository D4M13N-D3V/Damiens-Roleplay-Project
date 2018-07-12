using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.Threading.Tasks;
using client.Main.Criminal.Informant;
using client.Main.EmergencyServices;
using client.Main.EmergencyServices.Police;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;

namespace client.Main.Criminal.Robberies
{
    public enum RobberyTypes { Store, Bank }
    public class RobberiesManager : BaseScript
    {
        public static RobberiesManager Instance;

        public List<RobberySpot> Spots = new List<RobberySpot>()
        {
            new RobberySpot(new Vector3(1707.9324951172f,4920.3510742188f,42.063674926758f), "Grapseed Main Street 24/7", 300, 3, 20000, 6000),
            new RobberySpot(new Vector3(-2959.7761230469f,387.1662902832f,14.043292999268f), "Great Ocean Highway Liquior Store", 300, 3, 20000, 6000),
            new RobberySpot(new Vector3(2549.6787109375f,384.93060302734f,108.62294769287f), "Palimino Freeway 24/7", 300, 3, 20000, 6000),
            new RobberySpot(new Vector3(-709.68231201172f,-904.01232910156f,19.21561050415f), "Little Seoul 24/7", 300, 3, 20000, 6000),
            new RobberySpot(new Vector3(2672.7614746094f,3286.6235351563f,55.241134643555f), "Senory Fwy 24/7", 300, 3, 20000, 6000),
            new RobberySpot(new Vector3(-1829.1158447266f,798.81048583984f,138.18949890137f), "Banham Canyon Drive 24/7", 300, 3, 20000, 6000),
            new RobberySpot(new Vector3(147.01237487793f,-1044.9592285156f,29.368022918701f), "Legion Square Bank", 600, 4, 60000, 20000),
            new RobberySpot(new Vector3(255.65658569336f,226.54191589355f,101.87573242188f), "Pacific Standard Bank", 600, 4, 60000, 20000),
            new RobberySpot(new Vector3(-104.95664215088f,6476.5415039063f,31.626724243164f), "Blaine County Savings", 600, 4, 60000, 20000),
        };

        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private UIMenu _menu;
        private RobberySpot _activeSpot = null;

        private bool _currentlyRobbing = false;
        private int _timeLeftForRobbery = 0;

        public RobberiesManager()
        {
            Instance = this;
            SetupBlips(103, 1);
            InformantCheck();
            DrawMarkers();
            EventHandlers["StartRobbingStore"] += new Action(StartRobbery);
        }
        


        private async Task DrawMarkers()
        {
            while (true)
            {
                foreach (var pos in Spots)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(pos.Posistion, Game.PlayerPed.Position) < 30)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, pos.Posistion - new Vector3(0, 0, 0.8f), Vector3.Zero, Vector3.Zero, new Vector3(2, 2, 2), Color.FromArgb(255, 180, 0, 0));
                    }
                }
                await Delay(0);
            }
        }

        private void SetupBlips(int sprite, int color)
        {
            foreach (var var in Spots)
            {
                var blip = API.AddBlipForCoord(var.Posistion.X, var.Posistion.Y, var.Posistion.Z);
                API.SetBlipSprite(blip, sprite);
                API.SetBlipColour(blip, color);
                API.SetBlipScale(blip, 1f);
                API.SetBlipAsShortRange(blip, true);
                API.BeginTextCommandSetBlipName("STRING");
                API.AddTextComponentString("Robbable Spot");
                API.EndTextCommandSetBlipName(blip);
            }
        }

        private async Task InformantCheck()
        {
            while (true)
            {

                _menuOpen = false;
                foreach (var pos in Spots)
                {
                    var dist = API.Vdist(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, pos.Posistion.X, pos.Posistion.Y, pos.Posistion.Z);
                    if (dist < 3f)
                    {
                        _menuOpen = true;
                        _activeSpot = pos;
                    }
                }

                if (_menuOpen && !_menuCreated)
                {
                    _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(
                        InteractionMenu.Instance._interactionMenu, "Robbable Spot", "Rob this spot!", new PointF(5, Screen.Height / 2));
                    var button = new UIMenuItem("Rob Store", "~r~MIN:"+_activeSpot.MinReward+"~w~/~g~MAX"+ _activeSpot.MaxReward);
                    _menu.AddItem(button);
                    _menu.OnItemSelect += (sender, item, index) =>
                    {
                        if (item == button)
                        {
                            if (Police.Instance.CopCount >= _activeSpot.RequiredPolice)
                            {
                                TriggerServerEvent("StartRobbingStoreRequest", _activeSpot.Name);
                            }
                            else
                            {
                                Utility.Instance.SendChatMessage("[Robberies]","Not enough police on.",255,0,0);
                            }
                        }
                    };
                    _menuCreated = true;
                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                }
                else if (!_menuOpen && _menuCreated)
                {
                    _menuCreated = false;
                    if (_menu.Visible)
                    {
                        _menu.Visible = false;
                        InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                        InteractionMenu.Instance._interactionMenu.Visible = true;
                    }

                    var i = 0;
                    foreach (var item in InteractionMenu.Instance._interactionMenu.MenuItems)
                    {
                        if (item == _menu.ParentItem)
                        {
                            InteractionMenu.Instance._interactionMenu.RemoveItemAt(i);
                            break;
                        }

                        i++;
                    }

                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                }

                await Delay(1000);
            }
        }

        private async void StartRobbery()
        {
            TriggerEvent("911CallClientAnonymous", "This is Securo Serve, one of our clients is being robbed. ( "+_activeSpot.Name+" )");
            _currentlyRobbing = true;
            _timeLeftForRobbery = _activeSpot.TimeToRobInSeconds;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            HUD();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            async Task HUD()
            {
                while (_currentlyRobbing)
                {
                    Utility.Instance.DrawTxt(0.5f,0.2f,0,0,2,_timeLeftForRobbery+" seconds left to complete robbery",255,0,0,255,true);
                    await Delay(0);
                }
            }
            while (_currentlyRobbing)
            {
                await Delay(1000);
                _timeLeftForRobbery = _timeLeftForRobbery - 1;
                if (Utility.Instance.GetDistanceBetweenVector3s(Game.PlayerPed.Position, _activeSpot.Posistion) > 15)
                {
                    _currentlyRobbing = false;
                    TriggerServerEvent("EndRobbingStore", _activeSpot.Name);
                }

                if (_timeLeftForRobbery <= 0)
                {
                    _currentlyRobbing = false;
                    TriggerServerEvent("CompleteRobbingStore", _activeSpot.Name);
                }
            }
        }


    }
}
