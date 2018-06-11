using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;
using client.Users.Inventory;
using client.Main.Items;
using client.Main.Vehicles;

namespace client.Main.EmergencyServices
{
    public enum LEODepartments
    {
        LSPD,
        BCSO,
        LSCSO,
        SASP,
        SAHP,
        SAAO,
        USMS,
        FBI,
        DEA
    }

    public class PoliceUniformComponent
    {
        public int Component;
        public int Drawable;
        public int Texture;
        public int Pallet;

        public PoliceUniformComponent(int comp, int draw, int text, int pallet)
        {
            Component = comp;
            Drawable = draw;
            Texture = text;
            Pallet = pallet;
        }
    }

    public class Police : BaseScript
    {

        private readonly Dictionary<string, List<PoliceUniformComponent>> _maleUniforms =
            new Dictionary<string, List<PoliceUniformComponent>>()
            {
                ["SASP"] = new List<PoliceUniformComponent>()
                {
                    new PoliceUniformComponent(3, 0, 0, 0),
                    new PoliceUniformComponent(4, 10, 1, 0),
                    new PoliceUniformComponent(5, 0, 0, 0),
                    new PoliceUniformComponent(6, 24, 0, 0),
                    new PoliceUniformComponent(7, 0, 0, 0),
                    new PoliceUniformComponent(8, 122, 0, 0),
                    new PoliceUniformComponent(11, 55, 0, 0),
                }
            };

        private readonly Dictionary<string, List<PoliceUniformComponent>> _femaleUniforms =
            new Dictionary<string, List<PoliceUniformComponent>>()
            {
                ["SASP"] = new List<PoliceUniformComponent>()
                {
                    new PoliceUniformComponent(3, 14, 0, 0),
                    new PoliceUniformComponent(4, 64, 3, 0),
                    new PoliceUniformComponent(5, 3, 0, 0),
                    new PoliceUniformComponent(6, 52, 0, 0),
                    new PoliceUniformComponent(7, 0, 0, 0),
                    new PoliceUniformComponent(8, 152, 0, 0),
                    new PoliceUniformComponent(11, 48, 0, 0),
                }
            };

        public int CopCount = 0;
        public bool IsOnDuty = false;
        private string _rankName = "";
        private string _department = "";

        public static Police Instance;

        public Police()
        {
            Instance = this;
            StopDispatch();
            SetupItems();
            EventHandlers["Police:SetOnDuty"] += new Action<dynamic>(OnDuty);
            EventHandlers["Police:SetOffDuty"] += new Action(OffDuty);
            EventHandlers["Police:RefreshOnDutyOfficers"] += new Action<dynamic>(RefreshCops);
        }

        private async Task StopDispatch()
        {
            while (true)
            {
                for (int i = 0; i < 12; i++)
                {
                    API.EnableDispatchService(i, false);
                }

                API.SetPlayerWantedLevel(Game.Player.Handle, 0, false);
                API.SetPlayerWantedLevelNow(Game.Player.Handle, false);
                API.SetPlayerWantedLevelNoDrop(Game.Player.Handle, 0, false);

                await Delay(0);
            }
        }

        private void RefreshCops(dynamic copCount)
        {
            CopCount = copCount;
        }

        private void OnDuty(dynamic data)
        {
            var department = Convert.ToString(data);
            _department = department;
            Utility.Instance.SendChatMessage("[Police]", "You have gone on duty.", 0, 0, 255);
            IsOnDuty = true;
            PoliceGear.Instance.SetRestricted(false);
            PoliceGarage.Instance.MenuRestricted = false;
            GiveUniform();
            TriggerEvent("setAsCopForDoors");
        }

        private void OffDuty()
        {
            Utility.Instance.SendChatMessage("[Police]", "You have gone off duty.", 0, 0, 255);
            _rankName = "";
            IsOnDuty = false;
            PoliceGear.Instance.SetRestricted(true);
            PoliceGarage.Instance.MenuRestricted = true;
            _department = "";
            TakeUniform();
        }

