using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay.Main
{
    public enum RestraintTypes { Handcuffs, Zipties, Hobblecuff }

    public class Restraints : BaseScript
    {
        public static Restraints Instance;
        public bool IsRestrained = false;
        public RestraintTypes CurrentType = RestraintTypes.Handcuffs;
        public int Restrainer;
        public bool BeingDragged = false;

        private readonly Dictionary<RestraintTypes, Action> _restraintFuncs = new Dictionary<RestraintTypes, Action>();

        public int Restrainee;
        public bool RestraintsInUse = false;
        public bool IsDragging = false;

        public Restraints()
        {
            Instance = this;
            EventHandlers["Restrained"] += new Action<dynamic, dynamic>(Restrained);
            EventHandlers["Restrain"] += new Action<dynamic>(Restrain);
            EventHandlers["Dragged"] += new Action<dynamic>(Dragged);
            EventHandlers["Dragging"] += new Action(Dragging);
        }

        private void Restrain(dynamic restraineeDynamic)
        {
            if (RestraintsInUse)
            {
                RestraintsInUse = false;
                Restrainee = -1;
                if (IsDragging)
                {
                    TriggerServerEvent("DragRequest", API.GetPlayerServerId(Game.Player.Handle));
                }
                return;
            }

            Restrainee = restraineeDynamic;
            IsDragging = false;
            RestraintsInUse = true;

            DragLogic();
            async void DragLogic()
            {
                while (RestraintsInUse)
                {
                    if (Game.IsControlPressed(0, Control.Context))
                    {
                        var playerPos = Game.PlayerPed.Position;
                        var restraineePos = API.GetEntityCoords(API.GetPlayerPed(Restrainee),false);
                        if (Game.IsControlJustPressed(0, Control.Attack) && API.Vdist(restraineePos.X,restraineePos.Y,restraineePos.Z,playerPos.X,playerPos.Y,playerPos.Z)<4)
                        {
                            TriggerServerEvent("DragRequest",API.GetPlayerServerId(Game.Player.Handle));
                        }
                        if (Game.IsControlJustPressed(0, Control.Aim) && IsDragging)
                        {

                        }
                    }
                    await Delay(0);
                }
            }
        }

        private async void Restrained(dynamic typeDynamic, dynamic restrainerDynamic)
        {
            if (IsRestrained)
            {
                IsRestrained = false;
                CurrentType = RestraintTypes.Handcuffs;
                Restrainer = -1;
                return;
            }   
            IsRestrained = true;
            CurrentType = (RestraintTypes)restrainerDynamic;
            Restrainer = restrainerDynamic;

            API.RequestAnimDict("mp_arresting");

            while (!API.HasAnimDictLoaded("mp_arresting"))
            {
                await Delay(1);
            }

            switch (CurrentType)
            {
                case RestraintTypes.Handcuffs:
                    Handcuffs();
                    break;
                case RestraintTypes.Zipties:
                    Zipties();
                    break;
                case RestraintTypes.Hobblecuff:
                    HobbleCuff();
                    break;
            }

        }

        private void Dragged(dynamic copId)
        {
            if (!BeingDragged && IsRestrained)
            {
                BeingDragged = true;
                var cop = API.GetPlayerPed(API.GetPlayerFromServerId(copId));
                API.AttachEntityToEntity(Game.PlayerPed.Handle, cop, 0x2e28, 0.48f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, false, false, false, false, 2, true);
            }
            else if (BeingDragged)
            {
                BeingDragged = false;
                API.DetachEntity(Game.PlayerPed.Handle, true, false);
            }
        }

        private void Dragging()
        {
            IsDragging = !IsDragging;
        }
        
        #region Restraint Functions
        private async void Handcuffs()
        {
            while (IsRestrained)
            {
                Game.PlayerPed.Task.PlayAnimation("mp_arresting", "idle", 8.0f, -1, AnimationFlags.UpperBodyOnly);
                await Delay(1);
            }
        }

        private async void HobbleCuff()
        {
            while (IsRestrained)
            {
                Game.PlayerPed.Task.PlayAnimation("mp_arresting", "idle", 8.0f, -1, AnimationFlags.None);
                await Delay(1);
            }
        }

        private async void Zipties()
        {
            ResetZipties();
            async void ResetZipties()
            {
                await Delay(600000);
                IsRestrained = false;
                CurrentType = RestraintTypes.Handcuffs;
                Restrainer = -1;
            }
            while (IsRestrained)
            {
                Game.PlayerPed.Task.PlayAnimation("mp_arresting", "idle", 8.0f, -1, AnimationFlags.UpperBodyOnly);
                await Delay(1);
            }
        }
        #endregion

    }
}
