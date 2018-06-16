using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace client.Main.HUD
{
    public class VoiceHUD : BaseScript
    {
        public static VoiceHUD Instance;

        public VoiceHUD()
        {
            Instance = this;
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
            Tick += new Func<Task>(async delegate
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
            {
                Utility.Instance.DrawRct(0.155f, 0.94f, 0.015f, 0.055f, 0, 0, 0, 255);

                if (API.NetworkIsPlayerTalking(API.PlayerId()))
                {
                    Utility.Instance.DrawRct(0.1575f, 0.949f, 0.01f, 0.042f, 0, 0, 255, 255);
                }
                else
                {
                    switch (VoiceManager.Instance.CurrentVoiceLevel)
                    {
                        case VoiceLevels.Talk:
                            Utility.Instance.DrawRct(0.1575f, 0.943f, 0.01f, 0.048f, 0, 255, 0, 180);
                            Utility.Instance.DrawTxt(0.1625f, 0.954f, 1.0f, 1.0f, 0.4f, "~w~T", 255, 255, 255, 255, true);
                            break;
                        case VoiceLevels.Yell:
                            Utility.Instance.DrawRct(0.1575f, 0.943f, 0.01f, 0.048f, 255, 0, 0, 180);
                            Utility.Instance.DrawTxt(0.1625f, 0.954f, 1.0f, 1.0f, 0.4f, "~w~Y", 255, 255, 255, 255, true);
                            break;
                        case VoiceLevels.Scream:
                            Utility.Instance.DrawRct(0.1575f, 0.943f, 0.01f, 0.048f, 255, 0, 255, 180);
                            Utility.Instance.DrawTxt(0.1625f, 0.954f, 1.0f, 1.0f, 0.4f, "~w~S", 255, 255, 255, 255, true);
                            break;
                        case VoiceLevels.Whisper:
                            Utility.Instance.DrawRct(0.1575f, 0.943f, 0.01f, 0.048f, 0, 255, 255, 180);
                            Utility.Instance.DrawTxt(0.1625f, 0.954f, 1.0f, 1.0f, 0.4f, "~w~W", 255, 255, 255, 255, true);
                            break;
                    }
                }
            });
        }
    }
}
