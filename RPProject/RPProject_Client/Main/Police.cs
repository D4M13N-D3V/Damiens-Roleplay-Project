using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using roleplay.Main;
using roleplay.Users.Inventory;

namespace roleplay.Main.Police
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
                ["LSPD"] = new List<PoliceUniformComponent>()
                {
                    new PoliceUniformComponent(3,0,0,0),
                    new PoliceUniformComponent(4,35,0,0),
                    new PoliceUniformComponent(5,0,0,0),
                    new PoliceUniformComponent(6,24,0,0),
                    new PoliceUniformComponent(7,0,0,0),
                    new PoliceUniformComponent(8,122,0,0),
                    new PoliceUniformComponent(11,55,0,0),
                },
                ["BCSO"] = new List<PoliceUniformComponent>()
                {
                    new PoliceUniformComponent(3,11,0,0),
                    new PoliceUniformComponent(4,10,0,0),
                    new PoliceUniformComponent(5,0,0,0),
                    new PoliceUniformComponent(6,24,0,0),
                    new PoliceUniformComponent(7,125,0,0),
                    new PoliceUniformComponent(8,122,0,0),
                    new PoliceUniformComponent(11,26,2,0),
                }
            };

        private readonly Dictionary<string, List<PoliceUniformComponent>> _femaleUniforms =
            new Dictionary<string, List<PoliceUniformComponent>>()
            {
                ["LSPD"] = new List<PoliceUniformComponent>()
                {
                    new PoliceUniformComponent(3,14,0,0),
                    new PoliceUniformComponent(4,34,0,0),
                    new PoliceUniformComponent(5,0,0,0),
                    new PoliceUniformComponent(6,25,0,0),
                    new PoliceUniformComponent(7,0,0,0),
                    new PoliceUniformComponent(8,152,0,0),
                    new PoliceUniformComponent(11,48,0,0),
                },
                ["BCSO"] = new List<PoliceUniformComponent>()
                {
                    new PoliceUniformComponent(3,0,0,0),
                    new PoliceUniformComponent(4,64,1,0),
                    new PoliceUniformComponent(5,0,0,0),
                    new PoliceUniformComponent(6,52,0,0),
                    new PoliceUniformComponent(7,95,0,0),
                    new PoliceUniformComponent(8,152,0,0),
                    new PoliceUniformComponent(11,27,2,0),
                }
            };

        public int CopCount = 0;
        private bool _onDuty = false;
        private string _rankName = "";
        private string _department = "";

        public static Police Instance;

        public Police()
        {
            Instance = this;
            EventHandlers["Police:SetOnDuty"] += new Action<dynamic>(OnDuty);
            EventHandlers["Police:SetOffDuty"] += new Action(OffDuty);
            EventHandlers["Police:RefreshOnDutyOfficers"] += new Action<dynamic>(RefreshCops);
        }
        private void RefreshCops(dynamic copCount)
        {
            CopCount = copCount;
        }

        private void OnDuty(dynamic data)
        {
            var department = Convert.ToString(data);
            _department = department;
            Utility.Instance.SendChatMessage("[POLICE]", "You have gone on duty.", 0, 0, 255);
            _onDuty = true;
            PoliceGear.Instance.MenuRestricted = false;
            GiveUniform();
        }

        private void OffDuty()
        {
            Utility.Instance.SendChatMessage("[POLICE]", "You have gone off duty.", 0, 0, 255);
            _rankName = "";
            _onDuty = false;
            PoliceGear.Instance.MenuRestricted = true;
            _department = "";
            TakeUniform();
        }

        private void GiveUniform()
        {
            if (_department == "USMS" || _department == "") { return; }
            if (API.GetEntityModel(API.PlayerPedId()) == API.GetHashKey("mp_m_freemode_01"))
            {
                foreach (var uniformParts in _maleUniforms[_department])
                {
                    API.SetPedComponentVariation(API.PlayerPedId(), uniformParts.Component, uniformParts.Drawable, uniformParts.Texture, uniformParts.Pallet);
                }
            }
            else if (API.GetEntityModel(API.PlayerPedId()) == API.GetHashKey("mp_f_freemode_01"))
            {
                foreach (var uniformParts in _femaleUniforms[_department])
                {
                    API.SetPedComponentVariation(API.PlayerPedId(), uniformParts.Component, uniformParts.Drawable, uniformParts.Texture, uniformParts.Pallet);
                }
            }
        }

        private void TakeUniform()
        {
            TriggerServerEvent("RefreshClothes");
        }











        #region Items
        private void SetupItems()
        {
            InventoryProcessing.Instance.AddItemUse("Police Lock Tool(P)", PoliceLockTool);
        }

        public async void PoliceLockTool()
        {
            var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
            var vehicle = API.GetClosestVehicle(playerPos.X, playerPos.Y, playerPos.Z, 4, 0, 70);
            var random = new Random();
            var rdmInt = random.Next(3);
            InteractionMenu.Instance._interactionMenuPool.CloseAllMenus();
            Game.PlayerPed.Task.PlayAnimation("misscarstealfinalecar_5_ig_3", "crouchloop");
            var lockPicking = true;
            async void CancelLockpick()
            {
                while (lockPicking)
                {
                    if (API.IsControlJustPressed(0, (int)Control.PhoneCancel))
                    {
                        lockPicking = false;
                        API.ClearPedTasks(API.PlayerPedId());
                    }
                    await Delay(0);
                }
            }
            CancelLockpick();
            await Delay(15000);
            lockPicking = false;
            Game.PlayerPed.Task.ClearAll();
            API.SetVehicleDoorsLocked(vehicle, 0);
            Utility.Instance.SendChatMessage("[LOCKPICK]", "You successfully unlock the vehicle with your tool!", 255, 0, 0);
        }

        public async void FingerprintScanner()
        {
            TriggerServerEvent("RequestID", Game.Player.ServerId);
            Game.PlayerPed.Task.PlayAnimation("mp_arresting", "a_uncuff");
            await Delay(3000);
            Game.PlayerPed.Task.ClearAll();
            ClosestPlayerReturnInfo output;
            Utility.Instance.GetClosestPlayer(out output);
            if (output.Dist < 4)
            {
                TriggerServerEvent("FingerPrintScannerRequest", API.GetPlayerServerId(output.Pid));
            }
        }

        #endregion



    }

    public class PoliceGear : BaseStore
    {
        public PoliceGear() : base("Police Station", "Pick up your police gear here.", 60, 29,
            new List<Vector3>()
            {
                new Vector3(452.09893798828f,-979.99584960938f,30.689596176147f),
            },
            new Dictionary<string, int>()
            {
                ["Tazer(P)"] = 0,
                ["Nighstick(P)"] = 0,
                ["Binoculars(P)"] = 0,
                ["Police Lock Tool(P)"] = 0,
                ["Combat Pistol(P)"] = 0,
                ["Pump Shotgun(P)"] = 0,
                ["Carbine Rifle(P)"] = 0,
                ["Fingerprint Scanner(P)"] = 0,
                ["Spike Strips(P)"] = 0,
                ["Pistol Ammo"] = 10,
                ["Shotgun Ammo"] = 10,
                ["Rifle Ammo"] = 10
            })
        {
            MenuRestricted = false;
        }
    }
    internal class SpikeStripsScript : BaseScript
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
                return !playerPed.IsDead && !playerPed.IsInVehicle() && !playerPed.IsInCover() && !playerPed.IsJumping &&
                       !playerPed.IsAiming && !playerPed.IsClimbing && InventoryUI.Instance.HasItem("Spike Strips(P)") > 0;
            }
        }

        private bool CanRemoveSpikeStrips
        {
            get
            {
                Ped playerPed = LocalPlayer.Character;
                return !playerPed.IsDead && !playerPed.IsInVehicle() && !playerPed.IsInCover() && !playerPed.IsJumping && !playerPed.IsAiming && !playerPed.IsClimbing && InventoryUI.Instance.HasItem("Spike Strips(P)") > 0;
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

        private async void CreateSSpikeStrips()
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
                SpikeStrip s = await SpikeStrip.Create(playerPos + playerForwardVector * (SeparationFromPlayer + (SeparationBetweenStingers * i)), playerHeading);
                SpikesStrips.Add(s);
                await Delay(500);
            }
            Screen.ShowNotification("~g~Deployed 1 spike strip!", true);
        }

        private async void CreateSpikeStrips()
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

                SpikeStrip s = await SpikeStrip.Create(playerPos + playerForwardVector * (SeparationFromPlayer + (SeparationBetweenStingers * i)), playerHeading);
                SpikesStrips.Add(s);
                await Delay(500);
            }
            Screen.ShowNotification("~g~Deployed 2 spike strips!", true);
        }

        private async void DeleteAllSpikeStrips()
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
                    Vehicle closestVeh = new Vehicle(API.GetClosestVehicle(propPos.X, propPos.Y, propPos.Z, 10.0f, 0, 70));
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
                return Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, veh.Handle, wheelBone) != -1 && !Function.Call<bool>(Hash.IS_VEHICLE_TYRE_BURST, veh.Handle, wheelIndex, false);
            }

            private void BurstTyre(Vehicle veh, string wheelBone, int wheelIndex, Vector3 vehModelMinDim, Vector3 pointA, Vector3 pointB)
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

            private static Vector3 GetClosestPointOnLineSegment(Vector3 linePointStart, Vector3 linePointEnd, Vector3 testPoint)
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
}
