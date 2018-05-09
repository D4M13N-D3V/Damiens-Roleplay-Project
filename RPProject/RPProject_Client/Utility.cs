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

        public void DrawTxt(float x, float y, float width, float height, float scale, string text, int r, int g, int b, int a,bool centered)
        {
            API.SetTextFont(4);
            API.SetTextProportional(false);
            API.SetTextScale(scale,scale);
            API.SetTextColour(r,g,b,a);
            API.SetTextEntry("STRING");
            API.SetTextOutline();
            API.AddTextComponentString(text);
            if (centered)
            {
                API.SetTextCentre(true);
            }
            API.DrawText(x,y);
        }

        public void DrawRct(float x, float y, float width, float height, int r, int g, int b, int a)
        {
            API.DrawRect(x + width / 2, y + height / 2, width, height, r, g, b, a);
        }

    }
}



