using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using server.Main.Users;

namespace server.Main.Housing
{
    public class Manager : BaseScript
    {
        public static Manager Instance;

        public Dictionary<int,House> Houses = new Dictionary<int, House>();

        public Manager()
        {
            Instance = this;
            LoadHomes();
            SetupCommands();
            EventHandlers["Housing:Buy"] += new Action<Player, int>(HousingBuyEvent);
            EventHandlers["Housing:Sell"] += new Action<Player, int>(HousingSellEvent);
            EventHandlers["Housing:StopSell"] += new Action<Player, int>(HousingStopSellEvent);
            EventHandlers["Housing:SetPrice"] += new Action<Player, int, int>(HousingSetPriceEvent);
        }

        private void HousingBuyEvent([FromSource] Player player, int i)
        {
            if (Houses.ContainsKey(i))
            {
                Houses[i].BuyHouse(player);
            }
        }

        private void HousingSellEvent([FromSource] Player player, int i)
        {
            if (Houses.ContainsKey(i))
            {
                Houses[i].PostHouseForSale(player);
            }
        }

        private void HousingStopSellEvent([FromSource] Player player, int i)
        {
            if (Houses.ContainsKey(i))
            {
                Houses[i].RemoveHouseForSale(player);
            }
        }

        private void HousingSetPriceEvent([FromSource] Player player, int i, int amount)
        {
            if (Houses.ContainsKey(i))
            {
                Houses[i].SetHousePrice(player,amount);
            }
        }

        private async Task LoadHomes()
        {
            while (DatabaseManager.Instance == null)
            {
                await Delay(0);
            }
            var data = DatabaseManager.Instance.StartQuery("SELECT * FROM HOUSES ORDER BY id ASC;").Result;
            while (data.Read())
            {
                var id = Convert.ToInt32(data["id"]);
                var name = Convert.ToString(data["name"]);
                var description = Convert.ToString(data["description"]);
                var position = JsonConvert.DeserializeObject<Vector3>(Convert.ToString(data["position"]));
                var forSaleBool = Convert.ToInt16(data["forsale"]) == 1;
                var owner = Convert.ToString(data["owner"]);
                var price = Convert.ToInt32(data["price"]);
                var hasGarageBool = Convert.ToInt16(data["hasgarage"]) == 1;
                var garagepos = JsonConvert.DeserializeObject<Vector3>(Convert.ToString(data["garagepos"]));
                var house = new House(id,name,description,position,forSaleBool,owner,price,hasGarageBool,garagepos);
                Houses.Add(id,house);
            }
            DatabaseManager.Instance.EndQuery(data);
        }

        public void SendHouseInfo(Player player)
        {
            List<string> housess = new List<string>();
            foreach (var house in Houses.Values)
            {
                var str = house.Id+"|"+house.Name+"|"+house.Description+"|" + house.Position.X + "|" + house.Position.Y + "|" + house.Position.Z + "|"+house.ForSale+"|"+house.Owner+"|"+house.Price;
                housess.Add(str);
            }
            TriggerClientEvent(player, "Housing:LoadHouses", housess);
        }

        public void SendHouseInfoAll()
        {
            List<string> housess = new List<string>();
            foreach (var house in Houses.Values)
            {
                var str = house.Id + "|" + house.Name + "|" + house.Description + "|" + house.Position.X + "|" + house.Position.Y + "|" + house.Position.Z + "|" + house.ForSale + "|" + house.Owner + "|" + house.Price;
                housess.Add(str);
            }   
            TriggerClientEvent("Housing:LoadHouses", housess);
        }

