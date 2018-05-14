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
    public class ClosestPlayerReturnInfo
    {
        public int Pid = 0;
        public int Ped = 0;
        public float Dist = 0;
        public ClosestPlayerReturnInfo(int pid, int ped, float dist)
        {
            Pid = pid;
            Ped = ped;
            Dist = dist;
        }
    }

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

        public async void KeyboardInput(string title, string defaultText, int maxlength, Action<string> cb)
        {
            API.DisableAllControlActions(0);
            API.AddTextEntry("FMMC_KEY_TIP1", title + ":");
            API.DisplayOnscreenKeyboard(1, "FMMC_KEY_TIP1","",defaultText,"","","",maxlength);
            while( API.UpdateOnscreenKeyboard() != 1 && API.UpdateOnscreenKeyboard() != 2)
            {
                await Delay(0);
            }
            if (API.UpdateOnscreenKeyboard() != 2)
            {
                var result = API.GetOnscreenKeyboardResult();
                cb(result);
            }

            await (Delay(500));

            API.EnableAllControlActions(0);
        }

        public void GetClosestPlayer(out ClosestPlayerReturnInfo output)
        {
            var playerPos = API.GetEntityCoords(API.PlayerPedId(), true);
            int closestPlayer = -2;
            int closestPlayerPed = -2;
            float  dist = 9999;
            for (int i = 0; i < 32; i++)
            {
                if (i != API.PlayerId())
                {
                    var otherPlayerPed = API.GetPlayerPed(i);
                    var pos = API.GetEntityCoords(otherPlayerPed,true);
                    var distance = API.Vdist(playerPos.X, playerPos.Y, playerPos.Z, pos.X, pos.Y, pos.Z);
                    if (distance < dist)
                    {
                        closestPlayer = i;
                        closestPlayerPed = otherPlayerPed;
                        dist = distance;
                    }
                }
            }
            output = new ClosestPlayerReturnInfo(closestPlayer,closestPlayerPed,dist);
        }

        public void SendChatMessage(string title, string message, int r, int g, int b)
        {
            TriggerEvent("chatMessage", title, new[] { r, g, b }, message);
        }

    }
}



