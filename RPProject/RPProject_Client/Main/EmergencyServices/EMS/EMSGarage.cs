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

namespace client.Main.EmergencyServices.EMS
{
    public class EMSGarage : BaseScript
    {
        public static EMSGarage Instance;
        public List<dynamic> Vehicles;
        public List<Vector3> Posistions = new List<Vector3>()
        {
            new Vector3(-237.292f,6332.39f,32.8f),
            new Vector3(1842.64f,3667.10f,33.9f),
            new Vector3(-657.582f,293.92f,81.9f),
            new Vector3(-876.029f,-300.418f,39.9f),
            new Vector3(1169.01f,-1509.82f,34.9f),
            new Vector3(303.086f,-1439.04f,30.3f),
            new Vector3(364.135f,-591.145f,29.1f),
            new Vector3(-475.254f,-352.322f,34.8f),
        };
        public bool MenuRestricted = true;
        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private UIMenu _menu;

        private int _emsCar = -1;
        private bool _carIsOut = true;

        public EMSGarage()
        {
            Instance = this;
            SetupBlips(61, 24);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            GarageCheck();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.

            EventHandlers["UpdateEMSCars"] += new Action<List<dynamic>>(delegate (List<dynamic> list)
            {
                Vehicles = list;
            });
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            DrawMarkers();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }


        private async Task DrawMarkers()
        {
            while (true)
            {
                foreach (var pos in Posistions)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(pos, _playerPos) < 30)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, pos - new Vector3(0, 0, 0.8f), Vector3.Zero, Vector3.Zero, new Vector3(2, 2, 2), Color.FromArgb(255, 255, 255, 0));
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
                API.AddTextComponentString("EMS Garage");
                API.EndTextCommandSetBlipName(blip);
            }
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

        private async Task GarageCheck()
        {
            while (true)
            {

                _menuOpen = false;
                foreach (var pos in Posistions)
                {
                    var dist = API.Vdist(_playerPos.X, _playerPos.Y, _playerPos.Z, pos.X, pos.Y, pos.Z);
                    if (dist < 6f && !MenuRestricted)
                    {
                        _menuOpen = true;
                    }
                }

                if (_menuOpen && !_menuCreated)
                {
                    _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(
                        InteractionMenu.Instance._interactionMenu, "EMS Garage", "Pull out your EMS vehicles.", new PointF(5, Screen.Height / 2));
                    var putawayButton = new UIMenuItem("Put away car");
                    _menu.AddItem(putawayButton);

                    _menu.OnItemSelect += (sender, selectedItem, index) =>
                    {
                        if (selectedItem == putawayButton)
                        {
                            if (Game.PlayerPed.IsInVehicle() && Game.PlayerPed.CurrentVehicle.Handle == _emsCar &&
                                API.DecorExistOn(_emsCar, "PIRP_VehicleOwner") && API.DecorGetInt(_emsCar, "PIRP_VehicleOwner") == Game.Player.ServerId &&
                                _carIsOut)
                            {
                                API.DeleteVehicle(ref _emsCar);
                            }
                        }
                    };
                    var buttons = new List<UIMenuItem>();
                    foreach (var item in Vehicles)
                    {
                        var button = new UIMenuItem(item);
                        buttons.Add(button);
                        _menu.AddItem(button);
                        _menu.OnItemSelect += (sender, selectedItem, index) =>
                        {
                            if (selectedItem == button)
                            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                                Utility.Instance.SpawnCar(selectedItem.Text, delegate (int i)
                                {
                                    _carIsOut = true;
                                    _emsCar = i;
                                    API.SetVehicleNumberPlateText(i, "EMS");
                                    API.ToggleVehicleMod(i, 18, true);
                                    API.DecorSetInt(_emsCar, "PIRP_VehicleOwner", Game.Player.ServerId);
                                    API.TaskWarpPedIntoVehicle(Game.PlayerPed.Handle, i, -1);
                                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
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
