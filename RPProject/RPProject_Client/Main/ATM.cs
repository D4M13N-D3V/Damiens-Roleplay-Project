using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;
using roleplay.Main.Police;
using roleplay.Main.Vehicles;

namespace roleplay.Main
{
    public class ATM : BaseScript
    {

        public static ATM Instance;
        public List<Vector3> Posistions = new List<Vector3>()
        {
            new Vector3(-386.733f,6045.953f,31.501f),
            new Vector3(-284.037f,6224.385f,31.187f),
            new Vector3(-284.037f,6224.385f,31.187f),
            new Vector3(-135.165f,6365.738f,31.101f),
            new Vector3(-110.753f,6467.703f,31.784f),
            new Vector3(-94.9690f,6455.301f,31.784f),
            new Vector3(155.4300f,6641.991f,31.784f),
            new Vector3(174.6720f,6637.218f,31.784f),
            new Vector3(1703.138f,6426.783f,32.730f),
            new Vector3(1735.114f,6411.035f,35.164f),
            new Vector3(1702.842f,4933.593f,42.051f),
            new Vector3(1967.333f,3744.293f,32.272f),
            new Vector3(1821.917f,3683.483f,34.244f),
            new Vector3(1174.532f,2705.278f,38.027f),
            new Vector3(540.0420f,2671.007f,42.177f),
            new Vector3(2564.399f,2585.100f,38.016f),
            new Vector3(2558.683f,349.6010f,108.050f),
            new Vector3(2558.051f,389.4817f,108.660f),
            new Vector3(1077.692f,-775.796f,58.218f),
            new Vector3(1139.018f,-469.886f,66.789f),
            new Vector3(1168.975f,-457.241f,66.641f),
            new Vector3(1153.884f,-326.540f,69.245f),
            new Vector3(381.2827f,323.2518f,103.270f),
            new Vector3(236.4638f,217.4718f,106.840f),
            new Vector3(265.0043f,212.1717f,106.780f),
            new Vector3(285.2029f,143.5690f,104.970f),
            new Vector3(157.7698f,233.5450f,106.450f),
            new Vector3(-164.568f,233.5066f,94.919f),
            new Vector3(-1827.04f,785.5159f,138.020f),
            new Vector3(-1409.39f,-99.2603f,52.473f),
            new Vector3(-1205.35f,-325.579f,37.870f),
            new Vector3(-1215.64f,-332.231f,37.881f),
            new Vector3(-2072.41f,-316.959f,13.345f),
            new Vector3(-2975.72f,379.7737f,14.992f),
            new Vector3(-2962.60f,482.1914f,15.762f),
            new Vector3(-2955.70f,488.7218f,15.486f),
            new Vector3(-3044.22f,595.2429f,7.595f),
            new Vector3(-3144.13f,1127.415f,20.868f),
            new Vector3(-3241.10f,996.6881f,12.500f),
            new Vector3(-3241.11f,1009.152f,12.877f),
            new Vector3(-1305.40f,-706.240f,25.352f),
            new Vector3(-538.225f,-854.423f,29.234f),
            new Vector3(-711.156f,-818.958f,23.768f),
            new Vector3(-717.614f,-915.880f,19.268f),
            new Vector3(-526.566f,-1222.90f,18.434f),
            new Vector3(-256.831f,-719.646f,33.444f),
            new Vector3(-203.548f,-861.588f,30.205f),
            new Vector3(112.4102f,-776.162f,31.427f),
            new Vector3(112.9290f,-818.710f,31.386f),
            new Vector3(119.9000f,-883.826f,31.191f),
            new Vector3(149.4551f,-1038.95f,29.366f),
            new Vector3(-846.304f,-340.402f,38.687f),
            new Vector3(-1204.35f,-324.391f,37.877f),
            new Vector3(-1216.27f,-331.461f,37.773f),
            new Vector3(-56.1935f,-1752.53f,29.452f),
            new Vector3(-261.692f,-2012.64f,30.121f),
            new Vector3(-273.001f,-2025.60f,30.197f),
            new Vector3(314.187f,-278.621f,54.170f),
            new Vector3(-351.534f,-49.529f,49.042f),
            new Vector3(24.589f,-946.056f,29.357f),
            new Vector3(-254.112f,-692.483f,33.616f),
            new Vector3(-1570.197f,-546.651f,34.955f),
            new Vector3(-1415.909f,-211.825f,46.500f),
            new Vector3(-1430.112f,-211.014f,46.500f),
            new Vector3(33.232f,-1347.849f,29.497f),
            new Vector3(129.216f,-1292.347f,29.269f),
            new Vector3(287.645f,-1282.646f,29.659f),
            new Vector3(289.012f,-1256.545f,29.440f),
            new Vector3(295.839f,-895.640f,29.217f),
            new Vector3(1686.753f,4815.809f,42.008f),
            new Vector3(-302.408f,-829.945f,32.417f),
            new Vector3(5.134f,-919.949f,29.557f),
        };
        public bool MenuRestricted = false;
        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private UIMenu _menu;

        public ATM()
        {
            Instance = this;
            SetupBlips(277, 2);
            GarageCheck();
        }

        private void SetupBlips(int sprite, int color)
        {
            foreach (var var in Posistions)
            {
                var blip = API.AddBlipForCoord(var.X, var.Y, var.Z);
                API.SetBlipSprite(blip, sprite);
                API.SetBlipColour(blip, color);
                API.SetBlipScale(blip, 0.6f);
                API.SetBlipAsShortRange(blip, true);
                API.BeginTextCommandSetBlipName("STRING");
                API.AddTextComponentString("ATM");
                API.EndTextCommandSetBlipName(blip);
            }
        }

        private async void GarageCheck()
        {
            while (true)
            {

                _menuOpen = false;
                var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
                foreach (var pos in Posistions)
                {
                    var dist = API.Vdist(playerPos.X, playerPos.Y, playerPos.Z, pos.X, pos.Y, pos.Z);
                    if (dist < 1.25f && !MenuRestricted)
                    {
                        _menuOpen = true;
                    }
                }

                if (_menuOpen && !_menuCreated)
                {
                    _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenu(
                        InteractionMenu.Instance._interactionMenu, "ATM", "Access your bank account.");
                    var withdrawlButton = new UIMenuItem("Withdrawl");
                    _menu.AddItem(withdrawlButton);
                    _menu.OnItemSelect += (sender, selectedItem, index) =>
                    {
                        if (selectedItem == withdrawlButton)
                        {
                            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                            Utility.Instance.KeyboardInput(
                                "Amount of money to withdrawl from your bank account.", "", 10,
                                delegate (string s)
                                {
                                    TriggerServerEvent("WithdrawMoney", Convert.ToInt32(s));
                                });
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

                await Delay(0);
            }
        }
    }
}
