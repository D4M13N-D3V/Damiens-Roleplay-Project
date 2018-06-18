using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace client.Main.Criminal.Robberies
{
    /// <summary>
    /// A spot that can be robbed
    /// </summary>
    public class RobberySpot
    {
        /// <summary>
        /// The name of the spot
        /// </summary>
        public string Name;
        /// <summary>
        /// How long it takes to rob it
        /// </summary>
        public int TimeToRobInSeconds;
        /// <summary>
        /// How many polcie are required
        /// </summary>
        public int RequiredPolice;
        /// <summary>
        /// The maximum amount of money you can get
        /// </summary>
        public int MaxReward;
        /// <summary>
        /// The minimum amount of money youcan get
        /// </summary>
        public int MinReward;
        /// <summary>
        /// The posistion of the robbery
        /// </summary>
        public Vector3 Posistion;
        /// <summary>
        /// Who is currently robbing
        /// </summary>
        public Player Robber;
        /// <summary>
        /// Is it being robbed?
        /// </summary>
        public bool BeingRobbed = false;
        /// <summary>
        /// Can it be robbed?
        /// </summary>
        public bool CanBeRobbed = true;

        public RobberySpot(Vector3 pos, string name, int time, int police, int max, int min)
        {
            Name = name;
            Posistion = pos;
            MaxReward = max;
            MinReward = min;
            RequiredPolice = police;
            TimeToRobInSeconds = time;
        }
    }
}
