using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using server.Main.Users;

namespace server.Main.Activities
{
    /// <summary>
    /// Manager for boat rentals.
    /// </summary>
    public class TrailRentals : BaseScript
    {
        public static TrailRentals Instance;

        /// <summary>
        /// Dictionary for the spawn names of a vehicle, and the prices to rent them.
        /// </summary>
        private Dictionary<string, int> _rentalPrices = new Dictionary<string, int>()
        {
            ["Scorcher"] = 200,
            ["Blazer"] = 400,
            ["Sanchez"] = 600,
            ["Blazer3"] = 800,
        };

        public TrailRentals()
        {
            Instance = this;
            EventHandlers["TrailRentalRequest"] += new Action<Player, string>(VehicleRentalRequest);
            EventHandlers["TrailReturnRequest"] += new Action<Player, string>(VehicleRentalReturnRequest);
        }

        /// <summary>
        /// Event handler fore when someone trys to return a rental. Gives money back.
        /// </summary>
        /// <param name="player">Player calling it</param>
        /// <param name="s">vehicle model name.</param>
        private void VehicleRentalReturnRequest([FromSource] Player player, string s)
        {
            MoneyManager.Instance.AddMoney(player, MoneyTypes.Bank, _rentalPrices[s]);
            TriggerClientEvent(player, "TrailReturnRequest");
        }

        /// <summary>
        /// Event handler for when someone trys to 
        /// </summary>
        /// <param name="player">the player calling it</param>
        /// <param name="s">vehicle model name that would be in dictionary</param>
        private void VehicleRentalRequest([FromSource]Player player, string s)
        {
            Debug.WriteLine(s);
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (user.CurrentCharacter.Money.Bank >= _rentalPrices[s])
            {
                MoneyManager.Instance.RemoveMoney(player, MoneyTypes.Bank, _rentalPrices[s]);
                TriggerClientEvent(player, "TrailRentalRequest", s);
            }
            else if (user.CurrentCharacter.Money.Cash >= _rentalPrices[s])
            {
                MoneyManager.Instance.RemoveMoney(player, MoneyTypes.Cash, _rentalPrices[s]);
                TriggerClientEvent(player, "TrailRentalRequest", s);
            }
            else
            {
                Utility.Instance.SendChatMessage(player, "[Vehicle Rentals]", "Not enough money.", 255, 255, 0);
            }
        }
    }
}
