using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace roleplay.Main.Users
{
    public enum MoneyTypes { Cash, Bank, Untaxed }
    public class MoneyManager : BaseScript
    {
        public static MoneyManager Instance;
        public MoneyManager()
        {
            Instance = this;
        }

        private void RefreshMoney(Player player)
        {
            if (player != null)
            {
                TriggerClientEvent(player,"RefreshMoney", GetMoney(player,MoneyTypes.Cash),GetMoney(player,MoneyTypes.Bank),GetMoney(player,MoneyTypes.Untaxed));
            }
        }

        public void AddMoney(Player player, MoneyTypes type, int amount)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var character = user.CurrentCharacter;
            var money = character.Money;
            switch (type)
            {
                case MoneyTypes.Cash:
                    money.Cash += amount;
                    Utility.Instance.SendChatMessage(player, "MONEY", amount.ToString() + " has been added to your cash.", 0, 170, 60);
                    break;
                case MoneyTypes.Bank:
                    money.Bank += amount;
                    Utility.Instance.SendChatMessage(player, "MONEY", amount.ToString() + " has been added to your bank account.", 0, 170, 60);
                    break;
                case MoneyTypes.Untaxed:
                    money.UnTaxed += amount;
                    Utility.Instance.SendChatMessage(player, "MONEY", amount.ToString() + " has been added to your untaxed stash.", 0, 170, 60);
                    break;
            }

            RefreshMoney(player);
            user.CurrentCharacter.Money = money;
        }

        public void RemoveMoney(Player player, MoneyTypes type, int amount)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var character = user.CurrentCharacter;
            var money = character.Money;
            switch (type)
            {
                case MoneyTypes.Cash:
                    money.Cash -= amount;
                    Utility.Instance.SendChatMessage(player,"MONEY",amount.ToString()+" has been removed from your cash.",0,170,60);
                    break;
                case MoneyTypes.Bank:
                    money.Bank -= amount;
                    Utility.Instance.SendChatMessage(player, "MONEY", amount.ToString() + " has been removed from your bank account.", 0, 170, 60);
                    break;
                case MoneyTypes.Untaxed:
                    money.UnTaxed -= amount;
                    Utility.Instance.SendChatMessage(player, "MONEY", amount.ToString() + " has been removed from your untaxed stash.", 0, 170, 60);
                    break;
            }

            RefreshMoney(player);
            user.CurrentCharacter.Money = money;
        }

        public void SetMoney(Player player, MoneyTypes type, int amount)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var character = user.CurrentCharacter;
            var money = character.Money;
            switch (type)
            {
                case MoneyTypes.Cash:
                    money.Cash = amount;
                    Utility.Instance.SendChatMessage(player, "MONEY","You cash has been set to "+ amount +".", 0, 170, 60);
                    break;
                case MoneyTypes.Bank:
                    money.Bank = amount;
                    Utility.Instance.SendChatMessage(player, "MONEY", "You bank account has been set to " + amount + ".", 0, 170, 60);
                    break;
                case MoneyTypes.Untaxed:
                    money.UnTaxed = amount;
                    Utility.Instance.SendChatMessage(player, "MONEY", "You untaxed stash has been set to " + amount + ".", 0, 170, 60);
                    break;
            }

            RefreshMoney(player);
            user.CurrentCharacter.Money = money;
        }

        public int GetMoney(Player player, MoneyTypes type)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var character = user.CurrentCharacter;
            var money = character.Money;
            switch (type)
            {
                case MoneyTypes.Cash:
                    return money.Cash;
                case MoneyTypes.Bank:
                    return money.Bank ;
                case MoneyTypes.Untaxed:
                    return money.UnTaxed;
            }
            return -1;
        }
    }

}