        private void GiveUniform()
        {
            if (_department == "USMS" || _department == "")
            {
                return;
            }

            if (API.GetEntityModel(Game.PlayerPed.Handle) == API.GetHashKey("mp_m_freemode_01"))
            {
                foreach (var uniformParts in _maleUniforms[_department])
                {
                    API.SetPedComponentVariation(Game.PlayerPed.Handle, uniformParts.Component, uniformParts.Drawable,
                        uniformParts.Texture, uniformParts.Pallet);
                }
            }
            else if (API.GetEntityModel(Game.PlayerPed.Handle) == API.GetHashKey("mp_f_freemode_01"))
            {
                foreach (var uniformParts in _femaleUniforms[_department])
                {
                    API.SetPedComponentVariation(Game.PlayerPed.Handle, uniformParts.Component, uniformParts.Drawable,
                        uniformParts.Texture, uniformParts.Pallet);
                }
            }
        }

        private void TakeUniform()
        {
            TriggerServerEvent("RefreshClothes");
        }

        private async Task SetupItems()
        {
            while (InventoryProcessing.Instance == null)
            {
                await Delay(0);
            }

            InventoryProcessing.Instance.AddItemUse("Police Lock Tool(P)", PoliceLockTool);
            InventoryProcessing.Instance.AddItemUse("Fingerprint Scanner(P)", FingerprintScanner);
            InventoryProcessing.Instance.AddItemUse("GSR Kit(P)", GSRKit);
        }

        public void GSRKit()
        {
            Utility.Instance.GetClosestPlayer(out var output);
            if (output.Dist < 4)
            {
                if (API.DecorGetBool(output.Ped, "GSR_Active"))
                {
                    TriggerServerEvent("ActionCommandFromClient", "Swabs the persons hand and puts it in a capsule, shaking it. The liquid in the capsule turns red indicating that they had fired a firearm recently.");
                }
                else
                {
                    TriggerServerEvent("ActionCommandFromClient", "Swabs the persons hand and puts it in a capsule, shaking it. The liquid in the capsule turns blue indicating that they havent fired a firearm recently.");
                }
            }
        }

        public async void PoliceLockTool()
        {
            var playerPos = API.GetEntityCoords(Game.PlayerPed.Handle, true);
            var vehicle = Utility.Instance.ClosestVehicle.Handle;
            if (Utility.Instance.GetDistanceBetweenVector3s(playerPos, API.GetEntityCoords(vehicle, false)) > 4)
            {
                Utility.Instance.SendChatMessage("[Police Lock Tool]", "You are too far away from a vehicle.", 255, 0,
                    0);
                return;
            }

            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            Game.PlayerPed.Task.PlayAnimation("misscarstealfinalecar_5_ig_3", "crouchloop");
            var lockPicking = true;

            async Task CancelLockpick()
            {
                while (lockPicking)
                {
                    if (API.IsControlJustPressed(0, (int) Control.PhoneCancel))
                    {
                        lockPicking = false;
                        API.ClearPedTasks(Game.PlayerPed.Handle);
                    }

                    await Delay(0);
                }
            }

            CancelLockpick();
            Utility.Instance.SendChatMessage("[Police Lock Tool]", "You start using your tool to unlock the vehicle.",
                255, 0, 0);
            await Delay(15000);
            lockPicking = false;
            Game.PlayerPed.Task.ClearAll();
            API.SetVehicleDoorsLocked(vehicle, 0);
            Utility.Instance.SendChatMessage("[Police Lock Tool]",
                "You successfully unlock the vehicle with your tool!", 255, 0, 0);
        }

        public async void FingerprintScanner()
        {
            ClosestPlayerReturnInfo output;
            Utility.Instance.GetClosestPlayer(out output);
            if (output.Dist < 4)
            {
                Game.PlayerPed.Task.PlayAnimation("mp_arresting", "a_uncuff");
                await Delay(3000);
                Game.PlayerPed.Task.ClearAll();
                TriggerServerEvent("FingerPrintScannerRequest", API.GetPlayerServerId(output.Pid));
            }
            else
            {
                Utility.Instance.SendChatMessage("[Fingeprint Scanner]", "You are not close enough to a player!", 255,
                    0, 0);
            }
        }
    }

