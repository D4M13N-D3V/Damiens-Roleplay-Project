using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using roleplay.Main.Vehicles;
using CitizenFX.Core;
using System.Dynamic;
using roleplay.Main.Users;
using roleplay.Users.Inventory;

namespace roleplay.Main
{

    public class VehicleKeysItem : Item
    {

    }

    public class VehicleForSale
    {
        public string Model;
        public int Price;
        public int Stock;

        public VehicleForSale(string model, int price, int stock)
        {
            Model = model;
            Price = price;
            Stock = stock;
        }
    }

    public class VehicleManager:BaseScript
    {
        public static VehicleManager Instance;
            
        public VehicleManager()
        {
            Instance = this;
            LoadVehicles();
            CommandManager.Instance.AddCommand("transfercar", TransferCar);
            CommandManager.Instance.AddCommand("givecar", TransferCar);
            CommandManager.Instance.AddCommand("insurance", InsureCar);
            LoadVehicleShop();
            EventHandlers["BuyVehicle"] += new Action<Player, string, string>(BuyVehicle);
            EventHandlers["SetCarStatus"] += new Action<Player, string, int>(SetCarStatus);
            EventHandlers["PullCarRequest"] += new Action<Player, string>(PullCarRequest);
        }

        private void InsureCar(User user, string[] args)
        {
            if (args.Length >= 2)
            {
                var plate = args[1];
                if (LoadedVehicles.ContainsKey(plate))
                {
                    if (user.CurrentCharacter.Money.Bank >= LoadedVehicles[plate].Price / 4)
                    {
                        MoneyManager.Instance.RemoveMoney(user.Source, MoneyTypes.Bank, LoadedVehicles[plate].Price/4);
                        LoadedVehicles[plate].Status = VehicleStatuses.Stored;
                        Utility.Instance.SendChatMessage(user.Source,"[Vehicle Insurance]","You have claimed insurance on your vehicle. It costs 1/4th the price of the car.",255,255,0);
                    }
                    else if (user.CurrentCharacter.Money.Cash >= LoadedVehicles[plate].Price / 4)
                    {
                        MoneyManager.Instance.RemoveMoney(user.Source, MoneyTypes.Cash, LoadedVehicles[plate].Price / 4);
                        LoadedVehicles[plate].Status = VehicleStatuses.Stored;
                        Utility.Instance.SendChatMessage(user.Source, "[Vehicle Insurance]", "You have claimed insurance on your vehicle. It costs 1/4th the price of the car.", 255, 255, 0);
                    }
                    else
                    {

                        Utility.Instance.SendChatMessage(user.Source, "[Vehicle Insurance]", "You cant claim insurance on your vehicle. It costs 1/4th the price of the car.", 255, 255, 0);
                    }
                }
            }
        }

        private void TransferCar(User user, string[] args)
        {
            Utility.Instance.SendChatMessage(user.Source,"[Vehicle Transfers]"," THIS IS NO LONGER NEEDEd, WHO EVER USES THE KEY IS THE REGISTERED OWNER OF THE VEHICLE WHEN IT SPAWNS IN :)",255,0,0);
        }


        #region Vehicle Prices
        public Dictionary<string,VehicleForSale> VehiclePrices = new Dictionary<string, VehicleForSale>();
        #endregion 

        public Dictionary<string,Vehicle> LoadedVehicles = new Dictionary<string, Vehicle>();

        private async Task LoadVehicleShop()
        {
            while (Utility.Instance == null)
            {
                await Delay(1);
            }
            while (DatabaseManager.Instance == null)
            {
                await Delay(1);
            }
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM VEHICLESHOP ORDER BY price ASC;");
            while (data.Read())
            {
                var model = Convert.ToString(data["model"]);
                var price = Convert.ToInt32(data["price"]);
                var stock = Convert.ToInt32(data["stock"]);
                VehiclePrices.Add(model, new VehicleForSale(model,price,stock));
            }
            DatabaseManager.Instance.EndQuery(data);
        }

