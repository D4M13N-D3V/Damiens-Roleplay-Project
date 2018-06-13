using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Main.EmergencyServices.EMS
{
    public class EMSUniformComponent
    {
        public int Component;
        public int Drawable;
        public int Texture;
        public int Pallet;
        public EMSUniformComponent(int comp, int draw, int text, int pallet)
        {
            Component = comp;
            Drawable = draw;
            Texture = text;
            Pallet = pallet;
        }
    }
}