    public class PoliceGear : BaseStore
    {
        public static PoliceGear Instance;
        public PoliceGear() : base("Police Station", "Pick up your police gear here.", 60, 29,
            new List<Vector3>()
            {
                new Vector3(452.09893798828f, -979.99584960938f, 30.689596176147f),
                new Vector3(457.956909179688f, -992.72314453125f, 30.9f),
                new Vector3(-449.90f, 6016.22f, 31.72f),
                new Vector3(1857.01f, 3689.50f, 34.27f),
                new Vector3(-1113.65f, -849.21f, 13.8f),
                new Vector3(-561.28f, -132.60f, 38.04f),
                new Vector3(118.89972686768f, -731.19207763672f, 242.1519317627f)
            },
            new Dictionary<string, int>()
            {
                ["Bandages(P)"] = 0,
                ["Riot Shield(P)"] = 0,
                ["Body Armor(P)"] = 0,
                ["Flares(P)"] = 0,
                ["Flashlight"] = 25,
                ["Binoculars(P)"] = 0,
                ["Scuba Gear(P)"] = 0,
                ["Medical Supplies(P)"] = 0,
                ["Pain Killers(P)"] = 0,
                ["First Aid Kit(P)"] = 0,
                ["Handcuffs(P)"] = 0,
                ["Hobblecuffs(P)"] = 0,
                ["Tazer(P)"] = 0,
                ["Nighstick(P)"] = 0,
                ["Binoculars(P)"] = 0,
                ["Police Lock Tool(P)"] = 0,
                ["Combat Pistol"] = 2500,
                ["Pump Shotgun"] = 1200,
                ["Carbine Rifle(P)"] = 0,
                ["Fingerprint Scanner(P)"] = 0,
                ["GSR Kit(P)"] = 0,
                ["Spike Strips(P)"] = 0,
                ["Pistol Ammo"] = 10,
                ["Shotgun Ammo"] = 10,
                ["Rifle Ammo"] = 10
            })
        {
            Instance = this;
            MenuRestricted = true;
        }
    }

    public class RiotShield : BaseScript
    {
        public static RiotShield Instance;

        public bool ShieldActive = false;
        public int ShieldEntity = 0;
        public bool HadPistol = false;

        public string AnimDict = "combat@gestures@gang@pistol_1h@beckon";
        public string AnimName = "0";

        public string Prop = "prop_ballistic_shield";
        public int Pistol = API.GetHashKey("WEAPON_PISTOL");

        public RiotShield()
        {
            Instance = this;
            ShieldLogic();
            EventHandlers["RiotShield"] += new Action(() =>
            {
                if (ShieldActive)
                {
                    DisableShield();
                }
                else
                {
                    if (InventoryUI.Instance.HasItem("Riot Shield(P)") > 0)
                    {
                        EnableShield();
                    }
                }
            });
        }

        private async Task ShieldLogic()
        {
            while (true)
            {
                if (ShieldActive)
                {
                    if (!API.IsEntityPlayingAnim(Game.PlayerPed.Handle, AnimDict, AnimName, 1))
                    {
                        API.RequestAnimDict(AnimDict);
                        while (!API.HasAnimDictLoaded(AnimDict))
                        {
                            await Delay(100);
                        }

                        API.TaskPlayAnim(Game.PlayerPed.Handle, AnimDict, AnimName, 8.0f, 8.0f, -1, (2 + 16 + 32), 0.0f,
                            false, false, false);
                    }
                }

                await Delay(500);
            }
        }

