using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client;
using client.Main.Items;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay.Main.HUD
{
    public class HealthFoodHUD : BaseScript
    {
        public static HealthFoodHUD Instance;

        public HealthFoodHUD()
        {
            Instance = this;

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
            Tick += new Func<Task>(async delegate
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
            {
                var ply = API.PlayerPedId();

                float healthPerecent = (float)API.GetEntityHealth(ply) / (float)API.GetEntityMaxHealth(ply);
                float armorPercent = ((float)API.GetPedArmour(ply) / 100.0f) * 100.0f;
                float hungerPerecent = FoodManager.Instance.GetHungerPercentage();
                float thirstPercent = FoodManager.Instance.GetThirstPercentage();

                Utility.Instance.DrawRct(0.0147f, 0.963f, 0.145f, 0.025f, 0, 0, 0, 255);

                Utility.Instance.DrawRct(0.01625f, 0.9675f, 0.033f, 0.01375f, 50, 50, 50, 180);
                Utility.Instance.DrawRct(0.01625f, 0.9675f, healthPerecent * 0.033f, 0.01375f, 154, 205, 50, 180);

                Utility.Instance.DrawRct(0.05125f, 0.9675f, 0.033f, 0.01375f, 50, 50, 50, 180);
                Utility.Instance.DrawRct(0.05125f, 0.9675f, (armorPercent / 100.0f) * 0.033f, 0.01375f, 70, 130, 180, 180);

                Utility.Instance.DrawRct(0.08625f, 0.9675f, 0.033f, 0.01375f, 50, 50, 50, 180);
                Utility.Instance.DrawRct(0.08625f, 0.9675f, (hungerPerecent / 100.0f) * 0.033f, 0.01375f, 255, 140, 0, 180);

                Utility.Instance.DrawRct(0.12125f, 0.9675f, 0.033f, 0.01375f, 50, 50, 50, 180);
                Utility.Instance.DrawRct(0.12125f, 0.9675f, (thirstPercent / 100.0f) * 0.033f, 0.01375f, 135, 206, 250, 180);

                Utility.Instance.DrawTxt(0.0325f, 0.9635f, 1.0f, 1.0f, 0.3f, "~y~HEALTH", 255, 255, 255, 255, true);
                Utility.Instance.DrawTxt(0.0675f, 0.9635f, 1.0f, 1.0f, 0.3f, "~y~ARMOR", 255, 255, 255, 255, true);
                Utility.Instance.DrawTxt(0.103f, 0.9635f, 1.0f, 1.0f, 0.3f, "~y~HUNGER", 255, 255, 255, 255, true);
                Utility.Instance.DrawTxt(0.138f, 0.9635f, 1.0f, 1.0f, 0.3f, "~y~THIRST", 255, 255, 255, 255, true);
                //Utility.Instance.DrawTxt(0.01825f, 0.9675f, 1.0f, 1.0f, 0.35f,"~p~ARMR:~w~" + armorPercent, 255, 255, 255, 255, true);

            });
        }
    }
}
