using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using client.Main.EmergencyServices;
using client.Main.EmergencyServices.Police;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;

namespace client.Main.Activities
{
    public class Hunting : BaseScript
    {
        public static Hunting Instance;

        private Vector3 _sellLocation = new Vector3(-1129.7803955078f, 4926.2016601563f, 219.4998626709f);

        private Ped _targetedPed;
        private bool _isGathering;
        private int _gatherTimeLeft = 0;
        
        private bool _menuCreated = false;
        private UIMenu _menu;

        private const int GatherTimeMin = 60;
        private const int GatherTimeMax = 90;

        private bool _canGather = true;

        private Random _random = new Random();

        public Hunting()
        {
            Instance = this;
            HuntingLogic();
            SellingLogic();


            var blip = API.AddBlipForCoord(_sellLocation.X, _sellLocation.Y, _sellLocation.Z);
            API.SetBlipSprite(blip, 463);
            API.SetBlipColour(blip, 25);
            API.SetBlipScale(blip, 1f);
            API.SetBlipAsShortRange(blip, true);
            API.BeginTextCommandSetBlipName("STRING");
            API.AddTextComponentString("Hunting Buyer");
            API.EndTextCommandSetBlipName(blip);
        }

        private async Task SellingLogic()
        {
            while (true)
            {
                if (Utility.Instance.GetDistanceBetweenVector3s(_sellLocation, Game.PlayerPed.Position) < 30)
                {
                    World.DrawMarker(MarkerType.HorizontalCircleSkinny, _sellLocation - new Vector3(0, 0, 0.5f), Vector3.Zero, Vector3.Zero, new Vector3(2, 2, 2), Color.FromArgb(255, 255, 255, 0));

                    if (Utility.Instance.GetDistanceBetweenVector3s(_sellLocation, Game.PlayerPed.Position) < 6 && !_menuCreated)
                    {
                        _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(
                            InteractionMenu.Instance._interactionMenu, "Hunting Buyer", "Sell your animal skin and meat here.", new PointF(5, Screen.Height / 2));
                        var button1 = new UIMenuItem("Sell Hides");
                        var button2 = new UIMenuItem("Sell Meats");
                        _menu.AddItem(button1);
                        _menu.AddItem(button2);
                        _menu.OnItemSelect += (sender, item, index) =>
                        {
                            if (item == button1)
                            {
                                TriggerServerEvent("SellHuntingHides");
                            }
                            else if (item == button2)
                            {
                                TriggerServerEvent("SellHuntingMeats");
                            }
                        };
                        _menuCreated = true;
                    }
                    else if(Utility.Instance.GetDistanceBetweenVector3s(_sellLocation, Game.PlayerPed.Position) > 6 && _menuCreated)
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
                }
                await Delay(0);
            }
        }
        
        private async Task HuntingLogic()
        {
            while (true)
            {
                if (Game.IsControlJustPressed(0, Control.Context) && !Game.PlayerPed.IsInVehicle())
                {
                    var targetId = 0;
                    Ped target = Utility.Instance.ClosestPed;
                    if (target.Exists() && !target.IsAlive && Huntable(target) && Utility.Instance.GetDistanceBetweenVector3s(Game.PlayerPed.Position, target.Position) < 6)
                    {
                        StartGathering(target);
                    }
                }
                await Delay(0);
            }
        }

        public bool Huntable(Ped ped)
        {
            return ped.Model == PedHash.MountainLion || ped.Model == PedHash.Boar || ped.Model == PedHash.Deer ||
                   ped.Model == PedHash.Rabbit;
        }

        private async Task StartGathering(Ped target)
        {
            _canGather = false;
            _isGathering = true;
            _targetedPed = target;
            _gatherTimeLeft = _random.Next(GatherTimeMin, GatherTimeMax);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            mugTimeUI();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            async Task mugTimeUI()
            {
                while (_isGathering)
                {
                    Utility.Instance.DrawTxt(0.5f, 0.1f, 0, 0, 1, "Skinning and gathering fromt the animal! " + _gatherTimeLeft + " Seconds Left!", 255, 255, 0, 255, true);
                    await Delay(0);
                }
            }

            var amount = _gatherTimeLeft;
            for (int i = 0; i < amount; i++)
            {
                if (Game.IsControlPressed(0,Control.Context) && Utility.Instance.GetDistanceBetweenVector3s(Game.PlayerPed.Position, target.Position) < 8)
                {
                    _gatherTimeLeft = _gatherTimeLeft - 1;
                    await Delay(1000);
                }
                else
                {
                    CancelGathering();
                    return;
                }
            }
            Debug.WriteLine(Convert.ToString(_gatherTimeLeft));
            _isGathering = false;
            if (target.Model == PedHash.Boar)
            {
                TriggerServerEvent("HuntingReward");
                TriggerServerEvent("HuntingReward");
            }
            else if (target.Model == PedHash.MountainLion)
            {
                TriggerServerEvent("HuntingReward");
                TriggerServerEvent("HuntingReward");
                TriggerServerEvent("HuntingReward");
            }
            else if (target.Model == PedHash.Rabbit)
            {
                TriggerServerEvent("HuntingReward");
            }
            else if (target.Model == PedHash.Deer)
            {
                TriggerServerEvent("HuntingReward");
                TriggerServerEvent("HuntingReward");
            }
            target.IsPositionFrozen = false;
            target.Task.ClearAll();
            target.Delete();
        }

        private void CancelGathering()
        {
            _isGathering = false;
            _targetedPed.Task.ClearAll();
            _targetedPed.IsPositionFrozen = false;
        }
    }
}
