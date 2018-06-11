using System;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace client.Main.HUD
{
    public class HealthFoodHUD : BaseScript
    {
        public static HealthFoodHUD Instance;
        private float _healthPercent;
        private float _armorPercent;
        private float _hungerPerecent;
        private float _thirstPercent;
        public HealthFoodHUD()
        {
            Instance = this;

            Tick += new Func<Task>(async delegate
            {

                Utility.Instance.DrawRct(0.0147f, 0.963f, 0.145f, 0.025f, 0, 0, 0, 255);
                
                Utility.Instance.DrawRct(0.01625f, 0.9675f, 0.033f, 0.01375f, 50, 50, 50, 180);
                Utility.Instance.DrawRct(0.01625f, 0.9675f, _healthPercent * 0.033f, 0.01375f, 154, 205, 50, 180);

                Utility.Instance.DrawRct(0.05125f, 0.9675f, 0.033f, 0.01375f, 50, 50, 50, 180);
                Utility.Instance.DrawRct(0.05125f, 0.9675f, (_armorPercent / 100.0f) * 0.033f, 0.01375f, 70, 130, 180, 180);

                Utility.Instance.DrawRct(0.08625f, 0.9675f, 0.033f, 0.01375f, 50, 50, 50, 180);
                Utility.Instance.DrawRct(0.08625f, 0.9675f, (_hungerPerecent / 100.0f) * 0.033f, 0.01375f, 255, 140, 0, 180);

                Utility.Instance.DrawRct(0.12125f, 0.9675f, 0.033f, 0.01375f, 50, 50, 50, 180);
                Utility.Instance.DrawRct(0.12125f, 0.9675f, (_thirstPercent / 100.0f) * 0.033f, 0.01375f, 135, 206, 250, 180);

                Utility.Instance.DrawTxt(0.0325f, 0.9635f, 1.0f, 1.0f, 0.3f, "~y~HEALTH", 255, 255, 255, 255, true);
                Utility.Instance.DrawTxt(0.0675f, 0.9635f, 1.0f, 1.0f, 0.3f, "~y~ARMOR", 255, 255, 255, 255, true);
                Utility.Instance.DrawTxt(0.103f, 0.9635f, 1.0f, 1.0f, 0.3f, "~y~HUNGER", 255, 255, 255, 255, true);
                Utility.Instance.DrawTxt(0.138f, 0.9635f, 1.0f, 1.0f, 0.3f, "~y~THIRST", 255, 255, 255, 255, true);
                //Utility.Instance.DrawTxt(0.01825f, 0.9675f, 1.0f, 1.0f, 0.35f,"~p~ARMR:~w~" + armorPercent, 255, 255, 255, 255, true);

            });
            GetPlayerInfoEverySecond();
        }

        public Vector3 _playerPos;
        private async Task GetPlayerInfoEverySecond()
        {
            while (true)
            {
                _healthPercent = (float) Game.PlayerPed.Health / (float) Game.PlayerPed.MaxHealth;
                _armorPercent = (Game.PlayerPed.Armor / 100) * 100;
                _hungerPerecent = FoodManager.Instance.GetHungerPercentage();
                _thirstPercent = FoodManager.Instance.GetThirstPercentage();
                _playerPos = Game.PlayerPed.Position;
                await Delay(1000);
            }
        }
    }
}
