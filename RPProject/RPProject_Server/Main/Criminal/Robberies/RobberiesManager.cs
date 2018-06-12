using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client.Main.Criminal.Robberies;
using CitizenFX.Core;
using server.Main.Users;

namespace server.Main.Criminal.Robberies
{
    public class RobberiesManager : BaseScript
    {
        public static RobberiesManager Instance;

        private Random _random = new Random();

        public List<RobberySpot> Spots = new List<RobberySpot>()
        {
            new RobberySpot(new Vector3(1707.9324951172f,4920.3510742188f,42.063674926758f), "Grapseed Main Street 24/7", 60, 3, 8000, 1000),
            new RobberySpot(new Vector3(-2959.7761230469f,387.1662902832f,14.043292999268f), "Great Ocean Highway Liquior Store", 60, 3, 8000, 1000),
            new RobberySpot(new Vector3(2549.6787109375f,384.93060302734f,108.62294769287f), "Palimino Freeway 24/7", 60, 3, 8000, 1000),
            new RobberySpot(new Vector3(-709.68231201172f,-904.01232910156f,19.21561050415f), "Little Seoul 24/7", 60, 3, 8000, 1000),
            new RobberySpot(new Vector3(2672.7614746094f,3286.6235351563f,55.241134643555f), "Senory Fwy 24/7", 60, 3, 8000, 1000),
            new RobberySpot(new Vector3(-1829.1158447266f,798.81048583984f,138.18949890137f), "Banham Canyon Drive 24/7", 60, 3, 8000, 1000),
            new RobberySpot(new Vector3(147.01237487793f,-1044.9592285156f,29.368022918701f), "Legion Square Bank", 60, 3, 20000, 5000),
            new RobberySpot(new Vector3(255.65658569336f,226.54191589355f,101.87573242188f), "Pacific Standard Bank", 60, 3, 20000, 5000),
            new RobberySpot(new Vector3(-104.95664215088f,6476.5415039063f,31.626724243164f), "Blaine County Savings", 60, 3, 20000, 5000),
        };

        public RobberiesManager()
        {
            Instance = this;
            EventHandlers["StartRobbingStoreRequest"] += new Action<Player, string>(StartRobbery);
            EventHandlers["CompleteRobbingStore"] += new Action<Player, string>(CompleteRobbery);
            EventHandlers["EndRobbingStore"] += new Action<Player, string>(EndRobbery);
        }

        private void StartRobbery([FromSource]Player player, string s)
        {
            foreach (var spot in Spots)
            {
                if (spot.Name == s)
                {
                    if (spot.CanBeRobbed && !spot.BeingRobbed)
                    {
                        spot.BeingRobbed = true;
                        spot.CanBeRobbed = false;
                        spot.Robber = player;
                        TriggerClientEvent(player, "StartRobbingStore");
                        ResetCanBeRobbed(spot.Name);
                        return;
                    }
                    else
                    {
                        Utility.Instance.SendChatMessage(player, "[Robberies]", "Is being robbed or has been robbed to recently", 255, 0, 0);
                        return;
                    }
                }
            }
        }

        private async Task ResetCanBeRobbed(string name)
        {
            await Delay(61000);
            foreach (var spot in Spots)
            {
                if (spot.Name == name)
                {
                    spot.CanBeRobbed = true;
                    spot.BeingRobbed = false;
                    return;
                }
            }
        }

        private void CompleteRobbery([FromSource]Player player, string s)
        {
            foreach (var spot in Spots)
            {
                if (spot.Name == s)
                {
                    spot.BeingRobbed = false;
                    spot.Robber = null;
                    MoneyManager.Instance.AddMoney(player,MoneyTypes.Cash,_random.Next(spot.MinReward,spot.MaxReward));
                    return;
                }
            }
        }

        private void EndRobbery([FromSource]Player player, string s)
        {
            foreach (var spot in Spots)
            {
                if (spot.Name == s)
                {
                    spot.BeingRobbed = false;
                    spot.Robber = null;
                   return;
                }
            }
        }
    }
}
