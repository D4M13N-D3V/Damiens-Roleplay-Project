using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roleplay.Main.Clothes
{
    public class ClothesStore
    {
        public string Name = "Clothing Store";
        public float X = 0;
        public float Y = 0;
        public float Z = 0;

        public ClothesStore(float x, float y, float z, string name)
        {
            Name = name;
            X = x;
            Y = y;
            Z = z;
        }
    }
}
