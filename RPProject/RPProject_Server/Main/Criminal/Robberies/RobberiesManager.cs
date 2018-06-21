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
    /// <summary>
    /// The manager class for robberies.
    /// </summary>
    public class RobberiesManager : BaseScript
    {
        /// <summary>
        /// Instance for singleton
        /// </summary>
        public static RobberiesManager Instance;

        /// <summary>
        /// The random object for RNG.
        /// </summary>
        private Random _random = new Random();
        
        /// <summary>
        /// All the possible robbable spots.
        /// </summary>
        public List<RobberySpot> Spots = new List<RobberySpot>()
        {
            new RobberySpot(new Vector3(1707.9324951172f,4920.3510742188f,42.063674926758f), "Grapseed Main Street 24/7", 60, 3, 20000, 6000),
            new RobberySpot(new Vector3(-2959.7761230469f,387.1662902832f,14.043292999268f), "Great Ocean Highway Liquior Store", 60, 3, 20000, 6000),
            new RobberySpot(new Vector3(2549.6787109375f,384.93060302734f,108.62294769287f), "Palimino Freeway 24/7", 60, 3, 20000, 6000),
            new RobberySpot(new Vector3(-709.68231201172f,-904.01232910156f,19.21561050415f), "Little Seoul 24/7", 60, 3, 20000, 6000),
            new RobberySpot(new Vector3(2672.7614746094f,3286.6235351563f,55.241134643555f), "Senory Fwy 24/7", 60, 3, 20000, 6000),
            new RobberySpot(new Vector3(-1829.1158447266f,798.81048583984f,138.18949890137f), "Banham Canyon Drive 24/7", 60, 3, 20000, 6000),
            new RobberySpot(new Vector3(147.01237487793f,-1044.9592285156f,29.368022918701f), "Legion Square Bank", 60, 3, 60000, 20000),
            new RobberySpot(new Vector3(255.65658569336f,226.54191589355f,101.87573242188f), "Pacific Standard Bank", 60, 3, 60000, 20000),
            new RobberySpot(new Vector3(-104.95664215088f,6476.5415039063f,31.626724243164f), "Blaine County Savings", 60, 3, 60000, 20000),
        };

        public RobberiesManager()
        {
            Instance = this;
            EventHandlers["StartRobbingStoreRequest"] += new Action<Player, string>(StartRobbery);
            EventHandlers["CompleteRobbingStore"] += new Action<Player, string>(CompleteRobbery);
            EventHandlers["EndRobbingStore"] += new Action<Player, string>(EndRobbery);
        }

        /// <summary>
        /// The event hanlder for when someone starts a robbery
        /// </summary>
        /// <param name="player">the player who triggered it</param>
        /// <param name="s">the name of the spot were robbing.</param>
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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                        ResetCanBeRobbed(spot.Name);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
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

        /// <summary>
        /// Resets when the spot can be robbed
        /// </summary>
        /// <param name="name">The name of the spot.</param>
        /// <returns></returns>
        private async Task ResetCanBeRobbed(string name)
        {
            await Delay(900000);
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

        /// <summary>
        /// Event handler for when the robbery is compelted.
        /// </summary>
        /// <param name="player">the player who triggered</param>
        /// <param name="s">the name of the spot.</param>
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

        /// <summary>
        /// The event handler for when the robbery ends before it is over.
        /// </summary>
        /// <param name="player">the player who triggered it</param>
        /// <param name="s">the name of the spot</param>
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
