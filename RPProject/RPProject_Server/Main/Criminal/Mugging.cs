using System;
using CitizenFX.Core;
using server.Main.Users;

namespace server.Main.Criminal
{
    public class Mugging : BaseScript
    {
        private Random _random = new Random();
        private const int _minReward = 1;
        public const int _maxReward = 500;

        public Mugging()
        {
            EventHandlers["MuggingReward"] += new Action<Player>(MuggingReward);
        }

        private void MuggingReward([FromSource] Player ply)
        {
            var amount = _random.Next(_minReward, _maxReward);
            MoneyManager.Instance.AddMoney(ply,MoneyTypes.Cash,amount);

        }
    }
}
