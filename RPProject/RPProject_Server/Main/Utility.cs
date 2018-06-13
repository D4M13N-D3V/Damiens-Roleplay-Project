using System;
using System.Linq;
using System.Text;
using CitizenFX.Core;

namespace server.Main
{


    class Utility:BaseScript
    {
        public static Utility Instance;
        public Utility()
        {
            Instance = this;
        }

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
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
        public string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static Player GetPlayerFromArgs(string arg, PlayerList pl)
        {
            int id;

            if (Int32.TryParse(arg, out id))
            {
                return pl[id];
            }

            if (pl.Count(x => x.Name == arg) == 0)
            {
                return null;
            }

            return pl.Single(x => x.Name == arg);
        }
    }
}