        private async Task EnableShield()
        {
            API.RequestAnimDict(AnimDict);
            while (!API.HasAnimDictLoaded(AnimDict))
            {
                await Delay(100);
            }

            API.TaskPlayAnim(Game.PlayerPed.Handle, AnimDict, AnimName, 8.0f, 8.0f, -1, (2 + 16 + 32), 0.0f, false,
                false, false);

            API.RequestModel((uint) API.GetHashKey(Prop));
            while (!API.HasModelLoaded((uint) API.GetHashKey(Prop)))
            {
                await Delay(100);
            }

            ShieldEntity = API.CreateObject(API.GetHashKey(Prop), Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y,
                Game.PlayerPed.Position.Z, true, true, true);
            API.AttachEntityToEntity(ShieldEntity, Game.PlayerPed.Handle,
                API.GetEntityBoneIndexByName(Game.PlayerPed.Handle, "IK_L_HAND"),
                0.0f, -0.05f, -0.10f, -30.0f, 180.0f, 40.0f, false, false, true, false, 0, true);

            if (API.HasPedGotWeapon(Game.PlayerPed.Handle, (uint) Pistol, false) ||
                API.GetSelectedPedWeapon(Game.PlayerPed.Handle) == Pistol)
            {
                API.SetCurrentPedWeapon(Game.PlayerPed.Handle, (uint) Pistol, true);
                HadPistol = true;
            }
            else
            {
                API.GiveWeaponToPed(Game.PlayerPed.Handle, (uint) Pistol, 300, false, true);
                API.SetCurrentPedWeapon(Game.PlayerPed.Handle, (uint) Pistol, true);
                HadPistol = false;
            }

            ShieldActive = true;
            API.SetEnableHandcuffs(Game.PlayerPed.Handle, true);
        }

        private void DisableShield()
        {
            API.DeleteEntity(ref ShieldEntity);
            Game.PlayerPed.Task.ClearAll();
            if (!HadPistol)
            {
                API.RemoveWeaponFromPed(Game.PlayerPed.Handle, (uint) Pistol);
            }

            API.SetEnableHandcuffs(Game.PlayerPed.Handle, false);
            HadPistol = false;
            ShieldActive = false;
        }


    }

    public class SpikeStripsScript : BaseScript
    {
        public readonly List<SpikeStrip> SpikesStrips = new List<SpikeStrip>();
        public readonly List<Vehicle> PlayersVehicles = new List<Vehicle>();
        private DateTime lastPlayersVehiclesUpdateTime = DateTime.UtcNow;

        public SpikeStripsScript()
        {
            Tick += OnTick;
        }

        private bool CanDeploySpikeStrips
        {
            get
            {
                Ped playerPed = LocalPlayer.Character;
                return !playerPed.IsDead && !playerPed.IsInVehicle() && !playerPed.IsInCover() &&
                       !playerPed.IsJumping &&
                       !playerPed.IsAiming && !playerPed.IsClimbing &&
                       InventoryUI.Instance.HasItem("Spike Strips(P)") > 0;
            }
        }

        private bool CanRemoveSpikeStrips
        {
            get
            {
                Ped playerPed = LocalPlayer.Character;
                return !playerPed.IsDead && !playerPed.IsInVehicle() && !playerPed.IsInCover() &&
                       !playerPed.IsJumping && !playerPed.IsAiming && !playerPed.IsClimbing &&
                       InventoryUI.Instance.HasItem("Spike Strips(P)") > 0;
            }
        }

        private async Task OnTick()
        {
            if (Game.IsControlPressed(0, Control.Context))
            {
                if (Game.IsControlJustPressed(0, Control.SelectWeaponMelee))
                {
                    if (CanDeploySpikeStrips)
                        CreateSpikeStrips();
                }
                else if (Game.IsControlJustPressed(0, Control.SelectWeaponUnarmed))
                {
                    if (CanDeploySpikeStrips)
                        CreateSSpikeStrips();
                }
                else if (Game.IsControlJustPressed(0, Control.SelectWeaponShotgun))
                {
                    if (CanRemoveSpikeStrips)
                        DeleteAllSpikeStrips();
                }
            }

            if (SpikesStrips.Count > 0 && (DateTime.UtcNow - lastPlayersVehiclesUpdateTime).TotalSeconds > 1.0)
            {
                PlayersVehicles.Clear();
                foreach (Player player in Players)
                {
                    Ped ped = player.Character;
                    if (ped.IsInVehicle())
                    {
                        PlayersVehicles.Add(ped.CurrentVehicle);
                    }
                }

                lastPlayersVehiclesUpdateTime = DateTime.UtcNow;
            }

            for (int i = 0; i < SpikesStrips.Count; i++)
            {
                SpikesStrips[i].OnTick(PlayersVehicles);
            }

            await Task.FromResult(0);
        }