        public void RefreshVehicleMenuPlayer(Player ply)
        {
            var models = new List<dynamic>();
            foreach (var storeitem in VehiclePrices)
            {
                dynamic obj = new ExpandoObject();
                obj.model = storeitem.Value.Model;
                obj.price = storeitem.Value.Price;
                obj.stock = storeitem.Value.Stock;
                models.Add(obj);
            }
            TriggerClientEvent(ply, "UpdateVehicleStoreUI", models);
        }

        public void RefreshVehicleMenu()
        {
            var models = new List<dynamic>();
            foreach (var storeitem in VehiclePrices)
            {
                dynamic obj = new ExpandoObject();
                obj.model = storeitem.Value.Model;
                obj.price = storeitem.Value.Price;
                obj.stock = storeitem.Value.Stock;
                models.Add(obj);
            }
            TriggerClientEvent("UpdateVehicleStoreUI", models);
        }

        private async Task LoadVehicles()
        {
            while (Utility.Instance == null)
            {
                await Delay(1);
            }
            while (DatabaseManager.Instance == null)
            {
                await Delay(1);
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
                LoadedVehicles[veh.Plate].id = Convert.ToInt32(data["id"]);
                ItemManager.Instance.DynamicCreateItem(veh.Name+"-"+veh.Plate,"Vehicle Keys",0,0,0,false );
            }
            Utility.Instance.Log("Loaded Vehicles");
            DatabaseManager.Instance.EndQuery(data);
        }

        private void BuyVehicle([FromSource] Player ply, string name, string model)
        {
            if (VehiclePrices.ContainsKey(model) && VehiclePrices[model].Stock <= 1) { Utility.Instance.SendChatMessage(ply, "[Vehicle Shop]", "Not enough of these in stock.", 255, 255, 0); return; }
            var user = UserManager.Instance.GetUserFromPlayer(ply);
            var chara = user.CurrentCharacter;
            var price = VehiclePrices[model].Price;
            if (MoneyManager.Instance.GetMoney(ply, MoneyTypes.Cash) >= price)
            {
                MoneyManager.Instance.RemoveMoney(ply, MoneyTypes.Cash, price);
                var plate = GetRandomPlateThatsNotTaken();
                var vehicle = new Vehicle(name, model, VehicleStatuses.Stored, price, plate);
                vehicle.RegisteredOwner = chara.FirstName + " " + chara.LastName;
                var item = ItemManager.Instance.DynamicCreateItem(vehicle.Name + "-" + vehicle.Plate, "Vehicle Keys", 0, 0, 0, false);
                LoadedVehicles.Add(vehicle.Plate, vehicle);
                InventoryManager.Instance.AddItem(item.Id, 1, ply);
                DatabaseManager.Instance.Execute("INSERT INTO VEHICLES (vehicle) VALUES('" + JsonConvert.SerializeObject(vehicle) + "');");
                Debug.WriteLine(ItemManager.Instance.LoadedItems[item.Id].Name);
                Utility.Instance.Log(ply.Name + " bought a vehicle! [" + name + "]");
                VehiclePrices[model].Stock = VehiclePrices[model].Stock - 1;
                DatabaseManager.Instance.Execute("UPDATE VEHICLESHOP SET stock=" + (VehiclePrices[model].Stock - 1) + " WHERE model='" + model + "';");
                RefreshVehicleMenu();
            }
            else if (MoneyManager.Instance.GetMoney(ply, MoneyTypes.Bank) >= price)
            {
                MoneyManager.Instance.RemoveMoney(ply, MoneyTypes.Bank, price);
                var plate = GetRandomPlateThatsNotTaken();
                var vehicle = new Vehicle(name, model, VehicleStatuses.Stored, price, plate);
                vehicle.RegisteredOwner = chara.FirstName + " " + chara.LastName;
                var item = ItemManager.Instance.DynamicCreateItem(vehicle.Name + "-" + vehicle.Plate, "Vehicle Keys", 0, 0, 0, false);
                LoadedVehicles.Add(vehicle.Plate, vehicle);
                InventoryManager.Instance.AddItem(item.Id, 1, ply);
                DatabaseManager.Instance.Execute("INSERT INTO VEHICLES (vehicle) VALUES('" + JsonConvert.SerializeObject(vehicle) + "');");
                Utility.Instance.Log(ply.Name + " bought a vehicle! [" + name + "]");
                VehiclePrices[model].Stock = VehiclePrices[model].Stock - 1;
                DatabaseManager.Instance.Execute("UPDATE VEHICLESHOP SET stock=" + (VehiclePrices[model].Stock - 1) + " WHERE model='" + model + "';");
                RefreshVehicleMenu();
            }
            else
            {
                Utility.Instance.SendChatMessage(ply, "[Vehicle Shop]", "Not enough cash or money in the bank to buy this vehicle!.", 255, 255, 0);
                return;
            }
        }

