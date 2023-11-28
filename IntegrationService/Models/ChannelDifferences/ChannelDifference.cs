using IntegrationService.Models.Channels;
using IntegrationService.Models.EntityBases;
using IntegrationService.Models.Product;

namespace IntegrationService.Models.ChannelDifferences
{
    public class ChannelDifference : SimpleBaseEntity
    {
        public ChannelDifference()
        {
        }
        public Guid ProductStoresId { get; set; }

        public virtual ProductStores ProductStore { get; set; }
        public Guid ChannelId { get; set; }

        public virtual Channel Channel { get; set; }

        public string Value { get; set; }
        private ChannelDifference(Guid ProductStoresId, Guid ChannelId, string value)
        {
            var ChannelDifference = new ChannelDifference();
            ChannelDifference.ProductStoresId = ProductStoresId;
            ChannelDifference.ChannelId = ChannelId;
            ChannelDifference.Value = value;
        }
        public ChannelDifference Create(Guid ProductStoresId, Guid ChannelId, string value)
        {
            var ChannelDifference = new ChannelDifference(ProductStoresId, ChannelId, value);
            return ChannelDifference;
        }
        public void Update(Guid ProductStoresId, Guid ChannelId, string value)
        {
            this.ProductStoresId = ProductStoresId;
            this.ChannelId = ChannelId;
            this.Value = value;
        }
    }
}
