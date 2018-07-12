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

namespace client.Main.EmergencyServices.Police
{
    public class PoliceGarage : BaseScript
    {
        public static PoliceGarage Instance;
        public List<dynamic> Vehicles;

        public List<Vector3> Posistions = new List<Vector3>()
        {
            new Vector3(452.115966796875f, -1018.10681152344f, 28.9f),
            new Vector3(-457.88f, 6024.79f, 31.8f),
            new Vector3(1866.84f, 3697.15f, 33.9f),
            new Vector3(-1068.95f, -859.73f, 5.2f),
            new Vector3(-570.28f, -145.50f, 37.79f),
            new Vector3(-1498.3442382812f,4974.884765625f,63.255252838134f),
        };

        public bool MenuRestricted = true;
        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private UIMenu _menu;

        private int _policeCar;
        private bool _carIsOut = true;

        public PoliceGarage()
        {
            Instance = this;
            SetupBlips(60, 29);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            GarageCheck();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.

            EventHandlers["UpdatePoliceCars"] += new Action<List<dynamic>>(delegate (List<dynamic> list)
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
                    if (Utility.Instance.GetDistanceBetweenVector3s(pos, Game.PlayerPed.Position) < 30)
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
                API.AddTextComponentString("Police Garage");
                API.EndTextCommandSetBlipName(blip);
            }
        }

        private async Task GarageCheck()
        {
            while (true)
            {

                _menuOpen = false;
                foreach (var pos in Posistions)
                {
                    var dist = API.Vdist(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, pos.X, pos.Y, pos.Z);
                    if (dist < 6f && !MenuRestricted)
                    {
                        _menuOpen = true;
                    }
                }

                if (_menuOpen && !_menuCreated)
                {
                    _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(
                        InteractionMenu.Instance._interactionMenu, "Police Garage", "Pull out your police vehicles.", new PointF(5, Screen.Height / 2));
                    var putawayButton = new UIMenuItem("Put away car");
                    _menu.AddItem(putawayButton);

                    _menu.OnItemSelect += (sender, selectedItem, index) =>
                    {
                        if (selectedItem == putawayButton)
                        {
                            if (Game.PlayerPed.IsInVehicle() && Game.PlayerPed.CurrentVehicle.Handle == _policeCar &&
                               VehicleManager.Instance.OwnsCar(_policeCar) &&
                                _carIsOut)
                            {
                                API.DeleteVehicle(ref _policeCar);
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
                                    API.SetVehicleNumberPlateText(i, "POLICE");
                                    API.ToggleVehicleMod(i, 18, true);
                                    _policeCar = i;
                                    API.DecorSetInt(_policeCar, "PIRP_VehicleOwner", Game.Player.ServerId);
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
