using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Main.Criminal.Informant
{
    /// <summary>
    /// Information for the informant
    /// </summary>
    public class InformantInfo
    {
        /// <summary>
        /// The title of the hint, describes what the hint is for
        /// </summary>
        public string Title = "";
        /// <summary>
        /// The actual hint
        /// </summary>
        public string Hint = "";
        /// <summary>
        /// How much it costs.
        /// </summary>
        public int Price = 0;
        public InformantInfo(string title, string hint, int price)
        {
            Title = title;
            Hint = hint;
            Price = price;
        }
    }
}
