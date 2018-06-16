using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client.Main.Vehicles;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;

namespace client.Main.Activities
{
    public class GoKarts : BaseScript
    {
        public static GoKarts Instance;

        public List<Vector3> Posistions = new List<Vector3>()
            {
                new Vector3(1214.326171875f,333.49841308594f,81.990951538086f)      
            };

        private Dictionary<string, int> _rentalPrices = new Dictionary<string, int>()
        {
            ["Kart"] = 100,
        };

        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private UIMenu _menu;
        private int _rentedCar;

        public GoKarts()
        {
            Instance = this;
            SetupBlips(147, 4);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            RentalSpotCheck();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            DrawMarkers();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            EventHandlers["GoKartRentalRequest"] += new Action<string>(VehicleRentalRequestClient);
            EventHandlers["GoKartReturnRequest"] += new Action(VehicleReturnRequest);
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
        private void VehicleReturnRequest()
        {
            if (API.DoesEntityExist(_rentedCar))
            {
                API.DeleteVehicle(ref _rentedCar);
            }
        }

        private async void VehicleRentalRequestClient(string s)
        {
            int vehId = 0;
            Debug.WriteLine(s);
            await Utility.Instance.SpawnCar(s, i => { vehId = i; });
            API.SetVehicleNumberPlateText(vehId, "RENTAL");
            _rentedCar = vehId;
            Game.PlayerPed.SetIntoVehicle((Vehicle)Vehicle.FromHandle(_rentedCar), VehicleSeat.Driver);
            API.DecorSetInt(_rentedCar, "PIRP_VehicleOwner", Game.Player.ServerId);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            RentalSpotCheck();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            async Task RentalSpotCheck()
            {
                while (API.DoesEntityExist(_rentedCar))
                {
                    if (Vector3.Distance(Game.PlayerPed.Position,
                            new Vector3(1214.326171875f, 333.49841308594f, 81.990951538086f)) > 500)
                    {
                        API.DeleteVehicle(ref _rentedCar);
                    }
                    await Delay(5000);
                }
            }

        }


        private async Task DrawMarkers()
        {
            while (true)
            {
                foreach (var pos in Posistions)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(pos, _playerPos) < 30)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, pos - new Vector3(0, 0, 0.5f), Vector3.Zero, Vector3.Zero, new Vector3(2, 2, 2), Color.FromArgb(255, 255, 255, 0));
                    }
                }
                await Delay(0);
            }
        }

        private void SetupBlips(int sprite, int color)
        {
            foreach (var var in Posistions)
            {
                var blip = API.AddBlipForCoord(var.X, var.Y, var.Z);
                API.SetBlipSprite(blip, sprite);
                API.SetBlipColour(blip, color);
                API.SetBlipScale(blip, 1f);
                API.SetBlipAsShortRange(blip, true);
                API.BeginTextCommandSetBlipName("STRING");
                API.AddTextComponentString("Go Karts!");
                API.EndTextCommandSetBlipName(blip);
            }
        }

        private async Task RentalSpotCheck()
        {
            while (true)
            {
                _menuOpen = false;
                foreach (var pos in Posistions)
                {
                    var dist = API.Vdist(_playerPos.X, _playerPos.Y, _playerPos.Z, pos.X, pos.Y, pos.Z);
                    if (dist < 6f)
                    {
                        _menuOpen = true;
                    }
                }

                if (_menuOpen && !_menuCreated)
                {
                    _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(
                        InteractionMenu.Instance._interactionMenu, "Vehicle Rentals", "Rental your vehicle.", new PointF(5, Screen.Height / 2));
                    var returnButton = new UIMenuItem("Return Vehicle");
                    _menu.AddItem(returnButton);
                    _menu.OnItemSelect += (sender, item, index) =>
                    {
                        if (item == returnButton && API.DoesEntityExist(_rentedCar))
                        {
                            foreach (var rental in _rentalPrices)
                            {
                                if (Game.PlayerPed.CurrentVehicle.Model == API.GetHashKey(rental.Key))
                                {
                                    TriggerServerEvent("GoKartReturnRequest", rental.Key);
                                }
                            }
                        }
                    };
                    foreach (var rental in _rentalPrices)
                    {
                        var button = new UIMenuItem(rental.Key + "-~g~$" + rental.Value, "~g~Costs " + rental.Value);
                        _menu.AddItem(button);
                        _menu.OnItemSelect += (sender, item, index) =>
                        {
                            if (item == button)
                            {
                                if (API.DoesEntityExist(_rentedCar))
                                {
                                    Utility.Instance.SendChatMessage("[GoKarts]","You already have one out. Return it.",255,0,0);
                                }
                                else
                                {
                                    TriggerServerEvent("GoKartRentalRequest", rental.Key);
                                }
                            }
                        };
                    }
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
    }
}


