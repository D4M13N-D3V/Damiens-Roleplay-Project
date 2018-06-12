using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.Criminal.Informant
{
    public class InformantInfo
    {
        public string Title = "";
        public string Hint = "";
        public int Price = 0;
        public InformantInfo(string title, string hint, int price)
        {
            Title = title;
            Hint = hint;
            Price = price;
        }
    }
}
