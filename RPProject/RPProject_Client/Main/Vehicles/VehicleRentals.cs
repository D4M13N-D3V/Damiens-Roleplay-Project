using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;

namespace client.Main.Vehicles
{
    public class RentalSpot : BaseScript
    {
        public static RentalSpot Instance;

        public List<Vector3> Posistions = new List<Vector3>()
            {
                new Vector3(-79.95760345459f,6489.0693359375f,31.49089050293f)
            };

        private Dictionary<string, int> _rentalPrices = new Dictionary<string, int>()
        {
            ["BMX"] = 25,
            ["Tornado"] = 800,
            ["FQ2"] = 700,
            ["Asea"] = 1000,
            ["Emperor"] = 800,
            ["Emperor2"] = 700,
            ["Faggio3"] = 200,
            ["Ingot"] = 500,
            ["Stratum"] = 500,
        };

        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private UIMenu _menu;
        private int _rentedCar;

        public RentalSpot()
        {
            Instance = this;
            SetupBlips(85, 5);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            RentalSpotCheck();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            DrawMarkers();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            EventHandlers["VehicleRentalRequestClient"] += new Action<string>(VehicleRentalRequestClient);
            EventHandlers["VehicleReturnRequest"] += new Action(VehicleReturnRequest);
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
            await Utility.Instance.SpawnCar(s, i => { vehId = i; });
            API.SetVehicleNumberPlateText(vehId, "RENTAL");
            _rentedCar = vehId;
            API.DecorSetInt(_rentedCar, "PIRP_VehicleOwner", Game.Player.ServerId);
        }


        private async Task DrawMarkers()
        {
            while (true)
            {
                foreach (var pos in Posistions)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(pos, Game.PlayerPed.Position) < 30)
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
                API.AddTextComponentString("Vehicle Rentals");
                API.EndTextCommandSetBlipName(blip);
            }
        }

        private async Task RentalSpotCheck()
        {
            while (true)
            {
                _menuOpen = false;
                var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
                foreach (var pos in Posistions)
                {
                    var dist = API.Vdist(playerPos.X, playerPos.Y, playerPos.Z, pos.X, pos.Y, pos.Z);
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
                        if (item == returnButton &&  API.DoesEntityExist(_rentedCar))
                        {
                            foreach (var rental in _rentalPrices)
                            {
                                if (Game.PlayerPed.CurrentVehicle.Model == API.GetHashKey(rental.Key))
                                {
                                    TriggerServerEvent("VehicleRentalReturnRequest", rental.Key);
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
                                    Utility.Instance.SendChatMessage("[Vehicle Rentals]", "You already have one out. Return it.", 255, 0, 0);
                                }
                                else
                                {
                                    TriggerServerEvent("VehicleRentalRequest", rental.Key);
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

                await Delay(0);
            }
        }
    }
}