        private async Task CreateSSpikeStrips()
        {
            Ped playerPed = LocalPlayer.Character;
            PlayKneelAnim(2000);
            Vector3 playerPos = playerPed.Position;
            Vector3 playerForwardVector = playerPed.ForwardVector;
            float playerHeading = playerPed.Heading;
            const int SpikeStripsToSpawn = 2;
            for (int i = 0; i < SpikeStripsToSpawn; i++)
            {
                const float SeparationFromPlayer = 2f;
                const float SeparationBetweenStingers = 3.7f;
                SpikeStrip s = await SpikeStrip.Create(
                    playerPos + playerForwardVector * (SeparationFromPlayer + (SeparationBetweenStingers * i)),
                    playerHeading);
                SpikesStrips.Add(s);
                await Delay(500);
            }

            Screen.ShowNotification("~g~Deployed 1 spike strip!", true);
        }

        private async Task CreateSpikeStrips()
        {
            Ped playerPed = LocalPlayer.Character;
            PlayKneelAnim(4000);
            Vector3 playerPos = playerPed.Position;
            Vector3 playerForwardVector = playerPed.ForwardVector;
            float playerHeading = playerPed.Heading;
            const int SpikeStripsToSpawn = 4;
            for (int i = 0; i < SpikeStripsToSpawn; i++)
            {
                const float SeparationFromPlayer = 2f;
                const float SeparationBetweenStingers = 3.7f;

                SpikeStrip s = await SpikeStrip.Create(
                    playerPos + playerForwardVector * (SeparationFromPlayer + (SeparationBetweenStingers * i)),
                    playerHeading);
                SpikesStrips.Add(s);
                await Delay(500);
            }

            Screen.ShowNotification("~g~Deployed 2 spike strips!", true);
        }

        private async Task DeleteAllSpikeStrips()
        {
            if (SpikesStrips.Count != 0)
            {
                PlayKneelAnim(SpikesStrips.Count * 500);
                for (int i = SpikesStrips.Count; i-- > 0;)
                {
                    SpikesStrips[i].Prop.Delete();
                    SpikesStrips.Remove(SpikesStrips[i]);
                    await Delay(500);
                }

                SpikesStrips.Clear();
                Screen.ShowNotification("~r~Removed spike strips!", true);
            }
            else
                Screen.ShowNotification("~r~No spike strips deployed!", true);
        }

        public static void PlayKneelAnim(int duration)
        {
            if (!API.DoesEntityExist(API.GetPlayerPed(API.PlayerId())))
                return;
            if (API.IsPedArmed(API.GetPlayerPed(API.PlayerId()), 7))
                API.SetCurrentPedWeapon(API.GetPlayerPed(API.PlayerId()), 0xA2719263, true);
            AnimationFlags flags = AnimationFlags.None;
            Game.PlayerPed.Task.PlayAnimation("amb@medic@standing@kneel@idle_a", "idle_a", 10, duration, flags);
        }

        public class SpikeStrip
        {
            private static readonly Model Model = "p_ld_stinger_s";
            public Prop Prop { get; private set; }

            private SpikeStrip()
            {
            }

