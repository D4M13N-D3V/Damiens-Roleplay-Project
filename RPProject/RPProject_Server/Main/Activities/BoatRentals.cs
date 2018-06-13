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

        private void VehicleRentalReturnRequest([FromSource] Player player, string s)
        {
            MoneyManager.Instance.AddMoney(player, MoneyTypes.Bank, _rentalPrices[s]);
            TriggerClientEvent(player, "BoatReturnRequest");
        }

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
