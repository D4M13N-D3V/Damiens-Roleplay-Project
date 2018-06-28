using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace client.Main.Housing
{
    public class House
    {
        public int Id;
        public string Name;
        public string Description;
        public Vector3 Position;
        public bool ForSale;
        public string Owner;
        public int Price;

        public dynamic HasGarage = true;
        public dynamic GaragePosistion;

        public House(int id, string name, string desc, Vector3 pos, bool forsale, string owner, int price)
        {
            Id = id;
            Name = name;
            Description = desc;
            Position = pos;
            ForSale = forsale;
            Owner = owner;
            Price = price;
        }
    }
}
