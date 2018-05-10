using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace roleplay.Main.Users.Customization
{
    public class CustomizationDecoration
    {
        public CustomizationDecoration(string collection, string overlay)
        {
            Collection = collection;
            Overlay = overlay;
        }

        public string Collection;
        public string Overlay;
    }
}
