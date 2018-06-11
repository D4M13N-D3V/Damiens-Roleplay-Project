using System;
using CitizenFX.Core;

namespace server.Main.Users
{
    public enum MoneyTypes { Cash, Bank, Untaxed }
    public class MoneyManager : BaseScript
    {
        public static MoneyManager Instance;
        public MoneyManager()
        {
            Instance = this;
            EventHandlers["WithdrawMoney"] += new Action<Player, int>(WithdrawMoney);
            EventHandlers["DepositMoney"] += new Action<Player, int>(DepositMoney);
            EventHandlers["TransferMoney"] += new Action<Player, int, int>(TransferMoney);
            EventHandlers["TransferCash"] += new Action<Player, int, int>(TransferCash);
        }
        

        private void WithdrawMoney([FromSource] Player player, int amount)
        {
            if (amount <= 0)
            {
                Utility.Instance.SendChatMessage(player, "[Bank]", "Invalid amount", 0, 150, 0);
                return;
            }

            if (MoneyManager.Instance.GetMoney(player, MoneyTypes.Bank) < amount)
            {
                Utility.Instance.SendChatMessage(player, "[Bank]", "Not enough balance.", 0, 150, 0);
                return;
            }

            var user = UserManager.Instance.GetUserFromPlayer(player);
            Utility.Instance.SendChatMessage(player, "[Bank]", "You have withdrawled " + amount + " from your bank account.", 0, 150, 0);
            MoneyManager.Instance.RemoveMoney(player, MoneyTypes.Bank, amount);
            MoneyManager.Instance.AddMoney(player, MoneyTypes.Cash, amount);
        }

        private void DepositMoney([FromSource] Player player, int amount)
        {
            if (amount <= 0)
            {
                Utility.Instance.SendChatMessage(player, "[Bank]", "Invalid amount", 0, 150, 0);
                return;
            }

            if (MoneyManager.Instance.GetMoney(player, MoneyTypes.Cash) < amount)
            {
                Utility.Instance.SendChatMessage(player, "[Bank]", "Not enough cash.", 0, 150, 0);
                return;
            }

            var user = UserManager.Instance.GetUserFromPlayer(player);
            Utility.Instance.SendChatMessage(player, "[Bank]", "You have deposited " + amount + " into your bank account.", 0, 150, 0);
            MoneyManager.Instance.RemoveMoney(player, MoneyTypes.Cash, amount);
            MoneyManager.Instance.AddMoney(player, MoneyTypes.Bank, amount);
        }
        

        private void TransferMoney([FromSource] Player player, int amount, int id)
        {
            var plyList = new PlayerList();
            var tgtPly = plyList[id];
            if (tgtPly == null)
            {
                Utility.Instance.SendChatMessage(player, "[Bank]", "Invalid player", 0, 150, 0);
                return;
            }

            if (amount <= 0)
            {
                Utility.Instance.SendChatMessage(player, "[Bank]", "Invalid amount", 0, 150, 0);
                return;
            }

            if (MoneyManager.Instance.GetMoney(player, MoneyTypes.Bank) < amount)
            {
                Utility.Instance.SendChatMessage(player, "[Bank]", "Not enough balance.", 0, 150, 0);
                return;
            }

            var user = UserManager.Instance.GetUserFromPlayer(player);
            var tgtUser = UserManager.Instance.GetUserFromPlayer(tgtPly);
            Utility.Instance.SendChatMessage(player, "[Bank]", "You have transfered " + amount + " to " + tgtUser.CurrentCharacter.FullName + ".", 0, 150, 0);
            Utility.Instance.SendChatMessage(tgtPly, "[Bank]", user.CurrentCharacter.FullName + " has transfered " + amount + " to your bank account.", 0, 150, 0);
            MoneyManager.Instance.RemoveMoney(player, MoneyTypes.Bank, amount);
            MoneyManager.Instance.AddMoney(tgtPly, MoneyTypes.Bank, amount);
        }
        private void TransferCash([FromSource] Player player, int amount, int id)
        {
            var plyList = new PlayerList();
            var tgtPly = plyList[id];
            if (tgtPly == null)
            {
                Utility.Instance.SendChatMessage(player, "[Wallet]", "Invalid player", 0, 150, 0);
                return;
            }

            if (amount <= 0)
            {
                Utility.Instance.SendChatMessage(player, "[Wallet]", "Invalid amount", 0, 150, 0);
                return;
            }

            if (MoneyManager.Instance.GetMoney(player, MoneyTypes.Cash) < amount)
            {
                Utility.Instance.SendChatMessage(player, "[Wallet]", "Not enough balance.", 0, 150, 0);
                return;
            }

            var user = UserManager.Instance.GetUserFromPlayer(player);
            var tgtUser = UserManager.Instance.GetUserFromPlayer(tgtPly);
            Utility.Instance.SendChatMessage(player, "[Wallet]", "You have given " + amount + " to " + tgtUser.CurrentCharacter.FullName + ".", 0, 150, 0);
            Utility.Instance.SendChatMessage(tgtPly, "[Wallet]", user.CurrentCharacter.FullName + " has given you " + amount + ".", 0, 150, 0);
            MoneyManager.Instance.RemoveMoney(player, MoneyTypes.Cash, amount);
            MoneyManager.Instance.AddMoney(tgtPly, MoneyTypes.Cash, amount);
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
                    Utility.Instance.SendChatMessage(player, "[Money]", amount.ToString() + " has been added to your cash.", 0, 170, 60);
                    break;
                case MoneyTypes.Bank:
                    money.Bank += amount;
                    Utility.Instance.SendChatMessage(player, "[Money]", amount.ToString() + " has been added to your bank account.", 0, 170, 60);
                    break;
                case MoneyTypes.Untaxed:
                    money.UnTaxed += amount;
                    Utility.Instance.SendChatMessage(player, "[Money]", amount.ToString() + " has been added to your untaxed stash.", 0, 170, 60);
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
                    Utility.Instance.SendChatMessage(player, "[Money]", amount.ToString()+" has been removed from your cash.",0,170,60);
                    break;
                case MoneyTypes.Bank:
                    money.Bank -= amount;
                    Utility.Instance.SendChatMessage(player, "[Money]", amount.ToString() + " has been removed from your bank account.", 0, 170, 60);
                    break;
                case MoneyTypes.Untaxed:
                    money.UnTaxed -= amount;
                    Utility.Instance.SendChatMessage(player, "[Money]", amount.ToString() + " has been removed from your untaxed stash.", 0, 170, 60);
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
                    Utility.Instance.SendChatMessage(player, "[Money]", "You cash has been set to "+ amount +".", 0, 170, 60);
                    break;
                case MoneyTypes.Bank:
                    money.Bank = amount;
                    Utility.Instance.SendChatMessage(player, "[Money]", "You bank account has been set to " + amount + ".", 0, 170, 60);
                    break;
                case MoneyTypes.Untaxed:
                    money.UnTaxed = amount;
                    Utility.Instance.SendChatMessage(player, "[Money]", "You untaxed stash has been set to " + amount + ".", 0, 170, 60);
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
