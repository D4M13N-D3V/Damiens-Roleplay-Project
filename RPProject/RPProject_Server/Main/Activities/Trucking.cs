using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using roleplay.Main.Users;

namespace roleplay.Main.Activities
{

    public enum LoadTypes
    {
        Tanker,
        Logs,
        Trailer,
        DockTrailer,
        Cars,
        Boat
    }

    public class TruckingDestination
    {
        public string Name;
        public float X;
        public float Y;
        public float Z;
        public int Payout;

        public TruckingDestination(string name, float x, float y, float z, int payout)
        {
            Name = name;
            X = x;
            Y = y;
            Z = z;
            Payout = payout;
        }

    }
    public class Trucking:BaseScript
    {
        public static Trucking Instance;


        private Dictionary<string, int> _rentalPrices = new Dictionary<string, int>()
        {
            ["Hauler"] = 2000,
            ["Phantom"] = 5000,
            ["Packer"] = 5000
        };

        public Trucking()
        {
            Instance = this;
            EventHandlers["TruckingRentalRequest"] += new Action<Player,string>(TruckRentalRequest);
            EventHandlers["TruckReturnRequest"] += new Action<Player>(TruckReturnRequest);
            EventHandlers["SuccessfulTruckRentalReturn"] += new Action<Player, string>(TruckDepositRefund);
            EventHandlers["CompletedTruckerMission"] += new Action<Player, string>(DeliverySuccesful);
        }

        private Dictionary<LoadTypes, List<TruckingDestination>> _destinations = new Dictionary<LoadTypes, List<TruckingDestination>>()
        {
            [LoadTypes.Trailer] = new List<TruckingDestination>()
            {
                new TruckingDestination("Davis Ave. Shopping Center", 242.32659912109f, -1517.1733398438f, 28.644651412964f, 400),
                new TruckingDestination("Davis Ave. 24/7 Shop", -66.943641662598f, -1742.4818115234f, 28.814619064331f, 500),
                new TruckingDestination("Innocence Blvd 24/7 Shop", 27.832950592041f, -1306.0754394531f, 28.575904846191f, 300),
                new TruckingDestination("Elgin Ave. Ammunation", 37.941116333008f, -1102.9970703125f, 28.795278549194f, 300),
                new TruckingDestination("Banner Hotel", -287.2790222168f, -1028.03125f, 29.895013809204f, 400),
                new TruckingDestination("Clinton Ave. 24/7 Shop", 367.90087890625f, 339.16262817383f, 102.79844665527f, 600),
                new TruckingDestination("Split Sides Comedy Club", -422.29284667969f, 293.78341674805f, 82.739112854004f, 700),
                new TruckingDestination("Tequilala Bar & Grill", -554.65325927734f, 302.55145263672f, 83.225646972656f, 700),
                new TruckingDestination("Bahama Mamas", -1365.8630371094f, -589.89636230469f, 29.00389289856f, 700),
            }
        };
        private void DeliverySuccesful([FromSource] Player ply, string destination)
        {
            foreach (var var in _destinations.Values)
            {
                foreach (var var2 in var)
                {
                    if (var2.Name == destination)
                    {
                        MoneyManager.Instance.AddMoney(ply,MoneyTypes.Bank,var2.Payout);
                        return;
                    }
                }
            }
        }

        private void TruckRentalRequest([FromSource] Player ply, string truck)
        {
            if (_rentalPrices.ContainsKey(truck))
            {
                var user = UserManager.Instance.GetUserFromPlayer(ply);
                var cost = _rentalPrices[truck];
                if (MoneyManager.Instance.GetMoney(ply, MoneyTypes.Cash) >= _rentalPrices[truck])
                {
                    MoneyManager.Instance.RemoveMoney(ply,MoneyTypes.Cash,cost);
                    TriggerClientEvent(ply, "RentTruck", truck);
                }
                else if (MoneyManager.Instance.GetMoney(ply, MoneyTypes.Bank) >= _rentalPrices[truck])
                {
                    MoneyManager.Instance.RemoveMoney(ply, MoneyTypes.Bank, cost);
                    TriggerClientEvent(ply,"RentTruck",truck);
                }
            }
        }

        private void TruckReturnRequest([FromSource] Player ply)
        {
            //This is for security reasons. So people cant just call this event with a cheat tool to give them selfs money over and over.
            TriggerClientEvent(ply,"AttemptReturnTruck");
        }

        private void TruckDepositRefund([FromSource] Player ply, string truck)
        {
            Debug.Write(truck);
            if (_rentalPrices.ContainsKey(truck))
            {
                MoneyManager.Instance.AddMoney(ply,MoneyTypes.Bank,_rentalPrices[truck]);
            }
        }
    }
}
