using System;
using System.Collections.Generic;
using CitizenFX.Core;
using server.Main.Users;

namespace server.Main.Activities
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
                new TruckingDestination("Davis Ave. Shopping Center", 242.32659912109f, -1517.1733398438f, 28.644651412964f, 100),
                new TruckingDestination("Davis Ave. 24/7 Shop", -66.943641662598f, -1742.4818115234f, 28.814619064331f, 100),
                new TruckingDestination("Innocence Blvd 24/7 Shop", 27.832950592041f, -1306.0754394531f, 28.575904846191f, 150),
                new TruckingDestination("Elgin Ave. Ammunation", 37.941116333008f, -1102.9970703125f, 28.795278549194f, 150),
                new TruckingDestination("Banner Hotel", -287.2790222168f, -1028.03125f, 29.895013809204f, 200),
                new TruckingDestination("Clinton Ave. 24/7 Shop", 367.90087890625f, 339.16262817383f, 102.79844665527f, 200),
                new TruckingDestination("Split Sides Comedy Club", -422.29284667969f, 293.78341674805f, 82.739112854004f, 250),
                new TruckingDestination("Tequilala Bar & Grill", -554.65325927734f, 302.55145263672f, 83.225646972656f, 250),
                new TruckingDestination("Bahama Mamas", -1365.8630371094f, -589.89636230469f, 29.00389289856f, 250),

new TruckingDestination("Construciton Site, Paleto, Procopio Dr",79.707107543946f,6556.98046875f,31.302101135254f,580),
new TruckingDestination("Paleto Blvd Truck Stop",134.97045898438f,6622.0288085938f,31.52738571167f,540),
new TruckingDestination("Paleto Blvd/Great Ocen Fwy Dream View Motel",-85.227966308594f,6346.3979492188f,31.259307861328f,570),
new TruckingDestination("Cluckin Bell Paleto Blvd",-76.242240905762f,6269.8598632812f,31.144611358642f,630),
new TruckingDestination("Cluckin Bell Paleto Blvd 2",-128.8243560791f,6215.5913085938f,30.972526550292f,620),
new TruckingDestination("Jetsam Paleto Bay Blvd Baydoor",-249.0103302002f,6138.8979492188f,30.927858352662f,600),
new TruckingDestination("Great Ocean Hwy Resteraunt Hookies",-2200.3666992188f,4261.4013671875f,47.744899749756f,500),
new TruckingDestination("Great Ocean Hwy Shopping Center",-3149.0107421875f,1075.3111572266f,20.445943832398f,450),
new TruckingDestination("Inseno Road, Great Ocean 24/7",-3045.2829589844f,603.07781982422f,7.1277832984924f,360),
new TruckingDestination("Great Ocean Hwy Pacif,ic Bluf,f,s Country Club",-3019.8627929688f,97.129119873046f,11.395851135254f,450),
new TruckingDestination("Pipeline Inn, Del Perro Fwy, Great Ocean Hway",-2181.5187988282f,-381.40686035156f,13.021412849426f,300),
new TruckingDestination("East GAlileo Ave, Galileo Park Observatory",-412.65979003906f,1177.7895507812f,325.408203125f,300),
new TruckingDestination("Hollywood Sign Utility Center",813.57489013672f,1276.2419433594f,360.2700805664f,350),
new TruckingDestination("Bolingbroke State Prison",1865.3829345704f,2605.6430664062f,45.423557281494f,400),
new TruckingDestination("Sandy Shores Airfield",1731.0590820312f,3310.7241210938f,40.974891662598f,345),
new TruckingDestination("UTool Senora Fwy",2761.9626464844f,3468.806640625f,55.413646697998f,400),
new TruckingDestination("F,rankies Auto, Grapeseed",2305.3835449218f,4888.6489257812f,41.549850463868f,485),
new TruckingDestination("Grapeseed Airf,ield",2116.720703125f,4795.4140625f,40.845790863038f,500),
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
