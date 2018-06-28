using CitizenFX.Core;
using server.Main.Users;

namespace server.Main.Housing
{
    public class House
    {
        public int Id;
        public string Name;
        public string Description;
        public Vector3 Position;
        public bool ForSale;
        public string Owner;
        public int Price;

        public bool HasGarage = true;
        public Vector3 GaragePosistion;

        public House(int id, string name, string desc, Vector3 pos, bool forsale, string owner, int price, bool hasgarage, Vector3 garagepos)
        {
            Id = id;
            Name = name;
            Description = desc;
            Position = pos;
            ForSale = forsale;
            Owner = owner;
            Price = price;
            HasGarage = hasgarage;
            GaragePosistion = garagepos;
        }

        public bool BuyHouse(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (user == null) { Utility.Instance.SendChatMessage(player, "[Housing]", "Invalid user provided contact a system administrator.", 255, 255, 0); return false; }
            if (user.CurrentCharacter.FullName == Owner) { Utility.Instance.SendChatMessage(player, "[Housing]", "You own this house you can not buy it, just take it down from being for sale.", 255, 255, 0); return false; }
            if (user.CurrentCharacter.Money.Bank < Price) { Utility.Instance.SendChatMessage(player, "[Housing]", "You do not have enough money in your bank account.", 255, 255, 0); return false; }

            var ownerUser = UserManager.Instance.GetUserFromCharacterName(Owner);
            if (ownerUser != null)
            {
                MoneyManager.Instance.AddMoney(ownerUser.Source, MoneyTypes.Bank, Price);
                MoneyManager.Instance.RemoveMoney(user.Source,MoneyTypes.Bank,Price);
                Utility.Instance.SendChatMessage(ownerUser.Source, "[Housing]", "You have sold your property! (" + Name + ")", 255, 255, 0);
                Utility.Instance.SendChatMessage(player, "[Housing]", "You have bought a property. (" + Name + ")", 255, 255, 0);
                SetOwner(user.CurrentCharacter.FullName);
                RemoveHouseForSale(player);
            }
            else
            {
                MoneyManager.Instance.AddMoneyOffline(Owner,MoneyTypes.Bank,Price);
                MoneyManager.Instance.RemoveMoney(user.Source, MoneyTypes.Bank, Price);
                Utility.Instance.SendChatMessage(player, "[Housing]", "You have bought a property. (" + Name + ")", 255, 255, 0);
                SetOwner(user.CurrentCharacter.FullName);
                RemoveHouseForSale(player);
            }
            return false;
        }

        public bool PostHouseForSale(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (Owner != user.CurrentCharacter.FullName) { Utility.Instance.SendChatMessage(player, "[Housing]", "You do not own this property!", 255, 255, 0); return false; }
            if (ForSale) { Utility.Instance.SendChatMessage(player,"[Housing]","You already have this house posted for sale!",255,255,0); return false; }
            Utility.Instance.SendChatMessage(player, "[Housing]", "House put up for sale!", 255, 255, 0);
            SetForSale(true);
            return true;
        }

        public bool RemoveHouseForSale(Player player)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (Owner != user.CurrentCharacter.FullName) { Utility.Instance.SendChatMessage(player, "[Housing]", "You do not own this property!", 255, 255, 0); return false; }
            if (!ForSale) { Utility.Instance.SendChatMessage(player, "[Housing]", "This house is not for sale!", 255, 255, 0); return false; }
            Utility.Instance.SendChatMessage(player, "[Housing]", "House removed from sale!", 255, 255, 0);
            SetForSale(false);
            return true;
        }

        public bool SetHousePrice(Player player, int amount)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            if (Owner != user.CurrentCharacter.FullName) { Utility.Instance.SendChatMessage(player, "[Housing]", "You do not own this property!", 255, 255, 0); return false; }
            SetPrice(amount);
            Utility.Instance.SendChatMessage(player, "[Housing]", "House price set!", 255, 255, 0);
            return true;
        }

        public void SetForSale(bool forsale)
        {
            ForSale = forsale;
            var forSaleInt = 0;
            if (forsale)
            {
                forSaleInt = 1;
            }
            DatabaseManager.Instance.Execute("UPDATE HOUSES SET forsale='" + forSaleInt + "' WHERE id=" + Id + ";");
            Manager.Instance.SendHouseInfoAll();
        }

        public void SetPrice(int amount)
        {
            Price = amount;
            DatabaseManager.Instance.Execute("UPDATE HOUSES SET price='" + amount + "' WHERE id=" + Id + ";");
            Manager.Instance.SendHouseInfoAll();
        }

        public void SetOwner(string owner)
        {
            Owner = owner;
            DatabaseManager.Instance.Execute("UPDATE HOUSES SET owner='"+owner+"' WHERE id="+Id+";");
            Manager.Instance.SendHouseInfoAll();
        }
    }
}
