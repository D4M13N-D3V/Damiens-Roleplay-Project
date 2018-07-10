using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace client.Main.Interiors
{
    public class Interior
    {
        public string Name;
        public Vector3 Inside;
        public Vector3 Outside;

        public Interior( Vector3 outside, Vector3 inside, string name)
        {
            Name = name;
            Inside = inside;
            Outside = outside;
        }
    }
}
