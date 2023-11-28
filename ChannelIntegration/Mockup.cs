using ChannelIntegration.EfoodAPI.Images;
using ChannelIntegration.EfoodAPI.Rules;
using ChannelIntegration.EfoodAPI.Schedules;
using ChannelIntegration.EfoodAPI.Tiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelIntegration
{

    public class Rootobject
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string is_available { get; set; }
        public string order { get; set; }
        public Image images { get; set; }
        public string tag { get; set; }
        public string price_tag { get; set; }
        public string pickup_only { get; set; }
        public Rule rule { get; set; }
        public Schedule schedules { get; set; }
        public Tier[] tiers { get; set; }
    }










}
