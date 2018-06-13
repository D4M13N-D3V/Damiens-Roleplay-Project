using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;
using client.Main.Items;
using client.Main.Vehicles;

namespace client.Main.EmergencyServices.EMS
{
    public class EMS : BaseScript
    {
        //These still need to be changed to be ems instead of police
        private readonly Dictionary<string, List<EMSUniformComponent>> _maleUniforms =
            new Dictionary<string, List<EMSUniformComponent>>()
            {
                ["EMS"] = new List<EMSUniformComponent>()
                {
                    new EMSUniformComponent(3,30,1,0),
                    new EMSUniformComponent(4,23,8,0),
                    new EMSUniformComponent(5,0,0,0),
                    new EMSUniformComponent(6,24,0,0),
                    new EMSUniformComponent(7,127,0,0),
                    new EMSUniformComponent(8,129,0,0),
                    new EMSUniformComponent(11,250,1,0),
                }
            };

        private readonly Dictionary<string, List<EMSUniformComponent>> _femaleUniforms =
            new Dictionary<string, List<EMSUniformComponent>>()
            {
                ["EMS"] = new List<EMSUniformComponent>()
                {
                    new EMSUniformComponent(3,44,1,0),
                    new EMSUniformComponent(4,41,1,0),
                    new EMSUniformComponent(5,0,0,0),
                    new EMSUniformComponent(6,52,0,0),
                    new EMSUniformComponent(7,97,0,0),
                    new EMSUniformComponent(8,159,0,0),
                    new EMSUniformComponent(11,258,0,0),
                }
            };

        public int EMSCount = 0;
        private bool _onDuty = false;
        private string _rankName = "";
        private string _department = "";

        public static EMS Instance;

        public EMS()
        {
            Instance = this;
            StopDispatch();
            DisableAutospawn();
            DeathCheck();
            EventHandlers["EMS:SetOnDuty"] += new Action<dynamic>(OnDuty);
            EventHandlers["EMS:SetOffDuty"] += new Action(OffDuty);
            EventHandlers["EMS:RefreshOnDutyOfficers"] += new Action<dynamic>(RefreshCops);
            EventHandlers["Revive"] += new Action(Revive);
        }

        private async Task DisableAutospawn()
        {
            await Delay(5000);
            Exports["spawnmanager"].setAutoSpawn(false);
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
            EMSCount = copCount;
        }

        private void OnDuty(dynamic data)
        {
            var department = Convert.ToString(data);
            _department = department;
            Utility.Instance.SendChatMessage("[EMS]", "You have gone on duty.", 0, 255, 0);
            _onDuty = true;
            EMSGear.Instance.MenuRestricted = false;
            EMSGarage.Instance.MenuRestricted = false;
            GiveUniform();
        }

        private void OffDuty()
        {
            Utility.Instance.SendChatMessage("[EMS]", "You have gone off duty.", 0, 255, 0);
            _rankName = "";
            _onDuty = false;
            EMSGear.Instance.MenuRestricted = true;
            EMSGarage.Instance.MenuRestricted = true;
            _department = "";
            TakeUniform();
        }

        private void GiveUniform()
        {
            if (_department == "USMS" || _department == "") { return; }
            if (API.GetEntityModel(Game.PlayerPed.Handle) == API.GetHashKey("mp_m_freemode_01"))
            {
                foreach (var uniformParts in _maleUniforms[_department])
                {
                    API.SetPedComponentVariation(Game.PlayerPed.Handle, uniformParts.Component, uniformParts.Drawable, uniformParts.Texture, uniformParts.Pallet);
                }
            }
            else if (API.GetEntityModel(Game.PlayerPed.Handle) == API.GetHashKey("mp_f_freemode_01"))
            {
                foreach (var uniformParts in _femaleUniforms[_department])
                {
                    API.SetPedComponentVariation(Game.PlayerPed.Handle, uniformParts.Component, uniformParts.Drawable, uniformParts.Texture, uniformParts.Pallet);
                }
            }
        }

        private void TakeUniform()
        {
            TriggerServerEvent("RefreshClothes");
        }




        public bool _dead = false;
        private const int _respawnTime = 300;
        private int _respawnTimeLeft = _respawnTime;
        private List<Vector3> Hospitals = new List<Vector3>()
            {
                new Vector3(1155.26f, -1520.82f, 34.9f),
                new Vector3(295.411f, -1446.88f, 30.5f),
                new Vector3(361.145f, -584.414f, 29.2f),
                new Vector3(-449.639f, -340.586f, 34.8f),
                new Vector3(-874.467f, -307.896f, 39.8f),
                new Vector3(-677.135f, 310.275f, 83.5f),
                new Vector3(1839.39f, 3672.78f, 34.6f),
                new Vector3(-242.968f, 6326.29f, 32.8f)
            };
        private async Task DeathCheck()
        {
            while (true)
            {
                if (Game.PlayerPed.Health <= 0 && !_dead)
                {
                    Dead();
                }
                await Delay(1000);
            }
        }