            [System.Security.SecuritySafeCritical]
            public void OnTick(List<Vehicle> playerVehicles = null)
            {
                if (Entity.Exists(Prop))
                {
                    Vector3 pointA = Prop.GetOffsetPosition(new Vector3(0.0f, 1.7825f, 0.034225f));
                    Vector3 pointB = Prop.GetOffsetPosition(new Vector3(0.0f, -1.7825f, 0.034225f));
                    Vector3 propPos = Prop.Position;
                    if (playerVehicles != null)
                    {
                        for (int i = 0; i < playerVehicles.Count; i++)
                        {
                            Vehicle veh = playerVehicles[i];
                            if (Vector3.DistanceSquared(veh.Position, propPos) < 7.25f * 7.25f)
                            {
                                BurstVehicleTyres(veh, pointA, pointB);
                            }
                        }
                    }

                    Vehicle closestVeh =
                        new Vehicle(API.GetClosestVehicle(propPos.X, propPos.Y, propPos.Z, 10.0f, 0, 70));
                    BurstVehicleTyres(closestVeh, pointA, pointB);
                }
            }

            private void BurstVehicleTyres(Vehicle v, Vector3 pointA, Vector3 pointB)
            {
                if (Entity.Exists(v))
                {
                    OutputArgument outModelMin = new OutputArgument(), outModelMax = new OutputArgument();
                    Function.Call(Hash.GET_MODEL_DIMENSIONS, v.Model.Hash, outModelMin, outModelMax);
                    Vector3 modelMin = outModelMin.GetResult<Vector3>();
                    BurstTyre(v, "wheel_lf", 0, modelMin, pointA, pointB);
                    BurstTyre(v, "wheel_rf", 1, modelMin, pointA, pointB);
                    BurstTyre(v, "wheel_lm1", 2, modelMin, pointA, pointB);
                    BurstTyre(v, "wheel_rm1", 3, modelMin, pointA, pointB);
                    BurstTyre(v, "wheel_lr", 4, modelMin, pointA, pointB);
                    BurstTyre(v, "wheel_rr", 5, modelMin, pointA, pointB);
                }
            }

            private bool CanBurstTyre(Vehicle veh, string wheelBone, int wheelIndex)
            {
                return Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, veh.Handle, wheelBone) != -1 &&
                       !Function.Call<bool>(Hash.IS_VEHICLE_TYRE_BURST, veh.Handle, wheelIndex, false);
            }

            private void BurstTyre(Vehicle veh, string wheelBone, int wheelIndex, Vector3 vehModelMinDim,
                Vector3 pointA, Vector3 pointB)
            {
                if (CanBurstTyre(veh, wheelBone, wheelIndex))
                {
                    Vector3 wheelPos = Vector3.Zero;
                    wheelPos = veh.Bones[wheelBone].Position;
                    wheelPos.Z += (vehModelMinDim.Z / 2);
                    Vector3 wheelClosestPoint = GetClosestPointOnLineSegment(pointA, pointB, wheelPos);
                    float wheelClosestPointSqrDistance = Vector3.DistanceSquared(wheelPos, wheelClosestPoint);
                    if (wheelClosestPointSqrDistance < 0.275f * 0.275f)
                    {
                        API.SetVehicleTyreBurst(veh.Handle, wheelIndex, false, 940f);
                    }
                }
            }

            private static Vector3 GetClosestPointOnLineSegment(Vector3 linePointStart, Vector3 linePointEnd,
                Vector3 testPoint)
            {
                Vector3 lineDiffVect = linePointEnd - linePointStart;
                float lineSegSqrLength = lineDiffVect.LengthSquared();
                Vector3 lineToPointVect = testPoint - linePointStart;
                float dotProduct = Vector3.Dot(lineDiffVect, lineToPointVect);
                float percAlongLine = dotProduct / lineSegSqrLength;
                if (percAlongLine < 0.0f)
                    return linePointStart;
                else if (percAlongLine > 1.0f)
                    return linePointEnd;
                return (linePointStart + (percAlongLine * (linePointEnd - linePointStart)));
            }

