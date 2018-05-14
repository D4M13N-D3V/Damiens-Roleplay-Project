using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace roleplay.Main
{
    class Utility:BaseScript
    {
        public static Utility Instance;
        public Utility()
        {
            Instance = this;
        }

        public void Log(string message)
        {
            Debug.WriteLine("[PINEAPPLE ISLAND ROLEPALY] [DEBUG LOG] "+message);
        }

        public void SendChatMessage(Player ply, string title, string message, int r, int g, int b)
        {
            TriggerClientEvent(ply, "chatMessage", title, new[] { r, g, b }, message);
        }
        public void SendChatMessageAll( string title, string message, int r, int g, int b)
        {
            TriggerClientEvent("chatMessage", title, new[] { r, g, b }, message);
        }
    }
}
