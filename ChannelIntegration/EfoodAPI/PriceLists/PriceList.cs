using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelIntegration.EfoodAPI.PriceList
{
    public class Pricelist
    {
        public string id { get; set; }
        public string price { get; set; }
        public string is_available { get; set; }
        public string environmental_fee { get; set; }
    }
}
