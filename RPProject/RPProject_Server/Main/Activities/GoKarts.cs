using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using server.Main.Users;

namespace server.Main.Activities
{
    public class GoKarts : BaseScript
    {
        public static GoKarts Instance;


        private Dictionary<string, int> _rentalPrices = new Dictionary<string, int>()
        {
            ["Kart"] = 100,
        };

        public GoKarts()
        {
            Instance = this;
            EventHandlers["GoKartRentalRequest"] += new Action<Player, string>(VehicleRentalRequest);
            EventHandlers["GoKartReturnRequest"] += new Action<Player, string>(VehicleRentalReturnRequest);
        }

        private void VehicleRentalReturnRequest([FromSource] Player player, string s)
        {
            MoneyManager.Instance.AddMoney(player, MoneyTypes.Bank, _rentalPrices[s]);
            TriggerClientEvent(player, "GoKartReturnRequest");
        }

        private void VehicleRentalRequest([FromSource]Player player, string s)
        {
            Debug.WriteLine(s);
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (user.CurrentCharacter.Money.Bank >= _rentalPrices[s])
            {
                MoneyManager.Instance.RemoveMoney(player, MoneyTypes.Bank, _rentalPrices[s]);
                TriggerClientEvent(player, "GoKartRentalRequest", s);
            }
            else if (user.CurrentCharacter.Money.Cash >= _rentalPrices[s])
            {
                MoneyManager.Instance.RemoveMoney(player, MoneyTypes.Cash, _rentalPrices[s]);
                TriggerClientEvent(player, "GoKartRentalRequest", s);
            }
            else
            {
                Utility.Instance.SendChatMessage(player, "[Vehicle Rentals]", "Not enough money.", 255, 255, 0);
            }
        }
    }
}