        public void CreateVehicleKey(string name, string model,Player ply)
        {
            var chara = UserManager.Instance.GetUserFromPlayer(ply).CurrentCharacter;
            var plate = GetRandomPlateThatsNotTaken();
            var vehicle = new Vehicle(name, model, VehicleStatuses.Stored, 0, plate);
            vehicle.RegisteredOwner = chara.FirstName + " " + chara.LastName;
            var item = ItemManager.Instance.DynamicCreateItem(vehicle.Name + "-" + vehicle.Plate, "Vehicle Keys", 0, 0, 0, false);
            LoadedVehicles.Add(vehicle.Plate, vehicle);
            InventoryManager.Instance.AddItem(item.Id, 1, ply);
            DatabaseManager.Instance.Execute("INSERT INTO VEHICLES (vehicle) VALUES('" + JsonConvert.SerializeObject(vehicle) + "');");
            RefreshVehicleMenu();
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
                if (veh.Status == VehicleStatuses.Stored)
                {
                    veh.Status = VehicleStatuses.Missing;
                    if (veh.RegisteredOwner != name)
                    {
                        veh.RegisteredOwner = name;
                        DatabaseManager.Instance.Execute("UPDATE VEHICLES SET vehicle='"+JsonConvert.SerializeObject(veh)+"' WHERE id="+veh.id+";");
                    }
                    LoadedVehicles[plate] = veh;

                    dynamic obj = new ExpandoObject();
                    obj.Name = veh.Name;
                    obj.Model = veh.Model;
                    obj.RegisteredOwner = veh.RegisteredOwner;
                    obj.Price = veh.Price; 
                    obj.SellPrice = veh.SellPrice;
                    obj.InsurancePrice = veh.InsurancePrice;
                    obj.Insured = veh.Insured;
                    obj.Plate = veh.Plate;
                    obj.ColorPrimary = veh.ColorPrimary;
                    obj.ColorSecondary = veh.ColorSecondary;
                    obj.FrontNeons = veh.FrontNeons;
                    obj.BackNeons = veh.BackNeons;
                    obj.LeftNeons = veh.LeftNeons;
                    obj.RightNeons = veh.RightNeons;
                    obj.WindowTint = veh.WindowTint;
                    obj.WheelType = veh.WheelType;
                    obj.BulletProof = veh.BulletProof;
                    obj.VehicleMod1 = veh.VehicleMod1;
                    obj.VehicleMod2 = veh.VehicleMod2;
                    obj.VehicleMod3 = veh.VehicleMod3;
                    obj.VehicleMod4 = veh.VehicleMod4;
                    obj.VehicleMod5 = veh.VehicleMod5;
                    obj.VehicleMod6 = veh.VehicleMod6;
                    obj.VehicleMod7 = veh.VehicleMod7;
                    obj.VehicleMod8 = veh.VehicleMod8;
                    obj.VehicleMod9 = veh.VehicleMod9;
                    obj.VehicleMod10 = veh.VehicleMod10;
                    obj.VehicleMod11 = veh.VehicleMod11;
                    obj.VehicleMod12 = veh.VehicleMod12;
                    obj.VehicleMod13 = veh.VehicleMod13;
                    obj.VehicleMod14 = veh.VehicleMod14;
                    obj.VehicleMod15 = veh.VehicleMod15;
                    obj.VehicleMod16 = veh.VehicleMod16;
                    obj.VehicleMod17 = veh.VehicleMod17;
                    obj.VehicleMod18 = veh.VehicleMod18;
                    obj.VehicleMod19 = veh.VehicleMod19;
                    obj.VehicleMod20 = veh.VehicleMod20;
                    obj.VehicleMod21 = veh.VehicleMod21;
                    obj.VehicleMod22 = veh.VehicleMod22;
                    obj.VehicleMod23 = veh.VehicleMod23;
                    obj.VehicleMod24 = veh.VehicleMod24;
                    obj.VehicleMod25 = veh.VehicleMod25;
                    obj.VehicleMod26 = veh.VehicleMod26;
                    obj.VehicleMod27 = veh.VehicleMod27;
                    obj.VehicleMod28 = veh.VehicleMod28;
                    obj.VehicleMod29 = veh.VehicleMod29;
                    obj.VehicleMod30 = veh.VehicleMod30;
                    obj.VehicleMod31 = veh.VehicleMod31;
                    obj.VehicleMod32 = veh.VehicleMod32;
                    obj.VehicleMod33 = veh.VehicleMod33;
                    obj.VehicleMod34 = veh.VehicleMod34;
                    obj.VehicleMod35 = veh.VehicleMod35;
                    obj.VehicleMod36 = veh.VehicleMod36;
                    obj.VehicleMod37 = veh.VehicleMod37;
                    obj.VehicleMod38 = veh.VehicleMod38;
                    obj.VehicleMod39 = veh.VehicleMod39;
                    obj.VehicleMod40 = veh.VehicleMod40;
                    obj.VehicleMod41 = veh.VehicleMod41;
                    obj.VehicleMod42 = veh.VehicleMod42;
                    obj.VehicleMod43 = veh.VehicleMod43;
                    obj.VehicleMod44 = veh.VehicleMod44;
                    obj.VehicleMod45 = veh.VehicleMod45;
                    obj.VehicleMod46 = veh.VehicleMod46;
                    obj.VehicleMod47 = veh.VehicleMod47;
                    obj.VehicleMod48 = veh.VehicleMod48;
                    obj.VehicleMod49 = veh.VehicleMod49;
                    obj.VehicleMod50 = veh.VehicleMod50;

                    TriggerClientEvent(ply,"PullCar", obj);
                }
                else if (veh.RegisteredOwner == name && veh.Status == VehicleStatuses.Missing)
                {
                    LoadedVehicles[plate] = veh;
                    TriggerClientEvent(ply,"PutAwayCar",veh.Plate);
                }
            }
        }

        private void SetCarStatus([FromSource] Player ply, string plate, int status)
        {
            var user = UserManager.Instance.GetUserFromPlayer(ply);
            if (user.CurrentCharacter.FirstName + " " + user.CurrentCharacter.LastName ==
                LoadedVehicles[plate].RegisteredOwner)
            {
                LoadedVehicles[plate].Status = (VehicleStatuses)status;
            }
        }

        public void ChangeVehicleOwner(string plate, string name)
        {
            var veh = LoadedVehicles[plate];
            veh.RegisteredOwner = name;
            LoadedVehicles[plate] = veh;
            DatabaseManager.Instance.Execute("UPDATE VEHICLES SET vehicle='"+JsonConvert.SerializeObject(veh)+"' WHERE id="+veh.id+";");
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
