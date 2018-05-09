using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay
{
    public class Utility:BaseScript
    {
        public static Utility Instance;
        public Utility()
        {
            Instance = this;
        }

        public void Log(string message)
        {
            Debug.WriteLine("[PINEAPPLE ISLAND ROLEPALY] [DEBUG LOG] " + message);
        }

        public void DrawTxt(float x, float y, float width, float height, float scale, string text, int r, int g, int b, int a)
        {
            API.SetTextFont(4);
            API.SetTextProportional(false);
            API.SetTextScale(scale,scale);
            API.SetTextColour(r,g,b,a);
            API.SetTextDropshadow(0,0,0,0,255);
            API.SetTextEdge(2,0,0,0,255);
            API.SetTextDropShadow();
            API.SetTextOutline();
            API.SetTextEntry("STRING");
            API.AddTextComponentString(text);
            API.DrawText(x + width / 2, y - height / 2 + 0.005f);
        }

        public void DrawRct(float x, float y, float width, float height, int r, int g, int b, int a)
        {
            API.DrawRect(x + width / 2, y + height / 2, width, height, r, g, b, a);
        }

    }
}



