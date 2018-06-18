using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using server.Main.Users;

namespace server.Main.Criminal.Informant
{
    public class Informant : BaseScript
    {
        public static Informant Instance;

        public List<InformantInfo> Information = new List<InformantInfo>()
        {
            new InformantInfo("Meth Bulk Pickup", "Desert meth lab.", 5000),
            new InformantInfo("Meth Bulk Sell/Singles Pickup", "Bikers live here.", 6000),
            new InformantInfo("Weed Bulk Pickup", "Farm near the mountains", 8000),
            new InformantInfo("Weed Bulk Sell/Singles Pickup", "Shop on the beach",10000),
            new InformantInfo("Cocaine Bulk Pickup","Boat.",5000),
            new InformantInfo("Cocaine Bulk Sell/Singles","Big ass party house.",6000),
        };
            
        public Informant()
        {
            EventHandlers["BuyInformerInformation"] += new Action<Player, string>(BuyInformation);
        }

        private void BuyInformation([FromSource]Player player, string s)
        {
            try
            {
                InformantInfo info = null;
                foreach (var tmpinfo in Information)
                {
                    if (tmpinfo.Title == s)
                    {
                        info = tmpinfo;
                        break;
                    }
                }
                if (info != null)
                {
                    if (MoneyManager.Instance.GetMoney(player, MoneyTypes.Cash) >= info.Price)
                    {
                        MoneyManager.Instance.RemoveMoney(player,MoneyTypes.Cash, info.Price);
                        Utility.Instance.SendChatMessage(player,"[Informant]", "("+info.Title+") "+info.Hint,255,0,0);
                    }
                    else
                    {
                        Utility.Instance.SendChatMessage(player, "[Informant]", "Not enough money to buy this information!", 255, 0, 0);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
