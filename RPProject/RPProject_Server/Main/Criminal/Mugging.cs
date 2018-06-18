using System;
using CitizenFX.Core;
using server.Main.Users;

namespace server.Main.Criminal
{
    /// <summary>
    /// Server sid emugging manager
    /// </summary>
    public class Mugging : BaseScript
    {
        /// <summary>
        /// Random for RNG
        /// </summary>
        private Random _random = new Random();
        /// <summary>
        /// The minimum reward for mugging someone
        /// </summary>
        private const int _minReward = 1;
        /// <summary>
        /// The maximum reward for mugging someone
        /// </summary>
        public const int _maxReward = 500;

        public Mugging()
        {
            EventHandlers["MuggingReward"] += new Action<Player>(MuggingReward);
        }

        /// <summary>
        /// EVent handler for when someone scucessfully mugs someone
        /// </summary>
        /// <param name="ply">The player who triggered it.</param>
        private void MuggingReward([FromSource] Player ply)
        {
            var amount = _random.Next(_minReward, _maxReward);
            MoneyManager.Instance.AddMoney(ply,MoneyTypes.Cash,amount);

        }
    }
}
