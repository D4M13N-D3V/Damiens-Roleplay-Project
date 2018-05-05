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
        public CustomizationDecoration(Hash collection, Hash overlay)
        {
            Collection = collection;
            Overlay = overlay;
        }

        public Hash Collection;
        public Hash Overlay;
    }
}
