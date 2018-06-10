using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using roleplay;
using roleplay.Main;
using roleplay.Main.Users;
using roleplay.Main.Vehicles;

public class RentalSpot : BaseScript
{
    public static RentalSpot Instance;

    private Dictionary<string, int> _rentalPrices = new Dictionary<string, int>()
    {
        ["BMX"] = 25,
        ["Tornado"] = 800,
        ["FQ2"] = 700,
        ["Asea"] = 1000,
        ["Emperor"] = 800,
        ["Emperor2"] = 700,
        ["Faggio3"] = 200,
        ["Ingot"] = 500,
        ["Stratum"] = 500,
    };
    public RentalSpot()
    {
        Instance = this;
        EventHandlers["VehicleRentalRequest"] += new Action<Player, string>(VehicleRentalRequest);
        EventHandlers["VehicleRentalReturnRequest"] += new Action<Player, string>(VehicleRentalReturnRequest);
    }

    private void VehicleRentalReturnRequest([FromSource] Player player, string s)
    {
        MoneyManager.Instance.AddMoney(player,MoneyTypes.Bank,_rentalPrices[s]);
        TriggerClientEvent(player,"VehicleReturnRequest");
    }

    private void VehicleRentalRequest([FromSource]Player player, string s)
    {
        var user = UserManager.Instance.GetUserFromPlayer(player);
        if (user.CurrentCharacter.Money.Bank >= _rentalPrices[s])
        {
            MoneyManager.Instance.RemoveMoney(player, MoneyTypes.Bank, _rentalPrices[s]);
            TriggerClientEvent(player,"VehicleRentalRequestClient", s);
        }
        else if (user.CurrentCharacter.Money.Cash >= _rentalPrices[s])
        {
            MoneyManager.Instance.RemoveMoney(player, MoneyTypes.Cash, _rentalPrices[s]);
            TriggerClientEvent(player,"VehicleRentalRequestClient", s);
        }
        else
        {
            Utility.Instance.SendChatMessage(player,"[Vehicle Rentals]","Not enough money.",255,255,0);
        }
    }
}
