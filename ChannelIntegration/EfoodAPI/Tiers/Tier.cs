using ChannelIntegration.EfoodAPI.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static ChannelIntegration.EfoodAPI.Categories.TestMockup;

namespace ChannelIntegration.EfoodAPI.Tiers
{
    public class Tier
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string order { get; set; }
        public string free_options { get; set; }
        public string maximum_selections { get; set; }
        public Option[] options { get; set; }
    }
}
