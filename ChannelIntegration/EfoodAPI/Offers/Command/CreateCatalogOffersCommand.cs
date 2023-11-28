using ChannelIntegration.EfoodAPI.Images;
using ChannelIntegration.EfoodAPI.Rules;
using ChannelIntegration.EfoodAPI.Schedules;
using ChannelIntegration.EfoodAPI.Tiers;

namespace ChannelIntegration.EfoodAPI.Offers.Command
{
    public class CreateCatalogOffersCommand
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
