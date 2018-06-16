using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace client.Main
{
    public enum VoiceLevels { Whisper, Talk, Yell, Scream }
    public class VoiceManager : BaseScript
    {
        public static VoiceManager Instance;

        public VoiceLevels CurrentVoiceLevel = VoiceLevels.Talk;

        public VoiceManager()
        {
            Instance = this;

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
            Tick += new Func<Task>(async delegate
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
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
