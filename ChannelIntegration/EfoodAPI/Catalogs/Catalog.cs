using ChannelIntegration.EfoodAPI.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannelIntegration.EfoodAPI;
using ChannelIntegration.EfoodAPI.Images;
using ChannelIntegration.EfoodAPI.Tiers;
//using static ChannelIntegration.EfoodAPI.Categories.TestMockup;

namespace ChannelIntegration.EfoodAPI.Catalogs
{
    public class Catalog
    {
        public string id { get; set; }
        public Category category { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string order { get; set; }
        public string is_available { get; set; }
        public string max_item_count { get; set; }
        public string description { get; set; }
        public string full_price { get; set; }
        public string environmental_fee { get; set; }
        public string metric_unit_description { get; set; }
        public string size_info { get; set; }
        public string auto_reset { get; set; }
        public string is_hidden { get; set; }
        public string ignore_base_price { get; set; }
        public Image images { get; set; }
        public IEnumerable<Tier> tiers { get; set; }
    }
}