        public async Task Create(string name, string desc, Vector3 pos, int price)
        {
            if (Houses.Any())
            {
                Debug.Write(Convert.ToString(Houses.Last().Value.Id + 1));
                int toAdd = 1;
                int id = toAdd + 1;
                while (Houses.ContainsKey(id))
                {
                    toAdd++;
                    id = Houses.Last().Value.Id + toAdd;
                }

                var house = new House(id,name,desc,pos,true,"Pineapple Island",price,false, Vector3.Zero);
                Houses.Add(house.Id,house);
                DatabaseManager.Instance.Execute("INSERT INTO HOUSES (id,name,description,position,forsale,owner,price,hasgarage,garagepos)" +
                                                 "VALUES("+house.Id+",'"+house.Name+"','"+house.Description+"'," +
                                                 "'"+JsonConvert.SerializeObject(house.Position)+"',1,'Pineapple Island'," +
                                                 ""+price+",0,'"+JsonConvert.SerializeObject(Vector3.Zero)+"');");
            }
            else
            {
                var house = new House(1, name, desc, pos, true, "Pineapple Island", price, false, Vector3.Zero);
                Houses.Add(house.Id, house);
                DatabaseManager.Instance.Execute("INSERT INTO HOUSES (id,name,description,position,forsale,owner,price,hasgarage,garagepos)" +
                                                 "VALUES(" + house.Id + ",'" + house.Name + "','" + house.Description + "'," +
                                                 "'" + JsonConvert.SerializeObject(house.Position) + "',1,'Pineapple Island'," +
                                                 "" + price + ",0,'" + JsonConvert.SerializeObject(Vector3.Zero) + "');");
            }
            await Delay(1000);
            SendHouseInfoAll();
            Utility.Instance.SendChatMessageAll("[Housing]", "House Created ( "+name+" : "+desc+").", 255, 255, 0);
        }

        public void Delete(int id)
        {
            if (Houses.ContainsKey(id))
            {
                Utility.Instance.SendChatMessageAll("[Housing]", "House Deleted ( " + Houses[id].Name + " : " + Houses[id].Description + ").", 255, 255, 0);
                DatabaseManager.Instance.Execute("DELETE FROM HOUSES WHERE id=" + id + ";");
                Houses.Remove(id);
                SendHouseInfoAll();
            }
        }

        #region

        private async Task SetupCommands()
        {
            while (CommandManager.Instance == null)
            {
                await Delay(100);
            }
            await CommandManager.Instance.AddCommand("createhouse", CreateHouseCommand);
            await CommandManager.Instance.AddCommand("deletehouse", DeleteHouseCommand);
        }

        private async void CreateHouseCommand(User user, string[] args)
        {
            if (user.Permissions < 3) { Utility.Instance.SendChatMessage(user.Source,"[Housing]","You do not have high enough permisison level for this.", 255,255,0); return; }
            if (args.Length < 4) { Utility.Instance.SendChatMessage(user.Source,"[Housing]","Invalid amount of parameters!",255,255,0); return; }

            var name = args[1];
            var desc = args[2];

            if (Int32.TryParse(args[3], out var price))
            {
                Utility.Instance.SendChatMessage(user.Source, "[Housing]", "Creating house...(2 Seconds Please..)", 255, 255, 0);
                await Delay(2000);
                Debug.WriteLine(args[1]);
                Debug.WriteLine(args[2]);
                Debug.Write(Convert.ToString(price));
                Create(name,desc,user.CurrentCharacter.Pos,price);
            }
            else
            {
                Utility.Instance.SendChatMessage(user.Source,"[Housing]","Invalid price.",255,255,0);
            }

        }

        private void DeleteHouseCommand(User user, string[] args)
        {
            if (user.Permissions < 3) { Utility.Instance.SendChatMessage(user.Source, "[Housing]", "You do not have high enough permisison level for this.", 255, 255, 0); return; }
            if (args.Length < 2) { Utility.Instance.SendChatMessage(user.Source, "[Housing]", "Invalid amount of parameters!", 255, 255, 0); return; }

            if (Int32.TryParse(args[1], out var id))
            {
                Delete(id);
            }
            else
            {
                Utility.Instance.SendChatMessage(user.Source, "[Housing]", "Invalid id.", 255, 255, 0);
            }
        }
        #endregion
    }
}