            public static async Task<SpikeStrip> Create(Vector3 position, float heading)
            {
                SpikeStrip s = new SpikeStrip
                {
                    Prop = await World.CreateProp(Model, position, true, true)
                };
                s.Prop.Heading = heading;
                return s;
            }
        }
    }

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
            new Vector3(-570.28f, -145.50f, 37.79f)
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
            GarageCheck();

            EventHandlers["UpdatePoliceCars"] += new Action<List<dynamic>>(delegate(List<dynamic> list)
            {
                Vehicles = list;
            });
            DrawMarkers();
            GetPlayerPosEverySecond();
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
                foreach (var pos in Posistions)
                {
                    if (Utility.Instance.GetDistanceBetweenVector3s(pos, _playerPos) < 30)
                    {
                        World.DrawMarker(MarkerType.HorizontalCircleSkinny, pos - new Vector3(0, 0, 1.1f), Vector3.Zero, Vector3.Zero, new Vector3(2,2,2), Color.FromArgb(255, 255, 255, 0));
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
                API.SetBlipScale(blip, 0.6f);
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
                    var dist = API.Vdist(_playerPos.X, _playerPos.Y, _playerPos.Z, pos.X, pos.Y, pos.Z);
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
                                VehicleManager.Instance.Cars.Contains(Game.PlayerPed.CurrentVehicle.Handle) &&
                                _carIsOut)
                            {
                                VehicleManager.Instance.Cars.Remove(_policeCar);
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
                                Utility.Instance.SpawnCar(selectedItem.Text, delegate(int i)
                                {
                                    _carIsOut = true;
                                    API.SetVehicleNumberPlateText(i, "POLICE");
                                    API.ToggleVehicleMod(i, 18, true);
                                    VehicleManager.Instance.Cars.Add(i);
                                    _policeCar = i;
                                    API.TaskWarpPedIntoVehicle(Game.PlayerPed.Handle, i, -1);
                                });
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

    public class Jail : BaseScript
    {
        public static Jail Instance;

        public bool InJail = false;
        public int TimeLeft = 0;

        private readonly Vector3 _inPos = new Vector3(1710.7552490234f, 2672.4741210938f, 45.564888000488f);
        private readonly Vector3 _outPos = new Vector3(1878.4635f, 2606.99438f, 45.6720123f);

        public Jail()
        {
            Instance = this;
            EventHandlers["Jail"] += new Action<dynamic>(JailFunc);
            EventHandlers["Unjail"] += new Action(UnjailFunc);
        }

        private void JailFunc(dynamic timeDynamic)
        {
            Game.PlayerPed.Weapons.RemoveAll();
            Game.PlayerPed.IsInvincible = true;
            Game.PlayerPed.CanSwitchWeapons = false;

            API.SetEntityCoords(Game.PlayerPed.Handle, _inPos.X, _inPos.Y, _inPos.Z, false, false, false, false);
            TimeLeft = timeDynamic;
            InJail = true;
            Loop();
            Draw();
        }

        private async Task Draw()
        {
            while (InJail)
            {
                Utility.Instance.DrawTxt(0.5f, 0.05f, 0, 200, 1, "Time Left : " + TimeLeft, 255, 255, 255, 255, true);
                await Delay(0);
            }       
        }

        private async Task Loop()
        {
            while (InJail)
            {
                Game.PlayerPed.Weapons.RemoveAll();
                Game.PlayerPed.IsInvincible = true;
                Game.PlayerPed.CanSwitchWeapons = false;
                TimeLeft = TimeLeft - 1;
                var pos = Game.PlayerPed.Position;
                if (API.Vdist(pos.X, pos.Y, pos.Z, _inPos.X, _inPos.Y, _inPos.Z) > 30)
                {
                    API.SetEntityCoords(Game.PlayerPed.Handle, _inPos.X, _inPos.Y, _inPos.Z, false, false, false,
                        false);
                }

                TriggerServerEvent("UpdateJailTime", TimeLeft);
                await Delay(1000);

                if (TimeLeft <= 0)
                {
                    break;
                }
            }

            UnjailFunc();
            return;
        }

        private void UnjailFunc()
        {
            API.SetEntityCoords(Game.PlayerPed.Handle, _outPos.X, _outPos.Y, _outPos.Z, false, false, false, false);
            InJail = false;
            Game.PlayerPed.IsInvincible = false;
            Game.PlayerPed.CanSwitchWeapons = true;
            Weapons.Instance.RefreshWeapons();
        }

    }
}