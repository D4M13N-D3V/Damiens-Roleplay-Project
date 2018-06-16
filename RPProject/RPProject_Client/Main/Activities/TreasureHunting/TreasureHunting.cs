using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;

namespace client.Main.Activities.TreasureHunting
{
    public class TreasureHunting : BaseScript
    {
        public static TreasureHunting Instance;


        public readonly List<Vector3> GeneralLocations = new List<Vector3>()
        {
            new Vector3(-3375.7731933594f,503.45153808594f,1.9679600000381f),
            new Vector3(-3186.8557128906f,3033.4460449219f,-32.029102325439f),
            new Vector3(3198.4025878906f,-387.83627319336f,3.0627899169922f),
            new Vector3(738.86572265625f,7444.8208007813f,-169.24443054199f),
            new Vector3(-901.16687011719f,6649.1459960938f,-30.046863555908f),
            new Vector3(4191.376953125f,3641.6213378906f,-21.683450698853f),
        };

        private Vector3 _sellLocation = new Vector3(726.53100585938f, -2011.830078125f, 29.29203414917f);

        private List<TreasureLocation> _locations = new List<TreasureLocation>()
        {
            new TreasureLocation(new Vector3(-3353.6511230469f,509.04403686523f,-24.632040023804f), new List<string>(){"Jewlery","Antique Gun","Dinosaur Bone","Sea Monster Bone","Piece of metal"}),
            new TreasureLocation(new Vector3(-3370.3835449219f,527.53869628906f,-26.632040023804f), new List<string>(){"Jewlery","Antique Gun","Dinosaur Bone","Sea Monster Bone","Piece of metal"}),
            new TreasureLocation(new Vector3(-3385.6926269531f,517.6806640625f,-26.032039642334f), new List<string>(){"Jewlery","Antique Gun","Dinosaur Bone","Sea Monster Bone","Piece of metal"}),
            new TreasureLocation(new Vector3(-3421.6557617188f,513.7734375f,-29.232040405273f), new List<string>(){"Jewlery","Antique Gun","Dinosaur Bone","Sea Monster Bone","Piece of metal"}),
            new TreasureLocation(new Vector3(-3411.0744628906f,494.05322265625f,-31.732040405273f), new List<string>(){"Jewlery","Antique Gun","Dinosaur Bone","Sea Monster Bone","Piece of metal"}),
            new TreasureLocation(new Vector3(-3381.3666992188f,462.15539550781f,-28.132040023804f), new List<string>(){"Jewlery","Antique Gun","Dinosaur Bone","Sea Monster Bone","Piece of metal"}),
            new TreasureLocation(new Vector3(-3359.3078613281f,493.94512939453f,-25.132040023804f), new List<string>(){"Jewlery","Antique Gun","Dinosaur Bone","Sea Monster Bone","Piece of metal"}),

            new TreasureLocation(new Vector3(-3162.4340820313f,2997.1599121094f,-37.681610107422f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3174.2780761719f,2999.13671875f,-38.647926330566f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3177.1328125f,3010.5100097656f,-38.992805480957f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3172.2866210938f,3030.1535644531f,-34.99157333374f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3175.0146484375f,3040.1687011719f,-36.175594329834f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3181.4809570313f,3034.4848632813f,-36.761028289795f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3173.9438476563f,3025.9587402344f,-35.926536560059f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3181.2646484375f,3053.3110351563f,-39.06164932251f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3199.0578613281f,3054.3278808594f,-41.308925628662f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),
            new TreasureLocation(new Vector3(-3186.8557128906f,3033.4460449219f,-32.029102325439f), new List<string>(){"Pearl","Scrap Metal","Jewlery","Antique Gun","Watch","Boat Part"}),

            new TreasureLocation(new Vector3(3097.5026855469f,-251.03153991699f,-5.5886726379395f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3142.525390625f,-260.9123840332f,-20.188673019409f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3149.7543945313f,-265.08602905273f,-13.888672828674f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3145.8234863281f,-270.76745605469f,-11.188673019409f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3146.0908203125f,-273.75552368164f,-8.8886728286743f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3147.2492675781f,-283.47860717773f,-7.8886728286743f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3163.5620117188f,-297.10464477539f,-6.188672542572f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3167.5717773438f,-309.25122070313f,-12.91489315033f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3168.3293457031f,-321.69174194336f,-12.915464401245f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3167.220703125f,-320.30953979492f,-13.140027999878f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3166.7841796875f,-306.86361694336f,-12.858468055725f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3167.3811035156f,-330.27307128906f,-24.881834030151f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3180.5048828125f,-336.29653930664f,-27.882518768311f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3185.2727050781f,-357.76507568359f,-29.902994155884f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3192.4794921875f,-372.09683227539f,-29.766319274902f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3186.0991210938f,-382.15493774414f,-24.865468978882f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3193.4614257813f,-381.41271972656f,-17.47966003418f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3199.787109375f,-390.66363525391f,-22.823389053345f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3202.025390625f,-404.08636474609f,-25.16423034668f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),
            new TreasureLocation(new Vector3(3212.4555664063f,-410.81307983398f,-32.668796539307f), new List<string>(){"Car Part","Game Console","Jewlery", "Pearl", "Firearm", "Boat Part", "Sealed Electronics", "Hard Drive", "Ivory"}),

            new TreasureLocation(new Vector3(774.88745117188f,7405.6259765625f,-118.8558807373f), new List<string>(){"Plane Part", "Pearls", "Jewlery", "Hard Drive", "Firearm"}),
            new TreasureLocation(new Vector3(746.65539550781f,7409.609375f,-121.43287658691f), new List<string>(){"Plane Part", "Pearls", "Jewlery", "Hard Drive", "Firearm"}),
            new TreasureLocation(new Vector3(755.68218994141f,7385.625f,-114.78730010986f), new List<string>(){"Plane Part", "Pearls", "Jewlery", "Hard Drive", "Firearm"}),
            new TreasureLocation(new Vector3(801.1748046875f,7361.455078125f,-130.07354736328f), new List<string>(){"Plane Part", "Pearls", "Jewlery", "Hard Drive", "Firearm"}),
            new TreasureLocation(new Vector3(738.86572265625f,7444.8208007813f,-169.24443054199f), new List<string>(){"Plane Part", "Pearls", "Jewlery", "Hard Drive", "Firearm"}),

            new TreasureLocation(new Vector3(-931.36468505859f,6599.671875f,-29.363540649414f), new List<string>(){"Firearm","Pearls","Alien Bone","Alien Gun","Alien Ship Part"}),
            new TreasureLocation(new Vector3(-920.67474365234f,6623.7978515625f,-30.169900894165f), new List<string>(){"Firearm","Pearls","Alien Bone","Alien Gun","Alien Ship Part"}),
            new TreasureLocation(new Vector3(-925.27722167969f,6644.9301757813f,-29.637880325317f), new List<string>(){"Firearm","Pearls","Alien Bone","Alien Gun","Alien Ship Part"}),
            new TreasureLocation(new Vector3(-913.81701660156f,6664.2319335938f,-30.845436096191f), new List<string>(){"Firearm","Pearls","Alien Bone","Alien Gun","Alien Ship Part"}),
            new TreasureLocation(new Vector3(-901.16687011719f,6649.1459960938f,-30.046863555908f), new List<string>(){"Firearm","Pearls","Alien Bone","Alien Gun","Alien Ship Part"}),
            new TreasureLocation(new Vector3(-854.17895507813f,6649.0415039063f,-21.403295516968f), new List<string>(){"Firearm","Pearls","Alien Bone","Alien Gun","Alien Ship Part"}),
            new TreasureLocation(new Vector3(-845.91870117188f,6666.9243164063f,-26.820055007935f), new List<string>(){"Firearm","Pearls","Alien Bone","Alien Gun","Alien Ship Part"}),
            new TreasureLocation(new Vector3(-811.60137939453f,6665.45703125f,-13.750008583069f), new List<string>(){"Firearm","Pearls","Alien Bone","Alien Gun","Alien Ship Part"}),

            new TreasureLocation(new Vector3(4185.3559570313f,3576.5087890625f,-45.745246887207f), new List<string>(){"Antique Firearm", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearls"}),
            new TreasureLocation(new Vector3(4198.3979492188f,3601.5886230469f,-44.570709228516f), new List<string>(){"Antique Firearm", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearls"}),
            new TreasureLocation(new Vector3(4219.9985351563f,3606.5180664063f,-46.959362030029f), new List<string>(){"Antique Firearm", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearls"}),
            new TreasureLocation(new Vector3(4237.8662109375f,3594.8830566406f,-46.260566711426f), new List<string>(){"Antique Firearm", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearls"}),
            new TreasureLocation(new Vector3(4241.5927734375f,3588.9301757813f,-46.579597473145f), new List<string>(){"Antique Firearm", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearls"}),
            new TreasureLocation(new Vector3(4241.4741210938f,3610.6987304688f,-48.466213226318f), new List<string>(){"Antique Firearm", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearls"}),
            new TreasureLocation(new Vector3(4242.2919921875f,3617.5881347656f,-45.866725921631f), new List<string>(){"Antique Firearm", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearls"}),
            new TreasureLocation(new Vector3(4202.0517578125f,3643.4736328125f,-39.013248443604f), new List<string>(){"Antique Firearm", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearls"}),
            new TreasureLocation(new Vector3(4213.6840820313f,3647.2878417969f,-41.811008453369f), new List<string>(){"Antique Firearm", "Tank Part", "Antique War Clothing", "Mortar Shell", "Jewlery", "Pearls"}),
        };


        private bool _buttonOpen = false;
        private bool _buttonCreated = false;
        private bool _atSellPoint = false;
        private UIMenuItem _button;
        private TreasureLocation _nearbyLocation;

        public TreasureHunting()
        {
            Instance = this;
            SetupBlips();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            DrawMarkers();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            GetPlayerPosEverySecond();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            LootLogic();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.

            ButtonLogic();

            InteractionMenu.Instance._interactionMenu.OnItemSelect += async (sender, item, index) =>
            {
                if (item == _button)
                {
                    if (_atSellPoint)
                    {
                        InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                        TriggerServerEvent("SellAllTreasureLoot");
                    }
                    else
                    {
                        InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                        _nearbyLocation.CanLoot = false;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                        ResetLootability(_nearbyLocation);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                        async Task ResetLootability(TreasureLocation loc)
                        {
                            await Delay(60000);
                            loc.CanLoot = true;
                        }
                        Game.PlayerPed.Task.PlayAnimation("mini@repair", "fixing_a_player");
                        await Delay(1750);
                        Game.PlayerPed.Task.ClearAll();
                        TriggerServerEvent("TreasureHuntingLooted", _locations.IndexOf(_nearbyLocation));
                    }
                }
            };
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

        private void SetupBlips()
        {
            var sellBlip = API.AddBlipForCoord(_sellLocation.X, _sellLocation.Y, _sellLocation.Z);
            API.SetBlipSprite(sellBlip, 500);
            API.SetBlipColour(sellBlip, 4);
            API.SetBlipScale(sellBlip, 1f);
            API.SetBlipAsShortRange(sellBlip, true);
            API.BeginTextCommandSetBlipName("STRING");
            API.AddTextComponentString("Sell Diving Loot Here");
            API.EndTextCommandSetBlipName(sellBlip);
            foreach (var var in GeneralLocations)
            {
                var blip = API.AddBlipForCoord(var.X, var.Y, var.Z);
                API.SetBlipSprite(blip, 404);
                API.SetBlipColour(blip, 4);
                API.SetBlipScale(blip, 1f);
                API.SetBlipAsShortRange(blip, true);
                API.BeginTextCommandSetBlipName("STRING");
                API.AddTextComponentString("Diving Spot");
                API.EndTextCommandSetBlipName(blip);
            }
        }

        private async Task DrawMarkers()
        {
            while (true)
            {
                if (Utility.Instance.GetDistanceBetweenVector3s(_sellLocation, _playerPos) < 30)
                {
                    World.DrawMarker(MarkerType.HorizontalCircleSkinny, _sellLocation - new Vector3(0, 0, 0.5f), Vector3.Zero, Vector3.Zero, new Vector3(4, 4, 4), Color.FromArgb(255, 0, 175, 0));
                }
                foreach (var pos in _locations)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(pos.Posistion, _playerPos) < 30 && pos.CanLoot)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, pos.Posistion - new Vector3(0, 0, 0.5f), Vector3.Zero, Vector3.Zero, new Vector3(2, 2, 2), Color.FromArgb(255, 0, 0, 150));
                        World.DrawMarker(MarkerType.UpsideDownCone, pos.Posistion, Vector3.Zero, Vector3.Zero, new Vector3(1, 1, 1), Color.FromArgb(255, 0, 0, 150));
                    }
                }
                await Delay(0);
            }
        }

        private async Task ButtonLogic()
        {
            while (true)
            {
                if (_nearbyLocation != null && _nearbyLocation.CanLoot &&
                    Utility.Instance.GetDistanceBetweenVector3s(_nearbyLocation.Posistion, _playerPos) < 4)
                {
                    Utility.Instance.DrawTxt(0.45f,0.5f,0,0,1f,"Press E To Loot!",0,185,0,255,true);
                    if (Game.IsControlJustPressed(0, Control.Context)){
                        InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                        _nearbyLocation.CanLoot = false;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                        ResetLootability(_nearbyLocation);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                        async Task ResetLootability(TreasureLocation loc)
                        {
                            await Delay(60000);
                            loc.CanLoot = true;
                        }
                        Game.PlayerPed.Task.PlayAnimation("mini@repair", "fixing_a_player");
                        await Delay(1750);
                        Game.PlayerPed.Task.ClearAll();
                        TriggerServerEvent("TreasureHuntingLooted", _locations.IndexOf(_nearbyLocation));
                    }
                }

                await Delay(0);
            }
        }

        private async Task LootLogic()
        {
            while (true)
            {
                _buttonOpen = false;
                float dist;
                foreach (var location in _locations)
                {
                    dist = API.Vdist(_playerPos.X, _playerPos.Y, _playerPos.Z, location.Posistion.X, location.Posistion.Y, location.Posistion.Z);
                    if (dist < 4f && location.CanLoot)
                    {
                        _nearbyLocation = location;
                        _atSellPoint = false;
                        _buttonOpen = true;
                    }
                }

                dist = API.Vdist(_playerPos.X, _playerPos.Y, _playerPos.Z, _sellLocation.X, _sellLocation.Y, _sellLocation.Z);
                if (dist < 10f)
                {
                    _atSellPoint = true;
                    _buttonOpen = true;
                }

                if (_buttonOpen && !_buttonCreated)
                {
                    if (_atSellPoint)
                    {
                        _button = new UIMenuItem("Sell All Your Loot!");
                        InteractionMenu.Instance._interactionMenu.AddItem(_button);
                        _buttonCreated = true;
                        InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                    }
                    else
                    {
                        _button = new UIMenuItem("Loot Treasure");
                        InteractionMenu.Instance._interactionMenu.AddItem(_button);
                        _buttonCreated = true;
                        InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                    }
                }
                else if (!_buttonOpen && _buttonCreated)
                {
                    _buttonCreated = false;

                    var i = 0;
                    foreach (var item in InteractionMenu.Instance._interactionMenu.MenuItems)
                    {
                        if (item == _button)
                        {
                            InteractionMenu.Instance._interactionMenu.RemoveItemAt(i);
                            break;
                        }

                        i++;
                    }
                    if (InteractionMenu.Instance._interactionMenu.Visible)
                    {
                        InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
                        InteractionMenu.Instance._interactionMenu.Visible = true;
                    }

                    InteractionMenu.Instance._interactionMenuPool.RefreshIndex();
                }

                await Delay(1000);
            }
        }

    }
}
