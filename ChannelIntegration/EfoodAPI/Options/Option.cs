using ChannelIntegration.EfoodAPI.PriceList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelIntegration.EfoodAPI.Options
{
    public class Option
    {
        public string id { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string is_available { get; set; }
        public string environmental_fee { get; set; }
        public string selected { get; set; }
        public string pricelist_tier_id { get; set; }
        public IEnumerable<Pricelist> pricelist { get; set; }
    }
}
