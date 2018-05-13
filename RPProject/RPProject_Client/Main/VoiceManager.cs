using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay.Main
{
    public enum VoiceLevels { Whisper, Talk, Yell, Scream }
    public class VoiceManager : BaseScript
    {
        public static VoiceManager Instance;

        public VoiceLevels CurrentVoiceLevel = VoiceLevels.Talk;

        public VoiceManager()
        {
            Instance = this;

            Tick += new Func<Task>(async delegate
            {
                if (API.IsControlJustPressed(0, 47) && API.IsInputDisabled(2) == true)
                {
                    Utility.Instance.Log(" Voice cycled.");
                    CycleVoiceLevel();
                }
            });
        }

        private void CycleVoiceLevel()
        {
            switch (CurrentVoiceLevel)
            {
                case VoiceLevels.Talk:
                    CurrentVoiceLevel = VoiceLevels.Yell;
                    API.DisplayHelpTextThisFrame("Voice Level Changed : Yell", true);
                    API.NetworkSetTalkerProximity(25);
                    break;
                case VoiceLevels.Yell:
                    API.DisplayHelpTextThisFrame("Voice Level Changed : Scream", true);
                    CurrentVoiceLevel = VoiceLevels.Scream;
                    API.NetworkSetTalkerProximity(35);
                    break;
                case VoiceLevels.Scream:
                    API.DisplayHelpTextThisFrame("Voice Level Changed : Whisper", true);
                    CurrentVoiceLevel = VoiceLevels.Whisper;
                    API.NetworkSetTalkerProximity(5);
                    break;
                case VoiceLevels.Whisper:
                    API.DisplayHelpTextThisFrame("Voice Level Changed : Talking", true);
                    CurrentVoiceLevel = VoiceLevels.Talk;
                    API.NetworkSetTalkerProximity(15);
                    break;
            }
        }
    }
}
