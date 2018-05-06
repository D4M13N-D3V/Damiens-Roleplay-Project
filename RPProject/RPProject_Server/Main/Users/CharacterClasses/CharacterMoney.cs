namespace roleplay.Main.Users.CharacterClasses
{

    public enum  MoneyTypes { Cash, Bank, Untaxed }

    public class CharacterMoney
    {

        public Character Character;
        public int Cash;
        public int Bank;
        public int UnTaxed;

        public void AddMoney(MoneyTypes type, int amount)
        {
            switch (type)
            {
                case MoneyTypes.Cash:
                    Cash += amount;
                    break;
                case MoneyTypes.Bank:
                    Bank += amount;
                    break;
                case MoneyTypes.Untaxed:
                    UnTaxed += amount;
                    break;
            }
        }

        public void RemoveMoney(MoneyTypes type, int amount)
        {
            switch (type)
            {
                case MoneyTypes.Cash:
                    if (Cash - amount > 0)
                        Cash += amount;
                    break;
                case MoneyTypes.Bank:
                    if (Bank - amount > 0)
                        Bank += amount;
                    break;
                case MoneyTypes.Untaxed:
                    if (UnTaxed - amount > 0)
                        UnTaxed += amount;
                    break;
            }
        }

    }
}
