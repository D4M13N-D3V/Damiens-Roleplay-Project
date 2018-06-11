using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay.Main.Criminal
{
    public class Mugging : BaseScript
    {
        public static Mugging Instance;
        private Ped _plyPed;
        private Player _ply;

        private Ped _targetedPed;
        private bool _isMugging;
        private int _mugTimeLeft = 0;

        private const int _mugTimeMin = 35;
        private const int _mugTimeMax = 45;

        private List<string> _callMessages = new List<string>()
        {
            "HELP! I see someone being robbed!",
            "Send police someone is being robbed",
            "This guy is holding a gun to this guys head trying to take his shit",
            "There is someone trying to rob this person",
            "Holy shit this guy is tryingto rob this person",
            "Theres a maniac mugging someone over here , send police"
        };

        private bool _canMug = true;

        private Random _random = new Random();

        public Mugging()
        {
            Instance = this;
            MuggingLogic();
        }

        private async Task MuggingLogic()
        {
            while (true)
            {
                    if (Game.PlayerPed.IsAiming && Game.IsControlJustPressed(0, Control.Context))
                {
                    if (Police.Police.Instance.CopCount >= 1)
                    {
                        if (_canMug)
                        {
                            var targetId = 0;
                            Ped target = Utility.Instance.ClosestPed;
                            if (target.Exists() && Utility.Instance.GetDistanceBetweenVector3s(Game.PlayerPed.Position, target.Position) < 6)
                            {
                                StartRobbery(target);
                            }
                        }
                        else
                        {
                            Utility.Instance.SendChatMessage("[Mugging]", "Can only do this once every 5 minutes!", 255, 0, 0);
                        }
                    }
                    else
                    {
                        Utility.Instance.SendChatMessage("[Mugging]", "Not enough police on.", 255, 0, 0);
                    }
                }
                await Delay(0);
            }
        }

        private async Task StartRobbery(Ped target)
        {
            _canMug = false;
            _isMugging = true;
            _targetedPed = target;
            _mugTimeLeft = _random.Next(_mugTimeMin, _mugTimeMax);
            mugTimeUI();
            async Task mugTimeUI()
            {
                while (_isMugging)
                {
                    Utility.Instance.DrawTxt(0.5f,0.1f,0,0,1,"MUGGIN THE LOCAL! "+_mugTimeLeft+" Seconds Left!",255,0,0,255,true);
                    await Delay(0);
                }
            }

            var chance = _random.Next(0, 2);
            TriggerEvent("911CallClientAnonymous", _callMessages[_random.Next(0, _callMessages.Count)]);
            if (chance == 1)
            {
            }

            var amount = _mugTimeLeft;
            for (int i = 0; i < amount; i++)
            {
                if (Game.PlayerPed.IsAiming && Utility.Instance.GetDistanceBetweenVector3s(Game.PlayerPed.Position, target.Position)<8)
                {
                    _mugTimeLeft = _mugTimeLeft - 1;
                    target.Task.PlayAnimation("random@mugging3", "handsup_standing_base");
                    target.IsPositionFrozen = true;
                    await Delay(1000);
                }
                else
                {
                    CancelRobbery();
                    return;
                }
            }
            Debug.WriteLine(Convert.ToString(_mugTimeLeft));
            _isMugging = false;
            target.IsPositionFrozen = false;
            target.Task.ClearAll();
            TriggerServerEvent("MuggingReward");
            _canMug = true;
            await Delay(150000);
        }

        private void CancelRobbery()
        {
            _isMugging = false;
            _targetedPed.Task.ClearAll();
            _targetedPed.IsPositionFrozen = false;
            _canMug = true;
        }
    }
}