        private async Task Dead()
        {
            _dead = true;
            Game.PlayerPed.Health = Game.PlayerPed.MaxHealth;
            API.NetworkResurrectLocalPlayer(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, 0, true, false);
            Game.PlayerPed.IsInvincible = true;
            API.SetPlayerInvincible(Game.Player.Handle, true);
            API.SetEntityInvincible(Game.PlayerPed.Handle, true);
            RespawnTimer();
            while (_dead)
            {
                Game.PlayerPed.IsInvincible = true;
                API.SetPlayerInvincible(Game.Player.Handle, true);
                API.SetEntityInvincible(Game.PlayerPed.Handle, true);
                Game.PlayerPed.Health = Game.PlayerPed.MaxHealth;
                Game.PlayerPed.Ragdoll(4000,RagdollType.Normal);
                if (Game.PlayerPed.IsDead)
                {
                    API.NetworkResurrectLocalPlayer(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, 0, true, false);
                }
                await Delay(1500);
            }
            Game.PlayerPed.IsInvincible = false;
            API.SetPlayerInvincible(Game.Player.Handle, false);
            API.SetEntityInvincible(Game.PlayerPed.Handle, false);
            Game.PlayerPed.CancelRagdoll();
        }

        public bool NeedsPills = false;
        private async void Revive()
        {
            Game.PlayerPed.ResetVisibleDamage();
            _dead = false;
            API.NetworkResurrectLocalPlayer(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, 0, true, false);
            Game.PlayerPed.IsInvincible = false;
            API.SetPlayerInvincible(Game.Player.Handle, false);
            API.SetEntityInvincible(Game.PlayerPed.Handle, false);
            Game.PlayerPed.CancelRagdoll();
            API.RequestAnimSet("move_injured_generic");
            Weapons.Instance.RefreshWeapons();
            while (!API.HasAnimSetLoaded("move_injured_generic"))
            {
                await Delay(0);
            }
            API.SetPedMovementClipset(Game.PlayerPed.Handle,"move_injured_generic", 1);
            NeedsPills = true;
            PedDamage.Instance.ResetInjuries();
            while (NeedsPills)
            {
                Game.DisableControlThisFrame(0,Control.Sprint);
                await Delay(0);
            }
        }

        private async Task RespawnTimer()
        {
            _respawnTimeLeft = _respawnTime;
            UI();
            async Task UI()
            {
                while (_dead)
                {
                    if (_respawnTimeLeft == 0)
                    {
                        Utility.Instance.DrawTxt(0.5f, 0.3f, 0, 0, 1.5f, "Press E to respawn.", 255, 190, 190, 255, true);
                        if (Game.IsControlJustPressed(0, Control.Context))
                        {
                            Respawn();
                        }
                    }
                    else
                    {
                        Utility.Instance.DrawTxt(0.5f, 0.3f, 0, 0, 1.5f, _respawnTimeLeft + " left until you can respawn.", 255, 190, 190, 255, true);
                    }
                    await Delay(0);
                }
            }

            while (_dead)
            {
                _respawnTimeLeft = _respawnTimeLeft - 1;
                if (_respawnTimeLeft <= 0)
                {
                    return;
                }
                await Delay(1000);
            }
        }

        private async Task Respawn()
        {
            Vector3 closestSpot = Hospitals[0];
            float closestSpotDistance = 10000000;
            foreach (var spot in Hospitals)
            {
                var dist = Utility.Instance.GetDistanceBetweenVector3s(Game.PlayerPed.Position, spot);
                if (dist < closestSpotDistance)
                {
                    closestSpotDistance = dist;
                    closestSpot = spot;
                }
            }

            Game.PlayerPed.Position = closestSpot;
            Game.PlayerPed.ResetVisibleDamage();
            _dead = false;
            API.NetworkResurrectLocalPlayer(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z, 0, true, false);
            Game.PlayerPed.IsInvincible = false;
            API.SetPlayerInvincible(Game.Player.Handle, false);
            API.SetEntityInvincible(Game.PlayerPed.Handle, false);
            Game.PlayerPed.CancelRagdoll();
            API.RequestAnimSet("move_injured_generic");
            Weapons.Instance.RefreshWeapons();
            while (!API.HasAnimSetLoaded("move_injured_generic"))
            {
                await Delay(0);
            }
            API.SetPedMovementClipset(Game.PlayerPed.Handle, "move_injured_generic", 1);
            NeedsPills = true;
            PedDamage.Instance.ResetInjuries();
            while (NeedsPills)
            {
                Game.DisableControlThisFrame(0, Control.Sprint);
                await Delay(0);
            }
        }

    }
}

