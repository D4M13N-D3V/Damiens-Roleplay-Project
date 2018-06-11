using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;

namespace client.Main.Criminal
{

    public enum RobbableTypes { Bank, Safe, Register, Vault, Booth }

    public class RobbableSpot
    {
        public Vector3 Posistion;
        public RobbableTypes Type;
        public bool BeingRobbed = false;
        public int TimeToRob;
        public int CopsNeeded;

        public RobbableSpot(Vector3 pos, RobbableTypes type, int robtime, int cops)
        {
            Posistion = pos;
            Type = type;
            TimeToRob = robtime;
            CopsNeeded = cops;
        }

    }

    public class Robberies : BaseScript
    {
        public static Robberies Instance;

        private bool _isRobbing = false;
        private RobbableSpot _spotBeingRobbed = null;
        private Dictionary<string, RobbableSpot> _robbableSpots = new Dictionary<string, RobbableSpot>()
        {
            #region 24/7 Registers
            ["Little Seoul 24/7 Register #1"] = new RobbableSpot(
                new Vector3(706.03717041016f, -915.42755126953f, 19.215593338013f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Little Seoul 24/7 Register #2"] = new RobbableSpot(
                new Vector3(-706.0966796875f, -913.49053955078f, 19.215593338013f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Innocence Blvd 24/7 Register #1"] = new RobbableSpot(
                new Vector3(706.03717041016f, -915.42755126953f, 19.215593338013f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Innocence Blvd 24/7 Register #2"] = new RobbableSpot(
                new Vector3(24.487377166748f, -1347.4102783203f, 29.497039794922f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Mirror Park 24/7 Register #1"] = new RobbableSpot(
                new Vector3(1165.0561523438f, -324.41815185547f, 69.205062866211f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Mirror Park 24/7 Register #2"] = new RobbableSpot(
                new Vector3(1164.6981201172f, -322.61318969727f, 69.205062866211f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Downtown Vinewood 24/7 Register #1"] = new RobbableSpot(
                new Vector3(372.47518920898f, 326.35989379883f, 03.56636810303f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Downtown Vinewood 24/7 Register #2"] = new RobbableSpot(
                new Vector3(373.0817565918f, 328.75726318359f, 03.56636810303f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Rockford Drive 24/7 Register #1"] = new RobbableSpot(
                new Vector3(1818.8961181641f, 792.91729736328f, 38.08184814453f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Rockford Drive 24/7 Register #2"] = new RobbableSpot(
                new Vector3(1820.2630615234f, 794.45971679688f, 38.0887298584f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Route 68 24/7 Register #1"] = new RobbableSpot(
                new Vector3(549.36108398438f, 2669.0007324219f, 2.156490325928f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Route 68 24/7 Register #2"] = new RobbableSpot(
                new Vector3(549.05975341797f, 2671.443359375f, 156490325928f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["South Senora Fwy 24/7 Register #1"] = new RobbableSpot(
                new Vector3(2677.9641113281f, 3279.4440917969f, 5.241130828857f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["South Senora Fwy 24/7 Register #2"] = new RobbableSpot(
                new Vector3(2675.8774414063f, 3280.537109375f, 241130828857f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["North Senora Fwy 24/7 Register #1"] = new RobbableSpot(
                new Vector3(1727.8493652344f, 6415.2983398438f, 5.037227630615f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["North Senora Fwy 24/7 Register #2"] = new RobbableSpot(
                new Vector3(1728.8804931641f, 6417.4360351563f, 5.037227630615f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Great Ocean Fwy 24/7 Register #1"] = new RobbableSpot(
                new Vector3(372.47518920898f, 326.35989379883f, 03.56636810303f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Great Ocean Fwy 24/7 Register #2"] = new RobbableSpot(
                new Vector3(373.0817565918f, 328.75726318359f, 03.56636810303f),
                RobbableTypes.Register,
                90,
                2
            ),
            #endregion
            #region 24/7 Safes
            ["Little Soul 24/7 Safe"] = new RobbableSpot(
                new Vector3(709.66131591797f, -904.18121337891f, 19.215612411499f),
                RobbableTypes.Safe,
                360,
                3
            ),
            ["Innocence Blvd 24/7 Safe"] = new RobbableSpot(
                new Vector3(28.284742355347f, -1339.1623535156f, 29.497039794922f),
                RobbableTypes.Safe,
                360,
                3
            ),
            ["Mirror Park 24/7 Safe"] = new RobbableSpot(
                new Vector3(1159.55859375f, -314.06265258789f, 69.205062866211f),
                RobbableTypes.Safe,
                360,
                3
            ),
            ["Downtown Vinewood 24/7 Safe"] = new RobbableSpot(
                new Vector3(378.17330932617f, 333.39218139648f, 03.56636810303f),
                RobbableTypes.Safe,
                360,
                3
            ),
            ["Rockford Dr 24/7 Safe"] = new RobbableSpot(
                new Vector3(1829.2971191406f, 798.80505371094f, 38.19258117676f),
                RobbableTypes.Safe,
                360,
                3
            ),
            ["Route 68 24/7 Safe"] = new RobbableSpot(
                new Vector3(546.40972900391f, 2662.7551269531f, 2.156536102295f),
                RobbableTypes.Safe,
                360,
                3
            ),
            ["South Senora Fwy 24/7 Safe"] = new RobbableSpot(
                new Vector3(2672.9831542969f, 3286.49609375f, 241149902344f),
                RobbableTypes.Safe,
                360,
                3
            ),
            ["North Senora Fwy 24/7 Safe"] = new RobbableSpot(
                new Vector3(1735.1063232422f, 6420.5053710938f, 5.037227630615f),
                RobbableTypes.Safe,
                360,
                3
            ),
            ["Great Ocean Fwy 24/7 Safe"] = new RobbableSpot(
                new Vector3(378.17330932617f, 333.39218139648f, 03.56636810303f),
                RobbableTypes.Safe,
                360,
                3
            ),
            #endregion
            #region Liquior Store Registers
            ["Route 68 Liquior Store Register"] = new RobbableSpot(
                new Vector3(1165.9134521484f, 2710.7854003906f, 8.157711029053f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["El Rancho Blvd Liquior Store Register"] = new RobbableSpot(
                new Vector3(1134.2418212891f, -982.54541015625f, 46.41584777832f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Prosperity Liquior Store Register"] = new RobbableSpot(
                new Vector3(1486.2586669922f, -377.96697998047f, 40.163429260254f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Great Ocean Fwy Liquior Store Register"] = new RobbableSpot(
                new Vector3(2966.4309082031f, 390.98095703125f, 5.043313980103f),
                RobbableTypes.Register,
                90,
                2
            ),
            #endregion
            #region Liquior Store Safes
            ["Route 68 Liquior Store Safe"] = new RobbableSpot(
                new Vector3(1169.2316894531f, 2717.8447265625f, 7.157691955566f),
                RobbableTypes.Safe,
                360,
                3
            ),
            ["El Rancho Blvd Liquior Store Safe"] = new RobbableSpot(
                new Vector3(1126.8385009766f, -980.08166503906f, 45.415802001953f),
                RobbableTypes.Safe,
                360,
                3
            ),
            ["Prosperity Liquior Store Safe"] = new RobbableSpot(
                new Vector3(1479.0145263672f, -375.44979858398f, 39.1633644104f),
                RobbableTypes.Safe,
                360,
                3
            ),
            ["Great Ocean Fwy Liquior Store Safe"] = new RobbableSpot(
                new Vector3(2959.6789550781f, 387.15994262695f, 4.043292999268f),
                RobbableTypes.Safe,
                360,
                3
            ),
            #endregion
            #region Club
            ["Bahama Mamas Cash Register #1"] = new RobbableSpot(
                new Vector3(1380.1058349609f, -628.9775390625f, 0.81957244873f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Bahama Mamas Cash Register #2"] = new RobbableSpot(
                new Vector3(1376.9339599609f, -626.81805419922f, 30.81957244873f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Bahama Mamas Cash Register #3"] = new RobbableSpot(
                new Vector3(1373.8851318359f, -624.92364501953f, 30.81957244873f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Bahama Mamas Cash Register #4"] = new RobbableSpot(
                new Vector3(1390.2648925781f, -600.50628662109f, 30.319549560547f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Bahama Mamas Cash Register #5"] = new RobbableSpot(
                new Vector3(1391.0942382813f, -605.47589111328f, 30.319557189941f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Bahama Mamas Cash Register #6"] = new RobbableSpot(
                new Vector3(1387.6446533203f, -607.12426757813f, 30.340551376343f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Tequilala Register"] = new RobbableSpot(
                new Vector3(563.81359863281f, 279.33929443359f, 2.976669311523f),
                RobbableTypes.Register,
                90,
                2
            ),
            ["Tequilala Safe"] = new RobbableSpot(
                new Vector3(576.20080566406f, 291.33901977539f, 9.176681518555f),
                RobbableTypes.Safe,
                360,
                3
            ),
            #endregion
            #region Banks
            #region Booths
            ["Pacific Standard Bank Booth #1"] = new RobbableSpot(
                new Vector3(242.81385803223f, 226.59515380859f, 06.28727722168f),
                RobbableTypes.Booth,
                150,
                3
            ),
            ["Pacific Standard Bank Booth #2"] = new RobbableSpot(
                new Vector3(247.9873046875f, 224.75602722168f, 06.28736877441f),
                RobbableTypes.Booth,
                150,
                3
            ),
            ["Pacific Standard Bank Booth #3"] = new RobbableSpot(
                new Vector3(252.95489501953f, 222.85342407227f, 06.28684234619f),
                RobbableTypes.Booth,
                150,
                3
            ),
            #endregion
            ["Route 68 Flecca Bank Vault"] = new RobbableSpot(
                new Vector3(1175.8201904297f, 2711.6484375f, 38.088001251221f),
                RobbableTypes.Bank,
                900,
                4
            ),
            ["Pacific Standard Bank Vault"] = new RobbableSpot(
                new Vector3(254.30894470215f, 225.26997375488f, 101.8756942749f),
                RobbableTypes.Bank,
                900,
                4
            ),
            ["Legion Flecca Bank Vault"] = new RobbableSpot(
                new Vector3(147.43576049805f, -1044.9503173828f, 29.368032455444f),
                RobbableTypes.Bank,
                900,
                4
            ),
            ["Great Ocean Hwy Flecca Bank Vault"] = new RobbableSpot(
                new Vector3(147.43576049805f, -1044.9503173828f, 29.368032455444f),
                RobbableTypes.Bank,
                900,
                4
            ),
            ["West Hawick Flecca Bank Vault"] = new RobbableSpot(
                new Vector3(-1211.2392578125f, -335.38189697266f, 37.78101348877f),
                RobbableTypes.Bank,
                900,
                4
            ),
            ["Hawick Flecca Bank Vault"] = new RobbableSpot(
                new Vector3(311.76455688477f, -283.31527709961f, 54.16475677490f),
                RobbableTypes.Bank,
                900,
                4
            ),
            ["East Hawick Flecca Bank Vault"] = new RobbableSpot(
                new Vector3(311.76455688477f, -283.31527709961f, 54.16475677490f),
                RobbableTypes.Bank,
                900,
                4
            ),
            ["Blaine County Savings Vault"] = new RobbableSpot(
                new Vector3(-106.25449371338f, 6478.3061523438f, 31.626726150513f),
                RobbableTypes.Bank,
                900,
                4
            ),
            #endregion
        };
        public bool MenuRestricted = false;
        private bool _menuOpen = false;
        private bool _menuCreated = false;
        private UIMenu _menu;

        public Robberies()
        {
            Instance = this;
            //SetupBlips();
            //DrawMarkers();
        }

        private void SetupBlips()
        {
            foreach (var spot in _robbableSpots)
            {
                var blip = API.AddBlipForCoord(spot.Value.Posistion.X, spot.Value.Posistion.Y, spot.Value.Posistion.Z);
                API.SetBlipSprite(blip, 500);
                API.SetBlipColour(blip, 6);
                API.SetBlipAsShortRange(blip, true);
                API.BeginTextCommandSetBlipName("STRING");
                API.AddTextComponentString("Robbable Spot");
                API.EndTextCommandSetBlipName(blip);
                API.SetBlipScale(blip, 0.8f);
            }
        }

        private async Task DrawMarkers()
        {
            while (true)
            {
                foreach (var spot in _robbableSpots.Values)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(spot.Posistion, Game.PlayerPed.Position) < 30)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, spot.Posistion, Vector3.Zero, Vector3.Zero, Vector3.One, Color.FromArgb(255, 150, 0, 0));
                    }
                }
                await Delay(0);
            }
        }

        private async Task RobberyLogic()
        {
            while (true)
            {
                _menuOpen = false;
                var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
                foreach (var spot in _robbableSpots)
                {
                    var dist = API.Vdist(playerPos.X, playerPos.Y, playerPos.Z, spot.Value.Posistion.X, spot.Value.Posistion.Y, spot.Value.Posistion.Z);
                    if (dist < 3f && !MenuRestricted)
                    {
                        _menuOpen = true;
                    }
                }

                if (_menuOpen && !_menuCreated)
                {
                    _menu = InteractionMenu.Instance._interactionMenuPool.AddSubMenuOffset(
                        InteractionMenu.Instance._interactionMenu, "Robbable Store", "Access options for robbing a game.", new PointF(5, Screen.Height / 2));
                    _menu.OnItemSelect += (sender, selectedItem, index) =>
                    {
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
