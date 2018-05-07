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

        public void AddMoney(Player player, MoneyTypes type, int amount)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var character = user.CurrentCharacter;
            var money = character.Money;
            switch (type)
            {
                case MoneyTypes.Cash:
                    money.Cash += amount;
                    break;
                case MoneyTypes.Bank:
                    money.Bank += amount;
                    break;
                case MoneyTypes.Untaxed:
                    money.UnTaxed += amount;
                    break;
            }
        }

        public void RemoveMoney(Player player, MoneyTypes type, int amount)
        {
            var user = UserManager.Instance.GetUserFromPlayer(player);
            var character = user.CurrentCharacter;
            var money = character.Money;
            switch (type)
            {
                case MoneyTypes.Cash:
                    if (money.Cash - amount > 0)
                        money.Cash += amount;
                    break;
                case MoneyTypes.Bank:
                    if (money.Bank - amount > 0)
                        money.Bank += amount;
                    break;
                case MoneyTypes.Untaxed:
                    if (money.UnTaxed - amount > 0)
                        money.UnTaxed += amount;
                    break;
            }
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
                    break;
                case MoneyTypes.Bank:
                    money.Bank = amount;
                    break;
                case MoneyTypes.Untaxed:
                    money.UnTaxed = amount;
                    break;
            }
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
