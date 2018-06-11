using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using roleplay.Main.Users;

namespace roleplay.Main.Criminal
{
    public class Mugging : BaseScript
    {
        private Random _random = new Random();
        private const int _minReward = 25;
        public const int _maxReward = 120;

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
