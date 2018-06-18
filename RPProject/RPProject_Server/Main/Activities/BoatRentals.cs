using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using server.Main.Users;

namespace server.Main.Activities
{
    public class BoatRentals : BaseScript
    {
        public static BoatRentals Instance;

        /// <summary>
        /// Dictionary for the spawn names of a vehicle, and the prices to rent them.
        /// </summary>
        private Dictionary<string, int> _rentalPrices = new Dictionary<string, int>()
        {
            ["Tug"] = 500,
            ["Marquis"] = 1000,
            ["Suntrap"] = 1650,
            ["Tropic"] = 2000,
            ["Tropic2"] = 2000,
            ["Speeder"] = 5000,
            ["Speeder2"] = 5000,
            ["Toro"] = 5000,
            ["Toro2"] = 5000,
        };

        public BoatRentals()
        {
            Instance = this;
            EventHandlers["BoatRentalRequest"] += new Action<Player, string>(VehicleRentalRequest);
            EventHandlers["BoatReturnRequest"] += new Action<Player, string>(VehicleRentalReturnRequest);
        }

        /// <summary>
        /// Event handler fore when someone trys to return a rental. Gives money back.
        /// </summary>
        /// <param name="player">Player calling it</param>
        /// <param name="s">vehicle model name.</param>
        private void VehicleRentalReturnRequest([FromSource] Player player, string s)
        {
            MoneyManager.Instance.AddMoney(player, MoneyTypes.Bank, _rentalPrices[s]);
            TriggerClientEvent(player, "BoatReturnRequest");
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
                TriggerClientEvent(player, "BoatRentalRequest", s);
            }
            else if (user.CurrentCharacter.Money.Cash >= _rentalPrices[s])
            {
                MoneyManager.Instance.RemoveMoney(player, MoneyTypes.Cash, _rentalPrices[s]);
                TriggerClientEvent(player, "BoatRentalRequest", s);
            }
            else
            {
                Utility.Instance.SendChatMessage(player, "[Vehicle Rentals]", "Not enough money.", 255, 255, 0);
            }
        }
    }
}
