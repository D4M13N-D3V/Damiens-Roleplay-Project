using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using roleplay.Main.Users;

namespace roleplay.Main
{
    public class UserManager:BaseScript
    {
        private static UserManager instance;
        public UserManager()
        {
            SetupEvents();
        }
        public static UserManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserManager();
                }
                return instance;
            }
        }

        private void SetupEvents()
        {
            Debug.WriteLine("");
            EventHandlers["roleplay:spawned"] += new Action<Player>(Spawned);
        }

        private void Spawned([FromSource]Player source)
        {
            Debug.WriteLine("TEST");
        }

    }
}
