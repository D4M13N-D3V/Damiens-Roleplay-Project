using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay
{
    public enum VoiceLevels { Whisper, Talk, Yell, Scream }
    public class VoiceManager : BaseScript
    {
        public static VoiceManager Instance;

        private VoiceLevels _currentVoiceLevel = VoiceLevels.Talk;

        public VoiceManager()
        {
            Instance = this;

            Tick += new Func<Task>(async delegate
            {
                Utility.Instance.DrawRct(0.155f, 0.935f, 0.015f, 0.05f, 0, 0, 0, 120);
                switch (_currentVoiceLevel)
                {
                    case VoiceLevels.Talk:
                        Utility.Instance.DrawRct(0.1575f, 0.939f, 0.01f, 0.042f, 0, 255, 0, 150);
                        break;
                    case VoiceLevels.Yell:
                        Utility.Instance.DrawRct(0.1575f, 0.939f, 0.01f, 0.042f, 255, 0, 0, 170);
                        break;
                    case VoiceLevels.Scream:
                        Utility.Instance.DrawRct(0.1575f, 0.939f, 0.01f, 0.042f, 255, 0, 255, 190);
                        break;
                    case VoiceLevels.Whisper:
                        Utility.Instance.DrawRct(0.1575f, 0.939f, 0.01f, 0.042f, 0, 255, 255, 140);
                        break;
                }

                if (API.IsControlJustPressed(0, 47) && API.IsInputDisabled(2) == true)
                {
                    Utility.Instance.Log(" Voice cycled.");
                    CycleVoiceLevel();
                }
            });
        }

        private void CycleVoiceLevel()
        {
            switch (_currentVoiceLevel)
            {
                case VoiceLevels.Talk:
                    _currentVoiceLevel = VoiceLevels.Yell;
                    API.NetworkSetTalkerProximity(25);
                    break;
                case VoiceLevels.Yell:
                    _currentVoiceLevel = VoiceLevels.Scream;
                    API.NetworkSetTalkerProximity(35);
                    break;
                case VoiceLevels.Scream:
                    _currentVoiceLevel = VoiceLevels.Whisper;
                    API.NetworkSetTalkerProximity(5);
                    break;
                case VoiceLevels.Whisper:
                    _currentVoiceLevel = VoiceLevels.Talk;
                    API.NetworkSetTalkerProximity(15);
                    break;
            }
        }

    }
}
