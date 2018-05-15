using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using roleplay.Main.Vehicles;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using roleplay.Main.Users;

namespace roleplay.Main
{

    public class VehicleKeysItem : Item
    {

    }
    public class VehicleManager:BaseScript
    {
        public static VehicleManager Instance;

        public VehicleManager()
        {
            Instance = this;
            LoadVehicles();
            EventHandlers["BuyVehicle"] += new Action<Player, string, string>(BuyVehicle);
            EventHandlers["PullCarRequest"] += new Action<Player, string>(PullCarRequest);
        }

        #region Vehicle Prices
        private readonly Dictionary<string,int> _vehiclePrices = new Dictionary<string,int>()
        {
            {"Huntley",0}
        };
        #endregion

        public Dictionary<string,Vehicle> LoadedVehicles = new Dictionary<string, Vehicle>();

        private async void LoadVehicles()
        {
            while (Utility.Instance == null || !ItemManager.Instance.ItemsLoaded)
            {
                await Delay(0);
            }
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM VEHICLES");
            while (data.Read())
            {
                var veh = JsonConvert.DeserializeObject<Vehicle>(Convert.ToString(data["vehicle"]));
                if(veh.Status==VehicleStatuses.Missing)
                {
                    veh.Status = VehicleStatuses.Stored;
                }
                LoadedVehicles.Add(Convert.ToString(veh.Plate),veh);
                ItemManager.Instance.DynamicCreateItem(veh.Name+"-"+veh.Plate,"Vehicle Keys",0,0,0,false);
            }
            Utility.Instance.Log("Loaded Vehicles");
            DatabaseManager.Instance.EndQuery(data);
        }

        private void BuyVehicle([FromSource] Player ply, string name, string model)
        {

            var user = UserManager.Instance.GetUserFromPlayer(ply);
            var chara = user.CurrentCharacter;
            var price = _vehiclePrices[name];
            if (MoneyManager.Instance.GetMoney(ply, MoneyTypes.Cash) >= price)
            {
                MoneyManager.Instance.RemoveMoney(ply,MoneyTypes.Cash,price);
                var plate = GetRandomPlateThatsNotTaken();
                var vehicle = new Vehicle(name, model, VehicleStatuses.Stored, price, plate);
                vehicle.RegisteredOwner = chara.FirstName+" "+chara.LastName;
                LoadedVehicles.Add(vehicle.Plate, vehicle);
                var item = ItemManager.Instance.DynamicCreateItem(vehicle.Name + "-" + vehicle.Plate, "Vehicle Keys", 0, 0, 0, false);
                InventoryManager.Instance.AddItem(ply, item.Id, 1);
                DatabaseManager.Instance.Execute("INSERT INTO VEHICLES (vehicle) VALUES('" + JsonConvert.SerializeObject(vehicle) + "');");
                Utility.Instance.Log(ply.Name + " bought a vehicle! [" + name + "]");
            }
            else if (MoneyManager.Instance.GetMoney(ply, MoneyTypes.Bank) >= price)
            {
                MoneyManager.Instance.RemoveMoney(ply, MoneyTypes.Bank, price);
                var plate = GetRandomPlateThatsNotTaken();
                var vehicle = new Vehicle(name, model, VehicleStatuses.Stored, price, plate);
                vehicle.RegisteredOwner = chara.FirstName + " " + chara.LastName;
                LoadedVehicles.Add(vehicle.Plate, vehicle);
                var item = ItemManager.Instance.DynamicCreateItem(vehicle.Name + "-" + vehicle.Plate, "Vehicle Keys", 0, 0, 0, false);
                InventoryManager.Instance.AddItem(ply, item.Id, 1);
                DatabaseManager.Instance.Execute("INSERT INTO VEHICLES (vehicle) VALUES('" + JsonConvert.SerializeObject(vehicle) + "');");
                Utility.Instance.Log(ply.Name + " bought a vehicle! [" + name + "]");
            }
            else
            {
                return;
            }
        }

        private void PullCarRequest([FromSource] Player ply, string plate)
        {
            var user = UserManager.Instance.GetUserFromPlayer(ply);
            var chara = user.CurrentCharacter;
            var name = chara.FirstName + " " + chara.LastName;
            Debug.WriteLine(plate);
            if (LoadedVehicles.ContainsKey(plate))
            {
                var veh = LoadedVehicles[plate];
                if (veh.RegisteredOwner == name && veh.Status == VehicleStatuses.Stored)
                {
                    TriggerClientEvent(ply,"PullCar",veh.Model,veh.Plate);
                }
            }
        }

        private string GetRandomPlateThatsNotTaken()
        {
            var plate = "";
            var taken = true;

            while (taken)
            {
                plate = Utility.Instance.RandomString(8);
                foreach (var plateTmp in LoadedVehicles.Keys)
                {
                    if (plateTmp == plate)
                    {
                        taken = true;
                        break;
                    }
                    taken = false;
                }
                taken = false;
            }
            return plate;
        }

    }
}
