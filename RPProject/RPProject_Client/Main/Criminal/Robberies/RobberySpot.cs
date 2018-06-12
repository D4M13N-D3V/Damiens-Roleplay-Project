using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace client.Main.Criminal.Robberies
{
    public class RobberySpot
    {
        public string Name;
        public int TimeToRobInSeconds;
        public int RequiredPolice;
        public int MaxReward;
        public int MinReward;
        public Vector3 Posistion;

        public bool BeingRobbed = false;
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
